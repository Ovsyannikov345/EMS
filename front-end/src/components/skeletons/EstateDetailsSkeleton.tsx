import { Box, Container, Grid2 as Grid, Skeleton, Typography } from "@mui/material";

const EstateDetailsSkeleton = () => (
    <Container maxWidth="md" sx={{ mt: 4, pb: 6 }}>
        <Grid container justifyContent={"space-between"} mb={1}>
            <Skeleton variant="rectangular" width={100} height={40} />
            <Skeleton variant="rectangular" width={100} height={40} />
        </Grid>
        <Box>
            <Box sx={{ mb: 3 }}>
                <Typography variant="h4">
                    <Skeleton width="60%" />
                </Typography>
            </Box>
            <Box sx={{ mb: 4, maxWidth: "700px" }}>
                <Skeleton variant="rectangular" width="100%" height={300} />
            </Box>
            <Box sx={{ mb: 3 }}>
                <Typography variant="h5" gutterBottom>
                    <Skeleton width="80%" />
                </Typography>
                {Array.from({ length: 4 }).map((_, index) => (
                    <Typography variant="h6" key={index}>
                        <Skeleton width="50%" />
                    </Typography>
                ))}
            </Box>
            <Typography variant="h5" mb={2}>
                <Skeleton width="40%" />
            </Typography>
            <Box sx={{ display: "flex", alignItems: "center", gap: 2 }}>
                <Skeleton variant="circular" width={80} height={80} />
                <Box>
                    <Typography variant="h6">
                        <Skeleton width="70%" />
                    </Typography>
                    <Skeleton variant="rectangular" width={100} height={30} />
                </Box>
            </Box>
        </Box>
    </Container>
);

export default EstateDetailsSkeleton;
