import "@mantine/core/styles.css";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import HomePage from "./pages/HomePage";
import AdminPage from "./pages/AdminPage";
import ConfigPage from "./pages/ConfigPage";
import LoginPage from "./pages/LoginPage";
import OtherPage from "./pages/OtherPage";
import { NavbarSimple } from "./components/navbar/NavbarSimple";
import "./App.css";
import { ServerStatusProvider } from "./components/serverStatusProvider.tsx";
import { AuthProvider } from "./components/userAuthentication/AuthContext";
import PrivateRoute from "./components/PrivateRoute";
import NotFoundPage from "./pages/404Page";
import UnauthorisedPage from "./pages/401Page";

const App: React.FC = () => {
  return (
    <AuthProvider>
      <ServerStatusProvider>
        <Router>
          <NavbarSimple />
          <main className="main-content">
            <Routes>
              <Route path="/login" element={<LoginPage />} />
              <Route
                path="/"
                element={
                  <PrivateRoute
                    role="user"
                    element={
                      <div className="marginleft">
                        <HomePage />
                      </div>
                    }
                  />
                }
              />
              <Route
                path="/admin"
                element={
                  <PrivateRoute
                    role="user"
                    element={
                      <div className="marginleft">
                        <AdminPage />
                      </div>
                    }
                  />
                }
              />
              <Route
                path="/config"
                element={
                  <PrivateRoute
                    role="admin"
                    element={
                      <div className="marginleft">
                        <ConfigPage />
                      </div>
                    }
                  />
                }
              />
              <Route
                path="/other"
                element={
                  <PrivateRoute
                    role="user"
                    element={
                      <div className="marginleft">
                        <OtherPage />
                      </div>
                    }
                  />
                }
              />
              <Route
                path="/401"
                element={
                  <div className="marginleft">
                    <UnauthorisedPage />
                  </div>
                }
              />
              <Route path="/*" element={<NotFoundPage />} />
            </Routes>
          </main>
        </Router>
      </ServerStatusProvider>
    </AuthProvider>
  );
};

export default App;
