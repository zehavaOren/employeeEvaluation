interface Employee {
  employeeId: string;
  lastName: string;
  firstName: string;
  jobCode: number;
  jobName?: string |null;
  schoolCode: number;
  schoolName?: string |null;
  superiorId: number | null; // Assuming it can be null
  isSchoolManager: boolean;
  isSuperior: boolean;
  isGeneralManager: boolean;
  supervisorName?:string |null;
  currentYear?:number|0;
}

export default Employee;
