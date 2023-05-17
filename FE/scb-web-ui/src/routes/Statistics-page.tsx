import { FC } from "react";
import "./Statistics-page.style.scss"
import { Filter } from "../components/Filter";
import { GlobalContextProvider } from "../hooks/useGlobalContext";
import { List } from "../components/List";
import { ErrorModal } from "../components/Error-modal";

export const StatisticsPage: FC = () => {
  return (
    <GlobalContextProvider>
      <Filter />
      <List/>
      <ErrorModal/>
    </GlobalContextProvider>
  );
};
