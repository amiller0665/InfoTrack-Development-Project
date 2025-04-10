import React, { useEffect, useState } from 'react';
import { Line } from 'react-chartjs-2';
import { Chart as ChartJS, CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend } from 'chart.js';
import { fetchSearchResults } from '../FetchComponents/fetchSearchResults';
import './SearchResultsGraph.css';

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

interface SearchResultsGraphProps {
  query: string;
  url: string;
}

const SearchResultsGraph: React.FC<SearchResultsGraphProps> = ({ query, url }) => {
  const [graphData, setGraphData] = useState<number[]>([]);
  const [labels, setLabels] = useState<string[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchGraphData = async () => {
      if (!query || !url) return;

      setLoading(true);
      setError(null);

      try {
        const data = await fetchSearchResults(query, url);

        // Extract dates and count the number of positions
        const dates = data.map((result) =>
          new Date(result.searchDate).toLocaleDateString('en-GB')
        );
        const indexes = data.map((result) =>
          result.positions.split(',').length
        );

        setLabels(dates);
        setGraphData(indexes);
      } catch (err) {
        console.error('Error fetching data:', err);
        setError('There was an error fetching the search results.');
      } finally {
        setLoading(false);
      }
    };

    fetchGraphData();
  }, [query, url]);

  const chartData = {
    labels: labels,
    datasets: [
      {
        label: 'Count of Search Result Positions',
        data: graphData,
        borderColor: 'rgba(6, 150, 183, 0.7)',
        backgroundColor: 'rgba(6, 150, 183, 0.2)',
        tension: 0.4,
      },
    ],
  };

  const chartOptions = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top' as const,
      },
      title: {
        display: true,
        text: 'Search Results Over the Last 7 Days',
      },
    },
    scales: {
      y: {
        beginAtZero: true,
      },
    },
  };

  return (
    <div className="search-results-graph-container">
      {loading && <p>Loading...</p>}
      {error && <p className="error-text">{error}</p>}
      {!loading && !error && graphData.length > 0 && (
        <div className="chart-container">
          <Line data={chartData} options={chartOptions} />
        </div>
      )}
      {!loading && !error && graphData.length === 0 && (
        <p>No data available for the given query and URL.</p>
      )}
    </div>
  );
};

export default SearchResultsGraph;