import config from '../../config';

export const fetchGoogleAppearances = async (searchPhrase: string, url: string): Promise<number[]> => {
  if (!searchPhrase || !url) {
    throw new Error('Search phrase and URL are required');
  }

  const formattedQuery = searchPhrase.trim().toLowerCase().replace(/\s+/g, '-'); // Replace spaces with hyphens
  const encodedUrl = encodeURIComponent(url.trim()); // Encode the URL to make it safe for query parameters

  const response = await fetch(
    `${config.apiBaseUrl}/api/GoogleAppearances/GetCurrentAppearances/${formattedQuery}/${encodedUrl}`,
    {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
      mode: 'cors'
    }
  );

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const data: number[] = await response.json();
  return data;
};