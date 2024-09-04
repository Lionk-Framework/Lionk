# How to deploy LionkApp on Raspberry Pi

This part explains how to deploy LionkApp on a Raspberry Pi using docker and docker-compose.

## 1. Install docker

If Docker is not installed on your Raspberry Pi, you can install it by following the instructions below.

```bash
sudo apt-get update
sudo apt-get install docker
sudo apt-get install docker-compose
```

## 2. Install ufw

It is recommended to install ufw (Uncomplicated Firewall) to define the firewall rules for your Raspberry Pi and set the rules for the ports that you want to use on LionkApp.

```bash
sudo apt-get install ufw
```

## 3. Configure ufw

Now you need to configure the ufw to allow the ports that you want to use on LionkApp. You can use the following commands to allow the ports. 
Please keep in mind that you need to allow the port that you will use for LionkApp and SSH.

```bash
sudo ufw allow 22 # 22 or the port that you use for SSH
sudo ufw allow <LionkAppPort>
sudo ufw enable
```

## 4. Create a docker-compose.yml file

Create a docker-compose.yml file in the directory that you want to deploy LionkApp.

```yaml
version: '3.8'

services:
  lionkapp:
    image: ghcr.io/lionk-framework/lionkapp:latest
    privileged: true # Required to access GPIO
    user: root # Run as root to access GPIO
    devices:
      - "/dev/gpiomem:/dev/gpiomem"
      - "/dev/mem:/dev/mem"
      - "/dev/ttyAMA0:/dev/ttyAMA0"
      - "/dev/ttyS0:/dev/ttyS0"
    volumes:
      - /sys:/sys # Give the full access to sys folder
      - ./config:/app/config
      - ./plugins:/app/plugins
      - ./data:/app/data
      - ./logs:/app/logs
      - ./temp:/app/temp
      - ./keys:/root/.aspnet/DataProtection-Keys
    ports:
      - "5001:5001"
    restart: always # can be removed if you don't want to restart the container
```

### 4.1. Explanation of the docker-compose.yml file
- `image`: The image that will be used to deploy LionkApp. You can use the latest version of the image by using `ghcr.io/lionk-framework/lionkapp:latest`.
- `privileged`: This is required to access the GPIO pins on the Raspberry Pi.
- `user: root` : This is required to access the GPIO pins on the Raspberry Pi.
- `devices`: The devices that will be used by the LionkApp to access the GPIO pins.
- `volumes`: The volumes that will be used by the LionkApp to store the configuration, plugins, data, logs, temp files, and keys. Keys are used for the authentication protection, if you don't store the keys, you will need to re-authenticate every time you restart the LionkApp by clearing the cookies. `Sys` folder is used to access the GPIO pins. The `settings` folder is used to store the configuration file.
- `ports`: The port that will be used by the LionkApp. You can change the port by changing the first port number in the port mapping.
- `restart`: This is used to restart the container if it is stopped. You can remove this line if you don't want to restart the container.

### 4.2. Changing the ports settings

#### 4.2.1. Changing the container port

You can change the port that will be used by docker to expose LionkApp by changing the first port number in the port mapping. For example, if you want to use port `8080` instead of `5001`, you can change the port mapping as below.

```yaml	
ports:
  - "8080:5001"
```
## 5. Deploy LionkApp

Now you can deploy LionkApp by using the following command inside the directory that contains the `docker-compose.yml` file.

```bash
docker-compose up
```

if you want to run the container in the background, you can use the following command.

```bash
docker-compose up -d
```

## 6. Access LionkApp

If you have configured the ports and the firewall correctly, you can access LionkApp by using the following URL in your browser.

```bash
http://<RaspberryPiIP>:<LionkAppPort>
```

## 7. Stop or restart LionkApp

You can stop or restart LionkApp by using the following commands.

```bash
docker-compose stop
docker-compose up
```

## 8. Update LionkApp

To update LionkApp, you can use the following commands.

```bash
docker-compose down # if the container is running
docker-compose pull # to pull the latest image
docker-compose up
```

## 9. Attaching a terminal to the container

If you want to attach a terminal to the container, you can use the following command.

first list the running containers.
```bash	
docker container ps
```

the output will be like below.

```bash
CONTAINER ID   IMAGE                                     COMMAND        CREATED          STATUS          PORTS                                       NAMES
f72fef8584b0   ghcr.io/lionk-framework/lionkapp:latest   "./LionkApp"   29 minutes ago   Up 21 seconds   0.0.0.0:5001->5001/tcp, :::5001->5001/tcp   pi_lionkapp_1
```

then you can use the the `CONTEINER ID` to attach a terminal to the container with the following command.

```bash
docker attach <CONTAINER ID>
```

## 10. Attach an interactive terminal to the container

If you want to attach an interactive terminal to the container, you can use the following command.

```bash
docker exec -it <CONTAINER ID> /bin/bash
```

## 11. Access the docker logs

You can access the docker logs of the container by using the following command. These logs represent the logs if docker and the application (Like `Console.WriteLine()` etc.).

```bash
docker logs <CONTAINER ID>
```

## 12. Launch docker at startup

if you want to launch the docker container at the startup, you can use the following command.

```bash
sudo systemctl enable docker
```
