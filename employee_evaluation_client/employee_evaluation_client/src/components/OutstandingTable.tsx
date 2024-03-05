import { Button, Spin, Table, Tooltip } from "antd";
import React, { useEffect, useState } from "react"
import { UpdateOutstandingEmployees } from "./UpdateOutstandingEmployees.tsx";
import GeneralEmployeeEvaluation from "../models/GeneralEmployeeEvaluation.tsx";
import { employeeService } from "../services/employeeService.tsx";
import { exportToExcelService } from "../services/exportToExcelService.tsx";
import Message from './Message.tsx';


export const OutstandingTable = ({ outstandingEmployees, onUpdateClick, schoolDetails, outstandingStatuses, updatedGrades }) => {
    const [visible, setVisible] = useState(false);
    const [selectedEmployeeId, setSelectedEmployeeId] = useState(null);
    const [selectedGrades, setSelectedGrades] =  useState<number[]>([]);
    const [messageInfo, setMessageInfo] = useState({ message: '', type: '' });
    const [loading, setLoading] = useState(false);
    const MAX_URL_LENGTH = 10;
    const { schoolName } = schoolDetails;

    useEffect(() => {
        updateSelectedGrades(updatedGrades);
    }, [updatedGrades]);

    //עדכון הציונים שאפשר להשתמש בהם
    const updateSelectedGrades = async (outstandingGrade) => {
        if (outstandingGrade.length !== 0) {
            const outstandingCount = outstandingGrade.length;
            const additionalGradesNeeded = Math.max(0, outstandingCount - 5);
            let gradesArray: number[] = [];
            for (let i = 1; i <= 5; i++) {
                gradesArray.push(i);
            }
            let filteredGrades :number[]=[];
            filteredGrades = await gradesArray.filter(grade => !outstandingGrade.includes(grade));

            let count = 0;
            for (let index = 0; index < outstandingGrade.length; index++) {
                if (outstandingGrade[index] === 5) {
                    count++;
                }

            }
            if (count < additionalGradesNeeded + 1) {
                for (let j = 0; j < additionalGradesNeeded; j++) {
                    filteredGrades.push(5);
                }
            }
            setSelectedGrades(filteredGrades);
        }
    }

    //לחיצה על כפתור העדכון
    const handleUpdateClick = (employeeId) => {
        setVisible(true);
        setSelectedEmployeeId(employeeId);
    };

    //סגירת הטופס
    const handleClosePopup = () => {
        setVisible(false);
    };

    //שמירת הטופס
    const handleSave = async (employeeId, values) => {
        setLoading(true);
        const employeeEvaluation = {
            employeeId: employeeId,
            outstandingEmployeeRating: values.outstandingEmployeeRating,
            uniqueInitiative: values.uniqueInitiative,
            reasonSelectedRating: values.reasonSelectedRating,
            participateRatingDecision: values.participateRatingDecision,
        };
        await updateOutstandingEmployee(employeeEvaluation);
        setVisible(false);
        onUpdateClick();
        setLoading(false);
    };

    //עדכון הנתונים לעובד המצטיין
    const updateOutstandingEmployee = async (generalEmployeeEvaluation: GeneralEmployeeEvaluation) => {
        setLoading(true);
        try {
            const responseUpdateOutstandin = await employeeService.UpdateOutstandingEmployee(generalEmployeeEvaluation);
            if (responseUpdateOutstandin) {
                setMessageInfo({ message: 'חדשות טובות! הנתונים שלך עודכנו בהצלחה', type: 'success' });
            }
            else {
                setMessageInfo({ message: 'אופס! לא ניתן היה לעבד את הנתונים שלך. בבקשה נסה שוב מאוחר יותר', type: 'error' });
            }
        } catch (error) {
            setMessageInfo({ message: `אוי לא! משהו השתבש בעת שליפת הנתונים. בבקשה נסה שוב מאוחר יותר`, type: 'error' });
        }
        setLoading(false);
    }

    //חיתוך ה URL
    const truncateUrl = (url) => {
        return url.length > MAX_URL_LENGTH ? `${url.substring(0, MAX_URL_LENGTH)}...` : url;
    };

    //יצוא לאקסל עובדים מצטיינים
    const exportToExcelGeneral = () => {
        setLoading(true);
        const columns = [
            { key: 'employeeId', title: 'תעודת זהות' },
            { key: 'lastName', title: 'שם משפחה' },
            { key: 'firstName', title: 'שם פרטי' },
            { key: 'jobName', title: 'תפקיד' },
            { key: 'schoolName', title: 'מסגרת' },
            { key: 'supervisorName', title: 'ממונה' },
            { key: 'currentYear', title: 'שנת הערכה' },
            { key: 'weightedMeasureGrade', title: 'ציון מדדי הערכה משוקלל' },
            { key: 'generalEvaluation', title: 'הערכה כללית' },
            { key: 'evaluationDocument1', title: 'מסמך הערכה 1' },
            { key: 'evaluationDocument2', title: 'מסמך הערכה 2' },
            { key: 'evaluationDocument3', title: 'מסמך הערכה 3' }
        ];
        try {
            exportToExcelService.exportToExcel(columns, outstandingEmployees, 'דוח עובדים מצטיינים כללי');
        }
        catch (error) {
            setMessageInfo({ message: `אוי לא! אירעה שגיאה במהלך הייצוא לאקסל: ${error} אנא נסה שוב `, type: 'error' });
        }
        setLoading(false);
    };

    //בדיקת סטטוס תהליך ההערכה
    const checkStatus = (employeeId) => {
        if (outstandingStatuses.length !== 0) {
            const employee = outstandingStatuses.find(emp => emp.employeeId === employeeId);
            return employee && (employee.statusCode === 5 || employee.statusCode === 6);
        }
    }

    const columns = [
        {
            title: 'תעודת זהות',
            dataIndex: 'employeeId',
            key: 'employeeId',
        },
        {
            title: 'שם משפחה',
            dataIndex: 'lastName',
            key: 'lastName',
        },
        {
            title: 'שם פרטי',
            dataIndex: 'firstName',
            key: 'firstName',
        },
        {
            title: 'תפקיד',
            dataIndex: 'jobName',
            key: 'jobName',
        },
        {
            title: 'מסגרת',
            dataIndex: 'schoolName',
            key: 'schoolName',
        },
        {
            title: 'ממונה',
            dataIndex: 'supervisorName',
            key: 'supervisorName',
        },
        {
            title: 'שנת הערכה',
            dataIndex: 'currentYear',
            key: 'currentYear',
        },
        {
            title: 'ציון מדדי הערכה משוקלל',
            dataIndex: 'weightedMeasureGrade',
            key: 'weightedMeasureGrade',
        },
        {
            title: 'הערכה כללית',
            dataIndex: 'generalEvaluation',
            key: 'generalEvaluation',
        },
        {
            title: 'מסמך 1',
            dataIndex: 'evaluationDocument1',
            key: 'evaluationDocument1',
            render: (text, record) => (
                <Tooltip title={record.evaluationDocument2}>
                    <span>{truncateUrl(record.evaluationDocument1)}</span>
                </Tooltip>
            ),
        },
        {
            title: 'מסמך 2',
            dataIndex: 'evaluationDocument2',
            key: 'evaluationDocument2',
            render: (text, record) => (
                <Tooltip title={record.evaluationDocument2}>
                    <span>{truncateUrl(record.evaluationDocument2)}</span>
                </Tooltip>
            ),
        },
        {
            title: 'מסמך 3',
            dataIndex: 'evaluationDocument3',
            key: 'evaluationDocument3',
            render: (text, record) => (
                <Tooltip title={record.evaluationDocument3}>
                    <span>{truncateUrl(record.evaluationDocument3)}</span>
                </Tooltip>
            ),
        },
        {
            title: 'עדכון',
            key: 'actions',
            render: (text, record) => (
                <Button
                    onClick={() => handleUpdateClick(record.employeeId)}
                    disabled={checkStatus(record.employeeId)}
                >
                    עדכן
                </Button>
            ),
        }
    ];

    return (
        <>
            <div style={{ position: 'relative' }}>
                {messageInfo.message && <Message message={messageInfo.message} type={messageInfo.type} duration={3000} />}
                {loading && (
                    <div
                        style={{
                            position: 'fixed',
                            top: 0,
                            left: 0,
                            width: '100%',
                            height: '100%',
                            background: 'rgba(255, 255, 255, 0.5)', 
                            display: 'flex',
                            justifyContent: 'center',
                            alignItems: 'center',
                            zIndex: 9999, 
                        }}
                    >
                        <Spin size="large" />
                    </div>
                )}
                {!loading && (
                    <div style={{ position: 'relative' }}>
                        <div style={{ textAlign: 'center' }}>
                            <h2>עובדים מצטיינים במסגרת {schoolName}</h2>
                            <div style={{ marginRight: '120%' }}>
                                <div >
                                    <Button onClick={exportToExcelGeneral}>יצא לאקסל דוח עובדים מצטיינים כללי</Button>
                                </div>
                            </div>
                            <div style={{ direction: 'rtl' }}>
                                <Table columns={columns} dataSource={outstandingEmployees} />
                            </div>
                        </div>
                        {selectedGrades &&
                            <UpdateOutstandingEmployees
                                visible={visible}
                                onCancel={handleClosePopup}
                                onSave={handleSave}
                                employeeId={selectedEmployeeId}
                                selectedGrades={selectedGrades}/>
                        }
                    </div>
                )}
            </div>
        </>
    )
}
export default OutstandingTable;