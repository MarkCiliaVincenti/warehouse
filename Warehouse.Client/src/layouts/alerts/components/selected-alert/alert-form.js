import { useState } from "react";
import { useFormik } from "formik";
import { useMutation } from "react-query";
import * as yup from "yup";
import Stack from "@mui/material/Stack";
import { Icon, TextField, Box, FormControlLabel } from "@mui/material";
import SuiAlert from "components/SuiAlert";
import SuiButton from "components/SuiButton";
import DeletePromt from "./delete-promt";
import { deleteAlert, saveAlert } from "services/warehouse-service";
import Checkbox from "@mui/material/Checkbox";
import SuiBox from "../../../../components/SuiBox";
import SuiTypography from "../../../../components/SuiTypography";

const validationSchema = yup.object({
  name: yup.string("Enter alert name"),
});

export default function AlertForm({ onSave = () => {}, onDelete = () => {}, item = {} }) {
  const mutation = useMutation((item) => saveAlert(item), {
    onSuccess: () => {
      formik.resetForm();
      return onSave();
    },
  });

  const handleDelete = async (item) => {
    try {
      await deleteAlert(item);
      return onDelete();
    } catch (err) {
      console.log("delete-item", err);
    }
  };

  const formik = useFormik({
    enableReinitialize: true,
    initialValues: {
      name: item.name ? item.name : "",
      checkPeriod: item ? item.checkPeriod : 0,
      enabled: Boolean(item && item.enabled),
    },
    validationSchema: validationSchema,
    onSubmit: (values) => {
      mutation.mutate(values);
    },
  });

  return (
    <Box
      component="form"
      onSubmit={formik.handleSubmit}
      sx={{
        "& .MuiTextField-root": { m: 1 },
      }}
      noValidate
      autoComplete="off"
    >
      {mutation.isError && (
        <SuiAlert style={{ fontSize: "12px" }} color={"error"} dismissible>
          {mutation.error.title || mutation.error.error || "Some error occurred!"}
        </SuiAlert>
      )}

      <TextField
        fullWidth
        id="name"
        name="name"
        label="Name"
        value={formik.values.name}
        onChange={formik.handleChange}
        error={formik.touched.name && Boolean(formik.errors.name)}
        helperText={formik.touched.name && formik.errors.name}
      />

      <TextField
        id="checkPeriod"
        name="checkPeriod"
        label="Check Period (sec)"
        type="number"
        value={formik.values.checkPeriod}
        onChange={formik.handleChange}
        error={formik.touched.checkPeriod && Boolean(formik.errors.checkPeriod)}
        helperText={formik.touched.checkPeriod && formik.errors.checkPeriod}
      />

      <SuiBox px={3} pt={2}>
        <FormControlLabel
          control={
            <Checkbox
              checked={formik.values.enabled}
              onChange={formik.handleChange}
              id="enabled"
              name="enabled"
              sx={{ "&.MuiCheckbox-root": { backgroundColor: "white !important" } }}
            />
          }
          label="Enabled"
        />
        <Stack direction="row" spacing={1} mt={2} alignItems="center">
          <FormControlLabel
            control={
              <Checkbox
                disabled
                id="email"
                name="email"
                sx={{ "&.MuiCheckbox-root": { backgroundColor: "white !important" } }}
              />
            }
            label="Email"
          />
          <FormControlLabel
            control={
              <Checkbox
                disabled
                id="sms"
                name="sms"
                sx={{ "&.MuiCheckbox-root": { backgroundColor: "white !important" } }}
              />
            }
            label="SMS"
          />
        </Stack>
      </SuiBox>

      <Stack my={2} py={2} direction="row" spacing={1} justifyContent="end">
        <DeletePromt
          renderButton={(handleClickOpen) => (
            <SuiButton variant="text" color="error" onClick={handleClickOpen} disabled={!item.id}>
              <Icon>delete</Icon>&nbsp;delete
            </SuiButton>
          )}
          onDelete={() => handleDelete(item)}
        />
        <SuiButton color="success" variant="contained" type="submit">
          {mutation.isLoading ? (
            "Loading..."
          ) : (
            <>
              <Icon>save</Icon>&nbsp;save
            </>
          )}
        </SuiButton>
      </Stack>
    </Box>
  );
}
