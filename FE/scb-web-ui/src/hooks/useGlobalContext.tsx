import { createContext, useContext } from "react";

export interface IFilter {
  region: string;
  year?: string;
  gender?: string;
}

export interface GlobalFilter {
  filter: IFilter;
  setFilter: (filter: IFilter) => void;
}

export const MyGlobalContext = createContext<GlobalFilter>({
  filter: { region: "", year: "", gender: "" },
  setFilter: () => {},
});

export const useGlobalContext = () => useContext(MyGlobalContext);
