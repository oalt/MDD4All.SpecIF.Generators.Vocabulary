using MDD4All.SpecIF.DataProvider.Contracts;
using MDD4All.SpecIF.DataProvider.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MDD4All.SpecIF.Generators.Vocabulary
{
    public abstract class SpecIfGeneratorBase
    {
        protected SpecIF.DataModels.SpecIF _metaDataSpecIF = new SpecIF.DataModels.SpecIF();

        protected ISpecIfMetadataReader _specIfMetadataReader;

        protected Dictionary<string, SpecIF.DataModels.SpecIF> _domainClasses = new Dictionary<string, SpecIF.DataModels.SpecIF>();

        

        public SpecIfGeneratorBase(List<DirectoryInfo> directories)
        {
            foreach (DirectoryInfo directoryInfo in directories)
            {
                InitializeClassDefinitions(directoryInfo);
            }

            _specIfMetadataReader = new SpecIfFileMetadataReader(_metaDataSpecIF);
        }

        public string GenerateDocumentation()
        {
            string result = "";

            foreach (KeyValuePair<string, SpecIF.DataModels.SpecIF> domain in _domainClasses)
            {
                if (domain.Key != "_Packages" && domain.Key != "Vocabulary")
                {
                    result += GenerateDomainDocumentation(domain.Key, domain.Value);
                }
            }

            return result;
        }

        


        private void InitializeClassDefinitions(DirectoryInfo domainDirectory)
        {
            string domainName = domainDirectory.Name;

            FileInfo[] specIfFiles = domainDirectory.GetFiles("*.specif");

            SpecIF.DataModels.SpecIF domainSpecIF = new SpecIF.DataModels.SpecIF();

            int fileConuter = 0;

            foreach (FileInfo fileInfo in specIfFiles)
            {
                fileConuter++;

                SpecIF.DataModels.SpecIF specIF = SpecIfFileReaderWriter.ReadDataFromSpecIfFile(fileInfo.FullName);

                domainSpecIF.DataTypes.AddRange(specIF.DataTypes);
                domainSpecIF.PropertyClasses.AddRange(specIF.PropertyClasses);
                domainSpecIF.ResourceClasses.AddRange(specIF.ResourceClasses);
                domainSpecIF.StatementClasses.AddRange(specIF.StatementClasses);

                _metaDataSpecIF.DataTypes.AddRange(specIF.DataTypes);
                _metaDataSpecIF.PropertyClasses.AddRange(specIF.PropertyClasses);
                _metaDataSpecIF.ResourceClasses.AddRange(specIF.ResourceClasses);
                _metaDataSpecIF.StatementClasses.AddRange(specIF.StatementClasses);
            }

            _domainClasses.Add(domainName, domainSpecIF);

        }

        protected abstract string GenerateDomainDocumentation(string key, SpecIF.DataModels.SpecIF domainClasses);
    }
}
