import { useEffect, useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import {
    Box,
    AppBar,
    Toolbar,
    Grid2 as Grid,
    Button,
    Tooltip,
    IconButton,
    Menu,
    MenuItem,
    ListItemIcon,
    Avatar,
    Typography,
    Divider,
    List,
    ListItem,
    ListItemText,
    FormControlLabel,
    Switch,
    Badge,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import { CATALOGUE_ROUTE, CHAT_LIST_ROUTE, DEFAULT_ROUTE, ESTATE_CREATION_ROUTE, OWN_PROFILE_ROUTE } from "../../utils/consts";
import LogoutIcon from "@mui/icons-material/Logout";
import Logo from "../../images/logo.png";
import CatalogueIcon from "@mui/icons-material/MapsHomeWork";
import CreateIcon from "@mui/icons-material/Create";
import ProfileIcon from "@mui/icons-material/Person";
import MyEstateIcon from "@mui/icons-material/HolidayVillage";
import ChatIcon from "@mui/icons-material/Chat";
import NotificationsIcon from "@mui/icons-material/Notifications";
import useProfileApi from "../../hooks/useProfileApi";
import useNotificationsApi, { Notification } from "../../hooks/useNotficationApi";
import { useNotifications } from "@toolpad/core";
import MarkChatUnreadIcon from "@mui/icons-material/CheckCircleOutline";
import moment from "moment";

const Header = () => {
    const [menuAnchorEl, setMenuAnchorEl] = useState<Element | null>(null);

    const [notificationAnchorEl, setNotificationAnchorEl] = useState<Element | null>(null);

    const [notificationsList, setNotificationsList] = useState<Notification[]>([]);

    const [unreadCount, setUnreadCount] = useState(0);

    const [showUnreadOnly, setShowUnreadOnly] = useState(true);

    const { isAuthenticated, logout } = useAuth0();

    const navigate = useNavigate();

    const notifications = useNotifications();

    const { getOwnProfile, getProfileImage } = useProfileApi();

    const { getNotificationList, readNotification } = useNotificationsApi();

    const [imageSrc, setImageSrc] = useState<string>();

    useEffect(() => {
        if (isAuthenticated) {
            fetchNotifications();
        }
    }, [isAuthenticated]);

    const fetchNotifications = async () => {
        const response = await getNotificationList();

        if (Array.isArray(response)) {
            setNotificationsList(response);
            setUnreadCount(response.filter((n) => !n.isRead).length);
        } else {
            notifications.show("Error while loading notifications", { severity: "error", autoHideDuration: 3000 });
        }
    };

    const handleMarkAsRead = async (notificationId: string) => {
        const response = await readNotification(notificationId);

        if ("error" in response) {
            notifications.show("Error while reading the notification", { severity: "error", autoHideDuration: 3000 });
            return;
        }

        fetchNotifications();
    };

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

    const getUserId = async (): Promise<string | undefined> => {
        const response = await getOwnProfile();

        if ("error" in response) {
            return undefined;
        }

        return response.id;
    };

    return (
        <>
            <Box id="header">
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
                                            onClick={() => navigate(ESTATE_CREATION_ROUTE)}
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
                                            <IconButton
                                                size="large"
                                                color="secondary"
                                                onClick={(event) => setNotificationAnchorEl(event.currentTarget)}
                                            >
                                                <Badge badgeContent={unreadCount} color="secondary">
                                                    <NotificationsIcon fontSize="large" />
                                                </Badge>
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
                <MenuItem key={2} sx={{ mr: "5px" }} onClick={async () => onMenuClick(`${CATALOGUE_ROUTE}/user/${await getUserId()}`)}>
                    <ListItemIcon>
                        <MyEstateIcon fontSize="small" />
                    </ListItemIcon>
                    My estate
                </MenuItem>
                <MenuItem key={3} sx={{ mr: "5px" }} onClick={() => onMenuClick(CHAT_LIST_ROUTE)}>
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
            <Menu
                id="notification-menu"
                anchorEl={notificationAnchorEl}
                open={Boolean(notificationAnchorEl)}
                onClose={() => setNotificationAnchorEl(null)}
                PaperProps={{ style: { width: "300px" } }}
            >
                <Box p={1} display="flex" justifyContent="center" alignItems="center">
                    <Typography variant="h6" textAlign={"center"}>
                        Notifications
                    </Typography>
                </Box>
                <Divider />
                <Box px={2} py={1} display="flex" alignItems="center" justifyContent="space-between">
                    <FormControlLabel
                        control={<Switch checked={showUnreadOnly} onChange={() => setShowUnreadOnly(!showUnreadOnly)} />}
                        label="Show unread only"
                    />
                </Box>
                <List>
                    {notificationsList
                        .filter((n) => (showUnreadOnly ? !n.isRead : true))
                        .map((notification) => (
                            <ListItem
                                key={notification.id}
                                secondaryAction={
                                    !notification.isRead && (
                                        <Tooltip title="Mark as read">
                                            <IconButton edge="end" onClick={() => handleMarkAsRead(notification.id)}>
                                                <MarkChatUnreadIcon color="secondary" />
                                            </IconButton>
                                        </Tooltip>
                                    )
                                }
                            >
                                <ListItemText
                                    primary={notification.title}
                                    secondary={moment(notification.createdAt).format("MMM Do YYYY, hh:mm:ss")}
                                />
                            </ListItem>
                        ))}
                </List>
                {notificationsList.length === 0 && (
                    <Typography textAlign="center" p={2}>
                        No notifications
                    </Typography>
                )}
            </Menu>
        </>
    );
};

export default Header;
