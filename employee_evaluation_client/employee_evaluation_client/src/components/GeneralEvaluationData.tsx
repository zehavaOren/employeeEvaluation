import { Button, Card, Col, Row, Spin, Table } from 'antd';
import React, { useEffect, useState } from 'react';
import { exportToExcelService } from '../services/exportToExcelService.tsx';
import { useParams } from 'react-router-dom';
import { schoolService } from '../services/schoolService.tsx';
import Message from './Message.tsx';
import School from '../models/School.tsx';

const GeneralEvaluationData = () => {
    const { employeeId } = useParams();
    const [schoolList, setSchoolList] = useState<School[]>([]);
    const [messageInfo, setMessageInfo] = useState({ message: '', type: '' });
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        getSchoolList();
    }, [employeeId]);

    //קבלת רשימת המסגרות
    const getSchoolList = async () => {
        setLoading(true);
        try {
            const schools = await schoolService.getSchollsLostBySchoolManager();
            setSchoolList(schools);
        } catch (error) {
            setMessageInfo({ message: `אוי לא! משהו השתבש בעת שליפת הנתונים. בבקשה נסה שוב מאוחר יותר`, type: 'error' });
        }
        setLoading(false);
    }

    //בחירת הדוח המתאים לייצוא עפ"י בחירת המשתמש
    const exportToExcel = async (schoolId, reportType) => {
        try {
            switch (reportType) {
                case 'general':
                    await exportGeneralEvaluation(schoolId);
                    break;
                case 'weighted':
                    await exportWeightedEmployeeGrade(schoolId);
                    break;
                case 'outstanding':
                    await exportOutstandingEmployees(schoolId);
                    break;
                case 'outstandingEmployee':
                    await exportOutstandingEmployee(schoolId);
                    break;
                default:
                    break;
            }
        } catch (error) {
            setMessageInfo({ message: `אוי לא! משהו השתבש בעת שליפת הנתונים. בבקשה נסה שוב מאוחר יותר`, type: 'error' });
        }
    }

    //ייצוא לאקסל הערכה כללית
    const exportGeneralEvaluation = async (schoolId) => {
        setLoading(true);
        const columns = [
            { key: 'employeeId', title: 'תעודת זהות' },
            { key: 'lastName', title: 'שם משפחה' },
            { key: 'firstName', title: 'שם פרטי' },
            { key: 'jobName', title: 'תפקיד' },
            { key: 'schoolName', title: 'מסגרת' },
            { key: 'supervisorName', title: 'ממונה' },
            { key: 'currentYear', title: 'שנת הערכה' },
            { key: 'assessmentStatus', title: 'סטטוס הערכה' }
        ];
        try {
            const generalEmployeeEvaluation = await schoolService.getGeneralEmployeeEvaluation(schoolId);
            await exportToExcelService.exportToExcel(columns, generalEmployeeEvaluation, `דוח הערכת עובדים כללית`);
        } catch (error) {
            setMessageInfo({ message: `אוי לא! משהו השתבש בעת שליפת הנתונים. בבקשה נסה שוב מאוחר יותר`, type: 'error' });
        }
        setLoading(false);
    };

    //ייצוא לאקסל ציונים משוקללים
    const exportWeightedEmployeeGrade = async (schoolId) => {
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
            const weightedEmployeeGrade = await schoolService.getWeightedEmployeeGrade(schoolId);
            await exportToExcelService.exportToExcel(columns, weightedEmployeeGrade, `דוח ציון עובדים משוקלל`);
        } catch (error) {
            setMessageInfo({ message: `אוי לא! משהו השתבש בעת שליפת הנתונים. בבקשה נסה שוב מאוחר יותר`, type: 'error' });
        }
        setLoading(false);
    };

    //ייצוא לאקסל עובדים מצטיינים
    const exportOutstandingEmployees = async (schoolId) => {
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
            const outstandingEmployees = await schoolService.getOutstandingEmployees(schoolId);
            await exportToExcelService.exportToExcel(columns, outstandingEmployees, `דוח עובדים מצטיינים כללי`);
        } catch (error) {
            setMessageInfo({ message: `אוי לא! משהו השתבש בעת שליפת הנתונים. בבקשה נסה שוב מאוחר יותר`, type: 'error' });
        }
        setLoading(false);
    };

    //ייצוא לאקסל עובד מצטיין
    const exportOutstandingEmployee = async (schoolId) => {
        setLoading(true);
        const columns = [
            { key: 'employeeId', title: 'תעודת זהות' },
            { key: 'lastName', title: 'שם משפחה' },
            { key: 'firstName', title: 'שם פרטי' },
            { key: 'jobName', title: 'תפקיד' },
            { key: 'schoolName', title: 'מסגרת' },
            { key: 'superiorName', title: 'ממונה' },
            { key: 'currentYear', title: 'שנת הערכה' },
            { key: 'weightedMeasureGrade', title: 'ציון מדדי הערכה משוקלל' },
            { key: 'generalEvaluation', title: 'הערכה כללית' },
            { key: 'evaluationDocument1', title: 'מסמך הערכה 1' },
            { key: 'evaluationDocument2', title: 'מסמך הערכה 2' },
            { key: 'evaluationDocument3', title: 'מסמך הערכה 3' },
            { key: 'outstandingEmployeeRating', title: 'דירוג עובד מצטיין' },
            { key: 'uniqueInitiative', title: 'יוזמה ייחודית' },
            { key: 'reasonSelectedRating', title: 'סיבה להחלטת הדירוג' },
            { key: 'participateRatingDecision', title: 'משתתפים בהחלטת הדירוג' },
        ];
        try {
            const outstandingEmployee = await schoolService.getOutstandingEmployee(schoolId);
            await exportToExcelService.exportToExcel(columns, [outstandingEmployee], `עובד מצטיין למסגרת`);
        }
        catch (error) {
            setMessageInfo({ message: `אוי לא! משהו השתבש בעת שליפת הנתונים. בבקשה נסה שוב מאוחר יותר`, type: 'error' });
        }
        setLoading(false);
    };

    const columns = [
        {
            title: 'מסגרת',
            dataIndex: 'schoolDescription',
            key: 'schoolDescription',
        },
        {
            title: 'הערכת עובדים כללית',
            dataIndex: 'general',
            key: 'general',
            render: (_, record) => (
                <Button type="primary" onClick={() => exportToExcel(record.schoolCode, 'general')}>
                    ייצא לאקסל
                </Button>
            ),
        },
        {
            title: 'ציון עובדים משוקלל',
            dataIndex: 'weighted',
            key: 'weighted',
            render: (_, record) => (
                <Button type="primary" onClick={() => exportToExcel(record.schoolCode, 'weighted')}>
                    ייצא לאקסל
                </Button>
            ),
        },
        {
            title: 'עובדים מצטיינים',
            dataIndex: 'outstanding',
            key: 'outstanding',
            render: (_, record) => (
                <Button type="primary" onClick={() => exportToExcel(record.schoolCode, 'outstanding')}>
                    ייצא לאקסל
                </Button>
            ),
        },
        {
            title: 'עובד מצטיין',
            dataIndex: 'outstandingEmployee',
            key: 'outstandingEmployee',
            render: (_, record) => (
                <Button type="primary" onClick={() => exportToExcel(record.schoolCode, 'outstandingEmployee')}>
                    ייצא לאקסל
                </Button>
            ),
        },
    ];

    return (
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
            <div style={{ padding: '2%', direction: 'rtl' }}>
                <Row justify="center">
                    <Col span={24}>
                        <Card title="דוחות הערכת עובדים">
                            <Table
                                columns={columns}
                                dataSource={schoolList}
                                pagination={false} />
                        </Card>
                    </Col>
                </Row>
            </div>
        </div>
    );
}

export default GeneralEvaluationData;
