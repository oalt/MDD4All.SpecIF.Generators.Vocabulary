using MDD4All.SpecIF.DataProvider.File;
using System;
using System.IO;

namespace MDD4All.SpecIF.Generators.Vocabulary
{
    class Program
    {

        public Program()
        {
            GenerateDocumentation();

            GenerateClassDefinitionSpecIF();

        }

        private static void GenerateDocumentation()
        {
            string[] classDefinitionRoots = { @"d:\work\github\SpecIF\classDefinitions" };

            //VocabularyGenerator vocabularyGenerator = new VocabularyGenerator();

            //DataModels.SpecIF vocabulary = vocabularyGenerator.GenerateVocabulary(classDefinitionRoots);

            //SpecIfFileReaderWriter.SaveSpecIfToFile(vocabulary, @"c:\specif\GeneratedVocabulary.specif");


            WordDocumentationGenerator wordDocumentationGenerator = new WordDocumentationGenerator();

            string wordDocumentation = wordDocumentationGenerator.GenerateVocabularyDocumentation(classDefinitionRoots);

            File.WriteAllText(@"D:\work\github\SpecIF\documentation\04_SpecIF_Domain_Classes_Word.md", wordDocumentation);


            GithubDocumentationGenerator documentationGenerator = new GithubDocumentationGenerator();

            string githubDocumentation = documentationGenerator.GenerateVocabularyDocumentation(classDefinitionRoots);

            File.WriteAllText(@"D:\work\github\SpecIF\documentation\04_SpecIF_Domain_Classes_Github.md", githubDocumentation);
        }

        private static void GenerateClassDefinitionSpecIF()
        {
            string[] classDefinitionRoots = { @"d:\work\github\SpecIF\classDefinitions" };

            IntegratedClassGenerator integratedClassGenerator = new IntegratedClassGenerator();

            string outputPath = @"d:\work\github\SpecIF\classDefinitions\_Packages\SpecIF-Classes-1_1.specif";

            integratedClassGenerator.GenerateVocabulary(classDefinitionRoots, outputPath);
        }

        static void Main(string[] args)
        {
            Program program = new Program();
        }
    }
}
