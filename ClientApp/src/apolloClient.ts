import { ApolloClient, InMemoryCache } from '@apollo/client';

const client = new ApolloClient({
  uri: 'http://localhost:80/graphql', // replace with your server's URI
  cache: new InMemoryCache()
});

export default client;