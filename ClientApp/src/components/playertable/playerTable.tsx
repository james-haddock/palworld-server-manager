import cx from 'clsx';
import { useState } from 'react';
import { Table, ScrollArea } from '@mantine/core';
import classes from './TableScrollArea.module.css';

const data = [
  {
    player: 'Jmus',
    steamId: 'Jmus7',
  },
  {
    player: 'georgeOak',
    steamId: 'georgeOak655',
  },
  {
    player: 'Claradactyl',
    steamId: 'Claradactyl1345',
  },
];

export function TableScrollArea() {
  const [scrolled, setScrolled] = useState(false);

  const rows = data.map((row) => (
    <Table.Tr key={row.player}>
      <Table.Td>{row.player}</Table.Td>
      <Table.Td className={classes.rightAlign}>{row.steamId}</Table.Td> 
    </Table.Tr>
  ));

  return (
    <ScrollArea h={300} onScrollPositionChange={({ y }) => setScrolled(y !== 0)}>
        <Table className={classes.table}>
        <Table.Thead className={cx(classes.header, { [classes.scrolled]: scrolled })}>
          <Table.Tr>
            <Table.Th>Player Name</Table.Th>
            <Table.Th className={classes.rightAlign}>Steam ID</Table.Th>
          </Table.Tr>
        </Table.Thead>
        <Table.Tbody>{rows}</Table.Tbody>
      </Table>
    </ScrollArea> 
  );
}