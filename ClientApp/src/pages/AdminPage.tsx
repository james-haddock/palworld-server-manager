import React, { useState, useEffect } from 'react';
import { Text, Flex, SegmentedControl, Button } from '@mantine/core'; 
import { rebootModal } from '../components/modals';



const AdminPage: React.FC = () => {
  const [status, setStatus] = useState('Offline');

  function commandServer(status: string){
    if (status === 'Online') {
      console.log('Server is online');

    } else if (status === 'Offline') {
      console.log('Server is offline');

    }
  }

  return ( 
  <Flex direction="column" align="flex-start" gap="md">
  <Text size="xl" fw={700}>Server Admin</Text>
  <div>
  <Text size="lg">Server Status</Text>
  <SegmentedControl size="md" radius="xl" data={['Online', 'Offline']} defaultValue='Offline'   onChange={(value) => {
    setStatus(value);
    commandServer(status);
  }}/>
  </div>
  <div>
  <Text size="lg">Reboot Server</Text>
  <Button variant="gradient"
      gradient={{ from: 'red', to: 'grape', deg: 90 }}
      onClick={rebootModal}>
      Restart
    </Button>
  </div>
  </Flex>
)}

export default AdminPage;