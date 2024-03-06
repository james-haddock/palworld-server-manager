import React from "react";
import { Flex } from "@mantine/core";
import { AuthenticationTitle } from "../components/userAuthentication/AuthenticationTitle";

const LoginPage: React.FC = () => {
  return (
    <Flex direction="column" align="center">
      <AuthenticationTitle />
    </Flex>
  );
};

export default LoginPage;
