// import { useEffect, useState } from 'react';
// import { Paper } from '@mantine/core';
// import { useQuery, gql } from '@apollo/client';

// const SERVER_STATUS_CHECK = gql`query {
//     getServerStatus }`;

// export function ServerStatus() {
//     const [serverStatus, setServerStatus] = useState('Checking Server Status...');
//     const { data } = useQuery(SERVER_STATUS_CHECK);

//     useEffect(() => {
//         const intervalId = setInterval(() => {
//             try {
//                 if (data?.getServerStatus === "Online") {
//                     setServerStatus('Online 🟢');
//                 } else {
//                     setServerStatus('Offline 🔴');
//                 }
//             } catch (error) {
//                 console.error("Error gathering server status:", error);
//                 setServerStatus('Offline 🔴');
//             }
//         }, 5000);

//         return () => clearInterval(intervalId);
//     }, [data]);

//     return <Paper shadow="md" p="xl">Server Status<p>{serverStatus}</p></Paper>
// }