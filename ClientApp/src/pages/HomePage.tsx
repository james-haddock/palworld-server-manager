import React, { useContext, useState, useEffect } from 'react';
import { Grid, Container, Paper, Loader, Text } from '@mantine/core';
import { TableScrollArea } from '../components/playertable/playerTable';
import IPAddress from '../components/ipAddress';
import { ServerStatusContext } from '../components/serverStatusContext';

// const child = <Skeleton height={140} radius="md" animate={false} />;

const HomePage: React.FC = () => {
  const [ip, setIp] = useState(''); 
  const { serverStatus } = useContext(ServerStatusContext)

  useEffect(() => {
    IPAddress(setIp);
  }, []);

  return (
    <>  <Text size="xl" fw={700}>Dashboard</Text>
    <Container size="xl" my="md">
      <Grid>
        <Grid.Col span={{ base: 12, xs: 7 }}>
        <Paper shadow="md" p="xl">Server Status<p>{serverStatus === "Online" ? "Online 🟢" : "Offline 🔴"}</p></Paper>
        </Grid.Col>
        <Grid.Col span={{ base: 12, xs: 5 }}>
          <Paper shadow="md" p="xl">Server Up Time<p>0:00</p></Paper>
        </Grid.Col>
        <Grid.Col span={{ base: 12, xs: 8 }}>
          <Paper shadow="md" p="xl">Currently Playing<TableScrollArea /></Paper>
        </Grid.Col>
        <Grid.Col span={{ base: 12, xs: 4 }}>
          <Paper shadow="md" p="xl">
            IP Address
            <p>{ip === "" ? <Loader /> : ip}</p>
          </Paper>
        </Grid.Col>
        {/* <Grid.Col span={{ base: 12, xs: 3 }}>{child}</Grid.Col>
        <Grid.Col span={{ base: 12, xs: 3 }}>{child}</Grid.Col>
        <Grid.Col span={{ base: 12, xs: 6 }}>{child}</Grid.Col> */}
      </Grid>
    </Container>
    </>
  );
}

export default HomePage;