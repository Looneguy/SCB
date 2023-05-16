import React from "react";
import { FC } from "react";
import "./Filter.style.scss";
import { IFilter } from "../routes/Statistics";
import { Field, Form, Formik, FormikHelpers, FormikProvider, useFormik } from "formik";

export const Filter: FC = () => {

    
    const add2 = (values: any) => console.log(values)

    const formik = useFormik({
        initialValues: {
            region: '',
            year: '',
            gender: '',
        },
        onSubmit: add2
    })

  return (
    <div>
      <h1>Filter</h1>
      <FormikProvider value={formik}>
        <Field name="region"></Field>
        <Field name="year"></Field>
        <Field name="gender"></Field>
        <button type="submit" onClick={() => formik.handleSubmit()}>Add filter</button>
      </FormikProvider>
    </div>
  );
};
