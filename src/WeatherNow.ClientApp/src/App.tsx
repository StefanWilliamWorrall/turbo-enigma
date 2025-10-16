import { useMemo, useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import useWeatherForecast from './hooks/useWeatherForecast'

function App() {
  const [latInput, setLatInput] = useState('')
  const [lonInput, setLonInput] = useState('')

  const { data, loading, error, refetch } = useWeatherForecast({ auto: false })

  const parsed = useMemo(() => ({
    latitude: parseFloat(latInput),
    longitude: parseFloat(lonInput),
  }), [latInput, lonInput])

  const coordsValid = useMemo(() => (
    !Number.isNaN(parsed.latitude) && !Number.isNaN(parsed.longitude)
  ), [parsed])

  const handleGetForecast = () => {
    if (!coordsValid) return
    refetch({ latitude: parsed.latitude, longitude: parsed.longitude })
  }

  return (
    <>
      <div>
        <a href="https://vite.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>WeatherNow</h1>

      <div className="card" style={{ display: 'grid', gap: 12 }}>
        <div style={{ display: 'flex', gap: 8, alignItems: 'center', flexWrap: 'wrap' }}>
          <label>
            Latitude:
            <input
              type="number"
              step="any"
              placeholder="e.g. -37.8136"
              value={latInput}
              onChange={(e) => setLatInput(e.target.value)}
              style={{ marginLeft: 8 }}
            />
          </label>
          <label>
            Longitude:
            <input
              type="number"
              step="any"
              placeholder="e.g. 144.9631"
              value={lonInput}
              onChange={(e) => setLonInput(e.target.value)}
              style={{ marginLeft: 8 }}
            />
          </label>
          <button onClick={handleGetForecast} disabled={!coordsValid || loading}>
            {loading ? 'Loading…' : 'Get Forecast'}
          </button>
        </div>

        {!coordsValid && (latInput !== '' || lonInput !== '') && (
          <small style={{ color: 'tomato' }}>Please enter valid numeric latitude and longitude.</small>
        )}

        {error && (
          <div style={{ color: 'tomato' }}>Error: {error.message}</div>
        )}

        {data && (
          <div style={{ textAlign: 'left' }}>
            <h3>Current Forecast</h3>
            <div>
              Location: lat {data.forecastLocation.latitude}, lon {data.forecastLocation.longitude}
            </div>
            <div>
              Description: {data.description}
            </div>
            <div>
              Conditions: {data.conditions}
            </div>
            <div>
              Temperature: {data.temperature.value} {data.temperature.unit}
            </div>
            <div>
              Wind: {data.windCondition.speed} {data.windCondition.unit}
            </div>
            {data.recommendation && (
              <div>Recommendation: {data.recommendation}</div>
            )}
          </div>
        )}
      </div>

      <p className="read-the-docs">
        Enter coordinates and click Get Forecast to query the API.
      </p>
    </>
  )
}

export default App
