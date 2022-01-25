using MDD4All.SpecIF.DataModels;
using MDD4All.SpecIF.DataProvider.File;
using System;
using System.Collections.Generic;
using System.IO;
using MDD4All.SpecIF.DataModels.Manipulation;
using MDD4All.SpecIF.DataProvider.Contracts;

namespace MDD4All.SpecIF.Generators.Vocabulary
{
    public class GithubDocumentationGenerator : SpecIfGeneratorBase
    {
        private const string CRLF = "\r\n";

        public GithubDocumentationGenerator(List<DirectoryInfo> directories) : base(directories)
        {
        }

        protected override string GenerateDomainDocumentation(string key, SpecIF.DataModels.SpecIF domainClasses)
        {
            string result = "";

            if(_specIfMetadataReader == null)
            {
                _specIfMetadataReader = new SpecIfFileMetadataReader(domainClasses);
            }

            string domainName = key.Replace("_", ": ");

            result += "## Domain " + domainName + Environment.NewLine;

            result += Environment.NewLine;

            // data types
            if (domainClasses.DataTypes != null && domainClasses.DataTypes.Count != 0)
            {

                result += "### Data types of domain " + domainName + Environment.NewLine;

                result += Environment.NewLine;

                result += "|title|id|revision|type|description|" + Environment.NewLine;

                result += "|-|-|-|-|-|" + Environment.NewLine;

                foreach (DataType dataType in domainClasses.DataTypes)
                {
                    result += "|" + dataType.Title + "|" + dataType.ID + "|" + dataType.Revision;
                    result += "|" + dataType.Type;
                    result += "|" + GetDataTypeDescription(dataType) + "|" + Environment.NewLine;
                }
            }

            // properties
            if (domainClasses.PropertyClasses != null && domainClasses.PropertyClasses.Count != 0)
            {

                result += "### Property classes of domain " + domainName + Environment.NewLine;

                result += Environment.NewLine;

                result += "|title|id|revision|dataType|description|" + Environment.NewLine;

                result += "|-|-|-|-|-|" + Environment.NewLine;

                foreach (PropertyClass propertyClass in domainClasses.PropertyClasses)
                {
                    result += "|" + propertyClass.Title + "|" + propertyClass.ID + "|" + propertyClass.Revision;
                    result += "|" + propertyClass.GetDataTypeTitle(_specIfMetadataReader);

                    result += "|" + GetPropertyClassDescription(propertyClass) + "|" + Environment.NewLine;
                }
            }

            // resources
            if (domainClasses.ResourceClasses != null && domainClasses.ResourceClasses.Count != 0)
            {
                result += "### Resource classes of domain " + domainName + Environment.NewLine;

                result += Environment.NewLine;

                result += "|title|id|revision|description|" + Environment.NewLine;

                result += "|-|-|-|-|" + Environment.NewLine;

                foreach (ResourceClass resourceClass in domainClasses.ResourceClasses)
                {
                    result += "|" + resourceClass.Title + "|" + resourceClass.ID + "|" + resourceClass.Revision;
                    result += "|" + GetResourceClassDescription(resourceClass) + "|" + Environment.NewLine;
                }
            }

            // statements
            if (domainClasses.StatementClasses != null && domainClasses.StatementClasses.Count != 0)
            {
                result += "### Statement classes of domain " + domainName + Environment.NewLine;

                result += Environment.NewLine;

                result += "|title|id|revision|description|" + Environment.NewLine;

                result += "|-|-|-|-|" + Environment.NewLine;

                foreach (StatementClass statementClass in domainClasses.StatementClasses)
                {
                    result += "|" + statementClass.Title + "|" + statementClass.ID + "|" + statementClass.Revision;
                    result += "|" + GetResourceClassDescription(statementClass) + "|" + Environment.NewLine;
                }
            }

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

        private string GetPropertyClassDescription(PropertyClass propertyClass)
        {
            string result = "";

            if(propertyClass.Description.Count > 0)
            {
                result = propertyClass.Description[0].Text;
            }

            return result;
        }

        private string GetResourceClassDescription(ResourceClass resourceClass)
        {
            string result = "";

            result += "<p>" + resourceClass.Description[0].Text + "</p>";

            if (resourceClass.PropertyClasses != null && resourceClass.PropertyClasses.Count != 0)
            {
                result += "<p>Property classes:<br/><ul>";

                foreach (Key key in resourceClass.PropertyClasses)
                {
                    PropertyClass propertyClass = _specIfMetadataReader.GetPropertyClassByKey(key);

                    result += "<li>" + key.GetPropertyClassTitle(_specIfMetadataReader) + " [" + propertyClass.ID + " " + propertyClass.Revision + "]</li>";
                }

                result += "</ul></p>";
            }



            return result;
        }

        


    }

}