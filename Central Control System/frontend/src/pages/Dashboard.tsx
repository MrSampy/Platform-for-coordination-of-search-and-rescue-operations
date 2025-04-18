import { Menu } from 'primereact/menu';
import { useLocation, useNavigate, Outlet } from 'react-router-dom';
import '../styles/dashboard.css';

export default function Dashboard() {
    const navigate = useNavigate();
    const location = useLocation();

    const items = [
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
        },
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
    ];

    return (
        <div className="layout">
            <div className="card">
                <Menu model={items} />
            </div>
            <div className="content">
                <Outlet />
            </div>
        </div>
    );
}
