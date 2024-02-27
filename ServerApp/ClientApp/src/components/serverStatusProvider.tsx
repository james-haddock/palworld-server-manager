import { useEffect, useState } from "react";
import { useQuery, gql } from "@apollo/client";
import { ServerStatusContext } from "./serverStatusContext";

const SERVER_STATUS_CHECK = gql`
  query {
    getServerInfo {
      serverStatus
      serverUpTime
    }
  }
`;

export function ServerStatusProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  const [serverStatus, setServerStatus] = useState("Checking Server Status...");
  const [serverUpTime, setServerUpTime] = useState("0:00");
  const { refetch } = useQuery(SERVER_STATUS_CHECK);

  useEffect(() => {
    const intervalId = setInterval(async () => {
      try {
        const { data } = await refetch();
        if (data?.getServerInfo.serverStatus === "Online") {
          setServerStatus("Online");
          setServerUpTime(data.getServerInfo.serverUpTime);
        } else {
          setServerStatus("Offline");
          setServerUpTime("0:00");
        }
      } catch (error) {
        console.error("Error gathering server status:", error);
        setServerStatus("Offline");
        setServerUpTime("0:00");
      }
    }, 5000);

    return () => clearInterval(intervalId);
  }, [refetch]);

  return (
    <ServerStatusContext.Provider
      value={{
        serverStatus,
        setServerStatus,
        serverUpTime,
        setServerUpTime,
        refetch,
      }}
    >
      {children}
    </ServerStatusContext.Provider>
  );
}
