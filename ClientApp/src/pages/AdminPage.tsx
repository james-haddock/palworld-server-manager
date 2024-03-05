import React, { useContext, useState, useEffect } from "react";
import { Text, Flex, SegmentedControl } from "@mantine/core";
import { modals } from "@mantine/modals";
import { useMutation, gql } from "@apollo/client";
import { ServerStatusContext } from "../components/serverStatusContext";
import classes from "./static/GradientSegmentedControl.module.css";

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

const AdminPage: React.FC = () => {
  const [status, setStatus] = useState("Offline");
  const [message, setMessage] = useState("");
  const [startServer] = useMutation(START_SERVER);
  const [stopServer] = useMutation(STOP_SERVER);
  const [restartServer] = useMutation(RESTART_SERVER);

  const { serverStatus, refetch } = useContext(ServerStatusContext);

  useEffect(() => {
    setStatus(serverStatus);
  }, [serverStatus]);

  function commandServer(status: string) {
    if (status === "Online") {
      setMessage("Starting server...");
      startServer()
        .then(() => {
          setMessage("Server started successfully.");
        })
        .catch((error) => {
          setMessage(`Server start error! ${error.message}`);
        });
    } else if (status === "Offline") {
      setMessage("Stopping server...");
      stopServer()
        .then(() => {
          setMessage("Shutting down server...");
          const intervalId = setInterval(async () => {
            const { data } = await refetch();
            if (data?.getServerStatus === "Offline") {
              setMessage("Server shutdown successful.");
              clearInterval(intervalId);
            }
          }, 1000);
        })
        .catch((error) => {
          setMessage(`Server stop error. ${error.message}`);
        });
    } else if (status === "Reboot") {
      modals.openConfirmModal({
        title: "Please confirm your action",
        children: (
          <Text size="sm">
            Are you sure that you want to reboot the server?
          </Text>
        ),
        labels: { confirm: "Confirm", cancel: "Cancel" },
        onCancel: () => (console.log("Cancel"), setStatus("Online")),
        onConfirm: () => {
          setMessage("Rebooting server...");
          restartServer()
            .then(() => {
              setMessage(`Server restarted successfully.`);
              setStatus("Online");
            })
            .catch((error) => {
              setMessage(`Server restart error. ${error.message}`);
              setMessage(`Server restarted successfully.`);
            });
        },
      });
    }
  }

  return (
    <Flex direction="column" align="flex-start" gap="md">
      <Text size="xl" fw={700}>
        Server Admin
      </Text>
      <div>
        <Text size="lg">Server Status</Text>
        <SegmentedControl
          size="md"
          radius="xl"
          data={["Reboot", "Online", "Offline"]}
          classNames={classes}
          value={status}
          defaultValue={serverStatus}
          onChange={(value) => {
            setStatus(value);
            commandServer(value);
          }}
        />
        <Text>{message}</Text>
      </div>
    </Flex>
  );
};

export default AdminPage;
