import '@mantine/core/styles.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import HomePage from './pages/HomePage';
import AdminPage from './pages/AdminPage';
import ConfigPage from './pages/ConfigPage';
// import UsersPage from './pages/UsersPage';
import OtherPage from './pages/OtherPage';
import { NavbarSimple } from "./components/navbar/NavbarSimple";
import './App.css';


const App: React.FC = () => {
  return (
        <Router>
          <NavbarSimple />
          <main className='main-content'>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/admin" element={<AdminPage />} />
            <Route path="/config" element={<ConfigPage />} />
            <Route path="/other" element={<OtherPage />} />
          </Routes>
          </main>
        </Router>
  );
}

export default App;