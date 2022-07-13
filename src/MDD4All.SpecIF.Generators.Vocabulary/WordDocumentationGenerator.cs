using MDD4All.SpecIF.DataModels;
using MDD4All.SpecIF.DataProvider.File;
using System;
using System.Collections.Generic;
using System.IO;
using MDD4All.SpecIF.DataModels.Manipulation;
using MDD4All.SpecIF.DataProvider.Contracts;
using MDD4All.SpecIF.Generators.Vocabulary.DataModels;

namespace MDD4All.SpecIF.Generators.Vocabulary
{
    public class WordDocumentationGenerator : SpecIfGeneratorBase
    {
        private const string CRLF = "\r\n";

        public WordDocumentationGenerator(List<DirectoryInfo> directories) : base(directories)
        {
        }

        protected override string GenerateDomainDocumentation(string key, SpecIF.DataModels.SpecIF domainClasses)
        {
            string result = "";

            string domainName = key.Replace("_", ": ");

            result += "## Domain " + domainName + Environment.NewLine;

            result += Environment.NewLine;

            // data types
            if (domainClasses.DataTypes != null && domainClasses.DataTypes.Count != 0)
            {

                result += "### Data types of domain " + domainName + Environment.NewLine;

                Table table = new Table();

                List<TableCell> headerRow = new List<TableCell>();

                TableCell titleTableCell = new TableCell();
                titleTableCell.Content.Add("title");

                headerRow.Add(titleTableCell);


                TableCell idTableCell = new TableCell();
                idTableCell.Content.Add("id");

                headerRow.Add(idTableCell);

                TableCell revisionTableCell = new TableCell();
                revisionTableCell.Content.Add("revision");

                headerRow.Add(revisionTableCell);

                TableCell typeTableCell = new TableCell();
                typeTableCell.Content.Add("type");

                headerRow.Add(typeTableCell);

                TableCell descriptionTableCell = new TableCell();
                descriptionTableCell.Content.Add("description");

                headerRow.Add(descriptionTableCell);

                table.TableCells.Add(headerRow);

                foreach (DataType dataType in domainClasses.DataTypes)
                {
                    List<TableCell> contentRow = new List<TableCell>()
                    {
                        new TableCell(dataType.Title),
                        new TableCell(dataType.ID),
                        new TableCell(dataType.Revision),
                        new TableCell(dataType.Type)
                    };

                    contentRow.Add(GetDataTypeDescription(dataType));

                    table.TableCells.Add(contentRow);
                }

                result += table.GenerateGridTable();
            }

            // properties
            if (domainClasses.PropertyClasses != null && domainClasses.PropertyClasses.Count != 0)
            {
                result += "### Property classes of domain " + domainName + Environment.NewLine;

                Table table = new Table();

                List<TableCell> headerRow = new List<TableCell>();

                TableCell titleTableCell = new TableCell();
                titleTableCell.Content.Add("title");

                headerRow.Add(titleTableCell);


                TableCell idTableCell = new TableCell();
                idTableCell.Content.Add("id");

                headerRow.Add(idTableCell);

                TableCell revisionTableCell = new TableCell();
                revisionTableCell.Content.Add("revision");

                headerRow.Add(revisionTableCell);

                TableCell typeTableCell = new TableCell();
                typeTableCell.Content.Add("dataType");

                headerRow.Add(typeTableCell);

                TableCell descriptionTableCell = new TableCell();
                descriptionTableCell.Content.Add("description");

                headerRow.Add(descriptionTableCell);

                table.TableCells.Add(headerRow);

                foreach (PropertyClass propertyClass in domainClasses.PropertyClasses)
                {
                    string description = "";

                    if(propertyClass.Description.Count > 0)
                    {
                        description = propertyClass.Description[0].Text;
                    }

                    List<TableCell> contentRow = new List<TableCell>()
                    {
                        new TableCell(propertyClass.Title),
                        new TableCell(propertyClass.ID),
                        new TableCell(propertyClass.Revision),
                        new TableCell(propertyClass.GetDataTypeTitle(_specIfMetadataReader)),
                        new TableCell(description)
                    };

                    table.TableCells.Add(contentRow);
                }

                result += table.GenerateGridTable();

            }

            // resources
            if (domainClasses.ResourceClasses != null && domainClasses.ResourceClasses.Count != 0)
            {
                result += "### Resource classes of domain " + domainName + Environment.NewLine;

                Table table = new Table();

                List<TableCell> headerRow = new List<TableCell>();

                TableCell titleTableCell = new TableCell();
                titleTableCell.Content.Add("title");

                headerRow.Add(titleTableCell);


                TableCell idTableCell = new TableCell();
                idTableCell.Content.Add("id");

                headerRow.Add(idTableCell);

                TableCell revisionTableCell = new TableCell();
                revisionTableCell.Content.Add("revision");

                headerRow.Add(revisionTableCell);

                TableCell descriptionTableCell = new TableCell();
                descriptionTableCell.Content.Add("description");

                headerRow.Add(descriptionTableCell);

                table.TableCells.Add(headerRow);

                foreach (ResourceClass resourceClass in domainClasses.ResourceClasses)
                {
                    List<TableCell> contentRow = new List<TableCell>()
                    {
                        new TableCell(resourceClass.Title),
                        new TableCell(resourceClass.ID),
                        new TableCell(resourceClass.Revision)
                    };

                    contentRow.Add(GetResourceClassDescription(resourceClass));

                    table.TableCells.Add(contentRow);

                }

                result += table.GenerateGridTable();

            }

            // statements
            if (domainClasses.StatementClasses != null && domainClasses.StatementClasses.Count != 0)
            {
                result += "### Statement classes of domain " + domainName + Environment.NewLine;

                Table table = new Table();

                List<TableCell> headerRow = new List<TableCell>();

                TableCell titleTableCell = new TableCell();
                titleTableCell.Content.Add("title");

                headerRow.Add(titleTableCell);


                TableCell idTableCell = new TableCell();
                idTableCell.Content.Add("id");

                headerRow.Add(idTableCell);

                TableCell revisionTableCell = new TableCell();
                revisionTableCell.Content.Add("revision");

                headerRow.Add(revisionTableCell);

                TableCell descriptionTableCell = new TableCell();
                descriptionTableCell.Content.Add("description");

                headerRow.Add(descriptionTableCell);

                table.TableCells.Add(headerRow);

                foreach (StatementClass statementClass in domainClasses.StatementClasses)
                {
                    List<TableCell> contentRow = new List<TableCell>()
                    {
                        new TableCell(statementClass.Title),
                        new TableCell(statementClass.ID),
                        new TableCell(statementClass.Revision),
                    };

                    contentRow.Add(GetResourceClassDescription(statementClass));

                    table.TableCells.Add(contentRow);

                }

                result += table.GenerateGridTable();
            }

            return result;
        }

        private TableCell GetDataTypeDescription(DataType dataType)
        {
            TableCell result = new TableCell();

            if (dataType.Enumeration != null && dataType.Enumeration.Count > 0)
            {
                string description = "";
                if (dataType.Description != null && dataType.Description.Count > 0)
                {
                    description = dataType.Description[0].Text;
                }
                result.Content.Add(description);

                foreach (EnumerationValue value in dataType.Enumeration)
                {
                    result.Content.Add("<p>" + value.Value[0].Text + " [" + value.ID + "]</p>");
                }

            }
            else
            {
                if (dataType.Description != null)
                {
                    if (dataType.Description.ToString() == "[]")
                    {
                        result.Content.Add("");
                    }
                    else
                    {
                        if (dataType.Description.Count > 0)
                        {
                            result.Content.Add(dataType.Description[0].Text);
                        }
                    }
                }
                else
                {
                    result.Content.Add("");
                }
            }

            return result;
        }

        private TableCell GetResourceClassDescription(ResourceClass resourceClass)
        {
            TableCell result = new TableCell();

            result.Content.Add(resourceClass.Description[0].Text);

            if(resourceClass.PropertyClasses != null && resourceClass.PropertyClasses.Count != 0)
            {
                result.Content.Add("<p>Property classes:</p>");

                foreach(Key key in resourceClass.PropertyClasses)
                {
                    PropertyClass propertyClass = _specIfMetadataReader.GetPropertyClassByKey(key);

                    result.Content.Add("<p>" + key.GetPropertyClassTitle(_specIfMetadataReader) + " [" + propertyClass.ID + " "+ propertyClass.Revision + "]</p>");
                }
            }

            

            return result;
        }

        


    }

}