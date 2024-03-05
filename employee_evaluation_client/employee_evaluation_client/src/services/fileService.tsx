import axios from 'axios';
const BASE_URL = 'https://localhost:44379/api/FileUpload';

export const fileService = {

    uploadFiles: async (files) => {
        if(files.length===0){
            return [null,null,null];
        }
        
        try {
            const formData = new FormData();
            files.forEach(file => formData.append('files', file.originFileObj));
            const response = await fetch(`${BASE_URL}/upload`, {
                method: 'POST',
                body: formData
            });
            if (response.ok) {
                const data = await response.json();
                return data;
            } else {
                return null;
            }
        } catch (error) {
            console.error('Error uploading files:', error);
            return null;
        }
    },

    UploadEmployees: async (formData) => {
        const response = await axios.post(
            'https://localhost:44379/api/FileUpload/ProcessEmployeeExcelFile',
            formData,
            {
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
            }
        );
        if (response) {
            return response.data;
        }

    },
  

};


