import '@mantine/core/styles.css';
import { MantineProvider } from '@mantine/core';
import { ApolloProvider } from '@apollo/client';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import HomePage from './pages/HomePage';
import AdminPage from './pages/AdminPage';
// import ConfigPage from './pages/ConfigPage';
// import UsersPage from './pages/UsersPage';
// import OtherPage from './pages/OtherPage';
import { NavbarSimple } from "./components/navbar/NavbarSimple"; 
import client from './apolloClient';

const App: React.FC = () => {
  return (
    <ApolloProvider client={client}>
      <MantineProvider>
      <NavbarSimple />
        <Router>
          <Routes>
          <Route path="/" element={<HomePage />} />
    <Route path="/admin" element={<AdminPage />} />
          {/* <Route path="/config" element={<ConfigPage />} />
          <Route path="/users" element={<UsersPage />} />
  <Route path="/other" element={<OtherPage />} /> */}
          </Routes>
        </Router>
      </MantineProvider>
    </ApolloProvider>
  );
}

export default App;