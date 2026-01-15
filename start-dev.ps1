# Start Backend and WPF Frontend
Write-Host "Starting Backend and WPF Frontend..." -ForegroundColor Green

# Start Backend in background
Write-Host "Starting Backend on http://localhost:5000..." -ForegroundColor Cyan
Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "run --project ./backend-csharp/PrisonManagement.csproj"

# Wait a bit for backend to start
Start-Sleep -Seconds 3

# Start WPF Frontend
Write-Host "Starting WPF Frontend..." -ForegroundColor Cyan
dotnet run --project ./wpf-frontend/PrisonManagement/PrisonManagement.csproj

Write-Host "Done!" -ForegroundColor Green
