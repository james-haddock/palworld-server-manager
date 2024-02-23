import '@mantine/core/styles.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import HomePage from './pages/HomePage';
import AdminPage from './pages/AdminPage';
import ConfigPage from './pages/ConfigPage';
// import UsersPage from './pages/UsersPage';
import LoginPage from './pages/LoginPage';
import OtherPage from './pages/OtherPage';
import { NavbarSimple } from "./components/navbar/NavbarSimple";
import './App.css';
import { ServerStatusProvider } from './components/serverStatusProvider.tsx';


const App: React.FC = () => {
  return (
    <ServerStatusProvider>
        <Router>
          <NavbarSimple />
          <main className='main-content'>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/admin" element={<AdminPage />} />
            <Route path="/config" element={<ConfigPage />} />
            <Route path="/other" element={<OtherPage />} />
            <Route path="/login" element={<LoginPage />} />
          </Routes>
          </main>
        </Router>
        </ServerStatusProvider>
  );
}

export default App;