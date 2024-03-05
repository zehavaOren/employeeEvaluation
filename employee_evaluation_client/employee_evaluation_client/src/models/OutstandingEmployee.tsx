interface OutstandingEmployee{
    employeeId: string;
    lastName: string;
    firstName: string;
    jobName: string;
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
    outstandingEmployeeRating:number;
    uniqueInitiative:string;
    reasonSelectedRating:string;
    participateRatingDecision:string;
}
export default OutstandingEmployee;