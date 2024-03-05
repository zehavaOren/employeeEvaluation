import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { Login } from './components/Login.tsx';
import AnnualEmployeeEvaluation from './components/AnnualEmployeeEvaluation.tsx';
import OutstandingEmployeesSchool from './components/OutstandingEmployeesSchool.tsx';
import GeneralEvaluationData from './components/GeneralEvaluationData.tsx';
import React, { useState } from 'react';
import NavigationMenu from './components/NavigationMenu.tsx';
import LoadingEmployeeFile from './components/LoadingEmployeeFile.tsx'

function App() {
  const [employee, setEmployee] = useState(null);
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [rout, setRout] = useState([]);

  const handleLogin = (loggedInEmployee, route) => {
    setEmployee(loggedInEmployee);
    setIsLoggedIn(true);
    setRout(route);
  };
  const handleLogout = () => {
    setEmployee(null);
    setIsLoggedIn(false);
  };
  const routes = [
    { path: '/annual-employee-evaluation', label: 'הערכת עובד שנתית' },
    { path: '/outstanding-employees-school', label: 'עובדים מצטיינים במסגרת' },
    { path: '/general-evaluation-data', label: 'נתוני הערכה כללים' },
    { path: '/loading-employee-file', label: 'העלאת קובץ עובדים לעדכון' },
  ];

  const allowedRoutes = routes.filter((route) => {
    if (employee) {
      return rout.includes(route.path + '/' + employee.employeeId);
    }
    return false;
  });

  return (
    <Router>
      <div className="App">
        {isLoggedIn && <NavigationMenu employee={employee} onLogout={handleLogout} allowedRoutes={allowedRoutes} />}
        <Routes>
          {!isLoggedIn ? (
            <Route path="/" element={<Login onLogin={handleLogin} />} />
          ) : (
            <>
              <Route path="/annual-employee-evaluation/:employeeId" element={<AnnualEmployeeEvaluation />} />
              <Route path="/outstanding-employees-school/:employeeId" element={<OutstandingEmployeesSchool />} />
              <Route path="/general-evaluation-data/:employeeId" element={<GeneralEvaluationData />} />
              <Route path="/loading-employee-file/:employeeId" element={<LoadingEmployeeFile />} />
            </>
          )}
        </Routes>
      </div>
    </Router>
  );
}
export default App;
