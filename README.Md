![Coverage](https://img.shields.io/badge/Coverage-0%25-lightgrey)

# 📝 Task Manager API

A simple ASP.NET Core Web API for managing tasks. Supports adding, retrieving, updating, and deleting tasks with task status tracking.

---

## 🚀 Features

- Add new tasks
- Get all tasks (optionally sorted by favorites)
- Update task status (e.g., ToDo ➡️ InProgress)
- Delete tasks
- Built-in Swagger UI for API testing
- Unit tests for Repository, Service, and Controller layers

---

## 🛠️ Tech Stack

- ASP.NET Core 7 Web API
- In-Memory Repository (No database)
- NUnit + Moq for testing
- Swagger/OpenAPI for API docs

---

## 📦 Getting Started

### Prerequisites
- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- Visual Studio 2022+ or VS Code

### Run the API
```bash
dotnet run --project TaskManager
```

The API will be available at: `https://localhost:5001/swagger/index.html`
or
`http://localhost:5000/swagger/index.html`

## 🎯 API Endpoints
### 📄 Get All Tasks
```bash
GET /api/tasks?sort={true|false}
```
Returns a list of tasks.
Optional query param `sort=true` will return favorites first.
### ➕ Add Task
```bash
POST /api/tasks
```
Body:
```json
{
  "name": "Design API",
  "description": "Create endpoints and test cases",
  "deadline": "2025-05-30T00:00:00",
  "isFavorite": true
}
```
### 🔍 Get Task By Id
```bash
GET /api/tasks/{id}
```
### ♻️ Update Task Status
```bash
GET PUT /api/tasks/{id}/status/{newStatus}
```
`newStatus` (enum values):
- ToDo = 0
- InDesign = 1
- InProgress = 2
- ReadyForReview = 3
- ReadyForTest = 4
- Approved = 5
### ❌ Delete Task
```bash
DELETE /api/tasks/{id}
```
## ✅ Running Tests
```bash
dotnet test
```
This will run unit tests for:
- Repository
- Service
- Controller

