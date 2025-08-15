# LLM - ASP.NET Core Razor Pages Application

A modern ASP.NET Core Razor Pages web application built with .NET 9, featuring SQLite database integration and Docker support.

## 🎯 Features

- **ASP.NET Core 9.0** - Latest .NET framework
- **Razor Pages** - Server-side rendering with clean architecture
- **SQLite Database** - Lightweight, file-based database with Entity Framework Core
- **Docker Support** - Fully containerized for easy deployment
- **Bootstrap UI** - Responsive, modern interface
- **Sample Data** - Pre-seeded with example users

## 📋 Prerequisites

Before you begin, ensure you have the following installed on your system:

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (for containerization)
- A code editor like [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio](https://visualstudio.microsoft.com/)

## 🚀 Getting Started

### Local Development Setup

1. **Clone or navigate to the project directory:**
   ```bash
   cd c:\Projects\llm
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Build the application:**
   ```bash
   dotnet build
   ```

4. **Run the application locally:**
   ```bash
   dotnet run
   ```

5. **Access the application:**
   - Open your browser and navigate to `https://localhost:7000` or `http://localhost:5000`
   - The exact URLs will be displayed in the terminal when the application starts
   - Visit `/Users` to see the database in action

## 🗄️ Database Information

The application uses **SQLite** as its database, which provides:

- **File-based storage** - No server setup required
- **Automatic database creation** - Database is created on first run
- **Sample data** - Pre-seeded with example users
- **Entity Framework Core** - Modern ORM for data access

### Database Features:
- User management with basic CRUD operations
- Email uniqueness constraints
- Automatic timestamps
- Data validation attributes

### Database File Location:
- **Local Development**: `app.db` in the project root
- **Docker Container**: `/app/data/app.db`

## 🐳 Docker Deployment

### Building and Running with Docker

1. **Build the Docker image:**
   ```bash
   docker build -t llm-app .
   ```

2. **Run the container:**
   ```bash
   docker run -d -p 8080:8080 --name llm-container llm-app
   ```

3. **Access the dockerized application:**
   - Open your browser and navigate to `http://localhost:8080`
   - Visit `http://localhost:8080/Users` to see the database functionality

### Persistent Database Storage

To persist database data between container restarts, use a volume:

```bash
docker run -d -p 8080:8080 -v llm-data:/app/data --name llm-container llm-app
```

This creates a named volume `llm-data` that persists the SQLite database.

### Docker Management Commands

**View running containers:**
```bash
docker ps
```

**View container logs:**
```bash
docker logs llm-container
```

**Stop the container:**
```bash
docker stop llm-container
```

**Start the container:**
```bash
docker start llm-container
```

**Remove the container:**
```bash
docker rm llm-container
```

**Remove the image:**
```bash
docker rmi llm-app
```

## 📦 Publishing to Container Registry

### Tag and Push to Docker Hub

1. **Tag your image:**
   ```bash
   docker tag llm-app your-username/llm-app:latest
   ```

2. **Login to Docker Hub:**
   ```bash
   docker login
   ```

3. **Push the image:**
   ```bash
   docker push your-username/llm-app:latest
   ```

### Deploy from Registry

```bash
docker run -d -p 8080:8080 --name llm-app your-username/llm-app:latest
```

## 🏗️ Project Structure

```
llm/
├── Data/               # Database context and configuration
│   └── ApplicationDbContext.cs
├── Models/             # Data models and entities
│   └── User.cs         # User entity
├── Pages/              # Razor Pages
│   ├── Index.cshtml    # Home page
│   ├── Users.cshtml    # Users list page
│   ├── Privacy.cshtml  # Privacy page
│   └── Shared/         # Shared layouts and partials
├── wwwroot/            # Static files (CSS, JS, images)
├── Properties/         # Launch settings
├── appsettings.json    # Application configuration
├── Program.cs          # Application entry point
├── llm.csproj         # Project file
├── Dockerfile         # Docker configuration
├── .dockerignore      # Docker ignore file
└── README.md          # This file
```

## ⚙️ Configuration

### Environment Variables

The application uses the following environment variables in Docker:

- `ASPNETCORE_URLS=http://+:8080` - Specifies the binding address and port
- `ASPNETCORE_ENVIRONMENT=Production` - Sets the environment (automatically set in Docker)

### Application Settings

Configuration can be modified in:
- `appsettings.json` - Base configuration
- `appsettings.Development.json` - Development-specific settings
- `appsettings.Production.json` - Production-specific settings (create if needed)

## 🔧 Development Commands

**Watch for changes during development:**
```bash
dotnet watch run
```

**Run tests (if any):**
```bash
dotnet test
```

**Clean build artifacts:**
```bash
dotnet clean
```

**Publish for deployment:**
```bash
dotnet publish -c Release -o ./publish
```

## 🐛 Troubleshooting

### Common Issues

1. **Port already in use:**
   - Change the port mapping: `docker run -d -p 8081:8080 --name llm-container llm-app`

2. **Container won't start:**
   - Check logs: `docker logs llm-container`
   - Verify the image was built successfully: `docker images`

3. **Application not accessible:**
   - Ensure Docker Desktop is running
   - Check if the container is running: `docker ps`
   - Verify port mapping is correct

### Docker Issues

**Container stops immediately:**
```bash
docker logs llm-container
```

**Rebuild image after changes:**
```bash
docker stop llm-container
docker rm llm-container
docker build -t llm-app .
docker run -d -p 8080:8080 --name llm-container llm-app
```

## 📝 Notes

- The application runs on port 8080 inside the Docker container
- Data protection keys are stored in `/root/.aspnet/DataProtection-Keys` in the container
- For production deployments, consider using a persistent volume for data protection keys
- HTTPS is disabled in the Docker configuration for simplicity

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

---

**Built with ❤️ using ASP.NET Core 9.0**
