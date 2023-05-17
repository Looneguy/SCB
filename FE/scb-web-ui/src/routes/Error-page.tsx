import React from "react";
import { useRouteError, isRouteErrorResponse } from "react-router-dom";
import "./Error-page.style.scss"

const ErrorPage: React.FC = () => {
  const error = useRouteError();

  console.error(error);

  return (
    <div id="error-page">
      <h1>Oops!</h1>
      <p>Sorry, an unexpected error has occurred.</p>
      <p>
        <i>
          {isRouteErrorResponse(error)
            ? error.error?.message || error.statusText
            : "Unknown error message"}
        </i>
      </p>
    </div>
  );
};

export default ErrorPage
