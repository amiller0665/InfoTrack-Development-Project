import ScraperInput from '../Components/ScraperInputComponent/ScraperInput';
import './App.css';
import infoTrackLogo from '../Assets/InfoTrackLogo.png';

function App() {
  return (
    <div className="app-container">
      <div className="header-container">
        <img src={infoTrackLogo} alt="InfoTrack Logo" className="infoTrack-logo" />
        <h1>Welcome to the InfoTrack Google Appearances App</h1>
      </div>
      <p className="centered-text">Search for Google appearances using the search tool below.</p>
      <ScraperInput />
    </div>
  );
}

export default App;