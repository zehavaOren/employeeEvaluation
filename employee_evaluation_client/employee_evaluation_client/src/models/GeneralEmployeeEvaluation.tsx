interface GeneralEmployeeEvaluation {
    employeeId: string;
    evaluationYear?: number| null;
    weightedMeasureGrade?: number| null;
    generalEvaluation?: string| null;
    evaluationDocument1?: string | null;
    evaluationDocument2?: string | null;
    evaluationDocument3?: string | null;
    outstandingEmployeeRating?: number | null;
    uniqueInitiative?: string | null;
    reasonSelectedRating?: string | null;
    participateRatingDecision?: string | null;
    evaluationStatusCode?: number | null;
}

export default GeneralEmployeeEvaluation;
