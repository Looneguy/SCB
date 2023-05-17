import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import "./index.scss";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import ErrorPage from "./routes/Error-page";
import { StatisticsPage } from "./routes/Statistics-page";
import InfoPage from "./routes/Info";

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    errorElement: <ErrorPage />,
    children: [
      {
        errorElement: <ErrorPage />,
        children: [
          { index: true, element: <InfoPage /> },
          {
            path: "statistics",
            element: <StatisticsPage/>,
          },
          {
            path: "/info",
            element: <InfoPage/>
          },
          {
            path: "/error",
            element: <ErrorPage/>
          }
        ]
      }
    ],
  },
]);

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);
