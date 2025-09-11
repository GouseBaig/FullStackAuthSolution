# 🔐 Full Stack JWT Authentication Demo (ASP.NET Core Web API + MVC Razor)

![.NET Logo](https://upload.wikimedia.org/wikipedia/commons/e/ee/.NET_Core_Logo.svg)

A **Full Stack demo application** showcasing **JWT (JSON Web Token) authentication** using:

- **ASP.NET Core Web API** as backend (JWT token issuing & validation)
- **ASP.NET Core MVC (Razor)** as frontend (login + profile display)

This project demonstrates how to secure APIs with JWT, consume them from an MVC frontend, and handle authentication/authorization in a clean way.

---

## 🚀 Features

- ✅ **JWT Authentication** with `Microsoft.AspNetCore.Authentication.JwtBearer`
- ✅ **In-memory user store** (`test / 1234` demo credentials)
- ✅ **Login API** returning JWT token
- ✅ **Protected Profile API** (requires Bearer token)
- ✅ **MVC Razor Frontend** with login form & profile page
- ✅ **HttpClientFactory** for API calls
- ✅ **Session storage** for JWT tokens
- ✅ **Model validation** with DataAnnotations
- ✅ **Swagger/OpenAPI** for backend testing

---

## 📂 Project Structure
  FullStackAuthSolution/
  
    │── FullStackAuth.API/ # Backend Web API (JWT Authentication)
    
    │── FullStackAuth.Web/ # Frontend MVC (Razor Views)


---

## 🛠️ Technologies & Concepts

- **ASP.NET Core 7/8 Web API**
- **ASP.NET Core MVC (Razor Views)**
- **JWT (JSON Web Token) Authentication**
- **Swagger / OpenAPI**
- **HttpClientFactory**
- **Session Management**
- **Dependency Injection**
- **Bootstrap 5** (UI styling)


---

## ⚡ Getting Started

### Prerequisites
- [.NET 7/8 SDK](https://dotnet.microsoft.com/en-us/download)
- Visual Studio 2022 / VS Code
- Git

### Clone Repository
```bash
git clone https://github.com/GouseBaig/FullStackAuthSolution.git
cd FullStackAuthSolution
```

---

### Run Backend (FullStackAuth.API)
```bash
dotnet run --project FullStackAuth.API --urls "http://localhost:7000"
```

---

### Run Frontend (FullStackAuth.Web)
```bash
dotnet run --project FullStackAuth.Web --urls "http://localhost:5001"
```

---

### Demo Credentials

Username: test

Password: 1234

