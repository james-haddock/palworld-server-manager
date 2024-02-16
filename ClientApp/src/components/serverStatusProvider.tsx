import { useEffect, useState } from 'react';
import { useQuery, gql } from '@apollo/client';
import { ServerStatusContext } from './serverStatusContext';

const SERVER_STATUS_CHECK = gql`query {
    getServerStatus }`;

export function ServerStatusProvider({ children }: { children: React.ReactNode }) {
  const [serverStatus, setServerStatus] = useState('Checking Server Status...');
  const { refetch } = useQuery(SERVER_STATUS_CHECK);

  useEffect(() => {
    const intervalId = setInterval(async () => {
      try {
        const { data } = await refetch();
        if (data?.getServerStatus === "Online") {
          setServerStatus('Online');
        } else {
          setServerStatus('Offline');
        }
      } catch (error) {
        console.error("Error gathering server status:", error);
        setServerStatus('Offline');
      }
    }, 5000);

    return () => clearInterval(intervalId);
  }, [refetch]);

  return (
    <ServerStatusContext.Provider value={{ serverStatus, setServerStatus, refetch }}>
      {children}
    </ServerStatusContext.Provider>
  );
}