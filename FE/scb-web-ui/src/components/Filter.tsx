import React from "react";
import { FC } from "react";
import "./Filter.style.scss";
import { IFilter } from "../hooks/useGlobalContext";
import { Field, FormikProvider, useFormik } from "formik";
import { useGlobalContext } from "../hooks/useGlobalContext";

export const Filter: FC = () => {
  const { filter, setFilter } = useGlobalContext();

//   const filterValue: IFilter = {
//     region: filter.region,
//     year: filter.year,
//     gender: filter.gender,
//   };

  const updateFilter = (values: IFilter) => {
    setFilter(values);
  };

  const formik = useFormik({
    initialValues: {region: filter.region, year: filter.year, gender: filter.gender},
    onSubmit: updateFilter,
  });

  return (
    <div>
      <h2>Filter</h2>
      <FormikProvider value={formik}>
        <Field name="region" placeholder="Region"></Field>
        <Field name="year" placeholder="Year"></Field>
        <Field name="gender" placeholder="Gender"></Field>
        <button type="submit" onClick={() => formik.handleSubmit()}>
          Apply filter
        </button>
      </FormikProvider>
    </div>
  );
};
