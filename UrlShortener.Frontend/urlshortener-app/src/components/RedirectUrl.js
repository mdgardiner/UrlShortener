import React, { useState } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";

function RedirectUrl() {
  const { shortCode } = useParams();

  const getLongUrl = async () => {
    try {
      console.log(shortCode);

      const response = await axios.post(
        process.env.REACT_APP_SHORTENER_API_BASE_URL + "/api/expand",
        { ShortCode: shortCode }
      );

      console.log(response);
      window.location.href = response.data;
    } catch (error) {
      console.log(`Error getting long url: ${error}`);
    }
  };

  useState(() => {
    getLongUrl();
  }, []);

  return <p>Redirecting...</p>;
}

export default RedirectUrl;
