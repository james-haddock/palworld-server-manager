import { useState } from 'react';
import { Group, Code } from '@mantine/core';
import { Text } from '@mantine/core';
import {

  IconSettings,
  IconSwitchHorizontal,
  IconLogout,
  IconUsers,
  IconSmartHome,
  IconServerCog,
  IconPhotoCircle,
} from '@tabler/icons-react';
import classes from './NavbarSimple.module.css';

const data = [
  { link: '/', label: 'Home', icon: IconSmartHome },
  { link: 'admin', label: 'Server Admin', icon: IconPhotoCircle },
  { link: 'config', label: 'Server Config', icon: IconServerCog },
  { link: 'users', label: 'Users & Permissions', icon: IconUsers },
  { link: 'other', label: 'Other Settings', icon: IconSettings },
];

export function NavbarSimple() {
  const [active, setActive] = useState('Home');

  const links = data.map((item) => (
    <a
      className={classes.link}
      data-active={item.label === active || undefined}
      href={item.link}
      key={item.label}
      onClick={(event) => {
        event.preventDefault();
        setActive(item.label);
      }}
    >
      <item.icon className={classes.linkIcon} stroke={1.5} />
      <span>{item.label}</span>
    </a>
  ));

  return (
    <nav className={classes.navbar}>
      <div className={classes.navbarMain}>
        <Group className={classes.header} justify="space-between">
        <Text fw={800} size="xl"
      variant="gradient"
      gradient={{ from: 'yellow', to: 'purple', deg: 90 }}>Palworld Server Manager</Text>
          <Code fw={700}>v0.0.1</Code>
        </Group>
        {links}
      </div>

      <div className={classes.footer}>
        <a href="#" className={classes.link} onClick={(event) => event.preventDefault()}>
          <IconSwitchHorizontal className={classes.linkIcon} stroke={1.5} />
          <span>Change account</span>
        </a>

        <a href="#" className={classes.link} onClick={(event) => event.preventDefault()}>
          <IconLogout className={classes.linkIcon} stroke={1.5} />
          <span>Logout</span>
        </a>
      </div>
    </nav>
  );
}