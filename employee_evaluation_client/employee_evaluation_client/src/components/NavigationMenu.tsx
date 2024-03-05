import React, { useEffect, useState } from 'react';
import { Button, Menu } from 'antd';
import { Link, useLocation, useNavigate } from 'react-router-dom';

const NavigationMenu = ({ employee, onLogout, allowedRoutes }) => {

    const [activeRoute, setActiveRoute] = useState(null);
    const [employeeName, setEmployeeName] = useState('');
    const location = useLocation();
    const navigate = useNavigate();

    useEffect(() => {
        if (employee) {
            setEmployeeName(`${employee.firstName} ${employee.lastName}`);
        }
    }, [employee]);

    //צביעת הקומפוננטה שעליה עומדים בכניסה
    useEffect(() => {
        const currentRoute = allowedRoutes.find(route => location.pathname.includes(route.path));
        setActiveRoute(currentRoute ? currentRoute.path : null);
    }, [location.pathname, allowedRoutes]);

    //בחירת קומפוננטה
    const handleMenuItemClick = (routePath) => {
        setActiveRoute(routePath);
    };

    //כשמשתמש לוחץ יציאה
    const handleLogoutClick = () => {
        onLogout();
        navigate("/");
    };

    return (
        <Menu theme="dark" mode="horizontal" style={{ display: 'flex', alignItems: 'center', fontSize: '16px', lineHeight: '4rem' }}>
            <div style={{ marginRight: '50%' }}>
                <Button type="primary" onClick={handleLogoutClick}>יציאה</Button>
            </div>
            <span style={{ marginLeft: '-49.3%' }}>
                {employeeName && <span>{employeeName}</span>}
            </span>
            <div style={{ marginLeft: 'auto', display: 'flex', alignItems: 'center' }}>
                {allowedRoutes.map((route, index) => (
                    <Menu.Item
                        key={route.index}
                        style={{ backgroundColor: activeRoute === route.path ? '#1890ff' : 'transparent' }}
                        onClick={() => handleMenuItemClick(route.path)}>
                        <Link to={`${route.path}/${employee.employeeId}`} style={{ color: '#fff' }}>
                            {route.label}
                        </Link>
                    </Menu.Item>
                ))}
            </div>
        </Menu>

    );
};

export default NavigationMenu;
