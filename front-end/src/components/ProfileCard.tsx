import { Card, CardContent, Grid2 as Grid, Typography } from "@mui/material";

interface ProfileCardProps {
    title: string;
    value: any;
}

const ProfileCard = ({ title, value }: ProfileCardProps) => {
    return (
        <Grid size={{ xs: 12, sm: 6 }}>
            <Card sx={{ display: "flex", justifyContent: "center", alignItems: "center", p: 2 }}>
                <CardContent>
                    <Typography variant="h6" textAlign={"center"}>
                        {title}
                    </Typography>
                    <Typography variant="h4" color="primary" textAlign={"center"}>
                        {value}
                    </Typography>
                </CardContent>
            </Card>
        </Grid>
    );
};

export default ProfileCard;
