import { Button } from "@mui/material";
import ArrowBack from "@mui/icons-material/ArrowBack";
import { useNavigate } from "react-router-dom";

const GoBackButton = () => {
    const navigate = useNavigate();

    const handleGoBack = () => {
        navigate(-1);
    };

    return (
        <Button variant="text" size="large" color="primary" startIcon={<ArrowBack />} onClick={handleGoBack}>
            Back
        </Button>
    );
};

export default GoBackButton;
