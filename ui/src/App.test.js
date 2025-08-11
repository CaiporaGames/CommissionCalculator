import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import App from './App';

beforeEach(() => {
  global.fetch = jest.fn();
});

afterEach(() => {
  jest.resetAllMocks();
});

test('renders form and calculates on submit', async () => {

  // Mock API success
  global.fetch.mockResolvedValueOnce({
    ok: true,
    json: async () => ({
      fCamaraCommissionAmount: 550.0,
      competitorCommissionAmount: 95.5
    })
  });

  render(<App />);

  await userEvent.type(screen.getByLabelText(/Local Sales Count/i), '10');
  await userEvent.type(screen.getByLabelText(/Foreign Sales Count/i), '10');
  await userEvent.type(screen.getByLabelText(/Average Sale Amount/i), '100');

  await userEvent.click(screen.getByRole('button', { name: /calculate/i }));

  // We don’t assert exact symbol/spacing—number + two decimals is enough
  await waitFor(() => {
    expect(screen.getByText(/Total FCamara commission:/i)).toHaveTextContent('£550.00');
    expect(screen.getByText(/Total Competitor commission:/i)).toHaveTextContent('£95.50');
  });

  // Verify the request payload
  expect(global.fetch).toHaveBeenCalledTimes(1);
  const [url, opts] = global.fetch.mock.calls[0];
  expect(url).toMatch(/\/commission$/);
  expect(opts.method).toBe('POST');
  expect(JSON.parse(opts.body)).toEqual({
    localSalesCount: 10,
    foreignSalesCount: 10,
    averageSaleAmount: 100
  });
});

test('shows error when API fails', async () => {
  global.fetch.mockResolvedValueOnce({
    ok: false,
    status: 500,
    text: async () => 'Boom'
  });

  render(<App />);

   await userEvent.click(screen.getByRole('button', { name: /calculate/i }));

  await waitFor(() => {
    expect(screen.getByRole('alert')).toHaveTextContent(/API error 500: Boom/i);
  });
});

test('blocks negative inputs on client', async () => {
  render(<App />);

  await userEvent.type(screen.getByLabelText(/Local Sales Count/i), '-1');
  await userEvent.click(screen.getByRole('button', { name: /calculate/i }));

  await waitFor(() => {
    expect(screen.getByText(/non-negative/i)).toBeInTheDocument();
  });
});
