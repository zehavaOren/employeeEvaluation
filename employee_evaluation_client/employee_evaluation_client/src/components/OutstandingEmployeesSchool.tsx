import React, { useEffect, useState } from "react"
import { useParams } from "react-router-dom";
import { employeeService } from "../services/employeeService.tsx";
import OutstandingEmployees from "../models/OutstandingEmployees.tsx";
import { OutstandingTable } from "./OutstandingTable.tsx";
import Message from './Message.tsx';
import { Spin } from "antd";
import { statusService } from "../services/statusService.tsx";


export const OutstandingEmployeesSchool = () => {
    const { employeeId } = useParams();
    const [outstandingEmployees, setOutstandingEmployees] = useState<OutstandingEmployees[]>([]);
    const [schoolDetails, setSchoolDetails] = useState<{ schoolName: string, schooCode: number }>({ schoolName: '', schooCode:0});
    const [messageInfo, setMessageInfo] = useState({ message: '', type: '' });
    const [loading, setLoading] = useState(false);
    const [updatedGrades, setUpdatedGrades] = useState([]);
    const [outstandingStatuses, setOutstandingStatuses] = useState([]);


    useEffect(() => {
        if (employeeId) {
            GetOutstandingEmployees(employeeId);
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [employeeId]);

    //קבלת העובדים המצטיינים למסגרת
    const GetOutstandingEmployees = async (superiorId: string) => {
        setLoading(true);
        try {
            const outstandingEmployees = await employeeService.GetOutstandingEmployees(superiorId);
            if (outstandingEmployees.length === 0) {
                setMessageInfo({ message: 'נראה שלא נמצאו עובדים מצטיינים עבור המסגרת הזו', type: 'error' });
                setLoading(false)
                return;
            }
            setOutstandingEmployees(outstandingEmployees);
            setSchoolDetails({ schoolName: outstandingEmployees[0].schoolName, schooCode: outstandingEmployees[0].schoolCode });
            getOutstandingStatuses(outstandingEmployees[0].schoolCode);
            getOutstandingGrade(outstandingEmployees[0].schoolCode);
        } catch (error) {
            setMessageInfo({ message: `אוי לא! משהו השתבש בעת שליפת הנתונים. בבקשה נסה שוב מאוחר יותר`, type: 'error' });
        }
        setLoading(false);
    }

    //כשלוחצים על עדכון עובד מצטיין
    const handleUpdateClick = (employeeId: string | undefined) => {
        getOutstandingStatuses(schoolDetails.schooCode);
        getOutstandingGrade(schoolDetails.schooCode);
    };

    //קבלת הסטטוסים של העובדים המצטיינים המעודכן בכל רגע
    const getOutstandingStatuses = async (schooCode) => {
        setLoading(true);
        const outstandingStatusesFromServer = await statusService.getOutstandingEmployeeStatusCodes(schooCode);
        if (outstandingStatusesFromServer) {
            setOutstandingStatuses(outstandingStatusesFromServer);
        }
        setLoading(false);
    }

    //קבלת הציונים שכבר עודכנו ואי אפשר להשתמש בהם
    const getOutstandingGrade = async (schooCode) => {
        const getOutstandingGrade = await statusService.getUpdatedGradesForOutstandingEmployee(schooCode);
        if (getOutstandingGrade) {
            setUpdatedGrades(getOutstandingGrade);
        }
    }

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
                <div>
                    <OutstandingTable
                        outstandingEmployees={outstandingEmployees}
                        onUpdateClick={handleUpdateClick}
                        schoolDetails={schoolDetails}
                        outstandingStatuses={outstandingStatuses}
                        updatedGrades={updatedGrades}
                    />
                </div>
            </div>
        </>
    )
}
export default OutstandingEmployeesSchool;
