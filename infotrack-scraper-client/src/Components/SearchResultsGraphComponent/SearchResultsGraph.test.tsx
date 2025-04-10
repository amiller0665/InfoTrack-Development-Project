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

  test('renders loading state when fetching data', async () => {
    (fetchSearchResults as jest.Mock).mockImplementation(() => new Promise(() => {}));

    render(<SearchResultsGraph query="test query" url="https://example.com" />);

    expect(screen.getByText('Loading...')).toBeInTheDocument();
  });

  test('renders error message when fetch fails', async () => {
    (fetchSearchResults as jest.Mock).mockRejectedValue(new Error('Failed to fetch'));

    render(<SearchResultsGraph query="test query" url="https://example.com" />);

    await waitFor(() => {
      expect(screen.getByText('There was an error fetching the search results.')).toBeInTheDocument();
    });
  });

  test('renders no data message when no results are returned', async () => {
    (fetchSearchResults as jest.Mock).mockResolvedValue([]);

    render(<SearchResultsGraph query="test query" url="https://example.com" />);

    await waitFor(() => {
      expect(screen.getByText('No data available for the given query and URL.')).toBeInTheDocument();
    });
  });

  test('renders the chart when data is returned', async () => {
    (fetchSearchResults as jest.Mock).mockResolvedValue([
      { searchDate: '2025-04-10T12:34:56Z', positions: '1,2,3' },
      { searchDate: '2025-04-11T12:34:56Z', positions: '4,5' },
    ]);

    render(<SearchResultsGraph query="test query" url="https://example.com" />);

    await waitFor(() => {
      expect(screen.getByTestId('chart')).toBeInTheDocument();
    });
  });

  test('formats dates and counts positions correctly', async () => {
    (fetchSearchResults as jest.Mock).mockResolvedValue([
      { searchDate: '2025-04-10T12:34:56Z', positions: '1,2,3' },
      { searchDate: '2025-04-11T12:34:56Z', positions: '4,5' },
    ]);

    render(<SearchResultsGraph query="test query" url="https://example.com" />);

    await waitFor(() => {
      expect(Line).toHaveBeenCalledWith(
        expect.objectContaining({
          data: expect.objectContaining({
            labels: ['10/04/2025', '11/04/2025'],
            datasets: [
              expect.objectContaining({
                data: [3, 2]
              }),
            ],
          }),
        }),
        {}
      );
    });
  });
});