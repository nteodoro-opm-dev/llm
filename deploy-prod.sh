#!/bin/bash

# Production Deployment Script for LLM Application
# This script builds and deploys the application in production mode

echo "🚀 Starting production deployment..."

# Stop existing containers
echo "🛑 Stopping existing containers..."
docker-compose -f docker-compose.prod.yml down

# Build the application
echo "🔨 Building application..."
docker-compose -f docker-compose.prod.yml build --no-cache

# Start the services
echo "🚀 Starting services..."
docker-compose -f docker-compose.prod.yml up -d

# Wait for services to be ready
echo "⏳ Waiting for services to start..."
sleep 10

# Check health
echo "🔍 Checking application health..."
for i in {1..10}; do
    if curl -f http://localhost/health > /dev/null 2>&1; then
        echo "✅ Application is healthy!"
        break
    else
        echo "⏳ Waiting for application to be ready... (attempt $i/10)"
        sleep 5
    fi
    
    if [ $i -eq 10 ]; then
        echo "❌ Application failed to start properly"
        docker-compose -f docker-compose.prod.yml logs
        exit 1
    fi
done

echo "🎉 Production deployment completed successfully!"
echo "📊 Application is available at: http://localhost"
echo "🏥 Health check endpoint: http://localhost/health"
echo ""
echo "📋 To view logs: docker-compose -f docker-compose.prod.yml logs"
echo "🛑 To stop: docker-compose -f docker-compose.prod.yml down"
