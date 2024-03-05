import React, { useState } from 'react';
import { Button, Space, Spin, Upload } from 'antd';
import { UploadOutlined } from '@ant-design/icons';
import { fileService } from '../services/fileService.tsx';
import Message from './Message.tsx';

const LoadingEmployeeFile = () => {
    const [uploading, setUploading] = useState(false);
    const [messageInfo, setMessageInfo] = useState({ message: '', type: '' });
    const [loading, setLoading] = useState(false);

    //העלאת קובץ העובדים
    const customUpload = async ({ file }) => {
        setLoading(true);
        try {
            setUploading(true);
            const formData = new FormData();
            formData.append('file', file);
            const resFromServer = await fileService.UploadEmployees(formData);
            if (resFromServer) {
                setMessageInfo({ message: `וואוו! הקובץ שלך:${file.name} הועלה בהצלחה`, type: 'success' });
            }
            else {
                setMessageInfo({ message: `אופס! אירעה שגיאה בהעלאת הקובץ שלך: ${file.name} בבקשה נסה שוב`, type: 'error' });
            }
        }
        catch (error) {
            setMessageInfo({ message: `אופס! אירעה שגיאה בהעלאת הקובץ שלך: ${file.name} בבקשה נסה שוב`, type: 'error' });
        }
        finally {
            setUploading(false);
        }
        setLoading(false);
    };

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
            <div style={{ maxWidth: '60%', margin: '0 auto' }}>
                <div style={{ marginLeft: '110px' }}>
                </div >
                <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
                    <Space direction="vertical">
                        <Upload
                            customRequest={customUpload}
                            showUploadList={false}
                            disabled={uploading}>
                            <Button type="primary" icon={<UploadOutlined />} loading={uploading}>
                                {uploading ? 'Uploading' : 'לחץ להעלאת קובץ עובדים לעדכון'}
                            </Button>
                        </Upload>
                    </Space>
                </div>
            </div >
        </div>
    );
};

export default LoadingEmployeeFile;
