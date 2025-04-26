import React, { useMemo, useState } from 'react';
import { useLocation, useNavigate, Outlet } from 'react-router-dom';
import '../styles/dashboard.css';
import { UserDTO } from '../types/authTypes';

export default function Dashboard() {
  const navigate = useNavigate();
  const location = useLocation();

  const [openMenus, setOpenMenus] = useState<{ [key: string]: boolean }>({});

  const user: UserDTO | null = useMemo(() => {
    const storedUser = localStorage.getItem('user');
    return storedUser ? JSON.parse(storedUser) : null;
  }, []);

  const toggleMenu = (label: string) => {
    setOpenMenus(prev => ({ ...prev, [label]: !prev[label] }));
  };

  const logout = () => {
    localStorage.clear();
    navigate('/');
  };

  const hasRole = (roleName: string) => user?.roles?.some(role => role.name === roleName);

  const menuData = useMemo(() => {
    const hasRole = (roleName: string) => user?.roles?.some(role => role.name === roleName);
  
    const data = [];
  
    if (hasRole('Dispatcher')) {
      data.push(
        {
          label: 'Головна панель',
          items: [
            { label: 'Мапа з позначенням операцій', path: 'map', icon: 'pi pi-map' }
          ]
        },
        {
          label: 'Швидкі дії',
          items: [
            { label: 'Створити нову операцію', path: 'create', icon: 'pi pi-plus' }
          ]
        },
        {
          label: 'Управління поточними операціями',
          items: [
            { label: 'Поточні операції', path: 'operations', icon: 'pi pi-folder' },
            { label: 'Групи та призначені завдання', path: 'groups', icon: 'pi pi-users' }
          ]
        },
        {
          label: 'Сповіщення та повідомлення',
          items: [
            { label: 'Запити від координаторів', path: 'requests', icon: 'pi pi-question-circle' }
          ]
        }
      );
    }
  
    if (hasRole('Admin')) {
      data.push({
        label: 'Адміністрування та доступ',
        items: [
          { label: 'Зареєструвати працівника', path: 'registerWorker', icon: 'pi pi-user-plus' },
          { label: 'Відкрити операцію для погодження', path: 'approve', icon: 'pi pi-check-square' },
          { label: 'Звітність', path: 'reports', icon: 'pi pi-chart-line' }
        ]
      });
    }
  
    return data;
  }, [user]);
  
  const hasAccess = hasRole('Dispatcher') || hasRole('Admin');

  if (!hasAccess) {
    return (
      <div className="layout">
        <div className="content">
          <h2>У вас немає доступу до жодної панелі.</h2>
        </div>
      </div>
    );
  }

  return (
    <div className="layout">
      <div className="topbar">
        <div className="user-actions">
          <i className="pi pi-sign-out" onClick={logout} title="Вийти" />
        </div>
      </div>
      <div className="card side-menu custom-rounded">
        {menuData.map(section => (
          <div key={section.label} className="menu-section">
            <div className="menu-header" onClick={() => toggleMenu(section.label)}>
              <strong>{section.label}</strong>
              <i className={`pi ${openMenus[section.label] ? 'pi-chevron-down' : 'pi-chevron-right'}`} />
            </div>
            {openMenus[section.label] && (
              <ul className="menu-items">
                {section.items.map(item => (
                  <li
                    key={item.path}
                    className={`menu-item ${location.pathname === '/dashboard/' + item.path ? 'menu-item-active' : ''}`}
                    onClick={() => navigate(item.path)}
                  >
                    <i className={item.icon} style={{ marginRight: '0.5rem' }} />
                    {item.label}
                  </li>
                ))}
              </ul>
            )}
          </div>
        ))}
      </div>
      <div className="content">
        <Outlet />
      </div>
    </div>
  );
}
