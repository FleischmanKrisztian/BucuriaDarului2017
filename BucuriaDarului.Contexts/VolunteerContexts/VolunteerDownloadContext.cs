using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BucuriaDarului.Contexts.VolunteerContexts
{
    public class VolunteerDownloadContext
    {
        private readonly IVolunteerDownloadGateway dataGateway;

        public VolunteerDownloadContext(IVolunteerDownloadGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public string Execute(string dataString, string header)
        {
            var finalHeader = SplitHeader(header);
            string jsonString = string.Empty;
            var properties = GetProperties(dataString);
            var ids = GetIds(dataString);

            if (properties.Contains("0"))
                jsonString = GetVolunteersCsv(ids, finalHeader);
            else
                jsonString = GetVolunteers(properties, ids, finalHeader);

            return jsonString;
        }

        public string[] SplitHeader(string header)
        {
            var splitHeader = header.Split(",");

            return splitHeader;
        }

        public string GetProperties(string ids_)
        {
            var auxiliaryStrings = ids_.Split("(((");
            var properties = auxiliaryStrings[1];
            return properties;
        }

        public string[] GetIds(string ids_)
        {
            var auxiliaryStrings = ids_.Split("(((");
            var ids = auxiliaryStrings[0].Split(",");
            return ids;
        }

        public string[] VolunteerToArray(Volunteer volunteer)
        {
            var result = new string[19];
            result[0] = volunteer.Id;
            result[1] = volunteer.Fullname;
            result[2] = DateTime.Parse(volunteer.Birthdate.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
            result[3] = volunteer.Address;
            result[4] = volunteer.Gender.ToString();
            result[5] = volunteer.DesiredWorkplace;
            result[6] = volunteer.CNP;
            result[7] = volunteer.FieldOfActivity;
            result[8] = volunteer.Occupation;
            result[9] = volunteer.CI.HasId.ToString();
            result[10] = volunteer.CI.Info;
            result[11] = DateTime.Parse(volunteer.CI.ExpirationDate.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
            result[12] = volunteer.InActivity.ToString();
            result[13] = volunteer.HourCount.ToString();
            result[14] = volunteer.ContactInformation.PhoneNumber;
            result[15] = volunteer.ContactInformation.MailAddress;
            result[16] = volunteer.AdditionalInfo.HasDrivingLicense.ToString();
            result[17] = volunteer.AdditionalInfo.HasCar.ToString();
            result[18] = volunteer.AdditionalInfo.Remark;
            return result;
        }

        public string GetVolunteersCsv(string[] ids, string[] finalHeader)
        {
            var listOfVolunteers = dataGateway.GetListOfVolunteers();
            var finalListOfVolunteers = new List<Volunteer>();
            foreach (var volunteer in listOfVolunteers)
            {
                if (ids.Contains(volunteer.Id))
                {
                    finalListOfVolunteers.Add(volunteer);
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var data in finalHeader)
            {
                sb.Append("\"" + data + "\"" + ",");
            }
            sb.Append("\r\n");

            foreach (var item in finalListOfVolunteers)
            {
                string[] volunteerArray = VolunteerToArray(item);
                foreach (var data in volunteerArray)
                {
                    if (data == "-")
                        sb.Append("\"" + "" + "\"" + ",");
                    else
                        sb.Append("\"" + data + "\"" + ",");
                }

                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        public string[] GetUsedHeader(string properties, string[] header)
        {
            List<string> headerList = new List<string>();
            if (properties.Contains("1"))
            {
                headerList.Add(header[1]);
            }
            if (properties.Contains("2"))
            {
                headerList.Add(header[2]);
            }
            if (properties.Contains("3"))
            {
                headerList.Add(header[3]);
            }

            if (properties.Contains("4"))
            {
                headerList.Add(header[4]);
            }
            if (properties.Contains("5"))
            {
                headerList.Add(header[5]);
            }
            if (properties.Contains("6"))
            {
                headerList.Add(header[6]);
            }
            if (properties.Contains("7"))
            {
                headerList.Add(header[7]);
            }
            if (properties.Contains("8"))
            {
                headerList.Add(header[8]);
            }
            if (properties.Contains("9"))
            {
                headerList.Add(header[9]);
                headerList.Add(header[10]);
                headerList.Add(header[11]);
            }
            if (properties.Contains("A"))
            {
                headerList.Add(header[12]);
            }
            if (properties.Contains("B"))
            {
                headerList.Add(header[13]);
            }
            if (properties.Contains("C"))
            {
                headerList.Add(header[14]);
                headerList.Add(header[15]);
            }
            if (properties.Contains("D"))
            {
                headerList.Add(header[16]);
                headerList.Add(header[17]);
                headerList.Add(header[18]);
            }
            var returnedHeader = headerList.ToArray();
            return returnedHeader;
        }

        public string GetVolunteers(string properties, string[] ids, string[] header)
        {
            var usedHeader = GetUsedHeader(properties, header);
            var listOfVolunteers = dataGateway.GetListOfVolunteers();
            var idListForPrinting = new List<Volunteer>();
            foreach (var volunteer in listOfVolunteers)
            {
                if (ids.Contains(volunteer.Id))
                {
                    idListForPrinting.Add(volunteer);
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var data in usedHeader)
            {
                sb.Append("\"" + data + "\"" + ",");
            }

            sb.Append("\r\n");
            foreach (var item in idListForPrinting)
            {
                if (properties.Contains("1"))
                {
                    sb.Append("\"" + item.Fullname + "\"" + ",");
                }
                if (properties.Contains("2"))
                {
                    sb.Append("\"" + item.Birthdate.ToLongDateString() + "\"" + ",");
                }

                if (properties.Contains("3"))
                {
                    sb.Append("\"" + item.Address + "\"" + ",");
                }
                if (properties.Contains("4"))
                {
                    sb.Append("\"" + item.Gender.ToString() + "\"" + ",");
                }
                if (properties.Contains("5"))
                {
                    sb.Append("\"" + item.DesiredWorkplace + "\"" + ",");
                }
                if (properties.Contains("6"))
                {
                    sb.Append("\"" + item.CNP + "\"" + ",");
                }
                if (properties.Contains("7"))
                {
                    sb.Append("\"" + item.FieldOfActivity + "\"" + ",");
                }
                if (properties.Contains("8"))
                {
                    sb.Append("\"" + item.Occupation + "\"" + ",");
                }
                if (properties.Contains("9"))
                {
                    sb.Append("\"" + item.CI.HasId.ToString() + "\"" + ",");
                    sb.Append("\"" + item.CI.Info + "\"" + ",");
                    sb.Append("\"" + item.CI.ExpirationDate.ToLongDateString() + "\"" + ",");
                }
                if (properties.Contains("A"))
                {
                    sb.Append("\"" + item.InActivity + "\"" + ",");
                }
                if (properties.Contains("B"))
                {
                    sb.Append("\"" + item.HourCount + "\"" + ",");
                }
                if (properties.Contains("C"))
                {
                    sb.Append("\"" + item.ContactInformation.PhoneNumber + "\"" + ",");
                    sb.Append("\"" + item.ContactInformation.MailAddress + "\"" + ",");
                }
                if (properties.Contains("D"))
                {
                    sb.Append("\"" + item.AdditionalInfo.HasDrivingLicense + "\"" + ",");
                    sb.Append("\"" + item.AdditionalInfo.HasCar.ToString() + "\"" + ",");
                    sb.Append("\"" + item.AdditionalInfo.Remark + "\"" + ",");
                }

                sb.Append("\r\n");
            }
            return sb.ToString();
        }
    }
}