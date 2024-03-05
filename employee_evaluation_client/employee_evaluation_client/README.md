### Employee Evaluation System

---

### Purpose

The Employee Evaluation System aims to facilitate the annual evaluation process for all employees within the organization. Additionally, it allows for the selection of outstanding employees within specific organizational frameworks. The system includes various phases and user roles to manage and evaluate employee performance effectively.

### System Overview

#### Phases:
1. **Phase A - Annual Employee Evaluation:**
    - Supervisors evaluate their assigned employees based on predefined assessment measures.
2. **Phase B - Outstanding Employees in the Framework:**
    - Framework managers select outstanding employees within their respective frameworks.

#### User Roles:
1. **Phase A Users:** Supervisors
2. **Phase B Users:** Framework Managers
3. **Data Reports Users:** General Managers

### System Screens

1. **Login Screen:**
    - Users are required to enter a PIN to access the system.
    - Permissions are based on user roles as described in the "System Users" section.

2. **Annual Employee Evaluation Screen:**
    - Supervisors evaluate their assigned employees using predefined assessment measures.
    - Allows supervisors to enter evaluation data and attached documents.
    - Action buttons include "Save Data" to save assessment information and update evaluation statuses.

3. **Total Outstanding Employees in the Framework:**
    - Framework managers select outstanding employees within their frameworks.
    - Allows managers to enter employee ratings, unique initiatives, and reasons for selected ratings.
    - Action buttons include "Save Data" to save outstanding employee ratings and update evaluation statuses.

4. **General Evaluation Data Screen:**
    - Provides access to general evaluation reports, including:
        - General Employee Evaluation Report
        - Weighted Employee Score Report
        - General Outstanding Employees Report
        - Outstanding Employee Report
        - Report to be updated in the Melam system.

### Technical Details

- **Database:** SQL
- **Server:** C# with ADO.NET
- **Client:** React with the Ant Design library

### System Management

- **Loading Employee File:**
    - Allows for the loading of employee data from an Excel file generated from the Melam system.
    - New employee records are added, and existing records are updated during the loading process.

---

This README provides an overview of the Employee Evaluation System, including its purpose, phases, user roles, system screens, technical details, and system management functionalities. For detailed implementation instructions, please refer to the system documentation and codebase.