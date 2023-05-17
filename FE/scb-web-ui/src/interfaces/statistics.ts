import { ReactNode } from "react";

export interface SCBResponse {
  success: Boolean;
  errorMessage?: string;
}

export interface BornStatisticsResponse extends SCBResponse{
  value?: BornStatistic[];
}

export interface RegionsResponse extends SCBResponse{
  value?: string[];
}

export interface BornStatistic {
  id: string;
  fetchedAt: Date;
  regionCode: string;
  regionName: string;
  year: string;
  gender: string;
  amount: string;
}

export interface IFilter {
  region: string;
  year?: string;
  gender?: string;
}

export interface GlobalContext {
  filter: IFilter;
  statistics: StatisticProp[];
  loading: boolean;
  apiError: string;
  regions: string[];
  setFilter: (filter: IFilter) => void;
}

export interface GlobalContextProviderProps {
  children: ReactNode;
}

export interface StatisticProp {
  id: string;
  region: string;
  year: string;
  gender: string;
  amount: string;
}