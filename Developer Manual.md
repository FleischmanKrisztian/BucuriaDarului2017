## INSTRUCTIONS

This is a developer manual for the application made by Fleischman Krisztian and Gramada Corina.

### REQUIREMENTS
- Visual studio 2019 (https://visualstudio.microsoft.com)
- Mongo database (https://www.mongodb.com/try/download/community)
- dotnet SDK 4.7  (https://dotnet.microsoft.com/download/dotnet-core/3.0)

### Helpers (NOT A REQUIREMENT)
 - Mongo Compass (Helps visualise/manage the database)

### Developer Manual for working in Visual Studio:
After getting the application from https://github.com/FleischmanKrisztian/Finalapplication you have to do the following steps:
1. add Environment variables for the following:
- BucuriaDaruluiDirectory  =  (The folder where FinalAplication.sln is located)
- volmongo_databasename  =  BucuriaDaruluiLocal  (Local DB)
- volmongo_databasename2  = BucuriaDaruluiCommon   (Common DB)
- volmongo_port = 27017 (This is the port for the offline/local mongodatabase by default its 27017)
- volmongo_port2 = 32770 (The port of the MongoDB running on your NAS) 
- volmongo_server = localhost (the server name running locally is localhost)
- volmongo_server2  =  192.169.0.143 (The IP address of your NAS)

2. Make sure you have your servers up and running
- use Mongo compass to connect to the two DB's

3. in the ConfigFile.xml you have to change the port for the links (this is used by the secondary Wpf applications to know where to get the data from)
    - if started in IIS the default is 44395
    - if started from Visual studio the default is 5001

4. after modifying either of the wpf applications you have to remove them from the registry editor:
- search for "regedit"
- in "HKEY_CLASSES_ROOT" search for "CSVExporter" or "ContractPrinter" and delete them
- Now, you have to run the applications as administrator this will make sure to create a new registry with the modifications made to the applications ( this is achieved by running the exe files as admin in BucuriaDaruluiDirectory/wpfapp/bin/debug/)
