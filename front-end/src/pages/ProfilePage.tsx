import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import useProfileApi, { UserProfile } from "../hooks/useProfileApi";
import { Alert, Avatar, Box, Button, CircularProgress, Container, Grid2 as Grid, Typography } from "@mui/material";
import moment from "moment";
import UndoIcon from "@mui/icons-material/Undo";
import VisibilityOffIcon from "@mui/icons-material/VisibilityOff";
import ProfileCard from "../components/ProfileCard";
import GoBackButton from "../components/buttons/GoBackButton";
import EditButton from "../components/buttons/EditButton";
import EditProfileForm from "../components/forms/EditProfileForm";
import { useNotifications } from "@toolpad/core";

const ProfilePage = () => {
    const { id } = useParams();

    const notifications = useNotifications();

    const navigate = useNavigate();

    const { getOwnProfile, getProfile, getProfileImage, updateProfile } = useProfileApi();

    const [profile, setProfile] = useState<UserProfile | null>();

    const [imageSrc, setImageSrc] = useState("");

    const [isLoading, setIsLoading] = useState(true);

    const [error, setError] = useState<string | null>(null);

    const [isEditing, setIsEditing] = useState(false);

    const [reloadProfile, setReloadProfile] = useState(false);

    useEffect(() => {
        const loadProfile = async () => {
            setIsLoading(true);
            setError(null);

            const response = id ? await getProfile(id) : await getOwnProfile();

            if ("error" in response) {
                setError("Failed to load user profile.");
                setIsLoading(false);

                return;
            }

            setProfile(response);
            setIsLoading(false);

            const imageResponse = await getProfileImage(response.id);

            if ("error" in imageResponse) {
                return;
            }

            setImageSrc(URL.createObjectURL(imageResponse.blob));
        };

        loadProfile();
    }, [reloadProfile]);

    const handleProfileUpdate = async (updatedProfile: UserProfile) => {
        const response = await updateProfile(updatedProfile);

        if ("error" in response) {
            notifications.show("Failed to update profile", { severity: "error", autoHideDuration: 3000 });
            return;
        }

        setProfile(response);
        setIsEditing(false);
        notifications.show("Profile updated", { severity: "success", autoHideDuration: 3000 });
        setReloadProfile(!reloadProfile);
    };

    if (isLoading) {
        return (
            <Box sx={{ display: "flex", justifyContent: "center", alignItems: "center", height: "100vh" }}>
                <CircularProgress />
            </Box>
        );
    }

    if (error) {
        return (
            <Box sx={{ display: "flex", justifyContent: "center", alignItems: "center", height: "100vh" }}>
                <Alert
                    severity="error"
                    action={
                        <Button color="inherit" size="small" startIcon={<UndoIcon />} onClick={() => navigate(-1)}>
                            Back
                        </Button>
                    }
                >
                    {error}
                </Alert>
            </Box>
        );
    }

    return profile ? (
        <Container maxWidth="md" sx={{ mt: 2 }}>
            <Grid container justifyContent={"space-between"}>
                {!isEditing && <GoBackButton />}
                {!id && !isEditing && <EditButton onClick={() => setIsEditing(true)} />}
            </Grid>
            {isEditing ? (
                <EditProfileForm initialData={profile} onSubmit={handleProfileUpdate} onCancel={() => setIsEditing(false)} />
            ) : (
                <>
                    <Box sx={{ textAlign: "center", mb: 4 }}>
                        <Avatar
                            alt={`${profile.firstName} ${profile.lastName}`}
                            src={`${imageSrc}`}
                            sx={{ width: 150, height: 150, margin: "0 auto" }}
                        />
                        <Typography variant="h4" sx={{ mt: 2 }}>
                            {profile.firstName} {profile.lastName}
                        </Typography>
                    </Box>
                    <Grid container spacing={2}>
                        <ProfileCard title="Estate Count" value={profile.estateCount} />
                        <ProfileCard title="Member for" value={moment(profile.createdAt).fromNow(true)} />
                        <ProfileCard
                            title="Birth date"
                            value={profile.birthDate ? moment(profile.birthDate).format("ll") : <VisibilityOffIcon fontSize="large" />}
                        />
                        <ProfileCard title="Phone number" value={profile.phoneNumber ?? <VisibilityOffIcon fontSize="large" />} />
                    </Grid>
                </>
            )}
        </Container>
    ) : (
        <></>
    );
};

export default ProfilePage;
