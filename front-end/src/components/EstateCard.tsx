import { Button, Card, CardActions, CardContent, CardMedia, Typography } from "@mui/material";
import { EstateShortData, EstateType } from "../hooks/useCatalogueApi";
import moment from "moment";

interface EstateCardProps {
    estateData: EstateShortData;
}

const EstateCard = ({ estateData }: EstateCardProps) => {
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
                <Button variant="outlined" fullWidth>
                    Details
                </Button>
            </CardActions>
        </Card>
    );
};

export default EstateCard;