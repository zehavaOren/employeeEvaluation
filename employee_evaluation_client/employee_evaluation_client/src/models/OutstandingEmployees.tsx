interface OutstandingEmployees {
    employeeId: string;
    lastName: string;
    firstName: string;
    jobName: string;
    schoolCode: number;
    schoolName: string;
    superiorId: string;
    supervisorName?: string | null;
    currentYear: number;
    weightedMeasureGrade: number;
    generalEvaluation: string;
    evaluationDocument1?: string | null;
    evaluationDocument2?: string | null;
    evaluationDocument3?: string | null;
    evaluationStatusCode?: number|null;
}

export default OutstandingEmployees;
