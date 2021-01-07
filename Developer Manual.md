## INSTRUCTIONS

This is a developer manual for the application made by Fleischman Krisztian and Gramada Corina.

### REQUIREMENTS
- Visual studio 2019 (https://visualstudio.microsoft.com)
- Mongo database (https://www.mongodb.com/try/download/community)
- Docker (https://hub.docker.com/editions/community/docker-ce-desktop-windows/)
- dotnet SDK 3.x  (https://dotnet.microsoft.com/download/dotnet-core/3.0)
- you must have virtualisation enabled, this is achieved in the BIOS when the PC is booting up, and can be verified in task manager/performance tab


### Helpers (NOT A REQUIREMENT)
 - Mongo Compass (Helps visualise/manage the database)

### Developer Manual for working in Visual Studio:
After getting the application from https://github.com/FleischmanKrisztian/Finalapplication you have to do the following steps:
1. add Environment variables for the following:
- BucuriaDaruluiDirectory  =  (The folder where FinalAplication.sln is located)
- volmongo_databasename  =  BucuriaDarului  (online databasename)
- volmongo_databasename2  = BucuriaDaruluiOffline   (offline databasename)
- volmongo_port2 = 27017 (This is the port for the offline/local mongodatabase by default its 27017)
- volmongo_server  =  mongodb+srv://Krisztian:rock44ever@siemens-application-mtrya.mongodb.net/test?retryWrites=true&w=majority   (This is the connection link, unfortunately it has to contain the username and password, will remove this from the DevManual, when application is closer to deployment or contains sensitive data)
- volmongo_server2  =  localhost (uses the local server as the offline database)

2. Make sure you have your local server up and running
- use Mongo compass to connect to "localhost:27017"
OR
- launch mongod from the location you installed it too.(if you added the path to env variables you should be able to start it from anywhere)
- open a cmd in the current location and type "mongo"


3. in the ConfigFile.xml you have to change the port for the links (this is used by the secondary Wpf applications to know where to get the data from)
    - if started in IIS the default is 44395
    - if started from Visual studio the default is 5001
    - when the application is going to be deployed through docker it will be 5000

4. after modifying either of the wpf applications you have to remove them from the registry editor:
- search for "regedit"
- in "HKEY_CLASSES_ROOT" search for "CSVExporter" or "ContractPrinter" and delete them
- Now, you have to run the applications as administrator this will make sure to create a new registry with the modifications made to the applications ( this is achieved by running the exe files as admin in BucuriaDaruluiDirectory/wpfapp/bin/debug/)

### Developer instructions for DOCKER:


1. If you have made changes to the code and would like to see how it's going to run on docker have to build a docker image, this is done by going to the folder that contains Dockerfile in CMD and running the following command:
- **//   docker build -t bucuriadarului:latest .   //**  (dot included)
- in case there are SDK or runtime errors (The current Docker Version uses .net Core 3.1)
2. start the mongo database through docker with the following command:
- **//   docker run -d -p 2717:27017 -v :/data/db --name mymongo mongo:latest   //**
- If for some reason there is an error with the volume, caused by using "docker for windows", use the following command:
- **//   docker run -d -p 2717:27017 --name mymongo mongo:latest   //**
3. (Not essential) In order to check whats in the mongo database you can enter:
- **//   docker exec -it mymongo bash   //**
- **//   mongo   //**
- after this its mongocode that you can find online.
4. get the Ip address of the docker mongo database with the following command:
**//   docker inspect mymongo   //**
5. In next step you have to run the following command:
- **//   docker run -it --rm -p 8080:80 --link mymongo:bucuriadarului -e volmongo_databasename=BucuriaDarului -e volmongo_databasename2=BucuriaDaruluiOffline -e volmongo_port2=27017 -e volmongo_server="mongodb+srv://<username>:<password>@siemens-application-mtrya.mongodb.net/test?retryWrites=true&w=majority" -e volmongo_server2=<IP address of the mongo database> -d --name BucuriaDarului bucuriadarului   //**
6. get the IP address of the docker container we have just created with:
**//   docker inspect bucuriadarului   //**
7. We can go to the website at:
http://<Ipofcontainer>:8080
8. If your app is ready for deployment you should push it to dockerhub.(First you must create a dockerhub account and a repository)
- This is achieved by:
- checking images (**//   docker images   //**)
- tagging your image (**//   docker tag <IDOFIMAGE> yourhubusername/yourrepository:latest   //**) 
- Push the image (docker push yourhubusername/yourrepository   //**)
9. To run the dockerhub image you have to use the following command:
- **//   docker run -it --rm -p 8080:80 --link mymongo:krikysk8/bucuriadarului -e volmongo_databasename=BucuriaDarului -e volmongo_databasename2=BucuriaDaruluiOffline -e volmongo_port2=27017 -e volmongo_server="mongodb+srv://<username>:<password>@siemens-application-mtrya.mongodb.net/test?retryWrites=true&w=majority" -e volmongo_server2=172.17.0.2 -d --name BucuriaDarului krikysk8/bucuriadarului   //**
>  NOTE1:The above command takes the repository from docker hub, NOT the image that is currently on the local computer
>  NOTE:you have to use your own dockerhubname and repositoryname if you changed it.
10. Useful docker commands:
⋅⋅* kill <id> or <name> (stops the running container)
⋅⋅* docker ps -a (to check all containers, not just the running ones)
⋅⋅* docker images -a(to check all images)
⋅⋅* docker rm <id> (removes the stopped container)
⋅⋅* docker system prune -a (to Clear everything in docker that is not running, basically starting from 0)
⋅⋅* docker run --rm ... (--rm deletes the container if it gets stoped)
⋅⋅* docker run --restart always ... (Useful when deploying the application, makes the container start whenever docker is running, this is so that the user doesnt have to start it each time he starts the computer) 


### USER MANUAl
In Order to make the application work:
1. you have to first run the two Wpfapps as administrator, this is done so the custom url protocol handler gets made in the registry.
>This is achieved by going to the ~/bin/debug folder and running the exe file as administrator
2. We have to download the Docker application (https://docs.docker.com/docker-for-windows/install/) and install it as its shown in the tutorial(look online)!
3. After the instalation is complete we have to open a CMD terminal and run a mongo container, this creates a local mongo database on the machine that is running the docker. 
>This is achieved with the following command in the CMD:
- **//   docker run -d -p 2717:27017 -v :/data/db --name mymongo mongo:latest   //**
- If for some reason there is an error, use the following command:
- **//   docker run -d -p 2717:27017 --name mymongo mongo:latest   //**
4. After the server is up and running we have to run the main application this is made with the following command:
- **//   docker run -it --rm -p 8080:80 --link mymongo:krikysk8/bucuriadarului -e volmongo_databasename=BucuriaDarului -e volmongo_databasename2=BucuriaDaruluiOffline -e volmongo_port2=27017 -e volmongo_server="mongodb+srv://<username>:<password>@siemens-application-mtrya.mongodb.net/test?retryWrites=true&w=majority" -e volmongo_server2=172.17.0.2 -d --name BucuriaDarului krikysk8/bucuriadarului   //**
NOTE:
>You might have to change the IP address of volmongo_server2, to check whether this needs to be done you can check its ip with:
- **//   docker inspect mymongo   //**  (you should find the IP address in the bottom of the screen)
5. The application should be up and running at http://localhost:8080
NOTE:
> HTTP not HTTPS!!!

docker run -it --rm -p 8080:80 --link mymongo:bucuriadarului -e volmongo_databasename=BucuriaDarului -e volmongo_databasename2=BucuriaDaruluiOffline -e volmongo_port2=27017 -e volmongo_server="mongodb+srv://Krisztian:rock44ever@siemens-application-mtrya.mongodb.net/test?retryWrites=true&w=majority" -e volmongo_server2=172.27.106.111 -d --name BucuriaDarului bucuriadarului

