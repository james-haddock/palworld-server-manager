import React from 'react';
import ColourScheme from '../components/colourScheme';
import { Text, Flex } from '@mantine/core'; 

const OtherPage: React.FC = () => {
    return  (    <>
        <Text size="xl" fw={700}>Other Settings</Text>
        <Flex direction="column" align="flex-start" gap="md">
            <div>
            <Text size="lg">Appearance</Text>
        <ColourScheme />
        </div>
</Flex>
</>
)}

export default OtherPage;