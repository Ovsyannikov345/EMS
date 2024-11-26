import { Container, TextField, Button, MenuItem, Box, CircularProgress, Grid2 as Grid, Typography } from "@mui/material";
import { useFormik } from "formik";
import { EstateToUpdateData, EstateType } from "../../hooks/useCatalogueApi";
import estateValidationSchema from "../../utils/validation/estateValidationSchema";

interface EditEstateFormProps {
    initialValues: EstateToUpdateData;
    onSubmit: (values: EstateToUpdateData) => Promise<void>;
    onCancel: () => void;
}

const EditEstateForm = ({ initialValues, onSubmit, onCancel }: EditEstateFormProps) => {
    const formik = useFormik({
        initialValues: {
            type: initialValues.type,
            address: initialValues.address,
            area: initialValues.area,
            roomsCount: initialValues.roomsCount,
            price: initialValues.price,
        },
        validationSchema: estateValidationSchema,
        onSubmit: async (values, { setSubmitting }) => {
            await onSubmit({ ...initialValues, ...values });
            setSubmitting(false);
        },
    });

    return (
        <Container maxWidth="sm">
            <Box component="form" onSubmit={formik.handleSubmit} noValidate sx={{ mt: 3 }}>
                <Typography variant="h5" mb={1}>
                    Edit estate info
                </Typography>
                <TextField
                    fullWidth
                    select
                    id="type"
                    name="type"
                    label="Type"
                    value={formik.values.type}
                    onChange={formik.handleChange}
                    error={formik.touched.type && Boolean(formik.errors.type)}
                    helperText={formik.touched.type && formik.errors.type}
                    margin="normal"
                >
                    {Object.values(EstateType)
                        .filter((value) => typeof value === "number")
                        .map((value) => (
                            <MenuItem key={value} value={value}>
                                {EstateType[value as EstateType]}
                            </MenuItem>
                        ))}
                </TextField>
                <TextField
                    fullWidth
                    id="address"
                    name="address"
                    label="Address"
                    value={formik.values.address}
                    onChange={formik.handleChange}
                    error={formik.touched.address && Boolean(formik.errors.address)}
                    helperText={formik.touched.address && formik.errors.address}
                    margin="normal"
                />
                <TextField
                    fullWidth
                    id="area"
                    name="area"
                    label="Area (m²)"
                    type="number"
                    value={formik.values.area}
                    onChange={formik.handleChange}
                    error={formik.touched.area && Boolean(formik.errors.area)}
                    helperText={formik.touched.area && formik.errors.area}
                    margin="normal"
                />
                <TextField
                    fullWidth
                    id="roomsCount"
                    name="roomsCount"
                    label="Rooms Count"
                    type="number"
                    value={formik.values.roomsCount}
                    onChange={formik.handleChange}
                    error={formik.touched.roomsCount && Boolean(formik.errors.roomsCount)}
                    helperText={formik.touched.roomsCount && formik.errors.roomsCount}
                    margin="normal"
                />
                <TextField
                    fullWidth
                    id="price"
                    name="price"
                    label="Price"
                    type="number"
                    value={formik.values.price}
                    onChange={formik.handleChange}
                    error={formik.touched.price && Boolean(formik.errors.price)}
                    helperText={formik.touched.price && formik.errors.price}
                    margin="normal"
                />
                <Grid container justifyContent={"flex-end"} spacing={2} mt={1}>
                    <Button
                        variant="contained"
                        color="primary"
                        sx={{ width: "115px" }}
                        type="submit"
                        disabled={formik.isSubmitting}
                        startIcon={formik.isSubmitting && <CircularProgress size={20} />}
                    >
                        {formik.isSubmitting ? "Saving..." : "Save"}
                    </Button>
                    <Button variant="outlined" sx={{ width: "115px" }} onClick={onCancel} disabled={formik.isSubmitting}>
                        Cancel
                    </Button>
                </Grid>
            </Box>
        </Container>
    );
};

export default EditEstateForm;
