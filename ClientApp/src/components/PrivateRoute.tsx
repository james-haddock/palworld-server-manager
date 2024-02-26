import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "./userAuthentication/AuthContext";

const PrivateRoute = ({
  element,
  role,
}: {
  element: React.ReactElement;
  role?: string;
}) => {
  const { isAuthenticated, userRole } = useAuth();
  const location = useLocation();

  if (isAuthenticated && (!role || userRole === role)) {
    return element;
  } else if (isAuthenticated) {
    return <Navigate to="/401" state={{ from: location }} replace />;
  } else {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }
};

export default PrivateRoute;
