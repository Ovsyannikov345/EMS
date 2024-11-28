import { useState } from "react";
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
    IconButton,
    InputLabel,
    MenuItem,
    Select,
    SelectChangeEvent,
    TextField,
    Typography,
} from "@mui/material";
import { useNotifications } from "@toolpad/core";
import { useNavigate } from "react-router-dom";
import { ESTATE_DETAILS_ROUTE } from "../utils/consts";
import CloseIcon from "@mui/icons-material/Close";

const EstateCreationPage = () => {
    const navigate = useNavigate();

    const notifications = useNotifications();

    const { createEstate, uploadEstateImage } = useCatalogueApi();

    const [images, setImages] = useState<Image[]>([]);

    const [dragActive, setDragActive] = useState(false);

    const addImage = (file: File) => {
        console.log([...images, { file: file, url: URL.createObjectURL(file) }]);

        setImages([...images, { file: file, url: URL.createObjectURL(file) }]);
    };

    const handleAddImage = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.files && event.target.files.length > 0) {
            addImage(event.target.files[0]);
        }
    };

    const handleDeleteImage = (url: string) => {
        console.log(images.filter((img) => img.url !== url));

        setImages(images.filter((img) => img.url !== url));
    };

    const handleDragOver = (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault();
        setDragActive(true);
    };

    const handleDragLeave = () => {
        setDragActive(false);
    };

    const handleDrop = (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault();
        setDragActive(false);
        if (event.dataTransfer.files && event.dataTransfer.files.length > 0) {
            addImage(event.dataTransfer.files[0]);
        }
    };

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

            let imageResponse;

            for (let i = 0; i < images.length; i++) {
                imageResponse = await uploadEstateImage(response.id, images[i].file);

                if ("error" in imageResponse) {
                    notifications.show(imageResponse.message, { severity: "error", autoHideDuration: 3000 });

                    break;
                }
            }

            navigate(ESTATE_DETAILS_ROUTE.replace(":id", response.id));
            setSubmitting(false);
        },
    });

    return (
        <Container maxWidth="sm" sx={{ mt: 3, pb: 5 }}>
            <Typography variant="h4" mb={2}>
                Create estate
            </Typography>
            <Grid container spacing={1} mb={3}>
                {images.map((image) => (
                    <Grid size={{ xs: 12, sm: 6 }} key={image.url}>
                        <div
                            style={{
                                position: "relative",
                                width: "100%",
                                paddingTop: "56.25%",
                                overflow: "hidden",
                                borderRadius: "8px",
                            }}
                        >
                            <img
                                alt="estate"
                                style={{
                                    position: "absolute",
                                    top: 0,
                                    left: 0,
                                    width: "100%",
                                    height: "100%",
                                    objectFit: "cover",
                                }}
                                src={image.url}
                            />
                            <IconButton
                                size="small"
                                color="secondary"
                                onClick={() => handleDeleteImage(image.url)}
                                sx={{
                                    position: "absolute",
                                    top: 5,
                                    right: 5,
                                    backgroundColor: "primary.main",
                                    "&:hover": {
                                        backgroundColor: "primary.dark",
                                    },
                                }}
                            >
                                <CloseIcon fontSize="small" />
                            </IconButton>
                        </div>
                    </Grid>
                ))}
                <Grid
                    size={{ xs: 12, sm: 6 }}
                    sx={{
                        border: `2px dashed ${dragActive ? "blue" : "grey"}`,
                        padding: 4,
                        textAlign: "center",
                        borderRadius: 1,
                        cursor: "pointer",
                    }}
                    onDragOver={handleDragOver}
                    onDragLeave={handleDragLeave}
                    onDrop={handleDrop}
                    onClick={() => document.getElementById("file-input")?.click()}
                >
                    <Typography variant="body1" sx={{ marginBottom: 2 }}>
                        Drag and drop an image here or click to select
                    </Typography>
                    <input
                        id="file-input"
                        type="file"
                        accept="image/*"
                        style={{ display: "none" }}
                        onChange={(e) => {
                            handleAddImage(e);
                            e.target.value = "";
                        }}
                    />
                </Grid>
            </Grid>

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
                    <Grid size={{ xs: 12, sm: 4 }}>
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
                    <Grid size={{ xs: 12, sm: 4 }}>
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
                    <Grid size={{ xs: 12, sm: 4 }}>
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
