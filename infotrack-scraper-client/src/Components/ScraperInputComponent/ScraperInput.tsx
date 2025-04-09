import React, { useState } from 'react';
import './ScraperInput.css';
import { fetchGoogleAppearances } from '../FetchGoogleAppearancesComponent/fetchGoogleAppearances';

const ScraperInput = () => {
  const [query, setQuery] = useState('');
  const [url, setUrl] = useState('');
  const [results, setResults] = useState<number[]>([]);
  const [loading, setLoading] = useState(false);

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

    try {
      const data = await fetchGoogleAppearances(query, url);
      setResults(data);
    } catch (error) {
      console.error('Error fetching data:', error);
    } finally {
      setLoading(false);
    }
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
      <button onClick={handleSearch}>Search</button>
      {loading && (
        <div className="loading-container">
          <div className="throbber"></div>
          <span className="loading-text">Loading...</span>
        </div>
      )}
      <ul className="result-list">
        {results.map((result, index) => (
          <li key={index} className="result-item">{result}</li>
        ))}
      </ul>
    </div>
  );
};

export default ScraperInput;