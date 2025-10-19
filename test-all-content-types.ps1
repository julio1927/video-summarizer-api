# Test All Content Types Script
param(
    [string]$BaseUrl = "https://localhost:1927"
)

Write-Host "Testing All Content Types" -ForegroundColor Cyan
Write-Host "=========================" -ForegroundColor Cyan
Write-Host ""

$apiUrl = "$BaseUrl/api/videos"
$contentTypes = @(
    @{Name="Tutorial"; FileName="my-tutorial-video.mp4"},
    @{Name="Demo"; FileName="product-demo-showcase.mp4"},
    @{Name="Presentation"; FileName="quarterly-presentation.mp4"},
    @{Name="Meeting"; FileName="team-meeting-notes.mp4"},
    @{Name="General"; FileName="random-video-content.mp4"}
)

foreach ($contentType in $contentTypes) {
    Write-Host "Testing $($contentType.Name) content type..." -ForegroundColor Yellow
    Write-Host "Filename: $($contentType.FileName)" -ForegroundColor Gray
    Write-Host ""
    
    # Create video
    $createRequest = @{
        fileName = $contentType.FileName
        contentType = "video/mp4"
    } | ConvertTo-Json
    
    try {
        $createResponse = Invoke-RestMethod -Uri $apiUrl -Method "POST" -Body $createRequest -ContentType "application/json"
        $videoId = $createResponse.id
        Write-Host "  Video created: $videoId" -ForegroundColor Green
        
        # Upload file
        $uploadUrl = "$apiUrl/$videoId/upload"
        $testContent = "Test content for $($contentType.Name)"
        $tempFile = [System.IO.Path]::GetTempFileName()
        [System.IO.File]::WriteAllText($tempFile, $testContent)
        
        $uploadResponse = Invoke-RestMethod -Uri $uploadUrl -Method "POST" -InFile $tempFile -ContentType "video/mp4"
        Write-Host "  File uploaded" -ForegroundColor Green
        
        # Start processing
        $processUrl = "$apiUrl/$videoId/process"
        try {
            $processResponse = Invoke-RestMethod -Uri $processUrl -Method "POST" -ContentType "application/json"
            Write-Host "  Processing started" -ForegroundColor Green
        }
        catch {
            $statusCode = $_.Exception.Response.StatusCode.value__
            if ($statusCode -eq 202) {
                Write-Host "  Processing started (202 Accepted)" -ForegroundColor Green
            } else {
                Write-Host "  Processing failed: $($_.Exception.Message)" -ForegroundColor Red
                continue
            }
        }
        
        # Wait for processing
        Write-Host "  Waiting for processing to complete..." -ForegroundColor Yellow
        Start-Sleep -Seconds 5
        
        # Get results
        $statusUrl = "$apiUrl/$videoId"
        $detailsResponse = Invoke-RestMethod -Uri $statusUrl -Method "GET"
        
        Write-Host "  Status: $($detailsResponse.status)" -ForegroundColor Cyan
        
        if ($detailsResponse.summary) {
            Write-Host "  Summary Generated:" -ForegroundColor Green
            Write-Host "    $($detailsResponse.summary.bulletsMd)" -ForegroundColor White
        } else {
            Write-Host "  No summary available" -ForegroundColor Red
        }
        
        Remove-Item $tempFile -ErrorAction SilentlyContinue
    }
    catch {
        Write-Host "  Error: $($_.Exception.Message)" -ForegroundColor Red
    }
    
    Write-Host ""
    Write-Host "----------------------------------------" -ForegroundColor Gray
    Write-Host ""
}

Write-Host "All content type tests completed!" -ForegroundColor Green
