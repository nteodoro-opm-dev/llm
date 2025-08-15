# Production Deployment Script for LLM Application (PowerShell)
# This script builds and deploys the application in production mode

Write-Host "🚀 Starting production deployment..." -ForegroundColor Green

# Stop existing containers
Write-Host "🛑 Stopping existing containers..." -ForegroundColor Yellow
docker-compose -f docker-compose.prod.yml down

# Build the application
Write-Host "🔨 Building application..." -ForegroundColor Yellow
docker-compose -f docker-compose.prod.yml build --no-cache

# Start the services
Write-Host "🚀 Starting services..." -ForegroundColor Yellow
docker-compose -f docker-compose.prod.yml up -d

# Wait for services to be ready
Write-Host "⏳ Waiting for services to start..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# Check health
Write-Host "🔍 Checking application health..." -ForegroundColor Yellow
$maxAttempts = 10
$attempt = 1

while ($attempt -le $maxAttempts) {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost/health" -UseBasicParsing -TimeoutSec 5
        if ($response.StatusCode -eq 200) {
            Write-Host "✅ Application is healthy!" -ForegroundColor Green
            break
        }
    }
    catch {
        Write-Host "⏳ Waiting for application to be ready... (attempt $attempt/$maxAttempts)" -ForegroundColor Yellow
        Start-Sleep -Seconds 5
        $attempt++
    }
    
    if ($attempt -gt $maxAttempts) {
        Write-Host "❌ Application failed to start properly" -ForegroundColor Red
        docker-compose -f docker-compose.prod.yml logs
        exit 1
    }
}

Write-Host "🎉 Production deployment completed successfully!" -ForegroundColor Green
Write-Host "📊 Application is available at: http://localhost" -ForegroundColor Cyan
Write-Host "🏥 Health check endpoint: http://localhost/health" -ForegroundColor Cyan
Write-Host ""
Write-Host "📋 To view logs: docker-compose -f docker-compose.prod.yml logs" -ForegroundColor Gray
Write-Host "🛑 To stop: docker-compose -f docker-compose.prod.yml down" -ForegroundColor Gray
