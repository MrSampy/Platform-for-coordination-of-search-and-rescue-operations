import { Navigate, Outlet } from 'react-router-dom';

const RequireAuth = () => {
    const token = localStorage.getItem('token');

    if (!token) {
        return <Navigate to="/" replace />; // Redirect to login
    }

    return <Outlet />; // Proceed to child routes (like /dashboard)
};

export default RequireAuth;
