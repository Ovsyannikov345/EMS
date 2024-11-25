import { Button, Card, CardActions, CardContent, CardMedia, Typography } from "@mui/material";
import { EstateShortData, EstateType } from "../hooks/useCatalogueApi";
import moment from "moment";
import { useNavigate } from "react-router-dom";
import { ESTATE_DETAILS_ROUTE } from "../utils/consts";

interface EstateCardProps {
    estateData: EstateShortData;
}

const EstateCard = ({ estateData }: EstateCardProps) => {
    const navigate = useNavigate();

    return (
        <Card>
            <CardMedia
                sx={{ height: 140 }}
                component="img"
                src={
                    estateData.imageIds && estateData.imageIds.length > 0
                        ? `${process.env.REACT_APP_CATALOGUE_API_URL}/EstateImage/${estateData.id}/${estateData.imageIds[0]}`
                        : process.env.PUBLIC_URL + "/house-placeholder.png"
                }
            ></CardMedia>
            <CardContent>
                <Typography variant="h6" component="div">
                    {estateData.address}
                </Typography>
                <Typography variant="body1">Type: {EstateType[estateData.type]}</Typography>
                <Typography variant="body1">Price: ${estateData.price.toLocaleString()}</Typography>
                <Typography variant="body1">
                    {estateData.roomsCount} Rooms ({estateData.area} m² total)
                </Typography>
                <Typography variant="body1" textAlign={"end"}>
                    {moment(estateData.createdAt).fromNow()}
                </Typography>
            </CardContent>
            <CardActions>
                <Button variant="outlined" fullWidth onClick={() => navigate(ESTATE_DETAILS_ROUTE.replace(":id", estateData.id))}>
                    Details
                </Button>
            </CardActions>
        </Card>
    );
};

export default EstateCard;
