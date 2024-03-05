import React, { useState } from 'react';
import { Form, Input, Button, Typography, Spin } from 'antd';
import { UserOutlined } from '@ant-design/icons';
import { employeeService } from '../services/employeeService.tsx';
import { useNavigate } from 'react-router-dom';
import Message from './Message.tsx';

const { Title } = Typography;

export const Login = ({ onLogin }) => {
    const navigate = useNavigate();
    const [messageInfo, setMessageInfo] = useState({ message: '', type: '' });
    const [loading, setLoading] = useState(false);

    //לחיצה על כניסה
    const onFinish = async (values: any) => {  
        setLoading(true);
        setMessageInfo({ message: '', type: '' });
        //בדיקת ולידציית על ה ת.ז
        if (validateID(values.identityNmber)) {
            try {
                const employee = await employeeService.getEmployeeById(String(values.identityNmber));
                if (employee.status === 204) {
                    setMessageInfo({ message: 'אוי - לא! לא מצאנו שום תעודת זהות תואמת. אנא בדוק שוב ונסה שוב', type: 'error' });
                }
                else {
                    await Checking_permissions(employee);
                }

            } catch (error) {
                setMessageInfo({ message: `אוי לא! משהו השתבש בעת שליפת הנתונים. בבקשה נסה שוב מאוחר יותר`, type: 'error' });
            }
        }
        else {
            setMessageInfo({ message: 'אופס! נראה שתעודת הזהות שהזנת לא חוקית. אנא ודא שיש לו 9 ספרות בדיוק', type: 'error' });
        }
        setLoading(false);
        return;
    };

    //בדיקת ולידציית על ה ת.ז
    const validateID = (id) => {
        const regex = /^[0-9]{9}$/;
        return regex.test(id);
    };

    //בדיקת הרשאות עובד נכנס
    const Checking_permissions = (employee) => {
        const permissions = [''];
        if (employee?.isSchoolManager) {
            permissions.push(`/outstanding-employees-school/${employee.employeeId}`);
        }
        if (employee?.isSuperior || employee?.isSchoolManager) {
            permissions.push(`/annual-employee-evaluation/${employee.employeeId}`);
        }
        if (employee?.isGeneralManager) {
            permissions.push(`/general-evaluation-data/${employee.employeeId}`);
            permissions.push(`/loading-employee-file/${employee.employeeId}`);
        }

        permissions.forEach(permission => {
            navigate(permission);
        });

        if (permissions.length === 0) {
            setMessageInfo({ message: 'מצטערים, אין לך את ההרשאות הנדרשות לביצוע פעולה זו', type: 'errpr' });
        }
        onLogin(employee, permissions);
    };

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
                <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
                    <div style={{ width: 400 }}>
                        <Title level={2} style={{ textAlign: 'center' }}>כניסה למערכת</Title>
                        <Form
                            name="login-form"
                            initialValues={{ remember: true }}
                            onFinish={onFinish}
                            size="large">
                            <Form.Item
                                name="identityNmber"
                                rules={[{ required: true, message: 'אנא הזן תעודת זהות' }]}>
                                <Input prefix={<UserOutlined />} placeholder="תעודת זהות" />
                            </Form.Item>
                            <Form.Item>
                                <Button type="primary" htmlType="submit" style={{ width: '100%' }}>כניסה</Button>
                            </Form.Item>

                        </Form>
                    </div>
                </div>
            </div>

        </>
    );
}


