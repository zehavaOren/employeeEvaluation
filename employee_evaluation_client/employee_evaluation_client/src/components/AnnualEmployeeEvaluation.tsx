import React, { useEffect, useState } from 'react';
import EmployeeData from '../models/EmployeeData';
import EmployeeTable from './EmployeeTable.tsx';
import { employeeService } from '../services/employeeService.tsx';
import { useParams } from 'react-router-dom';
import UpdateEmployeeMeasure from './UpdateEmployeeMeasure.tsx';
import Message from './Message.tsx';
import { Spin } from 'antd';

export const AnnualEmployeeEvaluation = () => {
    const { employeeId } = useParams();
    const [employeeData, setEmployeeData] = useState<EmployeeData[]>([]);
    const [visibleEmployeeId, setVisibleEmployeeId] = useState('');
    const [messageInfo, setMessageInfo] = useState({ message: '', type: '' });
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        GetEmployeesBySupervisorId(employeeId);
    }, [employeeId]);

    //קבלת כל העובדים לפי התעודת זהות של הממונה
    const GetEmployeesBySupervisorId = async (supervisorId) => {
        setLoading(true);
        try {
            const employeesBySupervisorId = await employeeService.GetEmployeesBySupervisorId(supervisorId);
            setEmployeeData(employeesBySupervisorId);
        } catch (error) {
            setMessageInfo({ message: `אוי לא! משהו השתבש בעת שליפת הנתונים. בבקשה נסה שוב מאוחר יותר`, type: 'error' });
        }
        setLoading(false);
    };

    // כשלוחצים על כפתור העדכון
    const handleUpdateClick = (employeeId: string) => {
        setVisibleEmployeeId(employeeId);
    };

    //כשלוחצים על כפתור העדכון
    const updateEmployeeData = () => {
        GetEmployeesBySupervisorId(employeeId);
    };

    //כשיוצאים מהעדכון
    const handleCloseModal = () => {
        setVisibleEmployeeId('');
    };

    return (
        <>
            <div style={{ position: 'relative' }}>
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
                {messageInfo.message && <Message message={messageInfo.message} type={messageInfo.type} duration={3000} />}
                <div>
                    <EmployeeTable employeesData={employeeData} onUpdateClick={handleUpdateClick} employeeId={employeeId} />
                </div>
                {employeeData.map((employee) => (
                    <UpdateEmployeeMeasure
                        key={employee.employeeId}
                        isVisible={visibleEmployeeId === employee.employeeId}
                        onClose={() => {
                            handleCloseModal();
                            updateEmployeeData();
                        }}
                        employeeId={employee.employeeId}
                        updateEmployee={updateEmployeeData}
                    />
                ))}
            </div>
        </>
    );
}
export default AnnualEmployeeEvaluation;