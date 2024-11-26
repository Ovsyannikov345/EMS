import { useEffect, useState } from "react";
import useCatalogueApi, { EstateFullData, EstateType } from "../hooks/useCatalogueApi";
import { useNavigate, useParams } from "react-router-dom";
import { Alert, Avatar, Box, Button, Container, Grid2 as Grid, Typography } from "@mui/material";
import UndoIcon from "@mui/icons-material/Undo";
import GoBackButton from "../components/buttons/GoBackButton";
import EditButton from "../components/buttons/EditButton";
import EstateDetailsSkeleton from "../components/skeletons/EstateDetailsSkeleton";
import useProfileApi, { UserProfile } from "../hooks/useProfileApi";
import { Carousel } from "react-responsive-carousel";
import "react-responsive-carousel/lib/styles/carousel.min.css";
import { PROFILE_ROUTE } from "../utils/consts";
import PersonIcon from "@mui/icons-material/Person";
import SendIcon from "@mui/icons-material/Send";

const EstateDetailsPage = () => {
    const { id } = useParams();

    const navigate = useNavigate();

    const [estate, setEstate] = useState<EstateFullData>();

    const [userImageSrc, setUserImageSrc] = useState("");

    const [currentUser, setCurrentUser] = useState<UserProfile>();

    const [loading, setLoading] = useState<boolean>(true);

    const [error, setError] = useState<string | null>(null);

    const [editing, setEditing] = useState(false);

    const { getProfileImage, getOwnProfile } = useProfileApi();

    const { getEstate } = useCatalogueApi();

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

    return (
        <Container maxWidth="md" sx={{ mt: 4, pb: 6 }}>
            <Grid container justifyContent={"space-between"} mb={1}>
                {!editing && <GoBackButton />}
                {currentUser.id === estate.userId && !editing && <EditButton onClick={() => setEditing(true)} />}
            </Grid>
            <Box>
                <Box sx={{ mb: 3 }}>
                    <Typography variant="h4">Estate details</Typography>
                </Box>
                {/* Image Carousel */}
                {estate.imageIds.length > 0 ? (
                    <Box sx={{ mb: 4, maxWidth: "700px" }}>
                        <Carousel useKeyboardArrows={true}>
                            {estate.imageIds.map((imageId, index) => (
                                <div className="slide">
                                    <img
                                        alt="estate"
                                        src={`${process.env.REACT_APP_CATALOGUE_API_URL}/EstateImage/${estate.id}/${imageId}`}
                                        key={index}
                                    />
                                </div>
                            ))}
                        </Carousel>
                    </Box>
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
                            {estate.user.firstName} {estate.user.lastName}
                        </Typography>
                        <Grid container spacing={2} mt={1}>
                            <Button
                                variant="outlined"
                                startIcon={<PersonIcon />}
                                onClick={() => navigate(PROFILE_ROUTE.replace(":id", estate.user.id))}
                            >
                                Profile
                            </Button>
                            <Button variant="contained" startIcon={<SendIcon />}>
                                Message
                            </Button>
                        </Grid>
                    </Box>
                    <Box></Box>
                </Box>
            </Box>
        </Container>
    );
};

export default EstateDetailsPage;
