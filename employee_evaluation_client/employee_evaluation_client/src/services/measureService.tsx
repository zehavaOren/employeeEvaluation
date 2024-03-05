import MeasureList from "../models/MeasureList";

const BASE_URL = 'https://localhost:44379/api/Measure';

export const measureService = {

    GetAllMeasures: async () => {
        try {
            const response = await fetch(`${BASE_URL}/GetAllMeasures`);
            if (!response.ok) {
                throw new Error('Error fetching employees');
            }
            return await response.json() as MeasureList[];;
        } catch (error) {
            throw new Error(`Error fetching employees: ${error.message}`);
        }
    },

    GetAllMeasureGrade: async () => {
        try {
            const response = await fetch(`${BASE_URL}/GetAllMeasureGrade`);
            if (!response.ok) {
                throw new Error('Error fetching employees');
            }
            return await response.json();
        } catch (error) {
            throw new Error(`Error fetching employees: ${error.message}`);
        }
    },

    InsertEmployeeEvaluationMeasure: async (evaluationDataList) => {
        try {
            for (const evaluationData of evaluationDataList) {
                const response = await fetch(`${BASE_URL}/InsertOrUpdateEmployeeEvaluationMeasure`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(evaluationData)
                });
                    return await response.json();            
            }
        } catch (error) {
            return error;
        }
    }
    
}
