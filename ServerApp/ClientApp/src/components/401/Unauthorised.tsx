import {
  Image,
  Container,
  Title,
  Text,
  Button,
  SimpleGrid,
} from "@mantine/core";
import image from "./depresso.png";
import classes from "./Unauthorised.module.css";
import { useNavigate } from "react-router-dom";

export function Unauthorised() {
  const navigate = useNavigate();
  return (
    <Container className={classes.root}>
      <SimpleGrid spacing={{ base: 40, sm: 80 }} cols={{ base: 1, sm: 2 }}>
        <Image src={image} className={classes.mobileImage} />
        <div>
          <Title className={classes.title}>Unauthorized Access...</Title>
          <Text c="dimmed" size="lg">
            You do not have the necessary permissions to view this page. If you
            think this is an error contact support.
          </Text>
          <Button
            variant="outline"
            size="md"
            mt="xl"
            className={classes.control}
            onClick={() => {
              navigate("/");
            }}
          >
            Return to home page
          </Button>
        </div>
        <Image src={image} className={classes.desktopImage} />
      </SimpleGrid>
    </Container>
  );
}
