import axios from 'axios';

const IPAddress = async (setIp: (value: string) => void) => {
  const response = await axios.get('https://api.ipify.org?format=json');
  setIp(response.data.ip);
};

export default IPAddress;