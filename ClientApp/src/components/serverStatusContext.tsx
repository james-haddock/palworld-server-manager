import { ApolloQueryResult } from "@apollo/client";
import React from "react";

interface ServerStatusContextType {
  serverUpTime: string;
  setServerUpTime: (_: string) => void;
  serverStatus: string;
  setServerStatus: (_: string) => void;
  serverInfo: string;
  setServerInfo: (_: string) => void;
  refetch: () => Promise<ApolloQueryResult<any>>;
}

export const ServerStatusContext = React.createContext<ServerStatusContextType>(
  {
    serverStatus: "Checking Server Status...",
    setServerStatus: (_: string) => {},
    serverUpTime: "Checking Server Uptime...",
    setServerUpTime: (_: string) => {},
    serverInfo: "Checking Server Info...",
    setServerInfo: (_: string) => {},
    refetch: async () => {
      throw new Error("Refetch function must be overridden");
    },
  },
);
