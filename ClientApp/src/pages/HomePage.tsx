import React, { useContext, useState, useEffect } from "react";
import { Grid, Container, Paper, Loader, Text } from "@mantine/core";
import { TableScrollArea } from "../components/playertable/playerTable";
import IPAddress from "../components/ipAddress";
import { ServerStatusContext } from "../components/serverStatusContext";

// const child = <Skeleton height={140} radius="md" animate={false} />;

const HomePage: React.FC = () => {
  const [ip, setIp] = useState("");
  const { serverStatus, serverUpTime, setServerUpTime } =
    useContext(ServerStatusContext);

  useEffect(() => {
    IPAddress(setIp);
  }, []);

  useEffect(() => {
    if (serverStatus === "Online") {
      const intervalId = setInterval(() => {
        const matches = serverUpTime.match(/\d+/g);
        if (matches) {
          const [days, hours, minutes, seconds] = matches.map(Number);
          let totalSeconds =
            days * 24 * 60 * 60 + hours * 60 * 60 + minutes * 60 + seconds + 1;
          const newDays = Math.floor(totalSeconds / (24 * 60 * 60));
          totalSeconds %= 24 * 60 * 60;
          const newHours = Math.floor(totalSeconds / (60 * 60));
          totalSeconds %= 60 * 60;
          const newMinutes = Math.floor(totalSeconds / 60);
          const newSeconds = totalSeconds % 60;
          setServerUpTime(
            `${newDays}d ${newHours}h ${newMinutes}m ${newSeconds}s`,
          );
        }
      }, 1000);

      return () => clearInterval(intervalId);
    }
  }, [serverStatus, serverUpTime, setServerUpTime]);

  return (
    <>
      {" "}
      <Text size="xl" fw={700}>
        Dashboard
      </Text>
      <Container size="xl" my="md">
        <Grid>
          <Grid.Col span={{ base: 12, xs: 7 }}>
            <Paper shadow="md" p="xl">
              Server Status
              <p>{serverStatus === "Online" ? "Online 🟢" : "Offline 🔴"}</p>
            </Paper>
          </Grid.Col>
          <Grid.Col span={{ base: 12, xs: 5 }}>
            <Paper shadow="md" p="xl">
              Server Up Time
              <p>{serverStatus === "Online" ? serverUpTime : "0d 0h 0m 0s"}</p>
            </Paper>
          </Grid.Col>
          <Grid.Col span={{ base: 12, xs: 8 }}>
            <Paper shadow="md" p="xl">
              Currently Playing
              <TableScrollArea />
            </Paper>
          </Grid.Col>
          <Grid.Col span={{ base: 12, xs: 4 }}>
            <Paper shadow="md" p="xl">
              IP Address
              <p>{ip === "" ? <Loader /> : ip}</p>
            </Paper>
          </Grid.Col>
        </Grid>
      </Container>
    </>
  );
};

export default HomePage;
