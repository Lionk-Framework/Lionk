Here's the documentation for deploying LionkApp on a PC using Docker:

# How to Deploy LionkApp on a PC using Docker

This guide explains how to deploy LionkApp on a PC using Docker and Docker Compose.

## 1. Install Docker

If Docker is not installed on your PC, you can install it using the following instructions.

### On Ubuntu/Debian
```bash
sudo apt-get update
sudo apt-get install docker
sudo apt-get install docker-compose
```

### On Windows or macOS
Download and install Docker Desktop from the official [Docker website](https://docs.docker.com/get-started/).

## 2. Create a `docker-compose.yml` File

Create a `docker-compose.yml` file in the directory where you want to deploy LionkApp. You can use the following template:

```yaml
version: '3.8'

services:
  lionkapp:
    image: ghcr.io/lionk-framework/lionkapp:latest
    volumes:
      - ./config:/app/config
      - ./plugins:/app/plugins
      - ./data:/app/data
      - ./logs:/app/logs
      - ./temp:/app/temp
      - ./keys:/root/.aspnet/DataProtection-Keys
    ports:
      - "5001:5001"
    restart: always
```

### 2.1. Explanation of the `docker-compose.yml` File
- `image`: The image used to deploy LionkApp. Use `ghcr.io/lionk-framework/lionkapp:latest` for the latest version.
- `volumes`: The volumes used by LionkApp to store configuration, plugins, data, logs, temporary files, and keys.
- `ports`: The port used by LionkApp. You can change the port by modifying the first number in the port mapping.
- `restart`: This option restarts the container if it stops. You can remove it if you don’t want this behavior.

### 2.2. Changing the Port Settings

If you want to use a different port than `5001`, simply modify the port mapping as follows:

```yaml
ports:
  - "8080:5001"
```

## 3. Deploy LionkApp

Now you can deploy LionkApp by running the following command in the directory containing the `docker-compose.yml` file.

```bash
docker-compose up
```

To run the container in the background, use the following command:

```bash
docker-compose up -d
```

## 4. Access LionkApp

If you have correctly configured the ports, you can access LionkApp using the following URL in your browser:

```bash
http://localhost:<LionkAppPort>
```

## 5. Stop or Restart LionkApp

You can stop or restart LionkApp using the following commands:

```bash
docker-compose stop
docker-compose up
```

## 6. Update LionkApp

To update LionkApp, use the following commands:

```bash
docker-compose down
docker-compose pull
docker-compose up
```

## 7. Attach a Terminal to the Container

To attach a terminal to the container, first list the running containers:

```bash
docker container ps
```

Then, use the `CONTAINER ID` to attach a terminal to the container with the following command:

```bash
docker attach <CONTAINER ID>
```

## 8. Attach an Interactive Terminal to the Container

If you want to attach an interactive terminal to the container, use the following command:

```bash
docker exec -it <CONTAINER ID> /bin/bash
```

## 9. Access Docker Logs

You can access the Docker logs of the container using the following command:

```bash
docker logs <CONTAINER ID>
```

## 10. Launch Docker at Startup

If you want to launch the Docker container at startup, use the following command:

```bash
sudo systemctl enable docker
```

This documentation allows you to deploy LionkApp on a PC without needing to configure firewalls or GPIO, focusing solely on the Docker setup.