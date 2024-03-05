const BASE_URL = 'https://localhost:44379/api/School';

export const schoolService = {

    //קבלת רשימת המסגרות
    getSchollsLostBySchoolManager: async () => {
        try {
            const response = await fetch(`${BASE_URL}/GetAllSchools`);
            if (!response.ok) {
                throw new Error('Error fetching employees');
            }
            return await response.json();
        } catch (error) {
            throw new Error(`Error fetching employees: ${error.message}`);
        }
    },

    //קבלת רשימת עובדים במסגרת
    getGeneralEmployeeEvaluation:async(schoolId)=>{
        try {
            const response = await fetch(`${BASE_URL}/GetGeneralEmployeeEvaluation/${schoolId}`);
            if (!response.ok) {
                throw new Error('Error fetching employees');
            }
            return await response.json();
        } catch (error) {
            throw new Error(`Error fetching employees: ${error.message}`);
        }
    },

    //קבלת ציונים משוקללים לעובדים במסגרת
    getWeightedEmployeeGrade:async(schoolId)=>{
        try {
            const response = await fetch(`${BASE_URL}/GetWeightedEmployeeGrade/${schoolId}`);
            if (!response.ok) {
                throw new Error('Error fetching employees');
            }
            return await response.json();
        } catch (error) {
            throw new Error(`Error fetching employees: ${error.message}`);
        }
    },

    //קבלת עובדים מצטיינים למסגרת
    getOutstandingEmployees:async(schoolId)=>{
        try {
            const response = await fetch(`${BASE_URL}/GetOutstandingEmployees/${schoolId}`);
            if (!response.ok) {
                throw new Error('Error fetching employees');
            }
            return await response.json();
        } catch (error) {
            throw new Error(`Error fetching employees: ${error.message}`);
        }
    },
    
    //קבלת העובד המצטיין הנבחר של המסגרת
    getOutstandingEmployee:async(schoolId)=>{
        try {
            const response = await fetch(`${BASE_URL}/GetOutstandingEmployee/${schoolId}`);
            if (!response.ok) {
                throw new Error('Error fetching employees');
            }
            return await response.json();
        } catch (error) {
            throw new Error(`Error fetching employees: ${error.message}`);
        }
    }
}