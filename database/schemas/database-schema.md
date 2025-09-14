# Database Schema

## Overview

The Video Summarizer API uses SQLite with Entity Framework Core for data persistence.

## Entities

### Video
- **Id**: `string` (Primary Key)
- **FileName**: `string` - Original filename
- **ContentType**: `string` - MIME type
- **Status**: `string` - Processing status (pending, processing, completed, failed)
- **UploadedAt**: `DateTime` - Upload timestamp
- **ProcessedAt**: `DateTime?` - Processing completion timestamp
- **FileSize**: `long` - File size in bytes
- **Duration**: `TimeSpan?` - Video duration

### Job
- **Id**: `string` (Primary Key)
- **VideoId**: `string` (Foreign Key)
- **Status**: `string` - Job status (queued, running, completed, failed)
- **CreatedAt**: `DateTime` - Job creation timestamp
- **StartedAt**: `DateTime?` - Job start timestamp
- **CompletedAt**: `DateTime?` - Job completion timestamp
- **ErrorMessage**: `string?` - Error message if failed

### Shot
- **Id**: `string` (Primary Key)
- **VideoId**: `string` (Foreign Key)
- **StartTime**: `double` - Start time in seconds
- **EndTime**: `double` - End time in seconds
- **KeyframeUrl**: `string` - URL to keyframe image
- **Description**: `string?` - Shot description
- **Confidence**: `double?` - Detection confidence score

### Caption
- **Id**: `string` (Primary Key)
- **ShotId**: `string` (Foreign Key)
- **Text**: `string` - Extracted text
- **Confidence**: `double?` - OCR confidence score
- **Language**: `string?` - Detected language
- **CreatedAt**: `DateTime` - Creation timestamp

### Summary
- **Id**: `string` (Primary Key)
- **VideoId**: `string` (Foreign Key)
- **Type**: `string` - Summary type (bullet_points, paragraph, timeline)
- **Content**: `string` - Summary content
- **CreatedAt**: `DateTime` - Creation timestamp
- **UpdatedAt**: `DateTime` - Last update timestamp

## Relationships

- Video → Jobs (1:many)
- Video → Shots (1:many)
- Video → Summaries (1:many)
- Shot → Captions (1:many)

## Indexes

- Video.Id (Primary Key)
- Job.VideoId (Foreign Key)
- Shot.VideoId (Foreign Key)
- Caption.ShotId (Foreign Key)
- Summary.VideoId (Foreign Key)
- Job.Status (for job queue queries)
- Video.Status (for status queries)
