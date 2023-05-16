
import React from "react";
import { FC, useCallback, useEffect, useState } from "react";
import Table from "react-bootstrap/Table";
import axios from "axios";
import { Filter } from "../components/Filter";

interface SCBResponse {
  success: Boolean;
  value?: BornStatistic[];
  errorMessage?: string;
}

interface BornStatistic {
  id: string;
  fetchedAt: Date;
  regionCode: string;
  regionName: string;
  year: string;
  gender: string;
  amount: string;
}
export interface StatisticProp {
  id: string;
  region: string;
  year: string;
  gender: string;
  amount: string;
}

export interface IFilter {
  region: string;
  year?: string;
  gender?: string;
}

const url: string = "https://localhost:7091/born-statistics";

export const Statistics: FC = () => {
  const [loading, setLoading] = useState<boolean>(true);
  const [statistics, setStatistics] = useState<StatisticProp[]>([]);
  const [filter, setFilter] = useState<IFilter>({
    region: "Sweden",
  });

  const fetchStatistics = useCallback(async () => {
    try{
        await axios
        .get<SCBResponse>(
          `${url}?Region=${filter.region}&Year=${filter.year}&Gender=${filter.gender}`
        )
        .then((response) => {
          console.log(response.data);
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
        });
    }
    catch(error){
        if(axios.isAxiosError(error)){
            console.error(error.response?.data);
        }
        else{
            console.error(error);
        }
    }
  }, []);

  useEffect(() => {
    fetchStatistics();
  }, []);

  useEffect(() => {
    fetchStatistics();
  }, [filter]);

  return (
    <>
    <Filter/>
      <Table striped bordered hover size="sm">
        <thead>
          <tr>
            <th>Region</th>
            <th>Year</th>
            <th>Gender</th>
            <th>Amount</th>
          </tr>
        </thead>
        <tbody>
          {statistics.map((statistic) => {
            return (
              <tr key={statistic.id}>
                <td>{statistic.region}</td>
                <td>{statistic.year}</td>
                <td>{statistic.gender}</td>
                <td>{statistic.amount}</td>
              </tr>
            );
          })}
        </tbody>
      </Table>
    </>
  );
};
