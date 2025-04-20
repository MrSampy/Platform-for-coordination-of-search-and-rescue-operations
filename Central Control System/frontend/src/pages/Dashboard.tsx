import { Menu } from 'primereact/menu';
import { useLocation, useNavigate, Outlet } from 'react-router-dom';
import '../styles/dashboard.css';
import { useMemo } from 'react';
import { UserDTO } from '../types/authTypes'; // <-- if you have it, otherwise just type manually

export default function Dashboard() {
    const navigate = useNavigate();
    const location = useLocation();

    // Load user from localStorage
    const user: UserDTO | null = useMemo(() => {
        const storedUser = localStorage.getItem('user');
        return storedUser ? JSON.parse(storedUser) : null;
    }, []);

    const logout = () => {
        localStorage.clear();
        navigate('/');
    };

    const { items, hasAccess } = useMemo(() => {
        const hasRole = (roleName: string) => {
            return user?.roles?.some(role => role.name === roleName);
        };

        const menuItems = [];

        if (hasRole('Dispatcher')) {
            menuItems.push(
                {
                    label: 'Головна панель',
                    items: [
                        {
                            label: 'Поточні операції',
                            icon: 'pi pi-folder',
                            command: () => navigate('operations'),
                            className: location.pathname === '/dashboard/operations' ? 'menu-item-active' : ''
                        },
                        {
                            label: 'Мапа з позначенням операцій',
                            icon: 'pi pi-map',
                            command: () => navigate('map'),
                            className: location.pathname === '/dashboard/map' ? 'menu-item-active' : ''
                        }
                    ]
                },
                {
                    label: 'Швидкі дії',
                    items: [
                        {
                            label: 'Створити нову операцію',
                            icon: 'pi pi-plus',
                            command: () => navigate('create'),
                            className: location.pathname === '/dashboard/create' ? 'menu-item-active' : ''
                        }
                    ]
                },
                {
                    label: 'Управління поточними операціями',
                    items: [
                        {
                            label: 'Вибір операції зі списку',
                            icon: 'pi pi-list',
                            command: () => navigate('select'),
                            className: location.pathname === '/dashboard/select' ? 'menu-item-active' : ''
                        },
                        {
                            label: 'Групи та призначені завдання',
                            icon: 'pi pi-users',
                            command: () => navigate('groups'),
                            className: location.pathname === '/dashboard/groups' ? 'menu-item-active' : ''
                        }
                    ]
                },
                {
                    label: 'Сповіщення та повідомлення',
                    items: [
                        {
                            label: 'Запити від координаторів',
                            icon: 'pi pi-question-circle',
                            command: () => navigate('requests'),
                            className: location.pathname === '/dashboard/requests' ? 'menu-item-active' : ''
                        }
                    ]
                }
            );
        }

        if (hasRole('Admin')) {
            menuItems.push(
                {
                    label: 'Адміністрування та доступ',
                    items: [
                        {
                            label: 'Відкрити операцію для погодження',
                            icon: 'pi pi-check-square',
                            command: () => navigate('approve'),
                            className: location.pathname === '/dashboard/approve' ? 'menu-item-active' : ''
                        },
                        {
                            label: 'Звітність',
                            icon: 'pi pi-chart-line',
                            command: () => navigate('reports'),
                            className: location.pathname === '/dashboard/reports' ? 'menu-item-active' : ''
                        }
                    ]
                }
            );
        }

        const hasAccess = hasRole('Dispatcher') || hasRole('Admin');

        return { items: menuItems, hasAccess };
    }, [location.pathname, navigate, user]);

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
            <div className="card side-menu">
                <Menu model={items} />
            </div>
            <div className="content">
                <Outlet />
            </div>
        </div>
    );
}
