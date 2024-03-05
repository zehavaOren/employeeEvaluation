import React, { useEffect, useState } from 'react';
import { Table, Select } from 'antd';

const { Option } = Select;

const TableSection = ({ measureList, grades, handleSelectChange }) => {
    const [selectedValues, setSelectedValues] = useState({});

    useEffect(() => {
        const initialValues = {};
        measureList.forEach(measure => {
            initialValues[measure.measureCode] = undefined;
        });
        setSelectedValues(initialValues);
    }, [measureList]);

    const handleDropdownChange = (measureCode, value) => {
        setSelectedValues(prevValues => ({
            ...prevValues,
            [measureCode]: value
        }));
        handleSelectChange(measureCode, value);
    };

    const columns = [
        {
            title: 'שם מדד',
            dataIndex: 'measureName',
            key: 'measureName',
        },
        {
            title: 'קטגוריה',
            dataIndex: 'categoryDescription',
            key: 'categoryDescription',
        },
        {
            title: 'ציון',
            dataIndex: 'score',
            key: 'score',
            render: (_, record) => (
                <Select
                    style={{ width: 200 }}
                    onChange={value => handleDropdownChange(record.measureCode, value)}
                    value={selectedValues[record.measureCode]}
                    placeholder="בחר ציון"
                >
                    {grades.map(option => (
                        <Option key={option.gradeCode} value={option.gradeCode}>
                            {option.gradeDescription}
                        </Option>
                    ))}
                </Select>
            ),
        },
    ];

    return <Table dataSource={measureList} columns={columns} style={{ direction: 'rtl' }} />;
};

export default TableSection;
