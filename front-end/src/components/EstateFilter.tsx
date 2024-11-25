import {
    Button,
    Checkbox,
    FormControl,
    Grid2 as Grid,
    InputLabel,
    ListItemText,
    MenuItem,
    Select,
    SelectChangeEvent,
    TextField,
} from "@mui/material";
import { EstateQueryFilter, EstateType } from "../hooks/useCatalogueApi";
import { useState } from "react";

interface EstateFilterProps {
    filter: EstateQueryFilter;
    onFilterChange: (filter: EstateQueryFilter) => void;
}

const EstateFilter = ({ filter, onFilterChange }: EstateFilterProps) => {
    const [filters, setFilters] = useState<EstateQueryFilter>(filter);

    const estateTypeOptions = Object.entries(EstateType)
        .filter(([key, value]) => !isNaN(Number(value)) && Number(value) !== 0)
        .map(([key, value]) => ({
            label: key,
            value: Number(value),
        }));

    const handleChange = (field: keyof EstateQueryFilter) => (event: React.ChangeEvent<any> | SelectChangeEvent<any>) => {
        const value = event.target.value;
        setFilters((prev) => ({
            ...prev,
            [field]: value === "" ? undefined : value,
        }));
    };

    const handleEstateTypesChange = (event: any) => {
        const selectedOptions = event.target.value;

        const result = selectedOptions.reduce((acc: number, current: number) => acc | current, 0);

        setFilters((prev) => ({
            ...prev,
            types: result,
        }));
    };

    const handleApplyFilters = () => {
        onFilterChange(filters);
    };

    const handleClearFilters = () => {
        const resetFilter = { types: EstateType.None };

        setFilters(resetFilter);
        onFilterChange(resetFilter);
    };

    const isSelected = (value: EstateType) => (filters.types & value) === value;

    return (
        <Grid container spacing={2} size={{ xs: 12, sm: 8, lg: 6 }}>
            <Grid size={{ xs: 12, sm: 6 }}>
                <FormControl fullWidth>
                    <InputLabel id="estate-type-label">Select Estate Types</InputLabel>
                    <Select
                        labelId="estate-type-label"
                        label="Select Estate Types"
                        multiple
                        value={estateTypeOptions.filter((option) => isSelected(option.value)).map((option) => option.value)}
                        onChange={handleEstateTypesChange}
                        renderValue={(selected) =>
                            estateTypeOptions
                                .filter((option) => selected.includes(option.value))
                                .map((option) => option.label)
                                .join(", ")
                        }
                    >
                        {estateTypeOptions.map((option) => (
                            <MenuItem key={option.value} value={option.value}>
                                <Checkbox checked={isSelected(option.value)} />
                                <ListItemText primary={option.label} />
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>
            </Grid>
            <Grid size={{ xs: 12, sm: 6 }}>
                <TextField label="Address" fullWidth value={filters.address || ""} onChange={handleChange("address")} />
            </Grid>
            <Grid size={{ xs: 6 }}>
                <TextField label="Min Area" type="number" fullWidth value={filters.minArea || ""} onChange={handleChange("minArea")} />
            </Grid>
            <Grid size={{ xs: 6 }}>
                <TextField label="Max Area" type="number" fullWidth value={filters.maxArea || ""} onChange={handleChange("maxArea")} />
            </Grid>
            <Grid size={{ xs: 6 }}>
                <TextField
                    label="Min Rooms"
                    type="number"
                    fullWidth
                    value={filters.minRoomsCount || ""}
                    onChange={handleChange("minRoomsCount")}
                />
            </Grid>
            <Grid size={{ xs: 6 }}>
                <TextField
                    label="Max Rooms"
                    type="number"
                    fullWidth
                    value={filters.maxRoomsCount || ""}
                    onChange={handleChange("maxRoomsCount")}
                />
            </Grid>
            <Grid size={{ xs: 6 }}>
                <TextField label="Min Price" type="number" fullWidth value={filters.minPrice || ""} onChange={handleChange("minPrice")} />
            </Grid>
            <Grid size={{ xs: 6 }}>
                <TextField label="Max Price" type="number" fullWidth value={filters.maxPrice || ""} onChange={handleChange("maxPrice")} />
            </Grid>
            <Grid container spacing={2}>
                <Button variant="contained" color="primary" onClick={handleApplyFilters}>
                    Apply Filters
                </Button>
                <Button variant="contained" color="secondary" onClick={handleClearFilters}>
                    Clear filters
                </Button>
            </Grid>
        </Grid>
    );
};

export default EstateFilter;
