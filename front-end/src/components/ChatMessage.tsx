import { ListItem, ListItemAvatar, ListItemText, Avatar, Typography, Box } from "@mui/material";
import moment from "moment";
import { Message } from "../hooks/useChatApi";
import { UserProfile } from "../hooks/useProfileApi";

interface ChatMessageProps {
    message: Message;
    user: UserProfile;
    userImageSrc?: string;
    fromSelf: boolean;
}

const ChatMessage = ({ message, user, userImageSrc, fromSelf = false }: ChatMessageProps) => {
    return (
        <ListItem key={message.id} sx={{ alignItems: "flex-start", justifyContent: fromSelf ? "flex-end" : "flex-start" }}>
            {!fromSelf && (
                <ListItemAvatar sx={{ marginTop: "6px" }}>
                    <Avatar alt={[user.firstName, user.lastName].join(" ")} src={userImageSrc} style={{ width: 40, height: 40 }} />
                </ListItemAvatar>
            )}
            <ListItemText
                sx={{ maxWidth: "85%", flex: "none" }}
                primary={
                    <>
                        {!fromSelf && (
                            <Typography sx={{ display: "inline" }} component="span" variant="body1" color="text">
                                {[user.firstName, user.lastName].join(" ")}
                            </Typography>
                        )}
                        <Typography sx={{ display: "inline" }} component="span" variant="body2" color="text.secondary">
                            {(!fromSelf ? " - " : "") + moment(message.createdAt).format("HH:mm")}
                        </Typography>
                    </>
                }
                secondary={
                    <Box
                        sx={{
                            backgroundColor: "#C9DFFF",
                            padding: "10px",
                            borderRadius: "10px",
                            display: "inline-flex",
                        }}
                    >
                        <Typography sx={{ display: "inline" }} component="span" variant="body2" color="text.primary">
                            {message.text}
                        </Typography>
                    </Box>
                }
            />
        </ListItem>
    );
};

export default ChatMessage;
