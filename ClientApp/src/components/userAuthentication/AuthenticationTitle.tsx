import { useState, FormEvent } from "react";
import { useMutation, gql } from "@apollo/client";
import { useNavigate } from "react-router-dom";
import {
  TextInput,
  PasswordInput,
  Checkbox,
  Anchor,
  Paper,
  Title,
  Text,
  Container,
  Group,
  Button,
  Notification,
} from "@mantine/core";
import classes from "./AuthenticationTitle.module.css";
import { useAuth } from "../userAuthentication/AuthContext";

// interface AuthenticationTitleProps {
//   onLogin: (username: string, password: string, rememberMe: boolean) => void;
// }

const LOGIN_MUTATION = gql`
  mutation Login($input: LoginInput!) {
    login(input: $input) {
      token
      role
    }
  }
`;

export function AuthenticationTitle() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [rememberMe, setRememberMe] = useState(false);
  const [error, setError] = useState("");
  const navigate = useNavigate();
  const { login } = useAuth();

  const [loginMutation] = useMutation(LOGIN_MUTATION);

  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault();
    try {
      const response = await loginMutation({
        variables: {
          input: { username, password },
        },
      });
      if (response.data) {
        const { token, role } = response.data.login;
        localStorage.setItem("token", token);
        login(role);
        navigate("/");
      }
    } catch (error) {
      console.error("Login failed:", error);
      setError(
        "Failed to log in. Please check your username and password or try again later.",
      );
    }
  };

  return (
    <Container size={420} my={40}>
      <Title ta="center" className={classes.title}>
        Welcome back!
      </Title>
      <Text c="dimmed" size="sm" ta="center" mt={5}>
        Do not have an account yet?{" "}
        <Anchor size="sm" component="button">
          Create account
        </Anchor>
      </Text>

      {error && (
        <Notification color="red" onClose={() => setError("")}>
          {error}
        </Notification>
      )}

      <Paper withBorder shadow="md" p={30} mt={30} radius="md">
        <form onSubmit={handleSubmit}>
          <TextInput
            label="Username"
            placeholder="Your Username"
            required
            value={username}
            onChange={(event) => setUsername(event.currentTarget.value)}
          />
          <PasswordInput
            label="Password"
            placeholder="Your password"
            required
            mt="md"
            value={password}
            onChange={(event) => setPassword(event.currentTarget.value)}
          />
          <Group justify="space-between" mt="lg">
            <Checkbox
              label="Remember me"
              checked={rememberMe}
              onChange={() => setRememberMe(!rememberMe)}
            />
            <Anchor component="button" size="sm">
              Forgot password?
            </Anchor>
          </Group>
          <Button onClick={handleSubmit} fullWidth mt="xl">
            Sign in
          </Button>
        </form>
      </Paper>
    </Container>
  );
}
