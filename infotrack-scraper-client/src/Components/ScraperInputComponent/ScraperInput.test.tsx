import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import ScraperInput from './ScraperInput';
import { fetchGoogleAppearances } from '../FetchComponents/fetchGoogleAppearances';

jest.mock('../FetchGoogleAppearancesComponent/fetchGoogleAppearances', () => ({
  fetchGoogleAppearances: jest.fn(),
}));

describe('ScraperInput Component', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  test('renders input fields and search button', () => {
    render(<ScraperInput />);

    expect(screen.getByPlaceholderText('Enter your search phrase')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Enter the matching URL')).toBeInTheDocument();
    expect(screen.getByText('Search')).toBeInTheDocument();
  });

  test('displays loading state when search is initiated', async () => {
    (fetchGoogleAppearances as jest.Mock).mockResolvedValueOnce([]);

    render(<ScraperInput />);

    fireEvent.change(screen.getByPlaceholderText('Enter your search phrase'), {
      target: { value: 'test query' },
    });
    fireEvent.change(screen.getByPlaceholderText('Enter the matching URL'), {
      target: { value: 'https://example.com' },
    });
    fireEvent.click(screen.getByText('Search'));

    expect(screen.getByText('Loading...')).toBeInTheDocument();

    await waitFor(() => {
      expect(screen.queryByText('Loading...')).not.toBeInTheDocument();
    });
  });

  test('displays results when search is successful', async () => {
    (fetchGoogleAppearances as jest.Mock).mockResolvedValueOnce([1, 2, 3]);

    render(<ScraperInput />);

    fireEvent.change(screen.getByPlaceholderText('Enter your search phrase'), {
      target: { value: 'test query' },
    });
    fireEvent.change(screen.getByPlaceholderText('Enter the matching URL'), {
      target: { value: 'https://example.com' },
    });
    fireEvent.click(screen.getByText('Search'));

    await waitFor(() => {
      expect(screen.getByText('The requested URL appeared at indexes:')).toBeInTheDocument();
      expect(screen.getByText('1, 2, 3')).toBeInTheDocument();
    });
  });

  test('displays error message when search fails', async () => {
    (fetchGoogleAppearances as jest.Mock).mockRejectedValueOnce(new Error('Network error'));

    render(<ScraperInput />);

    fireEvent.change(screen.getByPlaceholderText('Enter your search phrase'), {
      target: { value: 'test query' },
    });
    fireEvent.change(screen.getByPlaceholderText('Enter the matching URL'), {
      target: { value: 'https://example.com' },
    });
    fireEvent.click(screen.getByText('Search'));

    await waitFor(() => {
      expect(screen.getByText('There was an error with the request.')).toBeInTheDocument();
    });
  });

  test('displays no results message when search returns an empty array', async () => {
    (fetchGoogleAppearances as jest.Mock).mockResolvedValueOnce([]);

    render(<ScraperInput />);

    fireEvent.change(screen.getByPlaceholderText('Enter your search phrase'), {
      target: { value: 'test query' },
    });
    fireEvent.change(screen.getByPlaceholderText('Enter the matching URL'), {
      target: { value: 'https://example.com' },
    });
    fireEvent.click(screen.getByText('Search'));

    await waitFor(() => {
      expect(
        screen.getByText(
          'The requested URL did not appear in any of the top 100 searches for that phrase.'
        )
      ).toBeInTheDocument();
    });
  });
});