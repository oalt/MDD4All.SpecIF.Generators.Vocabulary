using MDD4All.SpecIF.DataModels;
using MDD4All.SpecIF.DataModels.Manipulation;
using MDD4All.SpecIF.DataProvider.Contracts;
using MDD4All.SpecIF.DataProvider.File;
using System;
using System.Collections.Generic;
using System.IO;

namespace MDD4All.SpecIF.Generators.Vocabulary
{
    public class HtmlDocumentationGenerator : SpecIfGeneratorBase
    {
        public HtmlDocumentationGenerator(List<DirectoryInfo> directories) : base(directories)
        {
        }

        protected override string GenerateDomainDocumentation(string key, SpecIF.DataModels.SpecIF domainClasses)
        {
            string result = "<html>" + Environment.NewLine;

            result += @"<head>
<style>
h1 {
  font-family: arial, sans-serif;
}

h2 {
  font-family: arial, sans-serif;
}

h3 {
  font-family: arial, sans-serif;
  color: #008C7D;
}

table {
  font-family: arial, sans-serif;
  border-collapse: collapse;
  width: 100%;
}

th {
  border: 1px solid #dddddd;
  text-align: left;
  padding: 8px;
}

td {
  border: 1px solid #dddddd;
  text-align: left;
  padding: 8px;
}

tr:nth-child(even) {
  background-color: #dddddd;
}
</style>
</head>";

            result += "<body>" + Environment.NewLine;

            if (_specIfMetadataReader == null)
            {
                _specIfMetadataReader = new SpecIfFileMetadataReader(domainClasses);
            }

            string domainName = key.Replace("_", ": ");

            result += "<h1>Domain: " + domainName + Environment.NewLine + " (draft)</h1>";

            result += Environment.NewLine;



            // properties
            if (domainClasses.PropertyClasses != null && domainClasses.PropertyClasses.Count != 0)
            {

                Dictionary<string, List<PropertyClass>> sortedPropertyClasses = SortPropertyClassesBySubdomain(domainClasses.PropertyClasses);

                result += "<h2>Property classes of domain " + domainName + "</h2>" + Environment.NewLine;

                foreach (KeyValuePair<string, List<PropertyClass>> keyValuePair in sortedPropertyClasses)
                {
                    result += "<h3>Subdomain <i>" + keyValuePair.Key + "</i></h3>" + Environment.NewLine;

                    result += "<table border=\"1\">";

                    result += "<tr>" + Environment.NewLine;

                    result += "<th>Title</th>" + Environment.NewLine;
                    result += "<th>Description</th>";
                    result += "<th>Unit</th>";
                    result += "<th>Multiplicity</th>";
                    result += "<th>Data Type</th>" + Environment.NewLine;

                    result += "</tr>" + Environment.NewLine;

                    foreach (PropertyClass propertyClass in keyValuePair.Value)
                    {
                        string subdomain = "";
                        string title = "";

                        ParsePropertyClassTitle(propertyClass.Title, out subdomain, out title);

                        result += "<tr>" + Environment.NewLine;

                        result += "<td>" + title + "</td>";
                        result += "<td>" + propertyClass.Description[0].Text + "</td>" + Environment.NewLine;
                        result += "<td>" + propertyClass.Unit + "</td>";

                        string multiple = "1";

                        if (propertyClass.Multiple != null && propertyClass.Multiple == true)
                        {
                            multiple = "1..n";
                        }

                        result += "<td>" + multiple + "</td>" + Environment.NewLine;

                        //result += "<td>" + propertyClass.ID + "</td>";
                        //result += "<td>" + propertyClass.Revision + "</td>";
                        result += "<td>";

                        result += "<a href=\"#" + propertyClass.DataType.ID + "_R_" + propertyClass.DataType.Revision + "\">";

                        result += propertyClass.GetDataTypeTitle(_specIfMetadataReader) + "</a></td>";


                        result += "</tr>" + Environment.NewLine;
                    }

                    result += "</table>" + Environment.NewLine;

                }
            }

            // data types
            if (domainClasses.DataTypes != null && domainClasses.DataTypes.Count != 0)
            {
                result += "<h2>Data types of domain " + domainName + "</h2>" + Environment.NewLine;

                result += "<table border=\"1\">";

                result += "<tr>" + Environment.NewLine;

                result += "<th>Title</th><th>type</th><th>description</th><th>id</th><th>revision</th>" + Environment.NewLine;

                result += "</tr>" + Environment.NewLine;

                foreach (DataType dataType in domainClasses.DataTypes)
                {
                    result += "<tr id=\"" + dataType.ID + "_R_" + dataType.Revision +  "\">" + Environment.NewLine;

                    result += "<td>" + dataType.Title + "</td>";
                    result += "<td>" + dataType.Type + "</td>";
                    result += "<td>" + GetDataTypeDescription(dataType) + "</td>" + Environment.NewLine;
                    result += "<td>" + dataType.ID + "</td>";
                    result += "<td>" + dataType.Revision + "</td>";

                    result += "</tr>" + Environment.NewLine;
                }

                result += "</table>" + Environment.NewLine;
            }

            //// resources
            //if (domainClasses.ResourceClasses != null && domainClasses.ResourceClasses.Count != 0)
            //{
            //    result += "### Resource classes of domain " + domainName + Environment.NewLine;

            //    result += Environment.NewLine;

            //    result += "|title|id|revision|description|" + Environment.NewLine;

            //    result += "|-|-|-|-|" + Environment.NewLine;

            //    foreach (ResourceClass resourceClass in domainClasses.ResourceClasses)
            //    {
            //        result += "|" + resourceClass.Title + "|" + resourceClass.ID + "|" + resourceClass.Revision;
            //        result += "|" + GetResourceClassDescription(resourceClass) + "|" + Environment.NewLine;
            //    }
            //}

            //// statements
            //if (domainClasses.StatementClasses != null && domainClasses.StatementClasses.Count != 0)
            //{
            //    result += "### Statement classes of domain " + domainName + Environment.NewLine;

            //    result += Environment.NewLine;

            //    result += "|title|id|revision|description|" + Environment.NewLine;

            //    result += "|-|-|-|-|" + Environment.NewLine;

            //    foreach (StatementClass statementClass in domainClasses.StatementClasses)
            //    {
            //        result += "|" + statementClass.Title + "|" + statementClass.ID + "|" + statementClass.Revision;
            //        result += "|" + GetResourceClassDescription(statementClass) + "|" + Environment.NewLine;
            //    }
            //}

            result += "</body>" + Environment.NewLine;

            result += "<html>";

            return result;
        }

        private string GetDataTypeDescription(DataType dataType)
        {
            string result = "";

            if (dataType.Enumeration != null && dataType.Enumeration.Count > 0)
            {
                if (dataType.Description != null)
                {
                    result = "<p>" + dataType.Description[0].Text + "</p>";
                }

                result += "<ul>";
                foreach (EnumerationValue value in dataType.Enumeration)
                {
                    result += "<li>" + value.Value[0].Text + " [" + value.ID + "]</li>";
                }
                result += "</ul>";

            }
            else
            {
                if (dataType.Description != null)
                {
                    if (dataType.Description.ToString() == "[]")
                    {
                        result = "";
                    }
                    else
                    {
                        result = dataType.Description[0].Text;
                    }
                }
            }

            return result;
        }

        private Dictionary<string, List<PropertyClass>> SortPropertyClassesBySubdomain(List<PropertyClass> propertyClasses)
        {
            Dictionary<string, List<PropertyClass>> result = new Dictionary<string, List<PropertyClass>>();

            foreach(PropertyClass propertyClass in propertyClasses)
            {
                string subdomain = "";
                string title = "";

                ParsePropertyClassTitle(propertyClass.Title, out subdomain, out title);

                if(result.ContainsKey(subdomain))
                {
                    result[subdomain].Add(propertyClass);
                }
                else
                {
                    List<PropertyClass> entires = new List<PropertyClass>();
                    entires.Add(propertyClass);
                    result.Add(subdomain, entires);
                }
            }

            return result;
        }

        private void ParsePropertyClassTitle(string titleIn, out string subdomain, out string titleOut)
        {
            char[] seperator = { ':' };

            string[] tokens = titleIn.Split(seperator);

            subdomain = tokens[1];
            titleOut = tokens[2];
        }

    }
}
