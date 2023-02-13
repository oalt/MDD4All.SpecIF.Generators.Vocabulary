using MDD4All.SpecIF.DataProvider.File;
using System;
using System.Collections.Generic;
using System.IO;

namespace MDD4All.SpecIF.Generators.Vocabulary
{
    class Program
    {

        public Program()
        {
            //GenerateDocumentationAndClassFile();
            //GenerateDocumentationAndClassFile(false);


            #region SpecIF 1.2
            string[] classDefinitionRoots =
            {
                @"c:\Users\alto\work\github\SpecIF-Class-Definitions_dev\"
            };

            List<DirectoryInfo> directories = CalculateDirectoryInfos(classDefinitionRoots, true);

            string title = "Normative SpecIF 1.2";

            string outputPath = classDefinitionRoots[0] + "/_Packages/SpecIF-Classes-1_2.specif";

            GenerateClassDefinitionSpecIF(directories, title, outputPath, true);

            directories = CalculateDirectoryInfos(classDefinitionRoots, false);

            title = "Non-Normative SpecIF 1.2";

            outputPath = classDefinitionRoots[0] + "/_Packages/SpecIF-Classes-1_2_non_normative.specif";

            GenerateClassDefinitionSpecIF(directories, title, outputPath, false);
            #endregion


            //GenerateParameterDocumentation();
        }

        private static void GenerateParameterDocumentation()
        {
            //string parameterDefinitionFile = @"d:\test\SpecIF_Parameters\Parameter.specif";

            //HtmlDocumentationGenerator githubDocumentationGenerator = new HtmlDocumentationGenerator();

            //SpecIF.DataModels.SpecIF specIF =  SpecIfFileReaderWriter.ReadDataFromSpecIfFile(parameterDefinitionFile);

            //string markdown = githubDocumentationGenerator.GenerateDomainDocumentation("Textile Machines", specIF);

            //File.WriteAllText(@"d:\test\SpecIF_Parameters\Parameters.html", markdown);
        }

        private List<DirectoryInfo> CalculateDirectoryInfos(string[] classDefinitionRootPaths, bool normative = true)
        {
            List<DirectoryInfo> result = new List<DirectoryInfo>();

            foreach (string path in classDefinitionRootPaths)
            {
                DirectoryInfo classDefinitionRootDirectory = new DirectoryInfo(path);



                foreach (DirectoryInfo domainDirectoryInfo in classDefinitionRootDirectory.GetDirectories())
                {
                    if (normative)
                    {
                        if (domainDirectoryInfo.Name.StartsWith("01") ||
                            domainDirectoryInfo.Name.StartsWith("02") ||
                            domainDirectoryInfo.Name.StartsWith("03"))
                        {

                            result.Add(domainDirectoryInfo);
                        }
                    }
                    else
                    {
                        if (!domainDirectoryInfo.Name.StartsWith("01") &&
                            !domainDirectoryInfo.Name.StartsWith("02") &&
                            !domainDirectoryInfo.Name.StartsWith("03") &&
                            !domainDirectoryInfo.Name.StartsWith("vocabulary"))
                        {

                            result.Add(domainDirectoryInfo);
                        }
                    }
                }

            }

            return result;
        }

        private void GenerateDocumentationAndClassFile(bool normative = true)
        {
            string[] classDefinitionRoots = { @"d:\work\github\SpecIF\classDefinitions" };

            //VocabularyGenerator vocabularyGenerator = new VocabularyGenerator();

            //DataModels.SpecIF vocabulary = vocabularyGenerator.GenerateVocabulary(classDefinitionRoots);

            //SpecIfFileReaderWriter.SaveSpecIfToFile(vocabulary, @"c:\specif\GeneratedVocabulary.specif");

            List<DirectoryInfo> directories = CalculateDirectoryInfos(classDefinitionRoots, normative);

            GenerateDocumentation(directories, normative);

            string title = "Normative data type and class definitions for SpecIF 1.1";

            if (!normative)
            {
                title = "Non-Normative data type and class definitions for SpecIF 1.1";
            }

            

            string outputPath = @"d:\work\github\SpecIF\classDefinitions\_Packages\SpecIF-Classes-1_1.specif";

            if (!normative)
            {
                outputPath = @"d:\work\github\SpecIF\classDefinitions\_Packages\SpecIF-Classes-1_1_non_normative.specif";
            }

            GenerateClassDefinitionSpecIF(directories, title, outputPath, normative);


        }

        private static void GenerateDocumentation(List<DirectoryInfo> directoryInfos, bool normative = true)
        {
            

            WordDocumentationGenerator wordDocumentationGenerator = new WordDocumentationGenerator(directoryInfos);

            string wordDocumentation = wordDocumentationGenerator.GenerateDocumentation();

            string filenamePrefix = @"D:\work\github\SpecIF\documentation\04_SpecIF_Domain_Classes";
            string filenamePostfixGithub = "_Github.md";
            string filenamePostfixWord = "_Word.md";

            if (!normative)
            {
                filenamePrefix = @"D:\work\github\SpecIF\documentation\09_SpecIF_Domain_Classes";
                filenamePostfixGithub = "_Github_Non_Normative.md";
                filenamePostfixWord = "_Word_Non_Normative.md";
            }

            File.WriteAllText(filenamePrefix + filenamePostfixWord, wordDocumentation);


            GithubDocumentationGenerator documentationGenerator = new GithubDocumentationGenerator(directoryInfos);

            string githubDocumentation = documentationGenerator.GenerateDocumentation();

            File.WriteAllText(filenamePrefix + filenamePostfixGithub, githubDocumentation);
        }

        private static void GenerateClassDefinitionSpecIF(List<DirectoryInfo> directoryInfos, 
                                                          string title,
                                                          string outputPath,
                                                          bool normative = true)
        {
            IntegratedClassGenerator integratedClassGenerator = new IntegratedClassGenerator(directoryInfos, title);

            integratedClassGenerator.GenerateVocabulary(outputPath);
        }

        static void Main(string[] args)
        {
            Program program = new Program();
        }
    }
}
