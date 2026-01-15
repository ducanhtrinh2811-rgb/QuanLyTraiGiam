# Test API endpoints
$baseUrl = "http://localhost:5000/api"

# Test if backend is running
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/phamnhan" -Method Get -ErrorAction Stop
    Write-Host "✓ Backend is running and accessible"
    Write-Host "Response Status: $($response.StatusCode)"
} catch {
    Write-Host "✗ Backend is NOT accessible"
    Write-Host "Error: $($_.Exception.Message)"
    exit 1
}

# Test GET suckhoe (requires auth, so we expect 401)
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/suckhoe" -Method Get -SkipHttpErrorCheck -ErrorAction SilentlyContinue
    Write-Host "✓ SucKhoe endpoint exists"
    Write-Host "  Status: $($response.StatusCode) (Expected 401 without auth)"
} catch {
    Write-Host "✗ SucKhoe endpoint error: $($_.Exception.Message)"
}

# Test all other endpoints
@("thamgap", "laodong", "khenthuong", "kyluat", "suco") | ForEach-Object {
    try {
        $response = Invoke-WebRequest -Uri "$baseUrl/$_" -Method Get -SkipHttpErrorCheck -ErrorAction SilentlyContinue
        Write-Host "✓ $_ endpoint exists (Status: $($response.StatusCode))"
    } catch {
        Write-Host "✗ $_ endpoint error: $($_.Exception.Message)"
    }
}

Write-Host "`nAll endpoints checked!"
