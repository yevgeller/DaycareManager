# Daycare Manager Requirements

## 1. Project Overview
A web-based application for managing daycare operations, including child enrollment, parent details, and daily attendance tracking.

## 2. Technical Stack
- **Framework**: .NET 8 / ASP.NET Core
- **UI**: Blazor Interactive Server
- **Database**: SQLite
- **ORM**: Entity Framework Core

## 3. Current Data Models & Features

### 3.1 Children
* **Data**: First Name, Last Name, Date of Birth.
    - View child profiles.
* **Relationships**: 
    - Linked to multiple Parents (Many-to-Many).
    - Linked to a Classroom (via `ClassroomId`).

### 3.2 Parents
* **Data**: First Name, Last Name, Phone Number (US format only, 555-xxx-xxxx), Email.
* **Relationships**: Linked to multiple Children (Many-to-Many).
* **Requirements**:
    - Store contact information for emergencies.
    - Support realistic family structures (couples).

### 3.3 Attendance
* **Data**: Check-in Time, Check-out Time.
* **Relationships**: Linked to a Child.
* **Requirements**:
    - Log when a child arrives (Check-in).
    - Log when a child leaves (Check-out).
    - Maintain historical attendance records.

### 3.4 Classrooms
* **Data**:
    - Classroom Number/Name (e.g., "Infant 1")
    - Minimum Age (months)
    - Maximum Age (months)
* **Requirements**:
    - Manage classroom age ranges.
    - Assign children to appropriate classrooms based on age.

## 4. User Interface Structure
### 4.1 Navigation
* **Dashboard (Home)**: Central hub with quick-access cards to major sections (Children, Parents, Classrooms).
* **Sidebar Menu**: Clean navigation links to:
    - Home
    - Children
    - Parents
    - Classrooms
    - Attendance
* **Removed**: Default template pages (Counter, Weather).

### 4.2 Pages
* **Parents**: List, Details, Create, Edit, Delete.
* **Classrooms**: List, Create, Edit.
* **Children**: List, Details, Create, Edit, Delete.

## 5. Data Seeding Requirements
### 5.1 Volume & Distribution
- **Total Children**: 50
- **Total Parents**: 80 (40 couples)
- **Age Distribution**:
    - 10 children: 6 weeks - 11 months (Infant)
    - 10 children: 12 - 23 months (Toddler 1)
    - 10 children: 25 - 35 months (Toddler 2)
    - 10 children: 37 - 47 months (Pre-K 1)
    - 10 children: 48 - 59 months (Pre-K 2)

### 5.2 Family Logic
- **Couples**: Parents are seeded as couples (share Last Name).
- **Siblings**: 10 families have 2 children; 30 families have 1 child.
- **Consistency**: Every child has exactly 2 parents.

### 5.3 Naming Conventions
- **Parents**: Realistic first names (e.g., "James", "Mary").
- **Children**: Names correspond to classroom age group for easy identification:
    - Infant: First names start with A-E
    - Toddler 1: First names start with F-J
    - Toddler 2: First names start with K-O
    - Pre-K 1: First names start with P-T
    - Pre-K 2: First names start with U-Z

## 6. Proposed Features / To-Do
*(To be populated by user)*
- [ ] User Authentication (Login/Roles)?
- [ ] Billing & Invoicing?
- [ ] Medical/Allergy records?
- [ ] Staff management?
