import React, { useState, useEffect } from 'react';
import { Modal, Steps, Button, Input, Spin } from 'antd';
import { measureService } from '../services/measureService.tsx';
import FileUploadComponent from './FileUploadComponent.tsx';
import TableSection from './TableSection.tsx';
import { fileService } from '../services/fileService.tsx';
import GeneralEmployeeEvaluation from '../models/GeneralEmployeeEvaluation.tsx';
import { employeeService } from '../services/employeeService.tsx';
import Message from './Message.tsx';
import MeasureList from '../models/MeasureList.tsx';

const { Step } = Steps;

const UpdateEmployeeMeasure = ({ isVisible, onClose, employeeId, updateEmployee }) => {
  const [currentStep, setCurrentStep] = useState(0);
  const [measures, setMeasures] = useState<MeasureList[]>([]);
  const [grades, setGrades] = useState([]);
  const [selectedScores, setSelectedScores] = useState([{ measureID:0, grade: 0,weightedScore:0 }]);
  const [generalEvaluation, setGeneralEvaluation] = useState('');
  const [fileList, setFileList] = useState([]);
  const [totalWeightedScore, setTotalWeightedScore] = useState(0);
  const [messageInfo, setMessageInfo] = useState({ message: '', type: '' });
  const [loading, setLoading] = useState(false);


  useEffect(() => {
    getAllDataFromServer();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [employeeId]);

  //פונקציית קבלת המידע מהסרבר
  const getAllDataFromServer = async () => {
    setLoading(true);
    getAllMeasures();
    getAllMeasureGrade();
    setLoading(false);
  }

  //קבלת המדדים
  const getAllMeasures = async () => {
    try {
      const measures = await measureService.GetAllMeasures();
      setMeasures(measures)
    } catch (error) {
      setMessageInfo({ message: `אוי לא! משהו השתבש בעת שליפת הנתונים. בבקשה נסה שוב מאוחר יותר`, type: 'error' });
    }
  };

  //קבלת הציונים
  const getAllMeasureGrade = async () => {
    try {
      const grades = await measureService.GetAllMeasureGrade();
      setGrades(grades);
    } catch (error) {
      setMessageInfo({ message: `אוי לא! משהו השתבש בעת שליפת הנתונים. בבקשה נסה שוב מאוחר יותר`, type: 'error' });
    }
  };

  //הוספת מדד והציון הנבחר שלו למערך
  const handleScoreChange = (measureID, grade) => {
    setSelectedScores(prevScores => {
      const updatedScores = [...prevScores];
      const index = updatedScores.findIndex(score => score.measureID === measureID);
      const weight = measures.find(measure => measure.measureCode === measureID)?.measureWeight || 1;
      const weightedScore = grade * weight;
      if (index !== -1) {
        updatedScores[index].grade = grade;
        updatedScores[index].weightedScore = weightedScore;
      } else {
        updatedScores.push({ measureID, grade, weightedScore });
      }
      const totalScore = updatedScores.reduce((total, score) => total + (score.weightedScore || 0), 0);
      setTotalWeightedScore(totalScore);
      return updatedScores;

    });
  }

  //שלב הבא בטופס
  const handleNext = async () => {
    if (currentStep === 0) {
      const allmeasuresScored = measures.every(measure => {
        return selectedScores.find(score => score.measureID === measure.measureCode);
      });

      if (!allmeasuresScored) {
        setMessageInfo({ message: 'אנא הקפד לעדכן את כל ציוני האינדקס לפני שתמשיך', type: 'error' });
        return;
      }

      const measuresWithGrades = selectedScores.map(score => ({
        EmployeeId: employeeId,
        EvaluationYear: new Date().getFullYear(),
        GradeCode: score.measureID,
        MeasureGradeCode: score.grade
      }));
       await InsertEmployeeEvaluationMeasure(measuresWithGrades);
      setCurrentStep(currentStep + 1);

    }
    if (currentStep === 1) {
      if (generalEvaluation) {
        setCurrentStep(currentStep + 1);
      }
      else {
        setMessageInfo({ message: 'אל תשכחו לספק הערכה כללית לעובד', type: 'error' });
        return;
      }
    }
    else {
      setCurrentStep(currentStep + 1);
    }
  };

  //הכנסת המדדים והציונים לסרבר
  const InsertEmployeeEvaluationMeasure = async (data) => {
    try {
      const responseInsertMeasures = await measureService.InsertEmployeeEvaluationMeasure(data);
      if (responseInsertMeasures) {
        setMessageInfo({ message: 'הצלחה! הציונים שלך הוכנסו בהצלחה', type: 'sucess' });
      }
      return responseInsertMeasures;
    } catch (error) {
      setMessageInfo({ message: 'אופס! נראה שהציונים של המדדים לא הוזנו. בבקשה תן לזה עוד הזדמנות', type: 'error' });
    }
    setLoading(false);
  }

  //שלב קודם
  const handlePrev = () => {
    setCurrentStep(currentStep - 1);
  };

  //שמירת כל הנתונים
  const handleSave = async () => {
    const uploadRes = await uploadFiles(fileList);
    const generalEmployeeEvaluation: GeneralEmployeeEvaluation = {
      employeeId: employeeId,
      evaluationYear: new Date().getFullYear(),
      weightedMeasureGrade: Math.floor(totalWeightedScore / measures.length),
      generalEvaluation: generalEvaluation,
      evaluationDocument1: uploadRes[0],
      evaluationDocument2: uploadRes[1],
      evaluationDocument3: uploadRes[2]
    }
    await insertGeneralEmployeeEvaluation(generalEmployeeEvaluation);
    onClose();
    updateEmployee();
  };

  //הכנסת נתונים הערכה כללית לעובד
  const insertGeneralEmployeeEvaluation = async (generalEmployeeEvaluation: GeneralEmployeeEvaluation) => {
    setLoading(true);
    try {
      const responseInsertGeneralEvaluation = await employeeService.insertGeneralEmployeeEvaluation(generalEmployeeEvaluation);
      if (responseInsertGeneralEvaluation) {
        setMessageInfo({ message: 'הידד! נתוני העובדים עודכנו בהצלחה', type: 'success' });
      }
    } catch (error) {
      setMessageInfo({ message: 'מצטערים, לא הצלחנו לעדכן את נתוני העובדים. בבקשה נסה שוב מאוחר יותר', type: 'error' });
    }
    setLoading(false);
  }

  //העלאת הקבצים
  const uploadFiles = async (fileList) => {
    setLoading(true);
    const uploadedFiles = await fileService.uploadFiles(fileList);
    if (uploadedFiles && uploadedFiles.length > 0) {
      setMessageInfo({ message: 'מעולה! הקבצים שלך הועלו בהצלחה', type: 'success' });
      return uploadedFiles;
    } else {
      setMessageInfo({ message: 'אופס! אירעה שגיאה בעת העלאת הקבצים. בבקשה נסה שוב', type: 'error' });
    }
    setLoading(false);

  }

  const handleFileListChange = (newFileList) => {
    if (fileList.length === 0) {
      setFileList(newFileList);
    }
  };

  const handleFilesUploaded = () => {
  };

  const steps = [
    {
      title: 'עדכון ציונים למדדים',
      content: (
        <>
          {messageInfo.message && <Message message={messageInfo.message} type={messageInfo.type} duration={3000} />}
          <TableSection
            measureList={measures}
            grades={grades}
            handleSelectChange={handleScoreChange}
          />
          <div style={{ textAlign: 'right', marginTop: '2%' }}>
            <Button type="primary" onClick={handleNext}>
              הבא
            </Button>
          </div>

        </>
      ),
    },
    {
      title: 'הזנת הערכה כללית',
      content: (
        <>
          <h5>אנא הזן הערכה כללית</h5>
          <Input.TextArea
            value={generalEvaluation}
            onChange={e => setGeneralEvaluation(e.target.value)}
            placeholder="הזן הערכה כללית"
            autoSize={{ minRows: 3, maxRows: 6 }}
            style={{ direction: 'rtl' }}
          />
          <div style={{ textAlign: 'right', marginTop: '2%' }}>
            <Button type="primary" onClick={handleNext}>הבא</Button>
            <Button onClick={handlePrev} style={{ marginRight: '1%' }}>הקודם</Button>
          </div>
        </>
      ),
    },
    {
      title: 'העלאת קבצים',
      content: (
        <>
          <FileUploadComponent  onFilesChange={handleFileListChange} onFilesUploaded={handleFilesUploaded} />
          <div style={{ textAlign: 'right', marginTop: '2%' }}>
            <Button type="primary" onClick={handleSave}>שמור</Button>
            <Button onClick={handlePrev} style={{ marginRight: '1%' }}>הקודם</Button>
          </div>
        </>
      ),
    },
  ];

  return (
    <div>
      {messageInfo.message && <Message message={messageInfo.message} type={messageInfo.type} duration={3000} />}
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
        <div>
          <Modal
            open={isVisible}
            title="עדכן מדדי עובד"
            onCancel={onClose}
            footer={[
              <Button key="back" onClick={onClose}>
                ביטול
              </Button>,
            ]}
            width={1000}
            style={{ textAlign: 'center' }}
          >
            <Steps current={currentStep} style={{ direction: 'rtl' }} >
              {steps.map(item => (
                <Step key={item.title} title={item.title} />
              ))}
            </Steps>
            <div className="steps-content">{steps[currentStep].content}</div>
          </Modal>
        </div>
      </div>
    </div>
  );
};

export default UpdateEmployeeMeasure;
