import React, { } from 'react';
import { Form, Input, Select, Button, Modal } from 'antd';

const { Option } = Select;

export const UpdateOutstandingEmployees = ({ visible, onCancel, onSave, employeeId, selectedGrades }) => {
    const [form] = Form.useForm();
  
    const onFinish = (values) => {
        onSave(employeeId, values);
        onCancel();
        form.resetFields();
    };

    return (
        <Modal
            visible={visible}
            style={{ direction: 'rtl' }}
            title="עדכון עובד מצטיין"
            onCancel={onCancel}
            footer={[
                <Button key="cancel" onClick={onCancel}>
                    ביטול
                </Button>,
                <Button key="save" type="primary" onClick={() => form.submit()}>
                    שמירה
                </Button>,
            ]}
        >
            <Form form={form} onFinish={onFinish} layout="vertical">
                <Form.Item
                    name="outstandingEmployeeRating"
                    label="דירוג העובד המצטיין"
                    rules={[{ required: true, message: 'נא בחר/י דירוג עובד מצטיין' }]}
                >
                    <Select placeholder="בחר דירוג" >
                        {selectedGrades.map((grade, index) => (
                            <Option key={index} value={grade} style={{ direction: 'rtl' }}>{grade} </Option>
                        ))}
                    </Select>
                </Form.Item>

                <Form.Item
                    name="uniqueInitiative"
                    label="יוזמה ייחודית"
                    rules={[{ required: true, message: 'נא בחר יוזמה ייחודית' }]}
                >
                    <Input.TextArea />
                </Form.Item>

                <Form.Item
                    name="reasonSelectedRating"
                    label="סיבה לדירוג הנבחר"
                    rules={[{ required: true, message: 'נא בחר סיבה לדירוג הנבחר' }]}
                >
                    <Input.TextArea />
                </Form.Item>

                <Form.Item
                    name="participateRatingDecision"
                    label="משתתפים בהחלטת  הדירוג"
                    rules={[{ required: true, message: 'נא בחר משתתפים בהחלטת  הדירוג' }]}
                >
                    <Input.TextArea />
                </Form.Item>
            </Form>
        </Modal >
    );
}
export default UpdateOutstandingEmployees;