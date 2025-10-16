import { useEffect, useMemo, useRef, useState } from 'react'
import { getCurrentForecast, type WeatherForecastDto } from '../services/weatherService'

export interface UseWeatherForecastParams {
  latitude?: number
  longitude?: number
  // auto fetch on mount if both coords provided
  auto?: boolean
}

export interface UseWeatherForecastResult {
  data?: WeatherForecastDto
  loading: boolean
  error?: Error
  // triggers a fetch; if coords omitted, last known coords are used
  refetch: (coords?: { latitude: number; longitude: number }) => void
}

export function useWeatherForecast(params: UseWeatherForecastParams = {}): UseWeatherForecastResult {
  const [data, setData] = useState<WeatherForecastDto | undefined>(undefined)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<Error | undefined>(undefined)

  const lastCoordsRef = useRef<{ latitude: number; longitude: number } | undefined>(
    params.latitude !== undefined && params.longitude !== undefined
      ? { latitude: params.latitude, longitude: params.longitude }
      : undefined
  )

  const canAutoFetch = useMemo(() => {
    return params.auto !== false && params.latitude !== undefined && params.longitude !== undefined
  }, [params.auto, params.latitude, params.longitude])

  useEffect(() => {
    if (params.latitude !== undefined && params.longitude !== undefined) {
      lastCoordsRef.current = { latitude: params.latitude, longitude: params.longitude }
    }
  }, [params.latitude, params.longitude])

  const refetch = (coords?: { latitude: number; longitude: number }) => {
    const useCoords = coords ?? lastCoordsRef.current
    if (!useCoords) {
      setError(new Error('Coordinates are required to fetch weather forecast'))
      return
    }

    const abortController = new AbortController()
    setLoading(true)
    setError(undefined)

    getCurrentForecast(useCoords, abortController.signal)
      .then((result) => {
        setData(result)
      })
      .catch((err) => {
        if (err?.name === 'AbortError') return
        setError(err instanceof Error ? err : new Error('Failed to load weather forecast'))
      })
      .finally(() => setLoading(false))

    return () => abortController.abort()
  }

  useEffect(() => {
    if (canAutoFetch) {
      const cleanup = refetch()
      return () => {
        if (typeof cleanup === 'function') cleanup()
      }
    }
    return
  }, [canAutoFetch])

  return { data, loading, error, refetch }
}

export default useWeatherForecast
