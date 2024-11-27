import React, { useState } from "react";
import { Dialog, DialogActions, DialogContent, DialogTitle, Button, Box, Typography, CircularProgress } from "@mui/material";

interface UpdateProfileImageModalProps {
    open: boolean;
    onClose: () => void;
    onSave: (file: File) => Promise<boolean>;
}

const UpdateProfileImageModal = ({ open, onClose, onSave }: UpdateProfileImageModalProps) => {
    const [selectedFile, setSelectedFile] = useState<File | null>(null);

    const [dragActive, setDragActive] = useState(false);

    const [isUploading, setIsUploading] = useState(false);

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.files && event.target.files.length > 0) {
            setSelectedFile(event.target.files[0]);
        }
    };

    const handleDragOver = (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault();
        setDragActive(true);
    };

    const handleDragLeave = () => {
        setDragActive(false);
    };

    const handleDrop = (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault();
        setDragActive(false);
        if (event.dataTransfer.files && event.dataTransfer.files.length > 0) {
            setSelectedFile(event.dataTransfer.files[0]);
        }
    };

    const handleSave = async () => {
        if (!selectedFile) {
            return;
        }

        setIsUploading(true);

        const success = await onSave(selectedFile);

        setIsUploading(false);

        if (success) {
            onClose();
        }
    };

    return (
        <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
            <DialogTitle>Upload Image</DialogTitle>
            <DialogContent>
                <Box
                    sx={{
                        border: `2px dashed ${dragActive ? "blue" : "grey"}`,
                        padding: 4,
                        textAlign: "center",
                        borderRadius: 1,
                        cursor: "pointer",
                    }}
                    onDragOver={handleDragOver}
                    onDragLeave={handleDragLeave}
                    onDrop={handleDrop}
                    onClick={() => document.getElementById("file-input")?.click()}
                >
                    <Typography variant="body1" sx={{ marginBottom: 2 }}>
                        {selectedFile ? `${selectedFile.name}` : "Drag and drop a file here or click to select"}
                    </Typography>
                    <input id="file-input" type="file" accept="image/*" style={{ display: "none" }} onChange={handleFileChange} />
                </Box>
            </DialogContent>
            <DialogActions>
                <Button
                    onClick={handleSave}
                    variant="contained"
                    disabled={!selectedFile || isUploading}
                    startIcon={isUploading && <CircularProgress size={20} />}
                >
                    {isUploading ? "Saving..." : "Save"}
                </Button>
                <Button onClick={onClose} variant="outlined">
                    Cancel
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default UpdateProfileImageModal;
