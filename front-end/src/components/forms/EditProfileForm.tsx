import {
    Box,
    Button,
    TextField,
    Grid2 as Grid,
    Typography,
    CircularProgress,
    Select,
    InputLabel,
    FormControl,
    MenuItem,
    Collapse,
    SelectChangeEvent,
} from "@mui/material";
import useProfileApi, { InfoVisibility, InfoVisibilityOptions, UserProfile } from "../../hooks/useProfileApi";
import moment from "moment";
import { useFormik } from "formik";
import profileValidationSchema from "../../utils/validation/profileValidationSchema";
import { useEffect, useState } from "react";
import VisibilityIcon from "@mui/icons-material/Visibility";
import VisibilityOffIcon from "@mui/icons-material/VisibilityOff";
import { useNotifications } from "@toolpad/core";

interface EditProfileFormProps {
    initialData: UserProfile;
    onSubmit: (data: UserProfile) => Promise<void>;
    onCancel: () => void;
}

interface UserProfileFormValues {
    id: string;
    auth0Id: string;
    firstName: string;
    lastName: string;
    phoneNumber?: string;
    birthDate: string;
    estateCount: number;
    createdAt: Date;
    updatedAt: Date;
}

const EditProfileForm = ({ initialData, onSubmit, onCancel }: EditProfileFormProps) => {
    const { getProfileVisibility, updateProfileVisibility } = useProfileApi();

    const notifications = useNotifications();

    const [showProfileVisibility, setShowProfileVisibility] = useState(false);

    const [profileVisibility, setProfileVisibility] = useState<InfoVisibilityOptions | null>(null);

    const [isReadyToSave, setIsReadyToSave] = useState(false);

    useEffect(() => {
        const loadProfileVisibility = async () => {
            const response = await getProfileVisibility(initialData.id);

            if ("error" in response) {
                notifications.show("Failed to load settings", { severity: "error", autoHideDuration: 3000 });
                setIsReadyToSave(false);

                return;
            }

            setProfileVisibility(response);
            setIsReadyToSave(true);
        };

        loadProfileVisibility();
    }, []);

    const handleVisibilityChange = (field: keyof InfoVisibilityOptions) => async (event: React.ChangeEvent<any> | SelectChangeEvent<any>) => {
        const value = event.target.value;

        setProfileVisibility({ ...profileVisibility!, [field]: parseInt(value) });
    };

    const updateVisibility = async () => {
        const response = await updateProfileVisibility(profileVisibility!);

        if ("error" in response) {
            notifications.show("Failed to update settings", { severity: "error", autoHideDuration: 3000 });

            return false;
        }

        return true;
    };

    const formik = useFormik<UserProfileFormValues>({
        initialValues: {
            ...initialData,
            birthDate: moment(initialData.birthDate).format("YYYY-MM-DD"),
        },
        validationSchema: profileValidationSchema,
        onSubmit: async (values, { setSubmitting }) => {
            const formattedData: UserProfile = {
                ...values,
                birthDate: new Date(values.birthDate),
            };

            try {
                let success = await updateVisibility();

                if (success) {
                    await onSubmit(formattedData);
                }
            } finally {
                setSubmitting(false);
            }
        },
    });

    return (
        <Box component="form" onSubmit={formik.handleSubmit} sx={{ mt: 3 }}>
            <Typography variant="h5" sx={{ mb: 2 }}>
                Edit Profile
            </Typography>
            <Grid container spacing={2}>
                <Grid size={{ xs: 12, sm: 6 }}>
                    <TextField
                        fullWidth
                        label="First Name"
                        name="firstName"
                        value={formik.values.firstName}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        error={formik.touched.firstName && Boolean(formik.errors.firstName)}
                        helperText={formik.touched.firstName && formik.errors.firstName}
                    />
                </Grid>
                <Grid size={{ xs: 12, sm: 6 }}>
                    <TextField
                        fullWidth
                        label="Last Name"
                        name="lastName"
                        value={formik.values.lastName}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        error={formik.touched.lastName && Boolean(formik.errors.lastName)}
                        helperText={formik.touched.lastName && formik.errors.lastName}
                    />
                </Grid>
                <Grid size={{ xs: 12, sm: 6 }}>
                    <TextField
                        fullWidth
                        label="Phone Number"
                        name="phoneNumber"
                        value={formik.values.phoneNumber}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        error={formik.touched.phoneNumber && Boolean(formik.errors.phoneNumber)}
                        helperText={formik.touched.phoneNumber && formik.errors.phoneNumber}
                    />
                </Grid>
                <Grid size={{ xs: 12, sm: 6 }}>
                    <TextField
                        fullWidth
                        label="Birth Date"
                        name="birthDate"
                        type="date"
                        value={formik.values.birthDate}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        error={formik.touched.birthDate && Boolean(formik.errors.birthDate)}
                        helperText={formik.touched.birthDate && formik.errors.birthDate}
                        slotProps={{ inputLabel: { shrink: true } }}
                    />
                </Grid>
                <Button
                    variant="text"
                    startIcon={showProfileVisibility ? <VisibilityOffIcon /> : <VisibilityIcon />}
                    onClick={() => setShowProfileVisibility(!showProfileVisibility)}
                    disabled={!isReadyToSave}
                >
                    {showProfileVisibility ? "Hide Visibility" : "Edit visibility"}
                </Button>
                <Collapse in={showProfileVisibility} sx={{ width: "100%" }}>
                    <Grid container spacing={2}>
                        <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                            <FormControl fullWidth>
                                <InputLabel id="phone-visibility-label">Phone number</InputLabel>
                                <Select
                                    fullWidth
                                    labelId="phone-visibility-label"
                                    label="Phone number"
                                    value={profileVisibility ? profileVisibility.phoneNumberVisibility : ""}
                                    onChange={handleVisibilityChange("phoneNumberVisibility")}
                                >
                                    {Object.keys(InfoVisibility)
                                        .filter((key) => isNaN(Number(key)))
                                        .map((key) => (
                                            <MenuItem
                                                key={InfoVisibility[key as keyof typeof InfoVisibility]}
                                                value={InfoVisibility[key as keyof typeof InfoVisibility].toString()}
                                            >
                                                {key}
                                            </MenuItem>
                                        ))}
                                </Select>
                            </FormControl>
                        </Grid>
                        <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                            <FormControl fullWidth>
                                <InputLabel id="birth-date-label">Birth date</InputLabel>
                                <Select
                                    labelId="birth-date-label"
                                    label="Birth date"
                                    value={profileVisibility ? profileVisibility.birthDateVisibility : ""}
                                    onChange={handleVisibilityChange("birthDateVisibility")}
                                >
                                    {Object.keys(InfoVisibility)
                                        .filter((key) => isNaN(Number(key)))
                                        .map((key) => (
                                            <MenuItem
                                                key={InfoVisibility[key as keyof typeof InfoVisibility]}
                                                value={InfoVisibility[key as keyof typeof InfoVisibility].toString()}
                                            >
                                                {key}
                                            </MenuItem>
                                        ))}
                                </Select>
                            </FormControl>
                        </Grid>
                    </Grid>
                </Collapse>
            </Grid>

            <Box sx={{ mt: 3, display: "flex", justifyContent: "flex-end" }}>
                <Button
                    variant="contained"
                    color="primary"
                    type="submit"
                    sx={{ mr: 2 }}
                    disabled={!isReadyToSave || formik.isSubmitting}
                    startIcon={formik.isSubmitting && <CircularProgress size={20} />}
                >
                    {formik.isSubmitting ? "Saving..." : "Save"}
                </Button>
                <Button variant="outlined" onClick={onCancel} disabled={formik.isSubmitting}>
                    Cancel
                </Button>
            </Box>
        </Box>
    );
};

export default EditProfileForm;
