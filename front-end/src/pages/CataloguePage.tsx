import { useEffect, useState } from "react";
import useCatalogueApi, { EstateShortData, PagedResult } from "../hooks/useCatalogueApi";
import { useNotifications } from "@toolpad/core/useNotifications";
import { Box, Container, FormControl, InputLabel, MenuItem, Pagination, Select, SelectChangeEvent, Typography } from "@mui/material";
import Grid from "@mui/material/Grid2";
import EstateCard from "../components/headers/EstateCard";

export enum SortOption {
    DateDescending = 0,
    DateAscending = 1,
    PriceDescending = 2,
    PriceAscending = 3,
    AreaDescending = 4,
    AreaAscending = 5,
}

const CataloguePage = () => {
    const { getEstateList } = useCatalogueApi();

    const notifications = useNotifications();

    const [pagedEstateList, setPagedEstateList] = useState<PagedResult<EstateShortData> | null>(null);

    const [currentPage, setCurrentPage] = useState(1);

    const [selectedSortOption, setSelectedSortOption] = useState<SortOption>(SortOption.DateDescending);

    useEffect(() => {
        const loadEstate = async () => {
            // TODO make selectable
            const response = await getEstateList(currentPage, 10, selectedSortOption);

            if ("error" in response) {
                notifications.show(response.message, { severity: "error", autoHideDuration: 3000 });
                return;
            }

            setPagedEstateList(response);
        };

        loadEstate();
    }, [currentPage, selectedSortOption]);

    const changePage = (event: React.ChangeEvent<unknown>, value: number) => {
        setCurrentPage(value);
    };

    const changeSortOption = (event: SelectChangeEvent<SortOption>) => {
        setSelectedSortOption(Number(event.target.value) as SortOption);
    };

    const formatSortOption = (key: string): string => {
        const isDescending = key.includes("Descending");
        const field = key.replace("Descending", "").replace("Ascending", "");
        const arrow = isDescending ? "↓" : "↑";
        return `${field.trim()} ${arrow}`;
    };

    return (
        <Container maxWidth="lg" sx={{ pb: 5 }}>
            <Box mt={4} mb={4}>
                <Grid container justifyContent={"space-between"} mb={5}>
                    <Typography variant="h4">Real Estate Catalogue</Typography>
                    <FormControl>
                        <InputLabel id="sort-label">Order by</InputLabel>
                        <Select
                            labelId="sort-label"
                            label="Order by"
                            value={selectedSortOption}
                            onChange={changeSortOption}
                            sx={{ width: "180px" }}
                        >
                            {Object.keys(SortOption)
                                .filter((key) => isNaN(Number(key)))
                                .map((key) => (
                                    <MenuItem
                                        key={SortOption[key as keyof typeof SortOption]}
                                        value={SortOption[key as keyof typeof SortOption].toString()}
                                    >
                                        {formatSortOption(key)}
                                    </MenuItem>
                                ))}
                        </Select>
                    </FormControl>
                </Grid>
                <Grid container spacing={4}>
                    {pagedEstateList && pagedEstateList.results.length > 0 ? (
                        pagedEstateList.results.map((estate) => (
                            <Grid size={{ xs: 12, sm: 6, md: 4 }} key={estate.id}>
                                <EstateCard estateData={estate} />
                            </Grid>
                        ))
                    ) : (
                        <Typography variant="body1">No estate found</Typography>
                    )}
                </Grid>
                <Grid container justifyContent={"center"} mt={4}>
                    {pagedEstateList && pagedEstateList.totalPages > 1 && (
                        <Pagination
                            count={pagedEstateList.totalPages}
                            page={pagedEstateList.currentPage}
                            shape="rounded"
                            color="primary"
                            onChange={changePage}
                        />
                    )}
                </Grid>
            </Box>
        </Container>
    );
};

export default CataloguePage;
