# Bucuria Darului

***

### Initial requirements:
  - [x] an application for managing a volunteer database ;
  - [x] an application for managing a beenficiaries database;
  - [x] an application for managing a sponsors database ;
  - [x] an application for managing a events database ;


### Volunteer/Events/Beneficiaries/Sponsors app
 Volunteers-fields:
 
  - [x] Name; 
  - [x] Age;
  - [x] Gender;
  - [x] Address;
  - [x] Contact informations;
  - [x] Additional informations(ex:has a car,has driving license);
  - [x] Occupation;
  - [x] Desired field;
  - [x] availability;
  - [x] if is still active;
  - [x] Contract information;
  - [x] Birthdate;
  - [x] Birthday alert;
  - [x] Expiration contract alert;
  - [x] Hours count;
  - [x] Remarks;
  - [x] The ability to print reports.
 
Events-fields:
  - [x] name of event;
  - [x] date of event;
  - [x] Type of event;
  - [x] Number of volunteer needed;
  - [x] Allocated sponsors and volunteers.
  - [x] The ability to print reports.

Sponsors-fields:
 - [x] Date of sponsorship;
 - [x] Type of sponsorship(ex: money,goods);
 - [x] Contract information;
 - [x] Expiration contract alert;
 - [x] Contact information;
 - [x] The ability to print reports.
  
Beneficiaries-fields:
 - [x] General information;
 - [x] Status(active/inactive;
 - [x] Weekly package;
 - [x] Homedelivery driver;
 - [x] GDPR agreement;
 - [x] Address;
 - [x] ID card;
 - [x] Request of joining in information;
 - [x] Number of portion;
 - [x] Last time active;
 - [x] Contact informations;
 - [x] Studies,Profesion,Occupation,Seniority in workfield;
 - [x] HealthSate, disability, addictions, chronic illness;
 - [x] Health insurance and health card;
 - [x] Marital status;
 - [x] Housing information;
 - [x] The ability to print reports.


### Application architecture and functionality:
- Program(main);
- classes for volunteer,sponsors,beneficiaries,events;
- Controllers for each type of objects with the following facilities: create,edit,delete, allocate sponsors or volunteers;
- Other facilities: import and export csv files, print contracts for beneficiaries and volunteers,change language and other settings;
 - Birthday alert;
 - Expiration contract alert;




