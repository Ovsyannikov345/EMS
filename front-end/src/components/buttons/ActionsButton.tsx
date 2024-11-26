import { Button } from "@mui/material";
import MenuIcon from "@mui/icons-material/Menu";

interface ActionsButtonProps {
    onClick: (arg: any) => void;
}

const ActionsButton = ({ onClick }: ActionsButtonProps) => {
    return (
        <Button variant="contained" size="large" color="primary" startIcon={<MenuIcon />} onClick={onClick}>
            Actions
        </Button>
    );
};

export default ActionsButton;
