// Lightweight API client for the React app
// Reads base URL from Vite env (set in docker-compose) with sensible fallbacks for local dev.

export type HttpMethod = 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE'

export interface ApiClientOptions {
  baseUrl?: string
  defaultHeaders?: Record<string, string>
  timeoutMs?: number
}

export class ApiError extends Error {
  status?: number
  data?: unknown
  constructor(message: string, status?: number, data?: unknown) {
    super(message)
    this.name = 'ApiError'
    this.status = status
    this.data = data
  }
}

function getDefaultBaseUrl(): string {
  // Primary: Vite env (docker-compose sets VITE_API_URL=http://weathernow.api:8080)
  const fromEnv = import.meta?.env?.VITE_API_URL as string | undefined
  if (fromEnv) return fromEnv.replace(/\/$/, '')

  // Fallbacks for local dev:
  // 1) If the front-end is served behind the same host, try same origin
  if (typeof window !== 'undefined') {
    const { protocol, hostname } = window.location
    // Try common API ports for ASP.NET Core
    const portCandidates = [8080, 5000]
    return `${protocol}//${hostname}:${portCandidates[0]}`
  }

  // Final fallback
  return 'http://localhost:8080'
}

export class ApiClient {
  private readonly baseUrl: string
  private readonly headers: Record<string, string>
  private readonly timeoutMs: number

  constructor(options: ApiClientOptions = {}) {
    this.baseUrl = (options.baseUrl ?? getDefaultBaseUrl()).replace(/\/$/, '')
    this.headers = {
      'Content-Type': 'application/json',
      ...(options.defaultHeaders ?? {}),
    }
    this.timeoutMs = options.timeoutMs ?? 15000
  }

  private buildUrl(path: string, query?: Record<string, string | number | boolean | undefined>): string {
    const url = new URL(path.replace(/^\//, ''), this.baseUrl + '/')
    if (query) {
      Object.entries(query).forEach(([k, v]) => {
        if (v !== undefined && v !== null) url.searchParams.set(k, String(v))
      })
    }
    return url.toString()
  }

  private withTimeout<T>(promise: Promise<T>): Promise<T> {
    return new Promise<T>((resolve, reject) => {
      const timer = setTimeout(() => reject(new ApiError('Request timed out', 408)), this.timeoutMs)
      promise
        .then((value) => {
          clearTimeout(timer)
          resolve(value)
        })
        .catch((err) => {
          clearTimeout(timer)
          reject(err)
        })
    })
  }

  async request<TResponse = unknown, TBody = unknown>(
    method: HttpMethod,
    path: string,
    options?: {
      query?: Record<string, string | number | boolean | undefined>
      body?: TBody
      headers?: Record<string, string>
      signal?: AbortSignal
    }
  ): Promise<TResponse> {
    const url = this.buildUrl(path, options?.query)

    const response = await this.withTimeout(
      fetch(url, {
        method,
        headers: { ...this.headers, ...(options?.headers ?? {}) },
        body: options?.body !== undefined ? JSON.stringify(options.body) : undefined,
        signal: options?.signal,
      })
    )

    const contentType = response.headers.get('Content-Type') || ''
    const isJson = contentType.includes('application/json')
    const data = isJson ? await response.json().catch(() => undefined) : await response.text().catch(() => undefined)

    if (!response.ok) {
      const message = isJson && data && typeof data === 'object' && 'message' in (data as any)
        ? String((data as any).message)
        : `Request failed with status ${response.status}`
      throw new ApiError(message, response.status, data)
    }

    return data as TResponse
  }

  get<TResponse>(path: string, query?: Record<string, string | number | boolean | undefined>, signal?: AbortSignal) {
    return this.request<TResponse>('GET', path, { query, signal })
  }

  post<TResponse, TBody>(path: string, body?: TBody, signal?: AbortSignal) {
    return this.request<TResponse, TBody>('POST', path, { body, signal })
  }
}

// A default singleton instance for convenience
export const apiClient = new ApiClient()
