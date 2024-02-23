import { ApolloQueryResult } from "@apollo/client";
import React from "react";

interface ServerStatusContextType {
  serverStatus: string;
  setServerStatus: (_: string) => void;
  refetch: () => Promise<ApolloQueryResult<any>>;
}

export const ServerStatusContext = React.createContext<ServerStatusContextType>(
  {
    serverStatus: "Checking Server Status...",
    setServerStatus: (_: string) => {},
    refetch: async () => {
      throw new Error("Refetch function must be overridden");
    },
  },
);
