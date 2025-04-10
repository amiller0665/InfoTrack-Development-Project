import React, { useState } from 'react';
import './ScraperInput.css';
import { fetchGoogleAppearances } from '../FetchComponents/fetchGoogleAppearances';
import SearchResultsGraph from '../SearchResultsGraphComponent/SearchResultsGraph';

const ScraperInput = () => {
  const [query, setQuery] = useState('');
  const [url, setUrl] = useState('');
  const [results, setResults] = useState<string[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [hasSearched, setHasSearched] = useState(false);
  const [showGraph, setShowGraph] = useState(false);

  const handleQueryChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setQuery(event.target.value);
  };

  const handleUrlChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setUrl(event.target.value);
  };

  const handleSearch = async () => {
    if (!query || !url) return;

    setLoading(true);
    setResults([]);
    setError(null);

    try {
      const data = await fetchGoogleAppearances(query, url);
      setResults(data);
      setHasSearched(true);
    } catch (err) {
      console.error('Error fetching data:', err);
      setError('There was an error with the request.');
    } finally {
      setLoading(false);
    }
  };

  const handleShowGraph = () => {
    if (!query || !url) {
      setError('Please enter both a search phrase and a matching URL.');
      return;
    }
    setShowGraph(true);
  };

  const handleHideGraph = () => {
    setShowGraph(false);
  };

  return (
    <div className="search-phrase-input-container">
      <input
        type="text"
        value={query}
        onChange={handleQueryChange}
        placeholder="Enter your search phrase"
        className="input-field"
      />
      <input
        type="text"
        value={url}
        onChange={handleUrlChange}
        placeholder="Enter the matching URL"
        className="input-field"
      />
      <div className="button-container">
        <button onClick={handleSearch} className="search-button">Search</button>
        {showGraph ? (
          <button onClick={handleHideGraph} className="search-button">Hide Graph</button>
        ) : (
          <button onClick={handleShowGraph} className="search-button">Show Graph</button>
        )}
      </div>
      {loading && (
        <div className="loading-container">
          <div className="throbber"></div>
          <span className="loading-text">Loading...</span>
        </div>
      )}
      {error && (
        <div>
          <p className="error-text">{error}</p>
        </div>
      )}
      {!loading && !error && results.length > 0 && (
        <div>
          <p>The requested URL appeared at indexes:</p>
          <p className="result-text">{results.join(', ')}</p>
        </div>
      )}
      {!loading && !error && hasSearched && results.length === 0 && (
        <div>
          <p className="result-text">
            The requested URL did not appear in any of the top 100 searches for that phrase.
          </p>
        </div>
      )}
      {showGraph && <SearchResultsGraph query={query} url={url} />}
    </div>
  );
};

export default ScraperInput;