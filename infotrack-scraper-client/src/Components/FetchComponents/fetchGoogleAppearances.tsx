import config from '../../config';

export const fetchGoogleAppearances = async (searchPhrase: string, url: string): Promise<string[]> => {
  if (!searchPhrase || !url) {
    throw new Error('Search phrase and URL are required');
  }

  const formattedQuery = searchPhrase.trim().toLowerCase().replace(/\s+/g, '-');
  const encodedUrl = encodeURIComponent(url.trim());

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

  const data: string[] = await response.json();
  return data;
};