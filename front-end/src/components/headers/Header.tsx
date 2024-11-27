import { useEffect, useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { Box, AppBar, Toolbar, Grid2 as Grid, Button, Tooltip, IconButton, Menu, MenuItem, ListItemIcon, Avatar } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { CATALOGUE_ROUTE, DEFAULT_ROUTE, OWN_PROFILE_ROUTE } from "../../utils/consts";
import LogoutIcon from "@mui/icons-material/Logout";
import Logo from "../../images/logo.png";
import CatalogueIcon from "@mui/icons-material/MapsHomeWork";
import CreateIcon from "@mui/icons-material/Create";
import ProfileIcon from "@mui/icons-material/Person";
import MyEstateIcon from "@mui/icons-material/HolidayVillage";
import ChatIcon from "@mui/icons-material/Chat";
import NotificationsIcon from "@mui/icons-material/Notifications";
import useProfileApi from "../../hooks/useProfileApi";

const Header = () => {
    const [menuAnchorEl, setMenuAnchorEl] = useState<Element | null>(null);

    const { isAuthenticated, logout } = useAuth0();

    const navigate = useNavigate();

    const { getOwnProfile, getProfileImage } = useProfileApi();

    const [imageSrc, setImageSrc] = useState<string>();

    useEffect(() => {
        const loadProfileImage = async () => {
            const response = await getOwnProfile();

            if ("error" in response) {
                return;
            }

            const imageResponse = await getProfileImage(response.id);

            if ("error" in imageResponse) {
                return;
            }

            setImageSrc(URL.createObjectURL(imageResponse.blob));
        };

        loadProfileImage();
    }, []);

    const onMenuClick = (destinationRoute: string) => {
        setMenuAnchorEl(null);
        navigate(destinationRoute);
    };

    return (
        <>
            <Box>
                <AppBar position="static">
                    <Toolbar style={{ justifyContent: "space-between" }}>
                        <Grid container mt={"5px"} mb={"5px"}>
                            <Grid container gap={"60px"} alignItems={"center"}>
                                <img
                                    src={Logo}
                                    alt="Review Guru"
                                    style={{ maxWidth: "200px", height: "auto", borderRadius: "10px", cursor: "pointer" }}
                                    onClick={() => navigate(DEFAULT_ROUTE)}
                                />
                                {isAuthenticated && (
                                    <>
                                        <Button
                                            variant="text"
                                            color="secondary"
                                            style={{ fontSize: "18px", borderRadius: "0", borderBottom: "1px solid white" }}
                                            startIcon={<CatalogueIcon />}
                                            onClick={() => navigate(CATALOGUE_ROUTE)}
                                        >
                                            Catalogue
                                        </Button>
                                        <Button
                                            variant="text"
                                            color="secondary"
                                            style={{ fontSize: "18px", borderRadius: "0", borderBottom: "1px solid white" }}
                                            startIcon={<CreateIcon />}
                                        >
                                            Create estate
                                        </Button>
                                    </>
                                )}
                            </Grid>
                        </Grid>
                        <Grid container>
                            <Grid container gap={"10px"} alignItems={"center"}>
                                {isAuthenticated && (
                                    <>
                                        <Tooltip title="Notifications">
                                            <IconButton size="large" color="secondary">
                                                <NotificationsIcon fontSize="large" />
                                            </IconButton>
                                        </Tooltip>
                                        <Tooltip title="Actions">
                                            <IconButton
                                                size="large"
                                                onClick={(event) => {
                                                    setMenuAnchorEl(event.currentTarget);
                                                }}
                                            >
                                                <Avatar sx={{ width: 45, height: 45 }} src={imageSrc} />
                                            </IconButton>
                                        </Tooltip>
                                    </>
                                )}
                            </Grid>
                        </Grid>
                    </Toolbar>
                </AppBar>
            </Box>
            <Menu
                id="menu"
                anchorEl={menuAnchorEl}
                open={Boolean(menuAnchorEl)}
                onClose={() => setMenuAnchorEl(null)}
                transformOrigin={{ horizontal: "right", vertical: "top" }}
                anchorOrigin={{ horizontal: "right", vertical: "bottom" }}
            >
                <MenuItem key={1} onClick={() => onMenuClick(OWN_PROFILE_ROUTE)}>
                    <ListItemIcon sx={{ mr: "5px" }}>
                        <ProfileIcon fontSize="small" />
                    </ListItemIcon>
                    My profile
                </MenuItem>
                <MenuItem key={2} sx={{ mr: "5px" }}>
                    <ListItemIcon>
                        <MyEstateIcon fontSize="small" />
                    </ListItemIcon>
                    My estate
                </MenuItem>
                <MenuItem key={3} sx={{ mr: "5px" }}>
                    <ListItemIcon>
                        <ChatIcon fontSize="small" />
                    </ListItemIcon>
                    My chats
                </MenuItem>
                <MenuItem key={4} sx={{ mr: "5px" }} onClick={() => logout()}>
                    <ListItemIcon>
                        <LogoutIcon fontSize="small" />
                    </ListItemIcon>
                    Logout
                </MenuItem>
            </Menu>
        </>
    );
};

export default Header;
