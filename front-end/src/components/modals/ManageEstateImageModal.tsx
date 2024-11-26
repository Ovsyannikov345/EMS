import React, { useState } from "react";
import { Grid2 as Grid, Button, IconButton, Card, CardMedia, Box, CircularProgress, Dialog, DialogTitle, DialogContent } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import AddPhotoAlternateIcon from "@mui/icons-material/AddPhotoAlternate";
import { EstateShortData } from "../../hooks/useCatalogueApi";

interface ManageEstateImageModalProps {
    open: boolean;
    onClose: () => void;
    onImageCreate: (file: File) => Promise<void>;
    onImageDelete: (id: string) => Promise<void>;
    estateData: EstateShortData;
}

export interface EstateImage {
    id: string;
    url: string;
}

const ManageEstateImageModal = ({ open, onClose, onImageCreate, onImageDelete, estateData }: ManageEstateImageModalProps) => {
    const [isUploading, setIsUploading] = useState(false);

    const handleAddImage = async (event: React.ChangeEvent<HTMLInputElement>) => {
        if (!event.target.files || !event.target.files[0]) {
            return;
        }

        setIsUploading(true);

        const file = event.target.files[0];

        await onImageCreate(file);

        setIsUploading(false);
    };

    const handleDeleteImage = async (id: string) => {
        setIsUploading(true);

        await onImageDelete(id);

        setIsUploading(false);
    };

    return (
        <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
            <IconButton
                size="medium"
                onClick={onClose}
                sx={{
                    position: "absolute",
                    top: 8,
                    right: 8,
                    backgroundColor: "rgba(255, 255, 255, 0.8)",
                    "&:hover": {
                        backgroundColor: "rgba(255, 255, 255, 1)",
                    },
                }}
            >
                <CloseIcon />
            </IconButton>
            <DialogTitle>Manage Images</DialogTitle>
            <DialogContent>
                <Box sx={{ display: "flex", alignItems: "center", mb: 2 }}>
                    <Button variant="contained" component="label" startIcon={<AddPhotoAlternateIcon />} disabled={isUploading}>
                        {isUploading ? "Saving..." : "Add Image"}
                        <input type="file" accept="image/*" hidden onChange={handleAddImage} />
                    </Button>
                    {isUploading && <CircularProgress size={24} sx={{ ml: 2 }} />}
                </Box>

                <Grid container spacing={2}>
                    {estateData.imageIds.map((id) => (
                        <Grid size={{ xs: 12, sm: 6 }} key={id}>
                            <Box sx={{ position: "relative" }}>
                                <Card>
                                    <CardMedia
                                        component="img"
                                        height="140"
                                        src={`${process.env.REACT_APP_CATALOGUE_API_URL}/EstateImage/${estateData.id}/${id}`}
                                        alt="Estate Image"
                                    />
                                </Card>
                                <IconButton
                                    size="small"
                                    color="secondary"
                                    onClick={() => handleDeleteImage(id)}
                                    sx={{
                                        position: "absolute",
                                        top: -10,
                                        right: -10,
                                        backgroundColor: "primary.main",
                                        "&:hover": {
                                            backgroundColor: "primary.dark",
                                        },
                                    }}
                                >
                                    <CloseIcon fontSize="small" />
                                </IconButton>
                            </Box>
                        </Grid>
                    ))}
                </Grid>
            </DialogContent>
        </Dialog>
    );
};

export default ManageEstateImageModal;
