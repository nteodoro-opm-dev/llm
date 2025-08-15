# Azure AD (Office 365) Authentication Setup Guide

This guide walks you through setting up Azure AD authentication for your ASP.NET Core application.

## 🚀 Prerequisites

- **Azure AD Tenant**: You need access to an Azure AD tenant (Office 365 subscription)
- **Application Registration**: Admin rights to register applications in Azure AD
- **Development Environment**: .NET 9.0 and the configured application

## 📋 Step 1: Register Application in Azure AD

### 1.1 Navigate to Azure Portal
1. Go to [Azure Portal](https://portal.azure.com)
2. Sign in with your Office 365 account
3. Navigate to **Azure Active Directory** > **App registrations**

### 1.2 Create New Application Registration
1. Click **"New registration"**
2. Fill in the application details:
   - **Name**: `LLM User Management App`
   - **Supported account types**: 
     - For single tenant: "Accounts in this organizational directory only"
     - For multi-tenant: "Accounts in any organizational directory"
   - **Redirect URI**: 
     - Type: `Web`
     - URL: `https://localhost:8080/signin-oidc` (for development)
     - URL: `https://yourdomain.com/signin-oidc` (for production)

### 1.3 Note Application Details
After registration, copy these values:
- **Application (client) ID**: `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx`
- **Directory (tenant) ID**: `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx`

## 🔐 Step 2: Create Client Secret

### 2.1 Generate Secret
1. In your app registration, go to **Certificates & secrets**
2. Click **"New client secret"**
3. Add description: `LLM App Secret`
4. Set expiration: `24 months` (or as per your policy)
5. Click **"Add"**

### 2.2 Copy Secret Value
**⚠️ IMPORTANT**: Copy the secret value immediately - it won't be shown again!
- **Client Secret**: `your-secret-value-here`

## 🔧 Step 3: Configure Application Permissions

### 3.1 API Permissions
1. Go to **API permissions**
2. Click **"Add a permission"**
3. Select **Microsoft Graph**
4. Choose **Delegated permissions**
5. Add these permissions:
   - `User.Read` (to read user profile)
   - `User.ReadBasic.All` (to read basic user info)
   - `Directory.Read.All` (optional - for advanced scenarios)

### 3.2 Grant Admin Consent
1. Click **"Grant admin consent for [Your Organization]"**
2. Confirm the consent

## ⚙️ Step 4: Configure Application Settings

### 4.1 Update appsettings.json
Replace the placeholder values in your configuration files:

**appsettings.json:**
```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "yourdomain.onmicrosoft.com",
    "TenantId": "your-tenant-id-here",
    "ClientId": "your-client-id-here",
    "ClientSecret": "your-client-secret-here",
    "CallbackPath": "/signin-oidc",
    "SignedOutCallbackPath": "/signout-callback-oidc"
  }
}
```

### 4.2 Environment Variables (Recommended for Production)
Instead of storing secrets in config files, use environment variables:

```bash
export AzureAd__TenantId="your-tenant-id"
export AzureAd__ClientId="your-client-id"  
export AzureAd__ClientSecret="your-client-secret"
export AzureAd__Domain="yourdomain.onmicrosoft.com"
```

### 4.3 Docker Environment Variables
For Docker deployment, add to your docker run command:
```bash
docker run -d \
  -e AzureAd__TenantId="your-tenant-id" \
  -e AzureAd__ClientId="your-client-id" \
  -e AzureAd__ClientSecret="your-client-secret" \
  -e AzureAd__Domain="yourdomain.onmicrosoft.com" \
  -p 8080:8080 \
  llm-app-prod
```

## 🌐 Step 5: Configure Redirect URIs

### 5.1 Development URLs
Add these redirect URIs in Azure AD app registration:
- `http://localhost:5000/signin-oidc`
- `http://localhost:8080/signin-oidc`
- `https://localhost:7000/signin-oidc`

### 5.2 Production URLs
For production deployment:
- `https://yourdomain.com/signin-oidc`
- `https://your-app.azurewebsites.net/signin-oidc`

### 5.3 Logout URLs
Add logout URLs in **Authentication** > **Front-channel logout URL**:
- `http://localhost:8080/signout-callback-oidc`
- `https://yourdomain.com/signout-callback-oidc`

## 🔍 Step 6: Testing Authentication

### 6.1 Start Application
```bash
dotnet run
# or
docker run -p 8080:8080 llm-app-prod
```

### 6.2 Test Authentication Flow
1. Navigate to `http://localhost:8080`
2. Click **"Sign In with Office 365"**
3. You should be redirected to Microsoft login
4. Enter your Office 365 credentials
5. Grant consent if prompted
6. You should be redirected back to the application

### 6.3 Verify User Information
1. After successful login, check the navigation bar
2. Click on your user dropdown
3. Select **"View Profile"** to see user details from Azure AD

## 🛠️ Step 7: Advanced Configuration

### 7.1 Custom Claims Mapping
Add custom claims in Azure AD app registration:
1. Go to **Token configuration**
2. Click **"Add optional claim"**
3. Select token type: **ID**
4. Add claims like: `family_name`, `given_name`, `jobTitle`, `department`

### 7.2 Group Claims
To include group memberships:
1. In **Token configuration** > **Add groups claim**
2. Select **Security groups**
3. Configure as needed for your authorization requirements

### 7.3 Multi-Tenant Configuration
For multi-tenant apps:
1. Change **Supported account types** to **"Accounts in any organizational directory"**
2. Update TenantId in config to `"common"` or `"organizations"`

## 🚨 Troubleshooting

### Common Issues

**Error: "AADSTS50011: The reply URL specified in the request does not match"**
- **Solution**: Ensure redirect URI in Azure AD matches exactly (including http/https)

**Error: "AADSTS700051: response_type 'code' is not enabled for the application"**
- **Solution**: In Azure AD app registration, ensure **"Access tokens"** and **"ID tokens"** are checked

**Error: "AADSTS50020: User account from identity provider does not exist in tenant"**
- **Solution**: Ensure user exists in the Azure AD tenant or enable guest user access

**Error: "Client secret expired"**
- **Solution**: Generate new client secret in Azure AD and update configuration

### Debug Authentication
Enable detailed logging in appsettings.Development.json:
```json
{
  "Logging": {
    "LogLevel": {
      "Microsoft.AspNetCore.Authentication": "Debug",
      "Microsoft.AspNetCore.Authorization": "Debug"
    }
  }
}
```

## 🔒 Security Best Practices

### 1. Secret Management
- **Never commit secrets to source control**
- Use Azure Key Vault for production secrets
- Rotate client secrets regularly

### 2. Redirect URI Security
- Use HTTPS in production
- Limit redirect URIs to necessary domains only
- Validate all redirect URIs

### 3. Token Validation
- Ensure proper token validation
- Set appropriate token lifetimes
- Implement proper session management

### 4. Logging and Monitoring
- Monitor authentication failures
- Log security events
- Set up alerts for suspicious activities

## 📚 Additional Resources

- [Azure AD Documentation](https://docs.microsoft.com/en-us/azure/active-directory/)
- [Microsoft Identity Web Library](https://docs.microsoft.com/en-us/azure/active-directory/develop/microsoft-identity-web)
- [ASP.NET Core Authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/)
- [Azure AD App Registration Guide](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app)

## 🆘 Support

If you encounter issues:
1. Check the troubleshooting section above
2. Review Azure AD application logs
3. Enable debug logging in the application
4. Check the Microsoft Identity Web GitHub repository for known issues
