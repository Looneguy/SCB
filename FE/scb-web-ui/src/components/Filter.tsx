import React from "react";
import { FC } from "react";
import "./Filter.style.scss";
import { Field, FormikProvider, useFormik } from "formik";
import { useGlobalContext } from "../hooks/useGlobalContext";
import Button from "react-bootstrap/esm/Button";
import { IFilter } from "../interfaces/statistics";

export const Filter: FC = () => {
  const { setFilter, regions } = useGlobalContext();

  const filterValue: IFilter = {
    region: "Sweden",
    year: "",
    gender: "",
  };

  const updateFilter = (values: IFilter) => {
    setFilter(values);
  };

  const formik = useFormik({
    enableReinitialize: true,
    initialValues: filterValue,
    onSubmit: updateFilter,
  });

  return (
    <div className="filter-container">
      <h2>SCB Filter</h2>
      <FormikProvider value={formik}>
        <div className="fields-container">
          <Field
            as="select"
            name="region"
            className="form-control form-control-sm field"
            placeholder="Region"
          >
            {regions ? (
              regions.map((region) => {
                return (
                  <option key={region} value={region}>
                    {region}
                  </option>
                );
              })
            ) : (
              <></>
            )}
          </Field>
          <Field
            name="year"
            className="form-control form-control-sm field"
            placeholder="Year (2016-2020)"
          ></Field>
          <Field
            as="select"
            name="gender"
            className="form-control form-control-sm field"
            placeholder="Gender (Women, Men)"
          >
            <option value="">No gender</option>
            <option value="Women">Women</option>
            <option value="Men">Men</option>
          </Field>
        </div>
        <Button
          variant="secondary"
          type="submit"
          onClick={() => formik.handleSubmit()}
        >
          Apply filter
        </Button>
      </FormikProvider>
    </div>
  );
};
