import { render, screen } from '@testing-library/react';
import App from './App';

test('renders the header with the InfoTrack logo and title', () => {
  render(<App />);
  
  // Check for the InfoTrack logo
  const logoElement = screen.getByAltText('InfoTrack Logo');
  expect(logoElement).toBeInTheDocument();

  // Check for the title
  const titleElement = screen.getByText(/Welcome to the InfoTrack Google Appearances App/i);
  expect(titleElement).toBeInTheDocument();
});

test('renders the search tool description', () => {
  render(<App />);

  // Check for the description text
  const descriptionElement = screen.getByText(/Search for Google appearances using the search tool below./i);
  expect(descriptionElement).toBeInTheDocument();
});

test('renders the ScraperInput component', () => {
  render(<App />);
  
  // Check for the search phrase input field
  const searchPhraseInput = screen.getByPlaceholderText('Enter your search phrase');
  expect(searchPhraseInput).toBeInTheDocument();

  // Check for the URL input field
  const urlInput = screen.getByPlaceholderText('Enter the matching URL');
  expect(urlInput).toBeInTheDocument();

  // Check for the Search button
  const searchButton = screen.getByText('Search');
  expect(searchButton).toBeInTheDocument();

  // Check for the Show Graph button
  const showGraphButton = screen.getByText('Show Graph');
  expect(showGraphButton).toBeInTheDocument();
});
