import { useState, useMemo } from 'react';
import './App.css';

const API_BASE = process.env.REACT_APP_API_BASE_URL || 'https://localhost:5000';

export default function App() {
  const [form, setForm] = useState({
    localSalesCount: '',
    foreignSalesCount: '',
    averageSaleAmount: ''
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [result, setResult] = useState(null);

  const fmt = useMemo(
    () => new Intl.NumberFormat('en-GB', { style: 'currency', currency: 'GBP' }),
    []
  );

  function handleChange(e) {
    const { name, value } = e.target;
    setForm((f) => ({ ...f, [name]: value }));
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setError('');
    setResult(null);

    const payload = {
      localSalesCount: Number.parseInt(form.localSalesCount || '0', 10),
      foreignSalesCount: Number.parseInt(form.foreignSalesCount || '0', 10),
      averageSaleAmount: Number.parseFloat(form.averageSaleAmount || '0')
    };

    if (
      payload.localSalesCount < 0 ||
      payload.foreignSalesCount < 0 ||
      payload.averageSaleAmount < 0
    ) {
      setError('Values must be non-negative.');
      return;
    }

    setLoading(true);
    try {
      const res = await fetch(`${API_BASE}/commission`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      });
      if (!res.ok) {
        const text = await res.text();
        throw new Error(`API error ${res.status}: ${text}`);
      }
      const data = await res.json();
      setResult(data);
    } catch (err) {
      setError(err.message || 'Failed to fetch.');
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="App">
      <header className="App-header">
        <form onSubmit={handleSubmit} style={{ textAlign: 'left' }}>
          <label htmlFor="localSalesCount">Local Sales Count</label><br />
          <input
            id="localSalesCount"
            name="localSalesCount"
            type="number"
            min="0"
            step="1"
            value={form.localSalesCount}
            onChange={handleChange}
          /><br /><br />

          <label htmlFor="foreignSalesCount">Foreign Sales Count</label><br />
          <input
            id="foreignSalesCount"
            name="foreignSalesCount"
            type="number"
            min="0"
            step="1"
            value={form.foreignSalesCount}
            onChange={handleChange}
          /><br /><br />

          <label htmlFor="averageSaleAmount">Average Sale Amount</label><br />
          <input
            id="averageSaleAmount"
            name="averageSaleAmount"
            type="number"
            min="0"
            step="0.01"
            value={form.averageSaleAmount}
            onChange={handleChange}
          /><br /><br />

          <button type="submit" disabled={loading}>
            {loading ? 'Calculating…' : 'Calculate'}
          </button>
        </form>
      </header>

      <div style={{ marginTop: 24 }}>
        <h3>Results</h3>
        {error && <p style={{ color: 'salmon' }}>{error}</p>}
        {result && (
          <>
            <p>Total FCamara commission: {fmt.format(result.fCamaraCommissionAmount)}</p>
            <p>Total Competitor commission: {fmt.format(result.competitorCommissionAmount)}</p>
          </>
        )}
      </div>
    </div>
  );
}
