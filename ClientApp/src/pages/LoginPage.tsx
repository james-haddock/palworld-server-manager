import React from "react";
import { useNavigate } from "react-router-dom";
import { useMutation, gql } from "@apollo/client";
import { Flex } from "@mantine/core";
import { AuthenticationTitle } from "../components/userAuthentication/AuthenticationTitle";

const LOGIN_MUTATION = gql`
  mutation Login($username: String!, $password: String!) {
    login(username: $username, password: $password) {
      token
      username {
        id
        email
      }
    }
  }
`;

const LoginPage: React.FC = () => {
  const navigate = useNavigate();
  // const [login, { data }] = useMutation(LOGIN_MUTATION);
  const [login] = useMutation(LOGIN_MUTATION);

  const handleLogin = async (
    username: string,
    password: string,
    // rememberMe: boolean,
  ) => {
    const response = await login({ variables: { username, password } });

    if (response.data) {
      navigate("/");
    } else {
      console.error("Login failed");
    }
  };

  return (
    <Flex direction="column" align="center">
      <AuthenticationTitle onLogin={handleLogin} />
    </Flex>
  );
};

export default LoginPage;
