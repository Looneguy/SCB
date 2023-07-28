import axios from "axios";
import {
  FC,
  createContext,
  useCallback,
  useContext,
  useEffect,
  useState,
} from "react";
import {
  BornStatisticsResponse,
  GlobalContext,
  GlobalContextProviderProps,
  IFilter,
  RegionsResponse,
  StatisticProp,
} from "../interfaces/statistics";
import { BASEURL, BORNSTATISTICS, REGIONS } from "../constants/statistics";

const MyGlobalContext = createContext<GlobalContext | null>(null);

const baseUrl = BASEURL;
const bornStatisticsPartial = BORNSTATISTICS;
const regionsPartial = REGIONS;

export const useGlobalContext = () => {
  const ctx = useContext(MyGlobalContext);
  if (ctx === null) {
    throw new Error("Hook is not being used inside a provider");
  }
  return ctx;
};

export const GlobalContextProvider: FC<GlobalContextProviderProps> = ({
  children,
}) => {
  const [loading, setLoading] = useState<boolean>(true);
  const [filter, setFilter] = useState<IFilter>({
    region: "Sweden",
    year: "",
    gender: "",
  });
  const [regions, setRegions] = useState<string[]>([]);
  const [apiError, setApiError] = useState<string>("");
  const [statistics, setStatistics] = useState<StatisticProp[]>([]);

  const fetchStatistics = useCallback(async () => {
    setLoading(true);
    try {
      const response = await axios.get<BornStatisticsResponse>(
        `${baseUrl}${bornStatisticsPartial}?Region=${filter.region}&Year=${filter.year}&Gender=${filter.gender}`
      );

      setLoading(false);

      const data = response.data.value;
      const success = response.data.success;
      const errorMessage = response.data.errorMessage;

      if (success) {
        setStatistics([]);
        let newStatistics: StatisticProp[] = [];

        newStatistics = data
          ? data.map((val) => ({
              id: val.id,
              region: val.regionName,
              year: val.year,
              gender: val.gender,
              amount: val.amount,
            }))
          : [];

        setStatistics(newStatistics);
      }
    } catch (error) {
      setLoading(false);
      if (axios.isAxiosError(error)) {
        console.error(error.response?.data.errorMessage);
        setApiError(error.response?.data.errorMessage);
      } else {
        console.error(error);
      }
    }
  }, [filter]);

  useEffect(() => {
    setFilter(filter);
    fetchStatistics();
  }, [filter.region, filter.gender, filter.year]);

  useEffect(() => {
    const fetchRegions = async () => {
      try {
        const response = await axios.get<RegionsResponse>(
          `${baseUrl}${regionsPartial}`
        );
        const data = response.data.value;
        const success = response.data.success;
        const errorMessage = response.data.errorMessage;

        if (success) {
          if (data) {
            setRegions(data);
          }
        }
      } catch (error) {
        if (axios.isAxiosError(error)) {
          console.error(error.response?.data);
          setApiError(error.response?.data.errorMessage);
        } else {
          console.error(error);
        }
      }
    };

    fetchRegions();
  }, []);

  return (
    <MyGlobalContext.Provider
      value={{ filter, statistics, loading, apiError, regions, setFilter }}
    >
      {children}
    </MyGlobalContext.Provider>
  );
};
