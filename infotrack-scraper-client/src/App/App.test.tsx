import { render, screen } from '@testing-library/react';
import App from './App';

test('renders the header with the InfoTrack logo and title', () => {
  render(<App />);
  
  const logoElement = screen.getByAltText('InfoTrack Logo');
  expect(logoElement).toBeInTheDocument();

  const titleElement = screen.getByText(/Welcome to the InfoTrack Google Appearances App/i);
  expect(titleElement).toBeInTheDocument();
});

test('renders the search tool description', () => {
  render(<App />);

  const descriptionElement = screen.getByText(/Search for Google appearances using the search tool below./i);
  expect(descriptionElement).toBeInTheDocument();
});

test('renders the ScraperInput component', () => {
  render(<App />);
  
  const searchPhraseInput = screen.getByPlaceholderText('Enter your search phrase');
  const urlInput = screen.getByPlaceholderText('Enter the matching URL');
  expect(searchPhraseInput).toBeInTheDocument();
  expect(urlInput).toBeInTheDocument();
});
