import { useEffect, useState } from "react";
import useCatalogueApi, { EstateShortData } from "../hooks/useCatalogueApi";
import { useNotifications } from "@toolpad/core/useNotifications";
import { Box, Container, Typography } from "@mui/material";
import Grid from "@mui/material/Grid2";
import EstateCard from "../components/headers/EstateCard";

const CataloguePage = () => {
    const { getEstateList } = useCatalogueApi();

    const notifications = useNotifications();

    const [estateList, setEstateList] = useState<EstateShortData[] | null>(null);

    useEffect(() => {
        const loadEstate = async () => {
            const response = await getEstateList();

            if ("error" in response) {
                notifications.show(response.message, { severity: "error", autoHideDuration: 3000 });
                return;
            }

            setEstateList(response);
        };

        loadEstate();
    }, []);

    return (
        <Container maxWidth="lg" sx={{ pb: 5 }}>
            <Box mt={4} mb={4}>
                <Typography variant="h4" gutterBottom mb={5}>
                    Real Estate Catalogue
                </Typography>
                <Grid container spacing={4}>
                    {estateList ? (
                        estateList.map((estate) => (
                            <Grid size={{ xs: 12, sm: 6, md: 4 }} key={estate.id}>
                                <EstateCard estateData={estate} />
                            </Grid>
                        ))
                    ) : (
                        <Typography variant="body1">No estate found</Typography>
                    )}
                </Grid>
            </Box>
        </Container>
    );
};

export default CataloguePage;
