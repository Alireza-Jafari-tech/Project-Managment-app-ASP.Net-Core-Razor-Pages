# 🗂️ Project Manager (ASP.NET Core Razor Pages)

A simple project & task management web application built with **ASP.NET Core Razor Pages** and **Entity Framework Core**.
This project represents my learning journey into building real-world, role-based systems using Razor Pages, focusing on authentication, data modeling, invite codes, and task/project management workflows.

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-8.0-green.svg)](https://learn.microsoft.com/en-us/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-red.svg)](https://www.microsoft.com/sql-server)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A hands-on learning project built with ASP.NET Core Razor Pages + EF Core + SQL Server.
It demonstrates a complete multi-role management system: managers create projects and tasks; team members join projects via invite codes and track their assigned tasks.



## 🚀 Features

### 👤 User Roles

* Manager
* Team Member

### 📁 Project Management

* Managers create & edit projects
* Generate invite codes for members
* Members join projects using invite codes

### 📌 Task Management

* Managers assign tasks to members
* Members edit their own tasks
* Status, priority, and due date tracking

### 📊 Dashboard Filtering

* Both Manager & Member dashboards include:

  * Filter by project
  * Filter by priority
  * Filter by due date / status

### 🔒 Role-based Permissions

* Secure separation of access:

  * Managers cannot edit tasks assigned to members
  * Members cannot modify project-level data



## 🛠️ Technologies

* **ASP.NET Core 8.0** (Razor Pages)
* **Entity Framework Core 8** (Code-First + Migrations)
* **SQL Server** (local DB)
* **Bootstrap 5** (UI styling)
* **Identity** (built-in authentication)
* **LINQ + EF Core queries** for filtering and dashboards



## 📸 Snapshots / Screenshots

* Manager Dashboard Page
  
  ![manager-dashboard](https://github.com/user-attachments/assets/797bc8d8-3821-4a59-82b0-2f4e11fd69fb)

* Team Member Dashboard Page
  
  ![member-dashboard](https://github.com/user-attachments/assets/b8152d21-f164-4110-8d33-3a08d6867799)

* Project List Page
  
  ![main-page](https://github.com/user-attachments/assets/9488fe82-fb0f-46df-a4d7-0a8879f48ded)

* Create Project Page
  
  ![create-project](https://github.com/user-attachments/assets/c72e5830-c28d-4ce6-a193-de508227adf2)

* Edit Project Page
  
  ![edit-project](https://github.com/user-attachments/assets/bc1bdc8e-1410-411a-bf25-4677ba3f17e7)

* Create Task Page
  
  ![create-task](https://github.com/user-attachments/assets/c3933292-44b9-4fd4-a891-6abb5d6e7f08)

* View Task Page
  
  ![view-task](https://github.com/user-attachments/assets/6dffab87-1d33-4ec6-8f87-1fcfcb9bf152)

* Edit Task Page
  
  ![edit-task](https://github.com/user-attachments/assets/ddf800b6-59fc-482f-bb7a-d56f598e1a20)

* Invite Code Page
  
![invite-member](https://github.com/user-attachments/assets/4e979aea-7057-435c-aa90-d998a795c073)

* Join Project Page
  
![join-project](https://github.com/user-attachments/assets/4b7e24a4-c932-41e9-acfa-4de27425e939)


## ⚙️ Installation & Setup

1. **Clone the repository**

   ```bash
   git clone https://github.com/Alireza-Jafari-tech/Project-Managment-app-ASP.Net-Core-Razor-Pages.git
   cd Project-Managment-app-ASP.Net-Core-Razor-Pages
   ```

2. **Update the connection string in `appsettings.json`**

   ```json
   "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ProjectManagmentDb;Trusted_Connection=True;TrustServerCertificate=True"
  }

3. **Apply migrations**

   ```bash
   dotnet ef database update
   ```

4. **Run the application**

   ```bash
   dotnet run
   ```

Then open:

👉 [http://localhost:7000](http://localhost:7000)
(or whichever port is shown in the console)



## 📂 Project Structure

```pgsql
📦 ProjectManager (root)
 ┣ 📁Pages
 │
 │ 📦 Manager
 │ ┣ 📜Dashboard.cshtml                → Manager dashboard (projects, members, tasks)
 │ ┣ 📜Dashboard.cshtml.cs
 │ ┣ 📜Edit.cshtml                     → Edit project (Manager-only)
 │ ┗ 📜Edit.cshtml.cs
 │
 │ 📦 Member
 │ ┣ 📜Dashboard.cshtml                → Member dashboard (assigned tasks)
 │ ┣ 📜Dashboard.cshtml.cs
 │ ┣ 📜Edit.cshtml                     → Edit task (Member-only)
 │ ┗ 📜Edit.cshtml.cs
 │
 │ ┗ 📜_ViewImports.cshtml / _ViewStart.cshtml
 │
 ┣ 📁Models
 │ ┣ 📜ApplicationUser.cs              → Identity user (Manager/TeamMember + profiles)
 │ ┣ 📜InvitationCode.cs               → Stores project invite codes
 │ ┣ 📜ManagerProfile.cs               → Manager additional info
 │ ┣ 📜MemberExperienceLevel.cs        → Experience level entity
 │ ┣ 📜MemberProfile.cs                → Team member additional info + assigned tasks
 │ ┣ 📜MemberTaskStatus.cs             → Task status (ToDo, In Progress, Done…)
 │ ┣ 📜Project.cs                      → Main project entity
 │ ┣ 📜ProjectMember.cs                → Mapping: user ↔ project
 │ ┣ 📜ProjectStatus.cs                → Project status (Active, Pending, Completed)
 │ ┣ 📜RequiredIfRoleAttribute.cs      → Custom validation attribute for roles
 │ ┣ 📜TaskFilter.cs                   → Used for dashboard filtering (status/priority/project/member)
 │ ┣ 📜TaskPriority.cs                 → Priority (Low/Medium/High) + CSS class
 │ ┗ 📜TeamMemberTask.cs               → Tasks assigned to members
 │
 ┣ 📁Data
 │ ┗ 📜AppDbContext.cs                 → EF Core DbContext
 │
 ┣ 📁wwwroot                           → CSS, JS, images, static files
 │
 ┣ 📜Program.cs
 ┣ 📜appsettings.json
 ┗ 📜README.md

```



## 🎯 Learning Goals

* Understand Razor Pages architecture
* Implement multi-role authentication
* Practice Model Binding & Validation
* Use EF Core for relational data modeling
* Build real dashboards with filtering
* Work with invite codes and user–project linking
* Strengthen full-stack ASP.NET Core skills



## 🧑‍💻 Usage

* Managers create projects & tasks
* Members join projects with invite codes
* Members view and update their tasks
* Both roles use dashboards to filter and manage their work



## 📝 License

This project is licensed under the **MIT License**.
See the **LICENSE** file for details.



## 🤝 Contributing

This is a learning project, but feel free to fork it, improve it, or submit pull requests.
Ideas, suggestions, and contributions are always welcome!
