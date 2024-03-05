import GeneralEmployeeEvaluation from "../models/GeneralEmployeeEvaluation";
import EmployeeEvaluationStatus from '../models/EmployeeEvaluatioStatus'
import OutstandingEmployees from "../models/OutstandingEmployees";
const BASE_URL = 'https://localhost:44379/api/Employee';

export const employeeService = {

  getEmployees: async () => {
    try {
      const response = await fetch(`${BASE_URL}`);
      if (!response.ok) {
        throw new Error('Error fetching employees');
      }
      return await response.json();
    } catch (error) {
      throw new Error(`Error fetching employees: ${error.message}`);
    }
  },

  getEmployeeById: async (id: string) => {
    try {
      const response = await fetch(`${BASE_URL}/GetEmployeeById/${id}`);
      if (!response.ok) {
        return (`HTTP error! Status: ${response.status}`);
      }
      if (response.status === 204) {
        return (response);
      }
      const employeeData = await response.json();
      return employeeData;

    } catch (error) {
      return (`Error fetching employee by id: ${error.message}`);
    }
  },

  GetEmployeesBySupervisorId: async (supervisorId: string) => {
    try {
      const response = await fetch(`${BASE_URL}/GetEmployeesBySupervisorId/${supervisorId}`);

      if (!response.ok) {
        return (`HTTP error! Status: ${response.status}`);
      }
      if (response.status === 204) {
        return (response);
      }
      const employeeData = await response.json();
      return employeeData;

    } catch (error) {
      return (`Error fetching employees by supervisor id: ${error.message}`);
    }
  },

  insertGeneralEmployeeEvaluation: async (evaluationData: GeneralEmployeeEvaluation) => {
    try {
      const response = await fetch(`${BASE_URL}/InsertGeneralEmployeeEvaluation`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(evaluationData)
      });
        return await response.json(); 
    }
    catch (error) {
      return error;
    }

  },

  checkEmployeeExists: async (id: string): Promise<boolean> => {
    try {
      const response = await fetch(`${BASE_URL}/CheckEmployeeExists/${id}`);
      if (response.ok) {
        const data = await response.json();
        return data; 
      } else {
        console.error('Error checking employee existence:', response.statusText);
        return false;
      }
    } catch (error) {
      console.error('Error checking employee existence:', error);
      return false;
    }
  },

  GetEmployeeEvaluationStatuses: async () => {
    try {
      const response = await fetch(`${BASE_URL}/GetEmployeeEvaluationStatuses`);
      if (!response.ok) {
        throw new Error('Error fetching employees');
      }
      return await response.json() as EmployeeEvaluationStatus[];;
    } catch (error) {
      throw new Error(`Error fetching employees: ${error.message}`);
    }
  },

  GetWeightedEmployeeScore: async (supervisorId: string) => {

    try {
      const response = await fetch(`${BASE_URL}/GetWeightedEmployeeScore/${supervisorId}`);
      if (!response.ok) {
        return (`HTTP error! Status: ${response.status}`);
      }
      if (response.status === 204) {
        return (response);
      }
      const employeeData = await response.json();
      return employeeData;

    } catch (error) {
      return (`Error fetching employees by supervisor id: ${error.message}`);
    }
  },

  GetOutstandingEmployees: async (supervisorId: string) => {
    try {
      const response = await fetch(`${BASE_URL}/GetOutstandingEmployees/${supervisorId}`);
      if (!response.ok) {
        throw new Error('Error fetching employees');
      }
      return await response.json() as OutstandingEmployees[];
    } catch (error) {
      throw new Error(`Error fetching employees: ${error.message}`);
    }
  },

  UpdateOutstandingEmployee: async (evaluation: GeneralEmployeeEvaluation) => {
    try {
      const response = await fetch(`${BASE_URL}/UpdateOutstandingEmployee`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(evaluation),
      });

      const result: boolean = await response.json();
      return result;
    } catch (error) {
      return false;
    }
  },

};
