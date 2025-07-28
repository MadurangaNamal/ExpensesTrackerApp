# 💰 Expenses Tracker – ASP.NET Core MVC

An **Expense Tracking Application** built with **ASP.NET Core MVC** and **Entity Framework Core**. This project enables users to **record, categorize, and analyze** their personal expenses through a clean and user-friendly web interface.

---

## 🔍 Features

- 🧾 Add, edit, and delete expense entries  
- 🗂 Categorize expenses for better organization  
- 📊 View monthly and category-wise summaries  
- 📱 Responsive design with Bootstrap  
- 🧱 Code-first approach with Entity Framework Core  

---

## 🔑 Authentication

✅ **Google SSO Authentication Enabled** 🚀

<p align="center">
  <img src="https://raw.githubusercontent.com/github/explore/main/topics/google/google.png" alt="Google Logo" width="60"/>
</p>


Supports **secure, one-click sign-in with your google account**, allowing you to access your expenses from anywhere **without managing password hassle**.

---
## 🛠 Tech Stack

- ASP.NET Core MVC (.NET 6 or later)  
- Entity Framework Core  
- MS SQL Server
- Bootstrap 5  


## 🚀 Quick Start with Docker

<p align="center">
  <img src="https://www.docker.com/wp-content/uploads/2022/03/Moby-logo.png" alt="Docker Logo" width="60"/>
</p>
### Prerequisites

- [Docker](https://www.docker.com/get-started) installed on your machine
- [Docker Compose](https://docs.docker.com/compose/install/) (included with Docker Desktop)

### 1. Clone the Repository

```bash
git clone https://github.com/MadurangaNamal/ExpensesTracker.git
cd ExpensesTracker
```

### 2. Configure Environment Variables

Create a `.env` file in the root directory (same level as `docker-compose.yml`):

```env
# Database Configuration
SA_PASSWORD=YourStrong@Passw0rd

# Google Authentication (Optional - for Google SSO)
GOOGLE_CLIENT_ID=your-google-client-id-here
GOOGLE_CLIENT_SECRET=your-google-client-secret-here
```
### 3. Set Up Google OAuth (Optional)

If you want to enable Google Sign-In:

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project or select existing one
3. Enable Google+ API
4. Create OAuth 2.0 credentials
5. Add authorized redirect URIs:
   - `http://localhost:5206/signin-google`
   - `https://localhost:5206/signin-google`
6. Copy Client ID and Client Secret to your `.env` file

### 4. Run the Application

```bash
# Build and start all services
docker-compose up --build

# Or run in detached mode
docker-compose up --build -d
```

### 5. Access the Application

Open your browser and navigate to:
- **Main Application**: [http://localhost:5206](http://localhost:5206)
- **Database**: SQL Server is available on `localhost:1433`

### 6. Stop the Application

```bash
# Stop all services
docker-compose down

# Stop and remove volumes (clears database data)
docker-compose down -v
```
