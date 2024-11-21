import { Button } from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";

interface EditButtonProps {
    onClick: () => void;
}

const EditButton = ({ onClick }: EditButtonProps) => {
    return (
        <Button variant="contained" size="large" color="primary" startIcon={<EditIcon />} onClick={onClick}>
            Edit
        </Button>
    );
};

export default EditButton;
