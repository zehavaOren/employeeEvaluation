interface EmployeeData {
  evaluationStatusCode: number;
  employeeId: string;
  lastName: string;
  firstName: string;
  jobName: string;
  schoolName: string;
  superiorId:string;
  supervisorName:string;
  CurrentYear:number;
  weightedMeasureGrade?:number;
  assessmentStatus?:string;
}

export default EmployeeData;
