import React, { useState } from "react";
import axios from "axios";

function Home() {
  const [originalUrl, setOriginalUrl] = useState("");
  const [shortenedUrl, setShortenedUrl] = useState("");
  const [errorDisplay, setErrorDisplay] = useState("");

  const shortenUrl = async () => {
    try {
      const response = await axios.post(
        process.env.REACT_APP_SHORTENER_API_BASE_URL + "/api/shorten",
        { LongUrl: originalUrl }
      );
      setShortenedUrl(response.data);
    } catch (error) {
      setErrorDisplay(`An error occurred: ${error}`);
    }
  };

  return (
    <div>
      <h1>Url Shortener</h1>
      <div>
        <input
          type="text"
          placeholder="Enter URL to Shorten..."
          value={originalUrl}
          onChange={(e) => setOriginalUrl(e.target.value)}
        />
      </div>
      <div>
        <button onClick={shortenUrl}>Shorten URL</button>
      </div>
      <div>
        {shortenedUrl && (
          <a href={shortenedUrl} className="App-link">
            {shortenedUrl}
          </a>
        )}
      </div>
      <div>{errorDisplay && <p>{errorDisplay}</p>}</div>
    </div>
  );
}

export default Home;
