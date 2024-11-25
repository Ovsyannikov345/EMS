import { Card, CardActionArea, CardContent, Grid2 as Grid, Typography } from "@mui/material";

interface ProfileCardProps {
    title: string;
    value: any;
    onClick?: () => void;
}

const ProfileCard = ({ title, value, onClick }: ProfileCardProps) => {
    return (
        <Grid size={{ xs: 12, sm: 6 }}>
            <Card sx={{ display: "flex", justifyContent: "center", alignItems: "center", p: 2 }}>
                {onClick ? (
                    <CardActionArea onClick={onClick} sx={{ p: 0.5 }}>
                        <CardContent>
                            <Typography variant="h6" textAlign={"center"}>
                                {title}
                            </Typography>
                            <Typography variant="h4" color="primary" textAlign={"center"}>
                                {value}
                            </Typography>
                        </CardContent>
                    </CardActionArea>
                ) : (
                    <CardContent>
                        <Typography variant="h6" textAlign={"center"}>
                            {title}
                        </Typography>
                        <Typography variant="h4" color="primary" textAlign={"center"}>
                            {value}
                        </Typography>
                    </CardContent>
                )}
            </Card>
        </Grid>
    );
};

export default ProfileCard;
