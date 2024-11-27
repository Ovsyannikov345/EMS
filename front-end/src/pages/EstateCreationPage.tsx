import { useFormik } from "formik";
import useCatalogueApi, { EstateToCreateData, EstateType } from "../hooks/useCatalogueApi";
import estateValidationSchema from "../utils/validation/estateValidationSchema";
import {
    Box,
    Button,
    CircularProgress,
    Container,
    FormControl,
    Grid2 as Grid,
    InputLabel,
    MenuItem,
    Select,
    SelectChangeEvent,
    TextField,
    Typography,
} from "@mui/material";
import { useNotifications } from "@toolpad/core";
import { create } from "domain";
import { useNavigate } from "react-router-dom";
import { ESTATE_DETAILS_ROUTE } from "../utils/consts";

const EstateCreationPage = () => {
    const navigate = useNavigate();

    const notifications = useNotifications();

    const { createEstate } = useCatalogueApi();

    const formik = useFormik<EstateToCreateData>({
        initialValues: {
            type: EstateType.None,
            address: "",
            area: 0,
            roomsCount: 0,
            price: 0,
        },
        validationSchema: estateValidationSchema,
        onSubmit: async (values, { setSubmitting }) => {
            const response = await createEstate(values);

            if ("error" in response) {
                notifications.show(response.message, { severity: "error", autoHideDuration: 3000 });
                setSubmitting(false);

                return;
            }

            navigate(ESTATE_DETAILS_ROUTE.replace(":id", response.id));
            setSubmitting(false);
        },
    });

    return (
        <Container maxWidth="sm" sx={{ mt: 3, pb: 5 }}>
            <Typography variant="h5" mb={4}>
                Create estate
            </Typography>
            <form onSubmit={formik.handleSubmit}>
                <Grid container spacing={2}>
                    <Grid size={12}>
                        <FormControl fullWidth error={formik.touched.type && Boolean(formik.errors.type)}>
                            <InputLabel id="type-label">Estate Type</InputLabel>
                            <Select
                                labelId="type-label"
                                id="type"
                                name="type"
                                value={formik.values.type}
                                onChange={(event: SelectChangeEvent<EstateType>) => {
                                    formik.setFieldValue("type", event.target.value as EstateType);
                                }}
                                onBlur={formik.handleBlur}
                                label="Estate Type"
                            >
                                {Object.values(EstateType)
                                    .filter((value) => typeof value === "number")
                                    .map((value) => (
                                        <MenuItem key={value} value={value}>
                                            {EstateType[value as EstateType]}
                                        </MenuItem>
                                    ))}
                            </Select>
                            {formik.touched.type && formik.errors.type && (
                                <Box component="span" sx={{ color: "error.main", fontSize: "0.875rem" }}>
                                    {formik.errors.type}
                                </Box>
                            )}
                        </FormControl>
                    </Grid>
                    <Grid size={12}>
                        <TextField
                            fullWidth
                            id="address"
                            name="address"
                            label="Address"
                            autoComplete="address"
                            value={formik.values.address}
                            onChange={formik.handleChange}
                            onBlur={formik.handleBlur}
                            error={formik.touched.address && Boolean(formik.errors.address)}
                            helperText={formik.touched.address && formik.errors.address}
                        />
                    </Grid>
                    <Grid size={12}>
                        <TextField
                            fullWidth
                            id="area"
                            name="area"
                            label="Area (m²)"
                            type="number"
                            value={formik.values.area ? formik.values.area : ""}
                            onChange={formik.handleChange}
                            onBlur={formik.handleBlur}
                            error={formik.touched.area && Boolean(formik.errors.area)}
                            helperText={formik.touched.area && formik.errors.area}
                        />
                    </Grid>
                    <Grid size={12}>
                        <TextField
                            fullWidth
                            id="roomsCount"
                            name="roomsCount"
                            label="Rooms Count"
                            type="number"
                            value={formik.values.roomsCount ? formik.values.roomsCount : ""}
                            onChange={formik.handleChange}
                            onBlur={formik.handleBlur}
                            error={formik.touched.roomsCount && Boolean(formik.errors.roomsCount)}
                            helperText={formik.touched.roomsCount && formik.errors.roomsCount}
                        />
                    </Grid>
                    <Grid size={12}>
                        <TextField
                            fullWidth
                            id="price"
                            name="price"
                            label="Price ($)"
                            type="number"
                            value={formik.values.price ? formik.values.price : ""}
                            onChange={formik.handleChange}
                            onBlur={formik.handleBlur}
                            error={formik.touched.price && Boolean(formik.errors.price)}
                            helperText={formik.touched.price && formik.errors.price}
                        />
                    </Grid>
                    <Grid size={12}>
                        <Box display="flex" justifyContent="center" alignItems="center">
                            <Button
                                type="submit"
                                variant="contained"
                                color="primary"
                                fullWidth
                                disabled={formik.isSubmitting}
                                startIcon={formik.isSubmitting && <CircularProgress size={20} />}
                            >
                                {formik.isSubmitting ? "Creating..." : "Create"}
                            </Button>
                        </Box>
                    </Grid>
                </Grid>
            </form>
        </Container>
    );
};

export default EstateCreationPage;
