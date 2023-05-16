import React from "react";
import "./Info.style.scss";

const InfoPage: React.FC = () => {
  return (
    <div className="info-container">
      <div className="header-wrapper">
        <h2>
          This is a web-application that lets you see statistics of people born
          in sweden
        </h2>
      </div>
      <div className="header-wrapper">
        <h3>Please head to "Statistics" in the top navigation to try it out</h3>
      </div>
    </div>
  );
};

export default InfoPage;
