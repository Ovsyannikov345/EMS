import {
    Box,
    Chip,
    Divider,
    Grid2 as Grid,
    List,
    ListItem,
    TextField,
    Typography,
    IconButton,
    Alert,
    Button,
    CircularProgress,
} from "@mui/material";
import { useTheme } from "@mui/material/styles";
import useChatApi, { Chat, Message } from "../hooks/useChatApi";
import { useEffect, useMemo, useRef, useState } from "react";
import { EstateType } from "../hooks/useCatalogueApi";
import moment from "moment";
import ChatMessage from "./ChatMessage";
import useProfileApi from "../hooks/useProfileApi";
import SendIcon from "@mui/icons-material/Send";
import { useAuth0 } from "@auth0/auth0-react";
import { HttpTransportType, HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { useNotifications } from "@toolpad/core";
import UndoIcon from "@mui/icons-material/Undo";

interface ChatDetailsProps {
    chatId?: string;
    onClose: () => void;
}

const ChatDetails = ({ chatId, onClose }: ChatDetailsProps) => {
    const theme = useTheme();

    const notifications = useNotifications();

    const { getAccessTokenSilently } = useAuth0();

    const { getChatDetails } = useChatApi();
    const { getOwnProfile, getProfileImage } = useProfileApi();

    const [chat, setChat] = useState<Chat>();
    const [userId, setUserId] = useState<string>();
    const [userImageSrc, setUserImageSrc] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const [isError, setIsError] = useState(false);

    const scrollRef = useRef<HTMLDivElement | null>(null);

    const hubConnection = useRef<HubConnection>();

    const [newMessage, setNewMessage] = useState("");

    useEffect(() => {
        const loadUserImage = async (id: string) => {
            const imageResponse = await getProfileImage(id);

            if ("error" in imageResponse) {
                return;
            }

            setUserImageSrc(URL.createObjectURL(imageResponse.blob));
        };

        const loadUserId = async () => {
            const response = await getOwnProfile();
            if ("error" in response) return Promise.reject();
            setUserId(response.id);

            if (chat) {
                await loadUserImage(chat?.user.id === response.id ? chat.estate.user.id : chat?.user.id);
            }
        };

        const loadChat = async () => {
            if (!chatId) {
                return;
            }

            const response = await getChatDetails(chatId);

            if ("error" in response) {
                return Promise.reject();
            }

            setChat(response);
        };

        const connectToHub = async () => {
            try {
                const token = await getAccessTokenSilently();

                hubConnection.current = new HubConnectionBuilder()
                    .withUrl(process.env.REACT_APP_CHAT_HUB_URL!, {
                        accessTokenFactory: () => token,
                        transport: HttpTransportType.WebSockets | HttpTransportType.LongPolling,
                    })
                    .build();

                hubConnection.current.on("Receive", (m: Message) => {
                    console.log(m);
                    setChat((prevChat) => {
                        if (!prevChat) {
                            return prevChat;
                        }

                        return {
                            ...prevChat,
                            messages: [...prevChat.messages, m],
                        };
                    });
                });
                hubConnection.current.on("MessageSent", (m: Message) => {
                    console.log(m);
                    setChat((prevChat) => {
                        if (!prevChat) {
                            return prevChat;
                        }

                        return {
                            ...prevChat,
                            messages: [...prevChat.messages, m],
                        };
                    });
                });

                await hubConnection.current.start();
            } catch (e: any) {
                console.log(e.message);
                return Promise.reject();
            }
        };

        const loadData = async () => {
            setIsLoading(true);
            try {
                await loadUserId();
                await loadChat();
                await connectToHub();
                setIsError(false);
            } catch {
                setIsError(true);
            } finally {
                setIsLoading(false);
            }
        };

        if (chatId) {
            loadData();
        }

        return () => {
            if (hubConnection.current) {
                hubConnection.current.stop().catch((err) => console.log("Error stopping connection: ", err));
            }
        };
    }, [chatId]);

    type GroupedMessages = Record<string, Message[]>;

    const groupedMessages = useMemo(() => {
        if (!chat?.messages || chat.messages.length === 0) return [] as [string, Message[]][];
        const groups: GroupedMessages = {};
        chat.messages.forEach((message: Message) => {
            const date = moment(message.createdAt).format("YYYY-MM-DD");
            if (!groups[date]) groups[date] = [];
            groups[date].push(message);
        });
        const groupsArray = Object.entries(groups).sort((a, b) => new Date(a[0]).getTime() - new Date(b[0]).getTime());
        groupsArray.forEach(([_, messages]) => messages.sort((a, b) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()));
        return groupsArray;
    }, [chat]);

    useEffect(() => {
        if (scrollRef.current) {
            scrollRef.current.scrollTop = scrollRef.current.scrollHeight;
        }
      });

    const handleSendMessage = async () => {
        if (!newMessage.trim() || !chatId || !hubConnection.current) {
            return;
        }

        try {
            await hubConnection.current.invoke("Send", chatId, newMessage.trim());
            setNewMessage("");
        } catch {
            notifications.show("Error while sending message", { severity: "error", autoHideDuration: 3000 });
        }
    };

    const handleKeyDown = async (e: React.KeyboardEvent<HTMLDivElement>) => {
        if (e.key === "Enter" && !e.shiftKey) {
            e.preventDefault();
            await handleSendMessage();
        }
    };

    if (isError) {
        return (
            <Grid container justifyContent="center" alignItems="center" height="100%" sx={{ bgcolor: theme.palette.background.default }}>
                <Alert
                    severity="error"
                    action={
                        <Button color="inherit" size="small" startIcon={<UndoIcon />} onClick={onClose}>
                            Back
                        </Button>
                    }
                >
                    Chat is not available
                </Alert>
            </Grid>
        );
    }

    if (isLoading) {
        return (
            <Grid container justifyContent="center" alignItems="center" height="100%" sx={{ bgcolor: theme.palette.background.default }}>
                <CircularProgress />
            </Grid>
        );
    }

    return chat ? (
        <Box sx={{ height: "100%", display: "flex", flexDirection: "column" }}>
            <Grid container sx={{ bgcolor: theme.palette.action.hover, borderRadius: 2, pb: 2, pt: 2 }}>
                <Typography
                    variant="h6"
                    sx={{
                        fontWeight: "bold",
                        pl: 2,
                    }}
                >
                    {`${chat.estate.type !== 0 ? EstateType[chat.estate.type] + " at " : ""}${chat.estate.address}`}
                </Typography>
            </Grid>
            <Box
                ref={scrollRef}
                sx={{
                    flex: 1,
                    overflowY: "scroll",
                    p: 2,
                    "&::-webkit-scrollbar": {
                        width: "8px",
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
                <List>
                    <ListItem sx={{ justifyContent: "center" }}>
                        <Divider
                            sx={{
                                width: "90%",
                                "&::after": { borderTop: "2px solid #729CDB" },
                                "&::before": { borderTop: "2px solid #729CDB" },
                            }}
                        >
                            <Chip label="Start of the chat" size="small" sx={{ backgroundColor: "#FFF" }} />
                        </Divider>
                    </ListItem>
                    {groupedMessages.map(([date, messages]) => (
                        <>
                            <ListItem sx={{ justifyContent: "center" }} key={date}>
                                <Divider
                                    sx={{
                                        width: "90%",
                                        "&::after": { borderTop: "2px solid #729CDB" },
                                        "&::before": { borderTop: "2px solid #729CDB" },
                                    }}
                                >
                                    <Chip label={moment(date).format("DD.MM.YYYY")} size="small" sx={{ backgroundColor: "#FFF" }} />
                                </Divider>
                            </ListItem>
                            {messages.map((message) => (
                                <ChatMessage
                                    key={message.id}
                                    message={message}
                                    user={chat.estate.user.id === message.userId ? chat.estate.user : chat.user}
                                    userImageSrc={userImageSrc}
                                    fromSelf={message.userId === userId}
                                />
                            ))}
                        </>
                    ))}
                </List>
            </Box>
            <Box
                sx={{
                    borderTop: `1px solid ${theme.palette.divider}`,
                    display: "flex",
                    alignItems: "flex-end",
                    bgcolor: theme.palette.background.default,
                }}
            >
                <TextField
                    fullWidth
                    variant="standard"
                    placeholder="Type a message..."
                    multiline
                    value={newMessage}
                    onChange={(e) => setNewMessage(e.target.value)}
                    onKeyDown={handleKeyDown}
                    sx={{ mr: 2, pl: 1, pt: 1, pb: 1 }}
                    slotProps={{ input: { disableUnderline: true } }}
                />
                <IconButton color="primary" sx={{ height: "48px" }} onClick={handleSendMessage}>
                    <SendIcon />
                </IconButton>
            </Box>
        </Box>
    ) : (
        <Grid container justifyContent="center" alignItems="center" height="100%" sx={{ bgcolor: theme.palette.background.default }}>
            <Box
                sx={{
                    p: 2,
                    borderRadius: 2,
                    backgroundColor: theme.palette.secondary.light,
                    boxShadow: "0px 4px 10px rgba(0,0,0,0.1)",
                }}
            >
                <Typography variant="subtitle1" sx={{ color: theme.palette.secondary.contrastText }}>
                    Select a chat to start messaging
                </Typography>
            </Box>
        </Grid>
    );
};

export default ChatDetails;
