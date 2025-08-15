# Production Deployment Guide

This document outlines the production-ready features and deployment process for the LLM User Management Application.

## 🚀 Production Features Implemented

### Security Enhancements
- ✅ **Environment Configuration**: Proper Production/Development environment separation
- ✅ **Security Headers**: X-Frame-Options, X-Content-Type-Options, X-XSS-Protection, CSP
- ✅ **HTTPS Ready**: HSTS configuration and HTTPS redirection
- ✅ **Anti-forgery Protection**: CSRF protection for forms
- ✅ **Non-root User**: Docker container runs with non-privileged user
- ✅ **Input Validation**: Model validation and sanitization

### Performance & Reliability
- ✅ **Health Checks**: `/health` endpoint for monitoring
- ✅ **Connection Limits**: Kestrel server limits configured
- ✅ **Request Size Limits**: Protection against large payload attacks
- ✅ **Timeout Configuration**: Proper request/response timeouts
- ✅ **Database Persistence**: Persistent SQLite database with Docker volumes

### Monitoring & Logging
- ✅ **Structured Logging**: Production-appropriate log levels
- ✅ **Request Tracing**: Request ID tracking for debugging
- ✅ **Error Handling**: User-friendly error pages
- ✅ **Health Monitoring**: Container health checks

### Infrastructure
- ✅ **Multi-stage Docker Build**: Optimized container size
- ✅ **Reverse Proxy Ready**: Nginx configuration included
- ✅ **Docker Compose**: Production orchestration
- ✅ **Automated Deployment**: Scripts for easy deployment

## 📦 Deployment Methods

### Method 1: Simple Docker Deployment
```bash
# Build and run the application
docker build -t llm-app .
docker run -d --name llm-container -p 8080:8080 -v llm-data:/app/data llm-app

# Access the application
open http://localhost:8080
```

### Method 2: Production Docker Compose (Recommended)
```bash
# Deploy with Nginx reverse proxy
docker-compose -f docker-compose.prod.yml up -d

# Access the application
open http://localhost
```

### Method 3: Automated Deployment
```bash
# Linux/Mac
chmod +x deploy-prod.sh
./deploy-prod.sh

# Windows PowerShell
.\deploy-prod.ps1
```

## 🔧 Configuration Files

### Environment-Specific Settings

**appsettings.Production.json**
- Production logging levels
- Security configurations
- Performance limits
- Database path for containers

**appsettings.Development.json**
- Development-friendly settings
- Detailed error information
- Debug logging

### Security Configuration

**Program.cs Enhancements**
- Forwarded headers for proxy scenarios
- Security headers middleware
- Anti-forgery token configuration
- Health check endpoints

## 🏥 Health Monitoring

The application provides a health check endpoint at `/health` that:
- Verifies database connectivity
- Returns HTTP 200 when healthy
- Integrates with Docker health checks
- Compatible with load balancers and monitoring tools

## 🔒 Security Considerations

### Headers Implemented
- **X-Frame-Options**: Prevents clickjacking
- **X-Content-Type-Options**: Prevents MIME sniffing
- **X-XSS-Protection**: XSS protection
- **Content-Security-Policy**: Restricts resource loading
- **Referrer-Policy**: Controls referrer information

### Database Security
- SQLite database stored in persistent volume
- Proper file permissions in container
- Connection string externalized

### Container Security
- Non-root user execution
- Minimal attack surface
- Health check integration

## 🚦 Environment Variables

### Required for Production
```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=Data Source=/app/data/app.db
```

### Optional Configuration
```bash
Security__RequireHttps=true
Security__HstsMaxAge=31536000
Kestrel__Limits__MaxConcurrentConnections=100
```

## 📊 Monitoring & Maintenance

### Log Locations
- **Container Logs**: `docker logs llm-container`
- **Nginx Logs**: `/var/log/nginx/` (in nginx container)
- **Application Logs**: Console output with structured format

### Database Backup
```bash
# Backup SQLite database
docker exec llm-container cp /app/data/app.db /tmp/backup.db
docker cp llm-container:/tmp/backup.db ./backup-$(date +%Y%m%d).db
```

### Updates and Maintenance
```bash
# Update application
git pull origin main
docker-compose -f docker-compose.prod.yml build --no-cache
docker-compose -f docker-compose.prod.yml up -d

# View resource usage
docker stats llm-container
```

## 🆘 Troubleshooting

### Common Issues

**Application won't start**
```bash
# Check logs
docker logs llm-container

# Check health
curl http://localhost:8080/health
```

**Database issues**
```bash
# Check database file permissions
docker exec llm-container ls -la /app/data/

# Reset database (CAUTION: This deletes all data)
docker exec llm-container rm /app/data/app.db
docker restart llm-container
```

**Performance issues**
```bash
# Monitor resource usage
docker stats

# Check application logs for errors
docker logs llm-container --tail 100
```

## 🔄 CI/CD Integration

The application is ready for CI/CD integration with:
- Docker build process
- Health check endpoints
- Environment variable configuration
- Graceful shutdown support

### Example GitHub Actions Integration
```yaml
name: Deploy to Production
on:
  push:
    branches: [main]
jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      - name: Deploy to production
        run: ./deploy-prod.sh
```

## 📞 Support

For production support and maintenance, refer to:
- Application logs via `docker logs`
- Health check endpoint at `/health`
- This documentation for common procedures
