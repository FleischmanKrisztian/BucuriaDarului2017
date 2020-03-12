This is a developer manual for the application made by Fleischman Krisztian and Gramada Corina.

### In order to make the application work you have to add Environment variables:
- databasename   ex: Bucuria Darului
- mongoserver    ex: mongodb+srv://user:password@siemens-application-mtrya.mongodb.net/test?retryWrites=true&w=majority

### USER MANUAl
In Order to make the application work:
1. you have to first run the two Wpfapps as administrator, this is done so the custom url protocol handler gets made in the registry.
>This is achieved by going to the ~/bin/debug folder and running the exe file as administrator
2. We have to download the Docker application (https://docs.docker.com/docker-for-windows/install/) and install it as its shown in the tutorial(look online)!
3. After the instalation is complete we have to open a CMD terminal and run a mongo container, this creates a local mongo database on the machine that is running the docker. 
>This is achieved with the following command in the CMD: docker run -d -p 2717:27017 -v :/data/db --name mymongo mongo:latest
4. After the server is up and running we have to run the main application this is made with the following command: docker run -it --rm -p 5000:80 --link mymongo:krikysk8/bucuriadarului -e volmongo_databasename=BucuriaDarului -e volmongo_databasename2=BucuriaDaruluiOffline -e volmongo_port2=27017 -e volmongo_server="mongodb+srv://Krisztian:rock44ever@siemens-application-mtrya.mongodb.net/test?retryWrites=true&w=majority" -e volmongo_server2=172.17.0.2 -d --name BucuriaDarului krikysk8/bucuriadarului
NOTE:
>You might have to change the IP address of volmongo_server2, to check whether this needs to be done you can check its ip with: docker inspect mymongo                 you should find the IPaddress in the bottom of the screen!
5. The application should be up and running at http://localhost:5000
NOTE:
> HTTP not HTTPS!!!

***

### Developer instructions:
1. If you have made changes to the code and would like to see how it's going to run on docker have to build a docker image, this is done by going to the folder that contains Dockerfile in CMD and running the following command: docker build -t bucuriadarului:latest .  (dot included),after the image is built, you have to run the following command: docker run -it --rm -p 5000:80 --link mymongo:bucuriadarului -e volmongo_databasename=BucuriaDarului -e volmongo_databasename2=BucuriaDaruluiOffline -e volmongo_port2=27017 -e volmongo_server="mongodb+srv://Krisztian:rock44ever@siemens-application-mtrya.mongodb.net/test?retryWrites=true&w=majority" -e volmongo_server2=172.17.0.2 -d --name BucuriaDarului bucuriadarului
2. In order to check whats in the mongo database you can enter: docker exec -it mymongo bash   followed by mongo    after this its mongocode that you can find online.
3. If your app is ready for deployment you should push it to dockerhub.(First you must create a dockerhub account and a repository)After this is done you should push it to dockerhub This is achieved by:1.checking images (docker images)   2. tagging your image (docker tag <IDOFIMAGE> yourhubusername/yourrepository:latest) 3.Push the image (docker push yourhubusername/yourrepository)
4. To run the dockerhub image you have to use the following command docker run -it --rm -p 5000:80 --link mymongo:krikysk8/bucuriadarului -e volmongo_databasename=BucuriaDarului -e volmongo_databasename2=BucuriaDaruluiOffline -e volmongo_port2=27017 -e volmongo_server="mongodb+srv://Krisztian:rock44ever@siemens-application-mtrya.mongodb.net/test?retryWrites=true&w=majority" -e volmongo_server2=172.17.0.2 -d --name BucuriaDarului krikysk8/bucuriadarului
>  NOTE1:The above command takes the repository from docker hub, NOT the image that is currently on the local computer
>  NOTE:you have to use your own dockerhubname and repositoryname if you changed it.
5. Usefull docker commands:
⋅⋅* kill <id> or <name> (stops the running container)
⋅⋅* docker ps -a (to check all containers, not just the running ones)
⋅⋅* docker images -a(to check all images)
⋅⋅* docker rm <id> (removes the stopped container)
⋅⋅* docker system prune -a (to Clear everything in docker that is not running, basically starting from 0)
⋅⋅* docker run --rm ... (--rm deletes the container if it gets stoped)
⋅⋅* docker run --restart always ... (Useful when deploying the application, makes the container start whenever docker is running, this is so that the user doesnt have to start it each time he starts the computer) 
