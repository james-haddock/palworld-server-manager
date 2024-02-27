// import { gql, useMutation } from '@apollo/client';


// const START_SERVER = gql`
//   mutation {
//     startServer
//   }
// `;

// function StartServer() {
//     const [startServer, { data, loading, error }] = useMutation(START_SERVER);
//     if (loading) return 'Sending server start command...';
//     if (error) return `Server start  error! ${error.message}`;
//     if (data) return `Server started! ${data.startServer}`;

//     startServer()
//   }