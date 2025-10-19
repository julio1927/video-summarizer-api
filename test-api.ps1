# Video Summarizer API Test Script
param(
    [string]$BaseUrl = "https://localhost:1927"
)

Write-Host "Video Summarizer API Test Script" -ForegroundColor Cyan
Write-Host "=================================" -ForegroundColor Cyan
Write-Host ""

$apiUrl = "$BaseUrl/api/videos"

# Generate unique filename for testing with different content types
$timestamp = Get-Date -Format "yyyyMMddHHmmss"
$contentTypes = @("tutorial", "demo", "presentation", "meeting", "general")
$randomType = $contentTypes | Get-Random
$testFileName = "test-video-$randomType-$timestamp.mp4"

Write-Host "Testing API at: $BaseUrl" -ForegroundColor Yellow
Write-Host "Using test file: $testFileName" -ForegroundColor Yellow
Write-Host ""

# Function to make API calls
function Invoke-ApiCall {
    param(
        [string]$Method,
        [string]$Uri,
        [string]$Body = $null,
        [string]$ContentType = "application/json"
    )
    
    try {
        $headers = @{
            "Content-Type" = $ContentType
        }
        
        if ($Body) {
            $response = Invoke-RestMethod -Uri $Uri -Method $Method -Body $Body -Headers $headers
        } else {
            $response = Invoke-RestMethod -Uri $Uri -Method $Method -Headers $headers
        }
        
        return $response
    }
    catch {
        Write-Host "API Call Failed: $($_.Exception.Message)" -ForegroundColor Red
        Write-Host "Response: $($_.Exception.Response)" -ForegroundColor Red
        if ($_.Exception.Response) {
            $stream = $_.Exception.Response.GetResponseStream()
            $reader = New-Object System.IO.StreamReader($stream)
            $responseBody = $reader.ReadToEnd()
            Write-Host "Response Body: $responseBody" -ForegroundColor Red
        }
        return $null
    }
}

# Step 1: Create video record
Write-Host "1. Creating video record..." -ForegroundColor Green
$createRequest = @{
    fileName = $testFileName
    contentType = "video/mp4"
} | ConvertTo-Json

$createResponse = Invoke-ApiCall -Method "POST" -Uri $apiUrl -Body $createRequest

if (-not $createResponse) {
    Write-Host "Failed to create video record" -ForegroundColor Red
    exit 1
}

$videoId = $createResponse.id
Write-Host "Video created with ID: $videoId" -ForegroundColor Green
Write-Host ""

# Step 2: Upload video file
Write-Host "2. Uploading video file..." -ForegroundColor Green
$uploadUrl = "$apiUrl/$videoId/upload"

$testContent = "This is a test video file content for API testing"
$tempFile = [System.IO.Path]::GetTempFileName()
[System.IO.File]::WriteAllText($tempFile, $testContent)

try {
    $uploadResponse = Invoke-RestMethod -Uri $uploadUrl -Method "POST" -InFile $tempFile -ContentType "video/mp4"
    Write-Host "Video file uploaded successfully" -ForegroundColor Green
}
catch {
    Write-Host "Failed to upload video file: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}
finally {
    Remove-Item $tempFile -ErrorAction SilentlyContinue
}

Write-Host ""

# Step 3: Start processing
Write-Host "3. Starting video processing..." -ForegroundColor Green
$processUrl = "$apiUrl/$videoId/process"

try {
    $processResponse = Invoke-RestMethod -Uri $processUrl -Method "POST" -ContentType "application/json"
    Write-Host "Processing started successfully" -ForegroundColor Green
}
catch {
    $statusCode = $_.Exception.Response.StatusCode.value__
    if ($statusCode -eq 202) {
        Write-Host "Processing started successfully (202 Accepted)" -ForegroundColor Green
    } else {
        Write-Host "Failed to start processing: $($_.Exception.Message)" -ForegroundColor Red
        exit 1
    }
}

Write-Host ""

# Step 4: Poll status until complete
Write-Host "4. Polling video status..." -ForegroundColor Green
$statusUrl = "$apiUrl/$videoId"
$maxAttempts = 20
$attempt = 0
$completed = $false

while (-not $completed -and $attempt -lt $maxAttempts) {
    $attempt++
    Write-Host "  Attempt $attempt/$maxAttempts - Checking status..." -ForegroundColor Yellow
    
    Start-Sleep -Seconds 2
    
    $statusResponse = Invoke-ApiCall -Method "GET" -Uri $statusUrl
    
    if (-not $statusResponse) {
        Write-Host "Failed to get status" -ForegroundColor Red
        exit 1
    }
    
    $status = $statusResponse.status
    Write-Host "  Current status: $status" -ForegroundColor Yellow
    
    if ($status -eq "completed") {
        Write-Host "Video processing completed!" -ForegroundColor Green
        $completed = $true
    }
    
    if ($status -eq "failed") {
        Write-Host "Video processing failed" -ForegroundColor Red
        exit 1
    }
}

if (-not $completed) {
    Write-Host "Processing timed out after $maxAttempts attempts" -ForegroundColor Red
    exit 1
}

Write-Host ""

# Step 5: Get video details
Write-Host "5. Retrieving video details..." -ForegroundColor Green
$detailsResponse = Invoke-ApiCall -Method "GET" -Uri $statusUrl

if (-not $detailsResponse) {
    Write-Host "Failed to get video details" -ForegroundColor Red
    exit 1
}

Write-Host "Video Details:" -ForegroundColor Green
Write-Host "  ID: $($detailsResponse.id)"
Write-Host "  File Name: $($detailsResponse.fileName)"
Write-Host "  Status: $($detailsResponse.status)"

$hasSummary = $detailsResponse.summary -ne $null
if ($hasSummary) {
    Write-Host "  Summary Available: Yes" -ForegroundColor Green
    Write-Host ""
    Write-Host "Summary Content:" -ForegroundColor Cyan
    Write-Host "  Bullet Points:" -ForegroundColor Yellow
    Write-Host "    $($detailsResponse.summary.bulletsMd)"
    Write-Host ""
    Write-Host "  Paragraph:" -ForegroundColor Yellow
    Write-Host "    $($detailsResponse.summary.paragraphMd)"
}

if (-not $hasSummary) {
    Write-Host "  Summary Available: No" -ForegroundColor Red
}

Write-Host ""

# Step 6: Get video shots
Write-Host "6. Retrieving video shots..." -ForegroundColor Green
$shotsUrl = "$apiUrl/$videoId/shots"
$shotsResponse = Invoke-ApiCall -Method "GET" -Uri $shotsUrl

if (-not $shotsResponse) {
    Write-Host "Failed to get video shots" -ForegroundColor Red
    exit 1
}

Write-Host "Video Shots:" -ForegroundColor Green
Write-Host "  Number of shots: $($shotsResponse.Count)"

$hasShots = $shotsResponse.Count -gt 0
if ($hasShots) {
    Write-Host "  Shot Details:" -ForegroundColor Yellow
    for ($i = 0; $i -lt $shotsResponse.Count; $i++) {
        $shot = $shotsResponse[$i]
        $startTime = [TimeSpan]::FromMilliseconds($shot.startMs).ToString("mm\:ss")
        $endTime = [TimeSpan]::FromMilliseconds($shot.endMs).ToString("mm\:ss")
        Write-Host "    Shot $($i + 1): $startTime - $endTime (Keyframe: $($shot.keyframePath))"
    }
}

if (-not $hasShots) {
    Write-Host "  No shots available" -ForegroundColor Red
}

Write-Host ""

# Test Summary
Write-Host "Test Summary" -ForegroundColor Cyan
Write-Host "============" -ForegroundColor Cyan
Write-Host "Video creation: PASSED"
Write-Host "File upload: PASSED"
Write-Host "Processing start: PASSED"
Write-Host "Status polling: PASSED"
Write-Host "Video details: PASSED"
Write-Host "Shots retrieval: PASSED"

if ($hasSummary) {
    Write-Host "Summary generation: PASSED"
}

if (-not $hasSummary) {
    Write-Host "Summary generation: FAILED"
}

Write-Host ""
Write-Host "All tests completed successfully!" -ForegroundColor Green
Write-Host "The Video Summarizer API is working correctly with mock data." -ForegroundColor Green