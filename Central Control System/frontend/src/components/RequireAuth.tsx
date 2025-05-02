import { Navigate, Outlet } from 'react-router-dom';
import { getValidToken } from '../services/commonService';

const RequireAuth = () => {
    const token = getValidToken();

    if (!token) {
        return <Navigate to="/" replace />; // Redirect to login
    }

    return <Outlet />; // Proceed to child routes (like /dashboard)
};

export default RequireAuth;
