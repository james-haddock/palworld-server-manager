import { useMantineColorScheme, Button, Group } from '@mantine/core';

function ColourScheme() {
  const { colorScheme, setColorScheme } = useMantineColorScheme();

  return (
    <Group>
      <Button onClick={() => setColorScheme('light')} color={colorScheme === 'light' ? 'blue' : 'gray'}>Light</Button>
      <Button onClick={() => setColorScheme('dark')} color={colorScheme === 'dark' ? 'blue' : 'gray'}>Dark</Button>
      <Button onClick={() => setColorScheme('auto')} color={colorScheme === 'auto' ? 'blue' : 'gray'}>System</Button>
    </Group>
  );
}
export default ColourScheme;