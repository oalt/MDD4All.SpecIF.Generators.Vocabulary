using MDD4All.SpecIF.DataModels;
using MDD4All.SpecIF.DataProvider.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MDD4All.SpecIF.Generators.Vocabulary
{
    public class IntegratedClassGenerator
    {

        private Dictionary<string, SpecIF.DataModels.SpecIF> _domainClasses = new Dictionary<string, SpecIF.DataModels.SpecIF>();

        private SpecIF.DataModels.SpecIF _metaDataSpecIF = new SpecIF.DataModels.SpecIF();

        public void GenerateVocabulary(string[] classDefinitionRoot, string outputPath)
        {
            _metaDataSpecIF = new SpecIF.DataModels.SpecIF()
            {
                CreatedAt = DateTime.Now,
                Generator = "SpecIFicator Vocabulary Generator",
                Title = new List<MultilanguageText> {
                    new MultilanguageText("SpecIF classes for SpecIF 1.1")
                }
            };

            // read all datatype and class definition
            foreach (string path in classDefinitionRoot)
            {
                DirectoryInfo classDefinitionRootDirectory = new DirectoryInfo(path);

                foreach (DirectoryInfo domainDirectoryInfo in classDefinitionRootDirectory.GetDirectories())
                {
                    InitializeClassDefinitions(domainDirectoryInfo);
                }

            }

            SpecIfFileReaderWriter.SaveSpecIfToFile(_metaDataSpecIF, outputPath);
        }


        private void InitializeClassDefinitions(DirectoryInfo domainDirectory)
        {
            string domainName = domainDirectory.Name;

            FileInfo[] specIfFiles = domainDirectory.GetFiles("*.specif");

            SpecIF.DataModels.SpecIF domainSpecIF = null;

            foreach (FileInfo fileInfo in specIfFiles)
            {

                SpecIF.DataModels.SpecIF specIF = SpecIfFileReaderWriter.ReadDataFromSpecIfFile(fileInfo.FullName);

                foreach(DataType dataType in specIF.DataTypes)
                {
                    if(!_metaDataSpecIF.DataTypes.Any(el => el.ID == dataType.ID && el.Revision == dataType.Revision))
                    {
                        _metaDataSpecIF.DataTypes.Add(dataType);
                    }
                }

                foreach (PropertyClass propertyClass in specIF.PropertyClasses)
                {
                    if (!_metaDataSpecIF.PropertyClasses.Any(el => el.ID == propertyClass.ID && el.Revision == propertyClass.Revision))
                    {
                        _metaDataSpecIF.PropertyClasses.Add(propertyClass);
                    }
                }

                foreach (ResourceClass resourceClass in specIF.ResourceClasses)
                {
                    if (!_metaDataSpecIF.ResourceClasses.Any(el => el.ID == resourceClass.ID && el.Revision == resourceClass.Revision))
                    {
                        _metaDataSpecIF.ResourceClasses.Add(resourceClass);
                    }
                }

                foreach (StatementClass statementClass in specIF.StatementClasses)
                {
                    if (!_metaDataSpecIF.StatementClasses.Any(el => el.ID == statementClass.ID && el.Revision == statementClass.Revision))
                    {
                        _metaDataSpecIF.StatementClasses.Add(statementClass);
                    }
                }
            }

            


        }
    }
}
