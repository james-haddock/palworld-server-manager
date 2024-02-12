import './App.css'
import { ApolloProvider, useMutation, gql } from '@apollo/client';
import client from './apolloClient';

const STOP_SERVER = gql`
  mutation StopServer {
    stopServer
  }
`;

const START_SERVER = gql`
  mutation StartServer {
    startServer
  }
`;

const ServerControls: React.FC = () => {
  const [stopServer] = useMutation(STOP_SERVER);
  const [startServer] = useMutation(START_SERVER);

  return (
    <div>
      <button onClick={() => stopServer()}>Stop Server</button>
      <button onClick={() => startServer()}>Start Server</button>
    </div>
  );
}

const App: React.FC = () => {
  return (
    <ApolloProvider client={client}>
      <ServerControls />
    </ApolloProvider>
  );
}

export default App;