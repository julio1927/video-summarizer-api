# Data Directory

This directory contains user-generated data and processed files for the Video Summarizer API.

## Structure

```
data/
├── uploads/          # Original video files uploaded by users
├── keyframes/        # Extracted keyframe images from video processing
├── outputs/          # Processed videos and other artifacts
└── README.md        # This file
```

## What Goes Here

### ✅ Included in Git
- **Folder structure** - Directories are created automatically
- **`.gitkeep` files** - Ensure empty directories are tracked

### ❌ Ignored by Git
- **User video files** - All files in `uploads/`
- **Generated keyframes** - All files in `keyframes/`
- **Processed outputs** - All files in `outputs/`
- **Any user-specific data**

## Usage

1. **Upload videos** - Place test videos in `uploads/` folder
2. **Process videos** - Use the API to process videos
3. **Access results** - Keyframes and outputs will be generated automatically
4. **View files** - Use `/static/{path}` endpoint to access processed files

## Security

- All user data is ignored by Git
- Files are served via the `/static/` endpoint with path validation
- No sensitive data should be stored in this directory

---

**Note**: This directory is automatically created when you first run the application.
