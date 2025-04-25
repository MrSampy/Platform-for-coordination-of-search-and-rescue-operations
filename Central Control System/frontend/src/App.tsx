import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Login from './pages/Login';
import Signup from './pages/Signup'; 
import Dashboard from './pages/Dashboard'; 
import NotFound from './components/NotFound';
import './styles/common.css'
import 'primeicons/primeicons.css';
import 'primeflex/primeflex.css';
import 'primereact/resources/primereact.css';
import 'primereact/resources/themes/lara-light-indigo/theme.css';
import 'primereact/resources/primereact.min.css';
import OperationsPage from './pages/dashboard/OperationsPage';
import OperationsMap from './pages/dashboard/OperationsMap';
import CreateOperation from './pages/dashboard/CreateOperation';
import Groups from './pages/dashboard/Groups';
import CoordinatorsRequests from './pages/dashboard/CoordinatorsRequests';
import ApproveOperations from './pages/dashboard/ApproveOperations';
import Reports from './pages/dashboard/Reports';
import RequireAuth from './components/RequireAuth';
import 'leaflet/dist/leaflet.css';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/signup" element={<Signup />} />

        {/* Protected dashboard */}
        <Route element={<RequireAuth />}>
          <Route path="/dashboard" element={<Dashboard />}>
              <Route path="map" element={<OperationsMap />} />
              <Route path="create" element={<CreateOperation />} />
              <Route path="operations" element={<OperationsPage />} />
              <Route path="groups" element={<Groups />} />
              <Route path="requests" element={<CoordinatorsRequests />} />
              <Route path="approve" element={<ApproveOperations />} />
              <Route path="reports" element={<Reports />} />
          </Route>
        </Route>

        <Route path="*" element={<NotFound />} />
      </Routes>
    </Router>
  );
}
export default App;
