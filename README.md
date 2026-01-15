# Daycare Manager

A web-based application for managing daycare operations, including child enrollment, parent details, and daily attendance tracking.

## Technical Stack
- **Framework**: .NET 8 / ASP.NET Core
- **UI**: Blazor Interactive Server
- **Database**: SQLite
- **ORM**: Entity Framework Core

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Installation

1.  Clone the repository:
    ```bash
    git clone https://github.com/yevgeller/DaycareManager.git
    cd DaycareManager
    ```

2.  Run the application:
    ```bash
    dotnet run
    ```

    Or for hot-reloading during development:
    ```bash
    dotnet watch
    ```

The application uses an SQLite database (`app.db`). It will be automatically created and seeded with sample data (children, parents, classrooms) on the first run.

## Requirements & Features

### 1. Children
* **Data**: First Name, Last Name, Date of Birth.
* **Features**: View child profiles.
* **Relationships**:
    - Linked to multiple Parents (Many-to-Many).
    - Linked to a Classroom (via `ClassroomId`).

### 2. Parents
* **Data**: First Name, Last Name, Phone Number, Email.
* **Relationships**: Linked to multiple Children.
* **Features**:
    - Store contact information for emergencies.
    - Support realistic family structures (couples).

### 3. Attendance
* **Data**: Check-in Time, Check-out Time.
* **Relationships**: Linked to a Child.
* **Features**:
    - Log when a child arrives (Check-in).
    - Log when a child leaves (Check-out).
    - Maintain historical attendance records.

### 4. Classrooms
* **Data**: Classroom Number/Name (e.g., "Infant 1"), Age Ranges.
* **Features**:
    - Manage classroom age ranges.
    - Assign children to appropriate classrooms based on age.

## Data Seeding
The application automatically seeds:
- **50 Children** distributed across age groups (Infant to Pre-K).
- **80 Parents** (40 couples) with realistic naming.
- **Classrooms** with defined age ranges.

## Project Structure
- `Components/Pages`: Blazor pages (Children, Parents, Classrooms, Attendance).
- `Data`: DB Context and Initializer.
- `Models`: Entity definitions.

---
*Created for the Daycare Manager project.*
