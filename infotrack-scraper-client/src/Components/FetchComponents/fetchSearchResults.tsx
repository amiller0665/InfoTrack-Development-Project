import config from '../../config';

export interface SearchResults {
  id: number;
  searchQuery: string;
  url: string;
  positions: string;
  searchDate: string;
}

export const fetchSearchResults = async (searchPhrase: string, url: string): Promise<SearchResults[]> => {
  if (!searchPhrase || !url) {
    throw new Error('Search query and URL are required');
  }

  const formattedQuery = searchPhrase.trim().toLowerCase().replace(/\s+/g, '-');
  const encodedUrl = encodeURIComponent(url.trim());

  const response = await fetch(
    `${config.apiBaseUrl}/api/SearchResults/query-url/${formattedQuery}/${encodedUrl}`,
    {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
      mode: 'cors',
    }
  );

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const data: SearchResults[] = await response.json();
  return data;
};