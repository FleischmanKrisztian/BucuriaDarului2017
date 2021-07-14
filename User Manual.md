
### USER MANUAl
In Order to make the application work:
1. you have to first run the two Wpfapps as administrator.
>This is achieved by going to the ~/bin/debug folder and running the exe file as administrator
2. Copy the mongo.exe application into the mongodump folder.
3. add Environment variables for the following:
- BucuriaDaruluiDirectory  =  (The folder where ConfigFile.xml is located)
- volmongo_databasename  =  BucuriaDaruluiLocal  (Local DB)
- volmongo_databasename2  = BucuriaDaruluiCommon   (Common DB)
- volmongo_port = 27017 (This is the port for the offline/local mongodatabase by default its 27017)
- volmongo_port2 = 32770 (The port of the MongoDB running on your NAS) 
- volmongo_server = localhost (the server name running locally is localhost)
- volmongo_server2  =  192.169.0.143 (The IP address of your NAS)
- mongo_path = The path to your mongodump folder
4.Change the Port Correspondingly in the "ConfigFile.xml", which is located in folder