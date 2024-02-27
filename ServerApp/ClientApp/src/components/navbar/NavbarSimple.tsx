// import { useState } from 'react';
import { useLocation } from "react-router-dom";
import { Group, Code } from "@mantine/core";
import { Text } from "@mantine/core";
import { Link } from "react-router-dom";
import {
  IconSettings,
  IconLogout,
  IconUsers,
  IconServerCog,
  IconPhotoCircle,
  IconLayoutDashboard,
} from "@tabler/icons-react";
import classes from "./NavbarSimple.module.css";
import { useAuth } from "../userAuthentication/AuthContext";

const data = [
  { link: "/", label: "Dashboard", icon: IconLayoutDashboard },
  { link: "/admin", label: "Server Admin", icon: IconPhotoCircle },
  { link: "/config", label: "Server Config", icon: IconServerCog },
  { link: "/users", label: "Users & Permissions", icon: IconUsers },
  { link: "/other", label: "Other Settings", icon: IconSettings },
];

export function NavbarSimple() {
  const { logout, isAuthenticated } = useAuth();
  const currentPath = useLocation().pathname;

  if (!isAuthenticated || currentPath === "/*") {
    return null;
  }

  const links = data.map((item) => (
    <Link
      className={`${classes.link} ${location.pathname === item.link ? classes.linkActive : ""}`}
      to={item.link}
      key={item.label}
    >
      <item.icon className={classes.linkIcon} stroke={1.5} />
      <span>{item.label}</span>
    </Link>
  ));

  return (
    <nav className={classes.navbar}>
      <div className={classes.navbarMain}>
        <Group className={classes.header} justify="space-between">
          <Text
            fw={800}
            size="xl"
            variant="gradient"
            gradient={{ from: "yellow", to: "purple", deg: 90 }}
          >
            Palworld Server Manager
          </Text>
          <Code fw={700}>v0.0.1</Code>
        </Group>
        {links}
      </div>

      <div className={classes.footer}>
        <a href="/" className={classes.link} onClick={logout}>
          <IconLogout className={classes.linkIcon} stroke={1.5} />
          <span>Logout</span>
        </a>
      </div>
    </nav>
  );
}
