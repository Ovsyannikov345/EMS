import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import useProfileApi, { UserProfile } from "../hooks/useProfileApi";
import { Alert, Avatar, Box, Button, CircularProgress, Container, Grid2 as Grid, ListItemIcon, Menu, MenuItem, Typography } from "@mui/material";
import moment from "moment";
import UndoIcon from "@mui/icons-material/Undo";
import VisibilityOffIcon from "@mui/icons-material/VisibilityOff";
import ProfileCard from "../components/ProfileCard";
import GoBackButton from "../components/buttons/GoBackButton";
import EditButton from "../components/buttons/EditButton";
import { USER_ESTATE_ROUTE } from "../utils/consts";
import EditProfileForm from "../components/forms/EditProfileForm";
import { useNotifications } from "@toolpad/core";
import InfoIcon from "@mui/icons-material/Info";
import ImageIcon from "@mui/icons-material/Image";
import UpdateProfileImageModal from "../components/modals/UpdateProfileImageModal";

const ProfilePage = () => {
    const { id } = useParams();

    const notifications = useNotifications();

    const navigate = useNavigate();

    const { getOwnProfile, getProfile, getProfileImage, updateProfile, updateProfileImage } = useProfileApi();

    const [menuAnchorEl, setMenuAnchorEl] = useState<Element | null>(null);

    const [profile, setProfile] = useState<UserProfile | null>();

    const [imageSrc, setImageSrc] = useState("");

    const [isLoading, setIsLoading] = useState(true);

    const [error, setError] = useState<string | null>(null);

    const [isEditing, setIsEditing] = useState(false);

    const [isChangingImage, setIsChangingImage] = useState(false);

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
    }, [reloadProfile, id]);

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

    const handleImageSave = async (file: File): Promise<boolean> => {
        const response = await updateProfileImage(file);

        if ("error" in response) {
            notifications.show(response.message, { severity: "error", autoHideDuration: 3000 });

            return false;
        }

        notifications.show("Image saved", { severity: "success", autoHideDuration: 3000 });

        const imageResponse = await getProfileImage(profile!.id);

        if ("error" in imageResponse) {
            setReloadProfile(!reloadProfile);
            return true;
        }

        setImageSrc(URL.createObjectURL(imageResponse.blob));

        return true;
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
                {!id && !isEditing && (
                    <EditButton
                        onClick={(event) => {
                            setMenuAnchorEl(event.currentTarget);
                        }}
                    />
                )}
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
                        <ProfileCard
                            title="Estate Count"
                            value={profile.estateCount}
                            onClick={() => navigate(USER_ESTATE_ROUTE.replace(":userId", profile.id))}
                        />
                        <ProfileCard title="Member for" value={moment(profile.createdAt).fromNow(true)} />
                        <ProfileCard
                            title="Birth date"
                            value={profile.birthDate ? moment(profile.birthDate).format("ll") : <VisibilityOffIcon fontSize="large" />}
                        />
                        <ProfileCard title="Phone number" value={profile.phoneNumber ?? <VisibilityOffIcon fontSize="large" />} />
                    </Grid>
                    <Menu
                        id="menu"
                        anchorEl={menuAnchorEl}
                        open={Boolean(menuAnchorEl)}
                        onClose={() => setMenuAnchorEl(null)}
                        transformOrigin={{ horizontal: "right", vertical: "top" }}
                        anchorOrigin={{ horizontal: "right", vertical: "bottom" }}
                    >
                        <MenuItem
                            key={1}
                            onClick={() => {
                                setIsEditing(true);
                                setMenuAnchorEl(null);
                            }}
                        >
                            <ListItemIcon sx={{ mr: "5px" }}>
                                <InfoIcon fontSize="small" />
                            </ListItemIcon>
                            Edit info
                        </MenuItem>
                        <MenuItem
                            key={2}
                            sx={{ mr: "5px" }}
                            onClick={() => {
                                setIsChangingImage(true);
                                setMenuAnchorEl(null);
                            }}
                        >
                            <ListItemIcon>
                                <ImageIcon fontSize="small" />
                            </ListItemIcon>
                            Change image
                        </MenuItem>
                    </Menu>
                    <UpdateProfileImageModal open={isChangingImage} onClose={() => setIsChangingImage(false)} onSave={handleImageSave} />
                </>
            )}
        </Container>
    ) : (
        <></>
    );
};

export default ProfilePage;
