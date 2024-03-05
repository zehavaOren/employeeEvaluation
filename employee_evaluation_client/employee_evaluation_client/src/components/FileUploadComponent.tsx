import React, { useState } from 'react';
import { Upload, Button, Spin } from 'antd';
import { UploadOutlined } from '@ant-design/icons';

const FileUploadComponent = ({ onFilesChange, onFilesUploaded }) => {
    const [fileList, setFileList] = useState<any[]>([]);
    const [loading, setLoading] = useState(false);

    const handleChange = async info => {
        setLoading(true);
        let fileList = [...info.fileList];
        fileList = fileList.slice(0, 3);
        setFileList(fileList);
        onFilesChange(fileList);
        onFilesUploaded();
        setLoading(false);
    };

    return (
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
            <Upload
                action="/upload"
                fileList={fileList}
                onChange={handleChange}
                multiple
                maxCount={3}>
                <Button icon={<UploadOutlined />}>העלה קבצים</Button>
            </Upload>
        </div>
    );
};

export default FileUploadComponent;
