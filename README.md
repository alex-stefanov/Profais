# Profais

**Profais** is a .NET web application built with Entity Framework (EF) that uses SQL Server Management Studio (SSMS). It is designed to streamline project and worker management for **Profix**, a company specializing in **water distribution, irrigation systems (поливни системи), and hydrofores (хидрофори)**. The application aims to modernize and simplify the management processes, enhancing efficiency and convenience for both workers, clients and managers.

## Features

### For the Company:
- **Project Management**: Efficiently manage projects and track their progress.
- **Task Assignment**: Assign specific tasks to workers.
- **Worker Management**: Simplify scheduling and improve task delegation.

### For Clients:
- **Project Requests**: Submit project requests directly to the firm with ease.

## User Roles

The application supports five distinct user roles, reflecting the organizational structure:
1. **Clients**: Submit project requests.
2. **Managers**: Oversee projects and manage teams.
3. **Workers**: Access assigned tasks and update progress.
4. **Specialists**: Handle specialized tasks requiring expertise.
5. **Admin**: Manage users, system configurations, and oversee operations.

## Educational Purpose

This project is my **final-year project** for the **C# Web module at [SoftUni](https://softuni.bg/) (Software University)**. It incorporates best practices in software development to meet academic standards and produce high-quality, maintainable code. 

### Practices and Architectures Used:
- **MVC Architecture**: Implements the Model-View-Controller design pattern for better separation of concerns.
- **Unit Testing**: Ensures reliability and robustness through comprehensive testing.
- **Design Patterns**: Adopts industry-standard design patterns for scalability and maintainability.
- **Code First Approach**: Uses code first for creating the database

## Purpose and Vision

This project is:
- **Personal and Practical**: Created to address the real-world needs of [Profix](https://profix.bg/), owned by Alex Stefanov's father, by solving the challenge of efficient worker management.
- **Future-Ready**: With further development, it has the potential to become a fully functional application that adds significant value to Profix's operations.

## Technologies Used

- **ASP.NET Core**
- **Entity Framework (EF)**
- **SQL Server (SSMS)**

## Database Design

The database for Profais is built using **Entity Framework** and represents the relationships and structure required for managing projects, tasks, workers, and client requests effectively. Below is the database diagram:

![Database Diagram](https://cdn.discordapp.com/attachments/776883257596968991/1316519520449462342/image.png?ex=675b57d9&is=675a0659&hm=c5479c0e0db751d808aac837b0e8df21b779ff2b5dc8015927b2cbe04fb6c0c2&)

### Key Entities (Excluding Users and Roles)

The database contains **11 key entities** (excluding `AspNetUsers`, `AspNetRoles`, and related user-role mappings):

1. **ProjectsRequests**: Stores client-submitted project requests.
2. **Projects**: Represents the projects managed by the firm.
3. **Tasks**: Contains tasks assigned under specific projects.
4. **UserProjects**: Tracks the contributors working on each project.
5. **UsersTasks**: Maps workers to their assigned tasks.
6. **WorkerRequests**: Holds requests from workers for project involvement.
7. **SpecialistRequests**: Tracks requests for specialists.
8. **Materials**: Stores the materials required for completing tasks.
9. **TasksMaterials**: Tracks the materials used for specific tasks.
10. **Penalties**: Defines penalties that can be assigned to users.
11. **UsersPenalties**: Maps penalties to specific users.

### Connections and Relationships

- **ProjectsRequests** → **Projects**: A project is linked to its initial client request.
- **Projects** → **Tasks**: A project can have multiple tasks associated with it.
- **Tasks** → **TasksMaterials**: A task may require multiple materials.
- **TasksMaterials** → **Materials**: Links tasks to the specific materials used.
- **UserProjects**: Tracks which workers contribute to specific projects.
- **UsersTasks**: Maps tasks to workers for assignment and progress tracking.
- **Penalties** → **UsersPenalties**: Assigns penalties to users for tracking performance or issues.
- **WorkerRequests** and **SpecialistRequests**: Manage worker and specialist involvement requests.

This structure supports efficient management of project workflows, task delegation, and resource allocation while ensuring traceability and accountability for all activities within the firm.

## Contact

For inquiries or collaborations, feel free to reach out:

- **Email**: rlgalexbgto@gmail.com
- **Author**: Alex Stefanov
