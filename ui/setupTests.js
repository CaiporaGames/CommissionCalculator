import '@testing-library/jest-dom';

const RealNumberFormat = Intl.NumberFormat;

beforeAll(() => {
  // Make currency formatting deterministic in Node/Jest
  global.Intl.NumberFormat = jest.fn(() => ({
    format: (n) => `£${Number(n).toFixed(2)}`
  }));
});

afterAll(() => {
  global.Intl.NumberFormat = RealNumberFormat;
});

// Default: don't hit real network in RTL tests
beforeEach(() => {
  global.fetch = jest.fn();
});

afterEach(() => {
  jest.resetAllMocks();
});
