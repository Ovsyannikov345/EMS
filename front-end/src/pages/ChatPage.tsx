import React, { useEffect, useState } from "react";
import {
    Box,
    Typography,
    List,
    ListItem,
    ListItemButton,
    ListItemText,
    Divider,
    IconButton,
    useMediaQuery,
    ListItemAvatar,
    Avatar,
    ListSubheader,
} from "@mui/material";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import { useTheme } from "@mui/material/styles";
import { useNotifications } from "@toolpad/core";
import useChatApi, { Chat } from "../hooks/useChatApi";
import ChatDetails from "../components/ChatDetails";
import useProfileApi from "../hooks/useProfileApi";

const ChatPage = () => {
    const theme = useTheme();

    const notifications = useNotifications();

    const isSmallScreen = useMediaQuery(theme.breakpoints.down("sm"));

    const { getEstateChats, getUserChats } = useChatApi();

    const { getProfileImage } = useProfileApi();

    const [estateChats, setEstateChats] = useState<Chat[]>([]);

    const [myChats, setMyChats] = useState<Chat[]>([]);

    const [userAvatars, setUserAvatars] = useState<{ [id: string]: string }>({});

    const [selectedChatId, setSelectedChatId] = useState<string>();

    const [reloadChat, setReloadChat] = useState(false);

    useEffect(() => {
        const loadEstateChats = async (): Promise<void> => {
            const response = await getEstateChats();

            if ("error" in response) {
                return Promise.reject(response.message);
            }

            const imageLinks = await Promise.all(
                response.map(async (chat) => {
                    const image = await getProfileImage(chat.user.id);
                    if (!("error" in image)) {
                        return { [chat.user.id]: URL.createObjectURL(image.blob) };
                    }

                    return {};
                })
            );

            const mergedLinks = Object.assign({}, ...imageLinks);

            console.log(mergedLinks)

            setEstateChats(response);
            setUserAvatars((prev) => ({ ...prev, ...mergedLinks }));
        };

        const loadMyChats = async (): Promise<void> => {
            const response = await getUserChats();

            if ("error" in response) {
                return Promise.reject(response.message);
            }

            const imageLinks = await Promise.all(
                response.map(async (chat) => {
                    const image = await getProfileImage(chat.estate.user.id);
                    if (!("error" in image)) {
                        return { [chat.estate.user.id]: URL.createObjectURL(image.blob) };
                    }

                    return {};
                })
            );

            const mergedLinks = Object.assign({}, ...imageLinks);

            setMyChats(response);
            setUserAvatars((prev) => ({ ...prev, ...mergedLinks }));
        };

        const loadData = async () => {
            try {
                await loadEstateChats();
                await loadMyChats();
            } catch (err: any) {
                notifications.show(err, { severity: "error", autoHideDuration: 3000 });
            }
        };

        loadData();
    }, [reloadChat]);

    const handleChatSelect = (id: string) => {
        setSelectedChatId(id);
    };

    const handleBackToChatList = () => {
        if (selectedChatId) {
            setSelectedChatId(undefined);

            return;
        }

        setSelectedChatId(undefined);
        setReloadChat(!reloadChat);
    };

    return (
        <Box display="flex" flexDirection="row" height={`calc(100vh - ${document.getElementById("header")?.offsetHeight}px)`}>
            <Box
                sx={{
                    width: { xs: "100%", sm: "30%" },
                    borderRight: { sm: "1px solid #ddd" },
                    position: isSmallScreen && selectedChatId ? "absolute" : "relative",
                    zIndex: isSmallScreen && selectedChatId ? 1 : 0,
                    left: isSmallScreen && selectedChatId ? "-100%" : 0,
                    transition: "left 0.3s ease",
                    bgcolor: theme.palette.background.paper,
                    boxShadow: { xs: "0px 0px 10px rgba(0, 0, 0, 0.1)", sm: "none" },
                    overflowY: "auto",
                    height: "100%",
                    "&::-webkit-scrollbar": {
                        width: "0px",
                    },
                    "&:hover::-webkit-scrollbar": {
                        width: "8px",
                        zIndex: 4,
                    },
                    "&::-webkit-scrollbar-thumb": {
                        backgroundColor: theme.palette.primary.light,
                        borderRadius: "4px",
                    },
                    "&:hover::-webkit-scrollbar-thumb": {
                        backgroundColor: theme.palette.primary.main,
                    },
                }}
            >
                <Typography
                    variant="h5"
                    sx={{
                        p: 2,
                        fontWeight: "bold",
                        boxShadow: "0px 0px 10px rgba(0, 0, 0, 0.3)",
                    }}
                    textAlign={"left"}
                >
                    Your Chats
                </Typography>
                <Divider />
                <List style={{ paddingTop: "0px" }}>
                    {estateChats && estateChats.length > 0 && (
                        <>
                            <ListSubheader>{"Your estate"}</ListSubheader>
                            {estateChats?.map((chat) => (
                                <React.Fragment key={chat.id}>
                                    <ListItem disablePadding>
                                        <ListItemButton
                                            selected={selectedChatId === chat.id}
                                            onClick={() => handleChatSelect(chat.id)}
                                            sx={{
                                                "&.Mui-selected": {
                                                    backgroundColor: theme.palette.primary.main,
                                                    color: theme.palette.primary.contrastText,
                                                    "&:hover": {
                                                        backgroundColor: theme.palette.primary.main,
                                                    },
                                                },
                                            }}
                                        >
                                            <ListItemAvatar>
                                                <Avatar src={userAvatars[chat.estate.user?.id]} />
                                            </ListItemAvatar>
                                            <ListItemText
                                                primary={[chat.user.firstName, chat.user.lastName].join(" ")}
                                                secondary={chat.estate.address}
                                                primaryTypographyProps={{ fontWeight: 600 }}
                                                sx={{
                                                    "& .MuiTypography-root": {
                                                        "&.MuiTypography-body2": {
                                                            color: selectedChatId === chat.id ? theme.palette.primary.contrastText : "inherit",
                                                        },
                                                    },
                                                }}
                                            />
                                        </ListItemButton>
                                    </ListItem>
                                    <Divider />
                                </React.Fragment>
                            ))}
                        </>
                    )}
                    {myChats && myChats.length > 0 && (
                        <>
                            <ListSubheader>{"Other estate"}</ListSubheader>
                            {myChats?.map((chat) => (
                                <React.Fragment key={chat.id}>
                                    <ListItem disablePadding>
                                        <ListItemButton
                                            selected={selectedChatId === chat.id}
                                            onClick={() => handleChatSelect(chat.id)}
                                            sx={{
                                                "&.Mui-selected": {
                                                    backgroundColor: theme.palette.primary.main,
                                                    color: theme.palette.primary.contrastText,
                                                    "&:hover": {
                                                        backgroundColor: theme.palette.primary.main,
                                                    },
                                                },
                                            }}
                                        >
                                            <ListItemAvatar>
                                                <Avatar src={userAvatars[chat.estate.user.id]} />
                                            </ListItemAvatar>
                                            <ListItemText
                                                primary={[chat.estate.user.firstName, chat.estate.user.lastName].join(" ")}
                                                secondary={chat.estate.address}
                                                primaryTypographyProps={{ fontWeight: 600 }}
                                                sx={{
                                                    "& .MuiTypography-root": {
                                                        "&.MuiTypography-body2": {
                                                            color: selectedChatId === chat.id ? theme.palette.primary.contrastText : "inherit",
                                                        },
                                                    },
                                                }}
                                            />
                                        </ListItemButton>
                                    </ListItem>
                                    <Divider />
                                </React.Fragment>
                            ))}
                        </>
                    )}
                </List>
            </Box>
            <Box
                sx={{
                    flex: 1,
                    display: selectedChatId || !isSmallScreen ? "block" : "none",
                    position: isSmallScreen && selectedChatId ? "absolute" : "relative",
                    zIndex: isSmallScreen && selectedChatId ? 2 : 0,
                    width: isSmallScreen && selectedChatId ? "100%" : "auto",
                    left: isSmallScreen && !selectedChatId ? "100%" : 0,
                    transition: "left 0.3s ease",
                    bgcolor: theme.palette.background.default,
                    overflowY: "auto",
                    height: "100%",
                    "&::-webkit-scrollbar": {
                        width: "8px",
                    },
                    "&::-webkit-scrollbar-thumb": {
                        backgroundColor: theme.palette.secondary.light,
                        borderRadius: "4px",
                    },
                    "&::-webkit-scrollbar-thumb:hover": {
                        backgroundColor: theme.palette.secondary.main,
                    },
                }}
            >
                {isSmallScreen && (
                    <IconButton
                        onClick={handleBackToChatList}
                        sx={{
                            m: 1,
                            color: theme.palette.primary.main,
                        }}
                    >
                        <ArrowBackIcon />
                    </IconButton>
                )}
                <ChatDetails chatId={selectedChatId} onClose={handleBackToChatList} />
            </Box>
        </Box>
    );
};

export default ChatPage;
