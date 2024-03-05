import React, { useEffect, useState } from 'react';
import { Table, Button, Spin } from 'antd';
import EmployeeData from '../models/EmployeeData';
import { employeeService } from '../services/employeeService.tsx';
import { statusService } from '../services/statusService.tsx';
import { exportToExcelService } from '../services/exportToExcelService.tsx';
import Message from './Message.tsx';
import EvaluationStatus from '../models/EvaluationStatus.tsx';

export const EmployeeTable = ({ employeesData, onUpdateClick, employeeId }) => {
    const [statusDescriptions, setStatusDescriptions] = useState<EvaluationStatus[]>([]);
    const [messageInfo, setMessageInfo] = useState({ message: '', type: '' });
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        getAllStatuses();
    }, [employeesData]);

    //קבלת המידע מהסרבר
    const getAllStatuses = async () => {
        setLoading(true);
        try {
            const allStatuses = await statusService.getAllStatuses();
            setStatusDescriptions(allStatuses);
        } catch (error) {
            setMessageInfo({ message: `אוי לא! משהו השתבש בעת שליפת הנתונים. בבקשה נסה שוב מאוחר יותר`, type: 'error' });
        }
        setLoading(false);
    }

    //קבלת תיאור הסטטוס בהתאם לקוד הסטטוס
    const mapStatusCodeToDescription = (statusCode: number) => {
        if (statusDescriptions.length !== 0) {
            return statusDescriptions[statusCode - 1].statusDescription;
        }
    };

    //ייצוא לאקסל דוח הערכת עובדים כללית
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
            { key: 'evaluationStatusCode', title: 'סטטוס הערכה' }
        ];
        const updatedDataWithStatuses = employeesData.map((employee) => {
            const status = mapStatusCodeToDescription(employee.evaluationStatusCode);
            return { ...employee, evaluationStatusCode: status };
        })
        try {
            exportToExcelService.exportToExcel(columns, updatedDataWithStatuses, 'דוח הערכת עובדים כללית');
        }
        catch (error) {
            setMessageInfo({ message: `אוי לא! משהו השתבש בעת שליפת הנתונים. בבקשה נסה שוב מאוחר יותר`, type: 'error' });
        }
        setLoading(false);
    };

    //ייצוא לאקסל דוח עובדים משוקלל
    const exportToExcelWeighted = async () => {
        setLoading(true);
        const columns = [
            { key: 'employeeId', title: 'תעודת זהות' },
            { key: 'lastName', title: 'שם משפחה' },
            { key: 'firstName', title: 'שם פרטי' },
            { key: 'jobName', title: 'תפקיד' },
            { key: 'schoolName', title: 'מסגרת' },
            { key: 'supervisorName', title: 'ממונה' },
            { key: 'currentYear', title: 'שנת הערכה' },
            { key: 'weightedMeasureGrade', title: 'ציון הערכה משוקלל' }
        ];
        try {
            const weightedEmployeeScore = await employeeService.GetWeightedEmployeeScore((employeeId).toString());
            exportToExcelService.exportToExcel(columns, weightedEmployeeScore, 'דוח ציון עובד משוקלל');

        } catch (error) {
            setMessageInfo({ message: `אוי לא! משהו השתבש בעת שליפת הנתונים. בבקשה נסה שוב מאוחר יותר`, type: 'error' });
        }
        setLoading(false);
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
            title: 'סטטוס הערכה',
            dataIndex: 'evaluationStatusCode',
            key: 'evaluationStatusCode',
            render: (evaluationStatusCode) => (
                <span>{mapStatusCodeToDescription(evaluationStatusCode)}</span>
            ),
        },
        {
            title: 'עדכון',
            key: 'actions',
            render: (text: any, record: EmployeeData) => (
                <Button onClick={() => onUpdateClick(record.employeeId)} disabled={record.evaluationStatusCode !== 1}>עדכן</Button>
            ),
        }
    ];

    return (<>
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
            <div style={{ textAlign: 'center' }}>
                <h2>הערכת עובד שנתית</h2>
                <div style={{ marginBottom: '10px', marginLeft: '4%', display: 'flex', justifyContent: 'flex-start' }}>
                    <div >
                        <Button onClick={exportToExcelGeneral}>יצא לאקסל דוח הערכת עובדים כללי</Button>
                    </div>
                    <div style={{ marginRight: '20px' }}>
                        <Button onClick={exportToExcelWeighted}>יצא לאקסל דוח עובדים משוקלל</Button>
                    </div>
                </div>
                <br></br>
                <div style={{ direction: 'rtl' }}>
                    <Table columns={columns} dataSource={employeesData} />
                </div>
            </div>
        </div>
    </>
    )
};

export default EmployeeTable;
