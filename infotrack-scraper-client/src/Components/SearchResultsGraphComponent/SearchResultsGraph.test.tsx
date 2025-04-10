import { render, screen, waitFor } from '@testing-library/react';
import { Line } from 'react-chartjs-2';
import { fetchSearchResults } from '../FetchComponents/fetchSearchResults';
import SearchResultsGraph from './SearchResultsGraph';

jest.mock('../FetchComponents/fetchSearchResults', () => ({
  fetchSearchResults: jest.fn(),
}));

jest.mock('react-chartjs-2', () => ({
  Line: jest.fn(() => <div data-testid="chart">Mocked Chart</div>),
}));

describe('SearchResultsGraph Component', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  test('displays loading state while fetching data', async () => {
    (fetchSearchResults as jest.Mock).mockImplementation(() => new Promise(() => {})); // Simulate loading

    render(<SearchResultsGraph query="test-query" url="https://example.com" />);

    expect(screen.getByText('Loading...')).toBeInTheDocument();
  });

  test('displays error message when fetch fails', async () => {
    (fetchSearchResults as jest.Mock).mockRejectedValue(new Error('Failed to fetch'));

    render(<SearchResultsGraph query="test-query" url="https://example.com" />);

    await waitFor(() => {
      expect(screen.getByText('There was an error fetching the search results.')).toBeInTheDocument();
    });
  });

  test('displays no data message when no results are returned', async () => {
    (fetchSearchResults as jest.Mock).mockResolvedValue([]); // Mock empty array response

    render(<SearchResultsGraph query="test-query" url="https://example.com" />);

    await waitFor(() => {
      expect(screen.getByText('No data available for the given query and URL.')).toBeInTheDocument();
    });
  });

  test('does not render the chart when no data is returned', async () => {
    (fetchSearchResults as jest.Mock).mockResolvedValue([]); // Mock empty array response

    render(<SearchResultsGraph query="test-query" url="https://example.com" />);

    await waitFor(() => {
      expect(screen.queryByTestId('chart')).not.toBeInTheDocument();
    });
  });
});