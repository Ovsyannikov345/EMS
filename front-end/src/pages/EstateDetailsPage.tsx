import { useEffect, useState } from "react";
import useCatalogueApi, { EstateFullData, EstateToUpdateData, EstateType } from "../hooks/useCatalogueApi";
import { useNavigate, useParams } from "react-router-dom";
import { Alert, Avatar, Box, Button, Container, Grid2 as Grid, ListItemIcon, Menu, MenuItem, Typography } from "@mui/material";
import UndoIcon from "@mui/icons-material/Undo";
import GoBackButton from "../components/buttons/GoBackButton";
import EstateDetailsSkeleton from "../components/skeletons/EstateDetailsSkeleton";
import useProfileApi, { UserProfile } from "../hooks/useProfileApi";
import Carousel from "react-material-ui-carousel";
import { PROFILE_ROUTE } from "../utils/consts";
import PersonIcon from "@mui/icons-material/Person";
import SendIcon from "@mui/icons-material/Send";
import EditEstateForm from "../components/forms/EditEstateForm";
import { useNotifications } from "@toolpad/core";
import InfoIcon from "@mui/icons-material/Info";
import ImageIcon from "@mui/icons-material/Image";
import DeleteIcon from "@mui/icons-material/Delete";
import ActionsButton from "../components/buttons/ActionsButton";
import ManageEstateImageModal from "../components/modals/ManageEstateImageModal";

const EstateDetailsPage = () => {
    const { id } = useParams();

    const navigate = useNavigate();

    const notifications = useNotifications();

    const [menuAnchorEl, setMenuAnchorEl] = useState<Element | null>(null);

    const [estate, setEstate] = useState<EstateFullData>();

    const [userImageSrc, setUserImageSrc] = useState("");

    const [currentUser, setCurrentUser] = useState<UserProfile>();

    const [loading, setLoading] = useState<boolean>(true);

    const [error, setError] = useState<string | null>(null);

    const [editing, setEditing] = useState(false);

    const [changingImages, setChangingImages] = useState(false);

    const { getProfileImage, getOwnProfile } = useProfileApi();

    const { getEstate, updateEstate, uploadEstateImage, deleteEstateImage } = useCatalogueApi();

    useEffect(() => {
        const loadData = async () => {
            const response = await getEstate(id!);

            if ("error" in response) {
                setError(response.message);
                setLoading(false);

                return;
            }

            setEstate(response);

            const imageResponse = await getProfileImage(response.userId);

            if ("error" in imageResponse) {
                setUserImageSrc("");
            } else {
                setUserImageSrc(URL.createObjectURL(imageResponse.blob));
            }

            const profileResponse = await getOwnProfile();

            if ("error" in profileResponse) {
                setError(profileResponse.message);
                setLoading(false);

                return;
            }

            setCurrentUser(profileResponse);
            setLoading(false);
            setError(null);
        };

        loadData();
    }, [id]);

    const saveChanges = async (estateToUpdate: EstateToUpdateData) => {
        const response = await updateEstate(estateToUpdate);

        if ("error" in response) {
            notifications.show(response.message, { severity: "error", autoHideDuration: 3000 });

            return;
        }

        const updatedEstate = { ...estate!, ...response, imageIds: estate!.imageIds };

        notifications.show("Changes saved", { severity: "success", autoHideDuration: 3000 });
        setEstate(updatedEstate);
        setEditing(false);
    };

    const cancelChanges = () => {
        setEditing(false);
    };

    const createImage = async (file: File) => {
        if (!estate) {
            return;
        }

        const response = await uploadEstateImage(estate.id, file);

        if ("error" in response) {
            notifications.show(response.message, { severity: "error", autoHideDuration: 3000 });

            return;
        }

        setEstate({ ...estate, imageIds: [...estate.imageIds, response.id] });
        notifications.show("Changes saved", { severity: "success", autoHideDuration: 3000 });
    };

    const deleteImage = async (imageId: string) => {
        if (!estate) {
            return;
        }

        const response = await deleteEstateImage(estate.id, imageId);

        if ("error" in response) {
            notifications.show(response.message, { severity: "error", autoHideDuration: 3000 });

            return;
        }

        setEstate({ ...estate, imageIds: estate.imageIds.filter((id) => id !== imageId) });
        notifications.show("Changes saved", { severity: "success", autoHideDuration: 3000 });
    };

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

    if (loading || !estate || !currentUser) {
        return (
            <Container maxWidth="md" sx={{ mt: 4, display: "flex", justifyContent: "center" }}>
                <Box sx={{ width: "100%", textAlign: "center" }}>
                    <EstateDetailsSkeleton />
                </Box>
            </Container>
        );
    }

    if (editing) {
        return <EditEstateForm initialValues={estate} onSubmit={saveChanges} onCancel={cancelChanges} />;
    }

    return (
        <>
            <Container maxWidth="md" sx={{ mt: 4, pb: 6 }}>
                <Grid container justifyContent={"space-between"} mb={1}>
                    <GoBackButton />
                    {currentUser.id === estate.userId && (
                        <ActionsButton
                            onClick={(event) => {
                                setMenuAnchorEl(event.currentTarget);
                            }}
                        />
                    )}
                </Grid>
                <Box>
                    <Box sx={{ mb: 3 }}>
                        <Typography variant="h4">Estate details</Typography>
                    </Box>
                    {estate.imageIds.length > 0 ? (
                        <Container maxWidth="sm" sx={{ pb: 4 }}>
                            <Carousel sx={{ width: "100%" }} animation="slide">
                                {estate.imageIds.map((imageId, index) => (
                                    <div
                                        key={index}
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
                                            src={`${process.env.REACT_APP_CATALOGUE_API_URL}/EstateImage/${estate.id}/${imageId}`}
                                        />
                                    </div>
                                ))}
                            </Carousel>
                        </Container>
                    ) : (
                        <Box
                            sx={{
                                mb: 2,
                                p: 2,
                                border: "1px solid #ddd",
                                borderRadius: 2,
                                maxWidth: "200px",
                            }}
                        >
                            <img alt="estate" src={`${process.env.PUBLIC_URL}/house-placeholder.png`} style={{ width: "100%" }} />
                        </Box>
                    )}
                    <Box sx={{ mb: 3 }}>
                        <Typography variant="h5" gutterBottom>
                            {estate.address}
                        </Typography>
                        <Typography variant="h6">
                            <strong>Type:</strong> {EstateType[estate.type]}
                        </Typography>
                        <Typography variant="h6">
                            <strong>Area:</strong> {estate.area} m²
                        </Typography>
                        <Typography variant="h6">
                            <strong>Rooms:</strong> {estate.roomsCount}
                        </Typography>
                        <Typography variant="h6">
                            <strong>Price:</strong> ${estate.price.toLocaleString()}
                        </Typography>
                    </Box>
                    <Typography variant="h5" mb={2}>
                        Estate owner
                    </Typography>
                    <Box sx={{ display: "flex", alignItems: "center", gap: 2 }}>
                        <Avatar sx={{ width: 80, height: 80 }} src={userImageSrc} alt={`${estate.user.firstName} ${estate.user.lastName}`} />
                        <Box>
                            <Typography variant="h6">
                                {estate.user.firstName} {estate.user.lastName} {currentUser.id === estate.userId && "(You)"}
                            </Typography>
                            <Grid container spacing={2} mt={1}>
                                <Button
                                    variant="outlined"
                                    startIcon={<PersonIcon />}
                                    sx={{ width: "130px" }}
                                    onClick={() => navigate(PROFILE_ROUTE.replace(":id", estate.user.id))}
                                >
                                    Profile
                                </Button>
                                {currentUser.id !== estate.userId && (
                                    <Button variant="contained" startIcon={<SendIcon />} sx={{ width: "130px" }}>
                                        Message
                                    </Button>
                                )}
                            </Grid>
                        </Box>
                        <Box></Box>
                    </Box>
                </Box>
            </Container>
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
                        setEditing(true);
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
                        setChangingImages(true);
                        setMenuAnchorEl(null);
                    }}
                >
                    <ListItemIcon>
                        <ImageIcon fontSize="small" />
                    </ListItemIcon>
                    Manage Images
                </MenuItem>
                <MenuItem
                    key={3}
                    sx={{ mr: "5px" }}
                    onClick={() => {
                        // TODO implement deletion
                        setMenuAnchorEl(null);
                    }}
                >
                    <ListItemIcon>
                        <DeleteIcon fontSize="small" />
                    </ListItemIcon>
                    Delete estate
                </MenuItem>
            </Menu>
            <ManageEstateImageModal
                open={changingImages}
                onClose={() => setChangingImages(false)}
                onImageCreate={createImage}
                onImageDelete={deleteImage}
                estateData={estate}
            />
        </>
    );
};

export default EstateDetailsPage;
