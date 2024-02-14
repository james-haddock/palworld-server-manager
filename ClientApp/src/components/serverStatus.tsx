import { useEffect , useState } from 'react';
import { Paper } from '@mantine/core';
import { useQuery, gql } from '@apollo/client';

// const SERVER_INFO = gql`
// mutation {
//     sendRconCommand(input: { command: "Info" }) {
//       response
//     }
//   }
// `;

const SERVER_STATUS_CHECK = gql`query {
    getServerStatus {
      response
    }
  }
`;

export function ServerStatus() {
    const [serverStatus, setServerStatus] = useState('Checking Server Status...');
    const { data } = useQuery(SERVER_STATUS_CHECK);

    useEffect(() => {
        const handleServerStatus = async () => {
            try {
                if (data?.serverStatusChecker?.response === "Online") {
                    setServerStatus('Online 🟢');
                } else {
                    setServerStatus('Offline 🔴');
                }
            } catch (error) {
                console.error("Error gathering server status:", error);
                setServerStatus('Offline 🔴');
            }
        };

        handleServerStatus();
        const intervalId = setInterval(handleServerStatus, 4000);

        return () => clearInterval(intervalId);
    }, [data]);

    return <Paper shadow="md" p="xl">Server Status<p>{serverStatus}</p></Paper>
}