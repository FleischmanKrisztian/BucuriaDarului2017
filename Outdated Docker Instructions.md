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
- Push the image (**//   docker push yourhubusername/yourrepository   //**)
9. To run the dockerhub image you have to use the following command:
- **//   docker run -it --rm -p 8080:80 --link mymongo:krikysk8/bucuriadarului -e volmongo_databasename=BucuriaDarului -e volmongo_databasename2=BucuriaDaruluiOffline -e volmongo_port2=27017 -e volmongo_server="mongodb+srv://<username>:<password>@siemens-application-mtrya.mongodb.net/test?retryWrites=true&w=majority" -e volmongo_server2=172.17.0.2 -d --name BucuriaDarului krikysk8/bucuriadarului   //**
>  NOTE1:The above command takes the repository from docker hub, NOT the image that is currently on the local computer
>  NOTE:you have to use your own dockerhubname and repositoryname if you changed it.
10. Useful docker commands:
��* kill <id> or <name> (stops the running container)
��* docker ps -a (to check all containers, not just the running ones)
��* docker images -a(to check all images)
��* docker rm <id> (removes the stopped container)
��* docker system prune -a (to Clear everything in docker that is not running, basically starting from 0)
��* docker run --rm ... (--rm deletes the container if it gets stoped)
��* docker run --restart always ... (Useful when deploying the application, makes the container start whenever docker is running, this is so that the user doesnt have to start it each time he starts the computer) 