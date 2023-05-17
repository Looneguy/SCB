import { FC } from "react";
import { Table } from "react-bootstrap";
import { BeatLoader } from "react-spinners";
import { useGlobalContext } from "../hooks/useGlobalContext";

export const List: FC = () => {
  const { statistics, loading } = useGlobalContext();

  return (
    <>
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
          {loading ? (
            <div></div>
          ) : (
            statistics.map((statistic) => {
              return (
                <tr key={statistic.id}>
                  <td>{statistic.region}</td>
                  <td>{statistic.year}</td>
                  <td>{statistic.gender}</td>
                  <td>{statistic.amount}</td>
                </tr>
              );
            })
          )}
        </tbody>
      </Table>
      {loading ? (
        <div className="loader-wrapper">
          <BeatLoader />
        </div>
      ) : (
        <></>
      )}
    </>
  );
};
