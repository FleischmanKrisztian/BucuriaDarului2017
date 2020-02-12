﻿using Finalaplication.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using VolCommon;

namespace Finalaplication
{
    public delegate string DCallback(string duplicates);

    public delegate int Importedcallback(int documentsimported);
    public class ProcessedDataVolunteer
    {
        private IMongoCollection<Volunteer> vollunteercollection;
        private List<string[]> result;
        private string duplicates;
        private int documentsimported;
        private DCallback callback1;
        private Importedcallback callback2;

        public ProcessedDataVolunteer(IMongoCollection<Volunteer> vollunteercollection,
       List<string[]> result,
       string duplicates,
       int documentsimported,
       DCallback _callback1,
       Importedcallback _callback2
       )
        {
            this.vollunteercollection = vollunteercollection;
            this.result = result;
            this.duplicates = duplicates;
            this.documentsimported = documentsimported;
            this.callback1 = _callback1;
            this.callback2 = _callback2;
        }

        public void GetProcessedV(IMongoCollection<Volunteer> volunteercollection,List<string[]> result,string duplicates, int documentsimported)
        {
            foreach (var details in result)
            {
                try
                {
                    if (vollunteercollection.CountDocuments(z => z.CNP == details[9]) >= 1 && details[9] != "")
                    {
                        duplicates = duplicates + details[0] + " " + details[1] + ", ";
                    }
                    else if (vollunteercollection.CountDocuments(z => z.Firstname == details[0]) >= 1 && details[9] == "" && vollunteercollection.CountDocuments(z => z.Lastname == details[1]) >= 1)
                    {
                        duplicates = duplicates + details[0] + " " + details[1] + ", ";
                    }
                    else
                    {
                        documentsimported++;
                        //Mistake;we added a new aux in order to save the value from the last cell;
                        var aux = details[22];
                        for (int i = details.Length - 1; i > 0; i--)
                        {
                            details[i] = details[i - 1];
                        }
                        Volunteer volunteer = new Volunteer();
                        try
                        {
                            volunteer.Firstname = details[1];
                            volunteer.Lastname = details[2];
                        }
                        catch
                        {
                            volunteer.Firstname = "Error";
                            volunteer.Lastname = "Error";
                        }

                        try
                        {
                            if (details[3] != null || details[3] != "")
                            {
                                string[] date;
                                date = details[3].Split(" ");

                                string[] FinalDate = date[0].Split("/");
                                DateTime data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);

                                volunteer.Birthdate = data.AddDays(1);
                            }
                            else
                            {
                                volunteer.Birthdate = DateTime.MinValue;
                            }
                        }
                        catch
                        {
                            volunteer.Birthdate = DateTime.MinValue;
                        }

                        Address a = new Address();

                        if (details[4] != null || details[4] != "")
                        {
                            a.District = details[4];
                        }
                        else { a.District = "-"; }

                        if (details[5] != null || details[5] != "")
                        {
                            a.City = details[5];
                        }
                        else
                        {
                            a.City = "-";
                        }

                        if (details[6] != null || details[6] != "")
                        {
                            a.Street = details[6];
                        }
                        else { a.Street = "-"; }

                        if (details[7] != null || details[7] != "")
                        {
                            a.Number = details[7];
                        }
                        else
                        {
                            a.Number = "-";
                        }

                        try
                        {
                            if (details[8] == "True" || details[8] == "1")
                            {
                                volunteer.Gender = VolCommon.Gender.Female;
                            }
                            else
                            {
                                volunteer.Gender = VolCommon.Gender.Male;
                            }
                        }
                        catch
                        {
                            volunteer.Gender = Gender.Male;
                        }

                        if (details[9] != null || details[9] != "")
                        {
                            volunteer.Desired_workplace = details[9];
                        }
                        else { volunteer.Desired_workplace = "-"; }

                        if (details[10] != null || details[10] != "")
                        {
                            volunteer.CNP = details[10];
                        }
                        else { volunteer.CNP = "-"; }

                        if (details[11] != null || details[11] != "")
                        {
                            volunteer.Field_of_activity = details[11];
                        }
                        else { volunteer.Field_of_activity = "-"; }
                        if (details[12] != null || details[12] != "")
                        {
                            volunteer.Occupation = details[12];
                        }
                        else { volunteer.Occupation = "-"; }

                        if (details[13] != null || details[13] != "")
                        {
                            volunteer.CIseria = details[13];
                        }
                        else
                        {
                            volunteer.CIseria = "-";
                        }

                        if (details[14] != null || details[14] != "")
                        {
                            volunteer.CINr = details[13];
                        }
                        else
                        {
                            volunteer.CINr = "-";
                        }

                        try
                        {
                            if (details[15] != null || details[15] != "")
                            {
                                string[] date;
                                date = details[15].Split(" ");

                                string[] FinalDate = date[0].Split("/");
                                DateTime data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                                volunteer.CIEliberat = data.AddDays(1);
                            }
                            else
                            {
                                volunteer.CIEliberat = DateTime.MinValue;
                            }
                        }
                        catch
                        {
                            volunteer.CIEliberat = DateTime.MinValue;
                        }

                        if (details[16] != null || details[16] != "")
                        {
                            volunteer.CIeliberator = details[16];
                        }
                        else
                        {
                            volunteer.CIeliberator = "-";
                        }
                        if (details[17] == "True")
                        {
                            volunteer.InActivity = true;
                        }
                        else
                        {
                            volunteer.InActivity = false;
                        }

                        if (details[18] != null || details[18] != "0" || details[18] != "")
                        {
                            volunteer.HourCount = Convert.ToInt16(details[18]);
                        }
                        else
                        {
                            volunteer.HourCount = 0;
                        }
                        ContactInformation c = new ContactInformation();
                        if (details[19] != null || details[19] != "")
                        {
                            c.PhoneNumber = details[19];
                        }
                        else
                        {
                            c.PhoneNumber = "-";
                        }
                        if (details[20] != null || details[20] != "")
                        {
                            c.MailAdress = details[20];
                        }
                        else
                        {
                            c.MailAdress = "-";
                        }
                        volunteer.ContactInformation = c;
                        Additionalinfo ai = new Additionalinfo();

                        if (details[21] == "True")
                        {
                            ai.HasDrivingLicence = true;
                        }
                        else
                        {
                            ai.HasDrivingLicence = false;
                        }

                        if (details[22] == "True")
                        {
                            ai.HasCar = true;
                        }
                        else
                        {
                            ai.HasCar = false;
                        }

                        ai.Remark = aux;

                        volunteer.Address = a;
                        volunteer.Additionalinfo = ai;
                        vollunteercollection.InsertOne(volunteer);
                    }
                }
                catch
                {
                    if (vollunteercollection.CountDocuments(z => z.CNP == details[1]) >= 1 && details[1] != "")
                    {
                        duplicates = duplicates + details[1] + ", ";
                    }
                    else if (vollunteercollection.CountDocuments(z => z.Firstname == details[0]) >= 1 && details[1] == "")
                    {
                        duplicates = duplicates + details[0] + ", ";
                    }
                    else
                    {
                        documentsimported++;

                        Volunteer volunteer = new Volunteer();
                        try
                        {
                            if (details[0].Contains(" ") == true)

                            {
                                string[] splitName = details[0].Split();
                                volunteer.Lastname = splitName[0];
                                if (splitName.Count() == 2)
                                {
                                    volunteer.Firstname = splitName[1];
                                }
                                else
                                {
                                    volunteer.Firstname = splitName[1] + " " + splitName[2];
                                }
                            }
                        }
                        catch
                        {
                            volunteer.Firstname = "Error";
                            volunteer.Lastname = "Error";
                        }

                        volunteer.Birthdate = DateTime.MinValue;

                        Address a = new Address();

                        if (details[2] != null || details[2] != "")
                        {
                            a.District = details[2];
                        }
                        else
                        {
                            a.District = "-";
                        }
                        a.City = "-";
                        a.Street = "-";
                        a.Number = "-";

                        volunteer.Gender = Gender.Male;

                        if (details[9] != null || details[9] != "")
                        {
                            volunteer.Desired_workplace = details[9];
                        }
                        else
                        {
                            volunteer.Desired_workplace = "-";
                        }
                        if (details[1] != null || details[1] != "")
                        {
                            volunteer.CNP = details[1];
                        }
                        else
                        {
                            volunteer.CNP = "-";
                        }

                        if (details[3] != null)
                        {
                            if (details[3] != "")
                            {
                                string[] splited = details[3].Split(" ");
                                volunteer.CIseria = splited[0];
                                volunteer.CINr = splited[1];
                            }
                            else
                            {
                                volunteer.CIseria = "Error";
                                volunteer.CINr = "Error";
                            }
                        }

                        volunteer.CIEliberat = DateTime.MinValue;

                        volunteer.HourCount = 0;

                        ContactInformation c = new ContactInformation();
                        if (details[4] != null || details[4] != "")
                        {
                            c.PhoneNumber = details[4];
                        }
                        else
                        {
                            c.PhoneNumber = "-";
                        }

                        volunteer.ContactInformation = c;
                        Additionalinfo ai = new Additionalinfo();

                        ai.HasDrivingLicence = false;
                        if (details[8] != null)
                        {
                            ai.Remark = details[8];
                        }
                        else { ai.Remark = "-"; }
                        ai.HasCar = false;

                        volunteer.Occupation = "-";

                        volunteer.Address = a;
                        volunteer.Additionalinfo = ai;
                        vollunteercollection.InsertOne(volunteer);
                    }
                }
            }
            callback1?.Invoke(duplicates);
            callback2?.Invoke(documentsimported);
        }
    }
}
    