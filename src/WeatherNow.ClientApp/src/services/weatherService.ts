import { apiClient } from './api'

// TypeScript types aligned with backend domain models
export interface LocationDto {
  latitude: number
  longitude: number
}

export type TemperatureUnit = 'Kelvin' | 'Celsius' | 'Fahrenheit'
export interface TemperatureDto {
  value: number
  unit: TemperatureUnit
}

export type SpeedUnit = 'MetersPerSecond' | 'KilometersPerHour' | 'MilesPerHour'
export interface WindConditionDto {
  speed: number
  unit: SpeedUnit
}

export interface WeatherForecastDto {
  forecastLocation: LocationDto
  description: string
  conditions: string
  temperature: TemperatureDto
  windCondition: WindConditionDto
  recommendation?: string
}

export async function getCurrentForecast(params: { latitude: number; longitude: number }, abortSignal?: AbortSignal) {
  // Backend route: /WeatherForecast?latitude=..&longitude=..
  return apiClient.get<WeatherForecastDto>('WeatherForecast', {
    latitude: params.latitude,
    longitude: params.longitude,
  }, abortSignal)
}
