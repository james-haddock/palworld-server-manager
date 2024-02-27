import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import "./index.css";
import { MantineProvider } from "@mantine/core";
import { ApolloProvider } from "@apollo/client";
import client from "./apolloClient";
import { ModalsProvider } from "@mantine/modals";

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <ApolloProvider client={client}>
      <MantineProvider defaultColorScheme="auto">
        <ModalsProvider>
          <App />
        </ModalsProvider>
      </MantineProvider>
    </ApolloProvider>
  </React.StrictMode>,
);
