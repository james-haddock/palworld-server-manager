# Palworld Dedicated Server Manager

![Banner](https://github.com/james-haddock/palworld-server-manager/assets/123553781/216e5b6a-4a14-4c44-a879-7090e1f9e076)

## Overview

This application is a dedicated server manager for the game "Palworld". It's built with ASP.NET for the backend and React with Vite for the frontend. The application communicates with the frontend via GraphQL.

The application automates the process of setting up a Palworld dedicated server. It downloads and installs `steamcmd`, logs in anonymously, and downloads the Palworld server software. The application also provides a web-based user interface for managing the server locally or remotely.

## Prerequisites

- **CPU** - 4 Cores (Recommended).

- **RAM** - 16GB minimum / 32GB recommended for stable operation.

- **Network** - UDP Port 8211 (Default).

- **OS** - Windows 10 / 11.

- **Installed Software** - Steamcmd, Direct X and C++ runtime (Bundled in installer if not already installed).

## Features

- **Automated Server Setup**: The application automatically downloads and installs `steamcmd` and the Palworld server software. No Steam account is required as the application logs in anonymously.

- **Web-based User Interface**: The application provides a web-based user interface for managing the server. The interface includes a setup wizard for configuring the server.

- **UPnP Support**: The application uses UPnP to automatically configure port forwarding on the user's router, if the router supports UPnP. If the router does not support UPnP, port forwarding will need to be configured manually.

- **Server Management**: The user can start and stop the server through the web interface. The interface also provides administrative controls, such as the ability to kick or ban players.

- **Configurable Server Settings**: The server settings can be changed through the web interface. Changes can be applied by rebooting the server.

- **Automatic Updates**: The application automatically updates the server software.

- **Automated Backups**: The application automatically backs up the server and save files.

- **Remote Access**: The application can be accessed remotely from anywhere via the internet.

## Technical Implementation

The application is built with ASP.NET for the backend and React with Vite for the frontend. It uses GraphQL for communication between the frontend and backend.

`steamcmd` is used to download and install the Palworld server software. It logs in to Steam anonymously, so no Steam account is required.

A web-based user interface is provided for managing the server. The interface is built with React and Vite, and communicates with the backend via GraphQL.

Optional use of UPnP to automatically configure port forwarding on the user's router, if the router supports UPnP. This is done using a UPnP library.

Server settings can be changed through the web interface. The settings are stored in a configuration file, and changes are applied by rebooting the server.

Automatic software updates can be enabled whenever a new version is released.

Regular automated backups of the server and save files by copying them to a backup directory at regular intervals.

The server instance can be accessed remotely from anywhere via the internet.

## How to Use

1. Run the application. This will start the backend and open the web interface in your default web browser.

2. Follow the setup wizard in the web interface to configure your server.

3. Once the server is configured, you can start it through the web interface.

4. You can use the administrative controls in the web interface to manage your server. You can start and stop the server, kick or ban players, and change the server settings.

5. To apply changes to the server settings, you will need to reboot the server. You can do this through the web interface.

6. The application will automatically update the server software and back up the server and save files. You don't need to do anything to enable these features.

7. You can access the application remotely from anywhere via the internet. Just open the web interface in your web browser and log in.

## Future Work

This application is a work in progress. Future updates will include more features and improvements. If you have any suggestions or feedback, please feel free to open an issue or submit a pull request.
