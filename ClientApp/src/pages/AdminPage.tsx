import React, { useEffect, useState } from 'react';
import { Text, Flex, SegmentedControl } from '@mantine/core';
import { modals } from '@mantine/modals';
import { useMutation, gql } from '@apollo/client';

const START_SERVER = gql`
  mutation {
    startServer
  }
`;

const STOP_SERVER = gql`
  mutation {
    stopServer
  }
`;

const RESTART_SERVER = gql`
  mutation {
    restartServer
  }
`;

const SERVER_INFO = gql`
mutation {
    sendRconCommand(input: { command: "Info" }) {
      response
    }
  }
`;



const AdminPage: React.FC = () => {
  const [status, setStatus] = useState('Offline');
  const [message, setMessage] = useState('');
  const [startServer] = useMutation(START_SERVER);
  const [stopServer] = useMutation(STOP_SERVER);
  const [restartServer] = useMutation(RESTART_SERVER);

  const [serverInfo] = useMutation(SERVER_INFO);
    
  async function CheckServerStatus(): Promise<string> {
    return serverInfo().then(response => {
      if (response.data) {
        return "Online";
      } else {
        return "Offline";
      }
    });
  }


  function commandServer(status: string) {
    if (status === 'Online') {
      setMessage('Starting server...');
      startServer().then(() => {
        setMessage(`Server started successfully.`);
      }).catch((error) => {
        setMessage(`Server start error! ${error.message}`);
      });
    } else if (status === 'Offline') {
      setMessage('Stopping server...');
      stopServer().then(() => {
        setMessage(`Server stopped successfully.`);
      }).catch((error) => {
        setMessage(`Server stop error. ${error.message}`);
      });
    } else if (status === 'Reboot') {
      modals.openConfirmModal({
        title: 'Please confirm your action',
        children: (
          <Text size="sm">
            Are you sure that you want to reboot the server?
          </Text>
        ),
        labels: { confirm: 'Confirm', cancel: 'Cancel' },
        onCancel: () => (console.log('Cancel'), setStatus('Online')),
        onConfirm: () => {
          setMessage('Rebooting server...');
          restartServer().then(() => {
            setMessage(`Server restarted successfully.`);
          }).catch((error) => {
            setMessage(`Server restart error. ${error.message}`);
          });
        }
      });
    }
  }

  useEffect(() => {
    CheckServerStatus().then(result => {
      setStatus(result);
    });
  }, []);

  return (
    <Flex direction="column" align="flex-start" gap="md">
      <Text size="xl" fw={700}>Server Admin</Text>
      <div>
        <Text size="lg">Server Status</Text>
        <SegmentedControl size="md" radius="xl" data={['Reboot', 'Online', 'Offline']} value={status} defaultValue={status} onChange={(value) => {
          setStatus(value);
          commandServer(value);
        }} />
        <Text>{message}</Text>
      </div>
    </Flex>
  )
}


export default AdminPage;