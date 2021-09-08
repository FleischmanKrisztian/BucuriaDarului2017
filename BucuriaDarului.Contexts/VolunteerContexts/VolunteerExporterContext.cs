using Aspose.Cells;
using BucuriaDarului.Core;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BucuriaDarului.Contexts.VolunteerContexts
{
    public class VolunteerExporterContext
    {
        private readonly IStringLocalizer localizer;

        public VolunteerExporterContext(IStringLocalizer localizer)
        {
            this.localizer = localizer;
        }

        public VolunteerExporterResponse Execute(VolunteerExporterRequest request)
        {
            var lines = ReadMappingTemplate();
            var listOfColumns = GetListOfColumnsTemplate(lines);

            var excelToDownload = CreateExcelFile(request, listOfColumns);

            var response = new VolunteerExporterResponse();
            return response;
        }

        private Workbook CreateExcelFile(VolunteerExporterRequest request, List<KeyValuePair<string, string>> listOfColumns)
        {
            var volunteerIds = GetIds(request.ExportParameters.StringOfIDs);
            Workbook wb = new Workbook(fileFormatType: FileFormatType.Xlsx);
            Worksheet sheet = wb.Worksheets[0];
            Cell cell = sheet.Cells["A1"];

            if (request.ExportParameters.All)
            {
                cell = sheet.Cells["A1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "Ids").Value);
                cell = sheet.Cells["B1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "Name").Value);
                cell = sheet.Cells["C1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "Birthdate").Value);
                cell = sheet.Cells["D1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "Address").Value);
                cell = sheet.Cells["E1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "Gender").Value);
                cell = sheet.Cells["F1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "DesiredWorkplace").Value);
                cell = sheet.Cells["G1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "CNP").Value);
                cell = sheet.Cells["H1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "FieldOfActivity").Value);
                cell = sheet.Cells["I1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "Occupation").Value);
                cell = sheet.Cells["J1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "HasCI").Value);
                cell = sheet.Cells["K1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "CI").Value);
                cell = sheet.Cells["L1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "CIExpirationDate").Value);
                cell = sheet.Cells["M1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "WorkingHours").Value);
                cell = sheet.Cells["N1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "PhoneNumber").Value);
                cell = sheet.Cells["O1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "Mail").Value);
                cell = sheet.Cells["P1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "DriversLicense").Value);
                cell = sheet.Cells["Q1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "HasCar").Value);
                cell = sheet.Cells["R1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "Comments").Value);
                cell = sheet.Cells["S1"];
                cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "Active").Value);

                for (int i = 0; i < volunteerIds.Count; i++)
                {
                    var volunteer = Gateway.VolunteerGateways.SingleVolunteerReturnerGateway.ReturnVolunteer(volunteerIds[i]);
                    cell = sheet.Cells["A" + (i + 2)];
                    cell.PutValue(volunteer.Id);
                    cell = sheet.Cells["B" + (i + 2)];
                    cell.PutValue(volunteer.Fullname);
                    cell = sheet.Cells["C" + (i + 2)];
                    cell.PutValue(volunteer.Birthdate.ToShortDateString());
                    cell = sheet.Cells["D" + (i + 2)];
                    cell.PutValue(volunteer.Address);
                    cell = sheet.Cells["E" + (i + 2)];
                    if (volunteer.Gender == Gender.Male)
                        cell.PutValue("M");
                    else if (volunteer.Gender == Gender.Female)
                        cell.PutValue("F");
                    else
                        cell.PutValue("N");
                    cell = sheet.Cells["F" + (i + 2)];
                    cell.PutValue(volunteer.DesiredWorkplace);
                    cell = sheet.Cells["G" + (i + 2)];
                    cell.PutValue(volunteer.CNP);
                    cell = sheet.Cells["H" + (i + 2)];
                    cell.PutValue(volunteer.FieldOfActivity);
                    cell = sheet.Cells["I" + (i + 2)];
                    cell.PutValue(volunteer.Occupation);
                    cell = sheet.Cells["J" + (i + 2)];
                    cell.PutValue(volunteer.CI.HasId);
                    cell = sheet.Cells["K" + (i + 2)];
                    cell.PutValue(volunteer.CI.Info);
                    cell = sheet.Cells["L" + (i + 2)];
                    cell.PutValue(volunteer.CI.ExpirationDate.ToShortDateString());
                    cell = sheet.Cells["M" + (i + 2)];
                    cell.PutValue(volunteer.HourCount);
                    cell = sheet.Cells["N" + (i + 2)];
                    cell.PutValue(volunteer.ContactInformation.PhoneNumber);
                    cell = sheet.Cells["O" + (i + 2)];
                    cell.PutValue(volunteer.ContactInformation.MailAddress);
                    cell = sheet.Cells["P" + (i + 2)];
                    cell.PutValue(volunteer.AdditionalInfo.HasDrivingLicense);
                    cell = sheet.Cells["Q" + (i + 2)];
                    cell.PutValue(volunteer.AdditionalInfo.HasCar);
                    cell = sheet.Cells["R" + (i + 2)];
                    cell.PutValue(volunteer.AdditionalInfo.Remark);
                    cell = sheet.Cells["S" + (i + 2)];
                    cell.PutValue(volunteer.InActivity);
                }
            }
            else
            {
                for (int i = 0; i < volunteerIds.Count; i++)
                {
                    var volunteer = Gateway.VolunteerGateways.SingleVolunteerReturnerGateway.ReturnVolunteer(volunteerIds[i]);
                    if (request.ExportParameters.Fullname)
                    {
                        cell = sheet.Cells["B1"];
                        cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "Name").Value);
                        cell = sheet.Cells["B" + (i + 2)];
                        cell.PutValue(volunteer.Fullname);
                    }
                    if (request.ExportParameters.Birthdate)
                    {
                        cell = sheet.Cells["C1"];
                        cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "Birthdate").Value);
                        cell = sheet.Cells["C" + (i + 2)];
                        cell.PutValue(volunteer.Birthdate.ToShortDateString());
                    }
                    if (request.ExportParameters.Address)
                    {
                        cell = sheet.Cells["D1"];
                        cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "Address").Value);
                        cell = sheet.Cells["D" + (i + 2)];
                        cell.PutValue(volunteer.Address);
                    }
                    if (request.ExportParameters.Gender)
                    {
                        cell = sheet.Cells["E1"];
                        cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "Gender").Value);
                        cell = sheet.Cells["E" + (i + 2)];
                        if(volunteer.Gender==Gender.Male)
                            cell.PutValue("M");
                        else if (volunteer.Gender == Gender.Female)
                            cell.PutValue("F");
                        else 
                            cell.PutValue("N");
                    }
                    if (request.ExportParameters.DesiredWorkplace)
                    {
                        cell = sheet.Cells["F1"];
                        cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "DesiredWorkplace").Value);
                        cell = sheet.Cells["F" + (i + 2)];
                        cell.PutValue(volunteer.DesiredWorkplace);
                    }
                    if (request.ExportParameters.CNP)
                    {
                        cell = sheet.Cells["G1"];
                        cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "CNP").Value);
                        cell = sheet.Cells["G" + (i + 2)];
                        cell.PutValue(volunteer.CNP);
                    }
                    if (request.ExportParameters.FieldOfActivity)
                    {
                        cell = sheet.Cells["H1"];
                        cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "FieldOfActivity").Value);
                        cell = sheet.Cells["H" + (i + 2)];
                        cell.PutValue(volunteer.FieldOfActivity);
                    }
                    if (request.ExportParameters.Occupation)
                    {
                        cell = sheet.Cells["I1"];
                        cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "Occupation").Value);
                        cell = sheet.Cells["I" + (i + 2)];
                        cell.PutValue(volunteer.Occupation);
                    }
                    if (request.ExportParameters.HasId)
                    {
                        cell = sheet.Cells["J1"];
                        cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "HasCI").Value);
                        cell = sheet.Cells["J" + (i + 2)];
                        cell.PutValue(volunteer.CI.HasId);
                    }
                    if (request.ExportParameters.IdInfo)
                    {
                        cell = sheet.Cells["K1"];
                        cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "CI").Value);
                        cell = sheet.Cells["K" + (i + 2)];
                        cell.PutValue(volunteer.CI.Info);
                    }
                    if (request.ExportParameters.IdExpirationDate)
                    {
                        cell = sheet.Cells["L1"];
                        cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "CIExpirationDate").Value);
                        cell = sheet.Cells["L" + (i + 2)];
                        cell.PutValue(volunteer.CI.ExpirationDate.ToShortDateString());
                    }
                    if (request.ExportParameters.HourCount)
                    {
                        cell = sheet.Cells["M1"];
                        cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "WorkingHours").Value);
                        cell = sheet.Cells["M" + (i + 2)];
                        cell.PutValue(volunteer.HourCount);
                    }
                    if (request.ExportParameters.PhoneNumber)
                    {
                        cell = sheet.Cells["N1"];
                        cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "PhoneNumber").Value);
                        cell = sheet.Cells["N" + (i + 2)];
                        cell.PutValue(volunteer.ContactInformation.PhoneNumber);
                    }
                    if (request.ExportParameters.EmailAddress)
                    {
                        cell = sheet.Cells["O1"];
                        cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "Mail").Value);
                        cell = sheet.Cells["O" + (i + 2)];
                        cell.PutValue(volunteer.ContactInformation.MailAddress);
                    }
                    if (request.ExportParameters.DriversLicense)
                    {
                        cell = sheet.Cells["P1"];
                        cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "DriversLicense").Value);
                        cell = sheet.Cells["P" + (i + 2)];
                        cell.PutValue(volunteer.AdditionalInfo.HasDrivingLicense);
                    }
                    if (request.ExportParameters.HasCar)
                    {
                        cell = sheet.Cells["Q1"];
                        cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "HasCar").Value);
                        cell = sheet.Cells["Q" + (i + 2)];
                        cell.PutValue(volunteer.AdditionalInfo.HasCar);
                    }
                    if (request.ExportParameters.Active)
                    {
                        cell = sheet.Cells["S1"];
                        cell.PutValue(listOfColumns.FirstOrDefault(kvp => kvp.Key == "Active").Value);
                        cell = sheet.Cells["S" + (i + 2)];
                        cell.PutValue(volunteer.InActivity);
                    }
                }
            }
            SaveWorkbook(request.ExportParameters.FileName, wb);
            return wb;
        }

        private void SaveWorkbook(string fileName, Workbook wb)
        {
            if (fileName != null)
                wb.Save("C:\\Users\\z004ccfs\\Desktop\\" + fileName.TrimEnd() + ".xlsx", SaveFormat.Xlsx);
            else
                wb.Save("C:\\Users\\z004ccfs\\Desktop\\" + localizer["VolunteersReport"] + DateTime.Today.ToShortDateString().TrimEnd() + ".xlsx", SaveFormat.Xlsx);
        }

        public List<string> GetIds(string ids_)
        {
            var listOfIds = new List<string>();
            var splitString = ids_.Split(",");
            for (int i = 1; i < splitString.Count(); i++)
            {
                listOfIds.Add(splitString[i]);
            }
            return listOfIds;
        }

        public string[] ReadMappingTemplate()
        {
            var lines = new string[30];
            var path = Environment.GetEnvironmentVariable(Constants.PATH_TO_VOLUNTEER_EXCEL_MAPPING_FILE);
            int counter = 0; string line;
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                lines[counter] = line;
                counter++;
            }
            file.Close();

            return lines;
        }

        public List<KeyValuePair<string, string>> GetListOfColumnsTemplate(string[] lines)
        {
            var listOfKeyValuePairs = new List<KeyValuePair<string, string>>();
            foreach (var line in lines)
            {
                if (line != "" && line != null)
                {
                    var splitLine = line.Split("=");
                    listOfKeyValuePairs.Add(new KeyValuePair<string, string>(splitLine[0], splitLine[1]));
                }
            }

            return listOfKeyValuePairs;
        }
    }

    public class VolunteerExporterResponse
    {
        public Dictionary<string, string> Dictionary { get; set; }

        public bool IsValid { get; set; }
        public string Content { get; set; }

        //public VolunteerExporterResponse(Dictionary<string, string> dictionary)
        //{
        //    Dictionary = dictionary;
        //    IsValid = true;
        //}
    }

    public class VolunteerExporterRequest
    {
        public VolunteerExporterRequest(ExportParameters exportProperties)
        {
            ExportParameters = exportProperties;
        }

        public ExportParameters ExportParameters { get; set; }
    }

    public class ExportParameters
    {
        public string StringOfIDs { get; set; }
        public bool All { get; set; }
        public bool Fullname { get; set; }
        public bool Birthdate { get; set; }
        public bool Address { get; set; }
        public bool Gender { get; set; }
        public bool DesiredWorkplace { get; set; }
        public bool CNP { get; set; }
        public bool FieldOfActivity { get; set; }
        public bool Occupation { get; set; }
        public bool HasId { get; set; }
        public bool IdInfo { get; set; }
        public bool IdExpirationDate { get; set; }
        public bool Active { get; set; }
        public bool HourCount { get; set; }
        public bool PhoneNumber { get; set; }
        public bool EmailAddress { get; set; }
        public bool DriversLicense { get; set; }
        public bool HasCar { get; set; }
        public string FileName { get; set; }
    }
}