//using CommonYo.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace CommonYo
{
    public class YoProcessor
    {
        private DirectoryInfo _solutionDirectoryInfo;
        //private DirectorySystemDto _directorySystemDto;
        //private FileSystemDto _fileSystemDto;
        private string _generatorName;

        public YoProcessor()//string generatorName)
        {
            //_generatorName = generatorName;
        }

        //public FileSystemDto Initialise(Dictionary<string, string> replacementsDictionary)
        //{
        //    var regularProjectName = replacementsDictionary["$safeprojectname$"];
        //    var solutionDirectory = replacementsDictionary["$solutiondirectory$"];
        //    _solutionDirectoryInfo = new DirectoryInfo(solutionDirectory);
        //    var tempDirectory = Path.GetTempPath();

        //    _fileSystemDto = new FileSystemDto
        //    {
        //        RegularProjectName = regularProjectName,
        //        SolutionDirectory = solutionDirectory,
        //        TempDirectory = tempDirectory
        //    };

        //    _directorySystemDto = new DirectorySystemDto
        //    {
        //        FileSystemDtoBase = _fileSystemDto,
        //        SolutionDirectoryInfo = _solutionDirectoryInfo
        //    };

        //    return _fileSystemDto;
        //}

        public void Generate()
        {
            GenerateYeomanProject();// (_directorySystemDto.SolutionDirectoryInfo.Parent.FullName);
        }

        private void GenerateYeomanProject()// string generationDirectory)
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);

            // Matt's code already caters for the ang-bas folder already existing - we don't need to check anything here
            var yoBatchFile = $@"{assemblyDirectory}\yo.bat";
            // Wrap each arg in quotes so that batch file caters for spaces in path names etc
            //var args = $"\"{_generatorName}\" \"{generationDirectory}\" \"{assemblyDirectory}\"";
            CreateYoProjectOnDisc(yoBatchFile);//, args);
        }

        //public void ArchiveRegularProject(string solutionDirectory, string tempDirectory)
        //{
        //    ArchiveRegularProject(solutionDirectory, tempDirectory, _solutionDirectoryInfo);
        //}

        //private void ArchiveRegularProject(string solutionDirectory, string tempDirectory, DirectoryInfo solutionDirectoryInfo)
        //{
        //    var archiveLocation = GetArchiveLocation(tempDirectory, solutionDirectoryInfo);
        //    Directory.Move(solutionDirectory, archiveLocation);
        //}

        //private string GetArchiveLocation(string tempDirectory, DirectoryInfo solutionDirectoryInfo)
        //{
        //    var dateTime = GetUniqueDateTime();
        //    var archiveLocation = $"{tempDirectory}\\{solutionDirectoryInfo.Name}_{dateTime}";
        //    if (Directory.Exists(archiveLocation))
        //    {
        //        archiveLocation = GetUniqueLocation(archiveLocation);
        //    }
        //    return archiveLocation;
        //}

        //private string GetUniqueLocation(string location)
        //{
        //    var random = new Random();
        //    return $"{location}_{random.Next(1, 999999)}";
        //}

        //private string GetUniqueDateTime()
        //{
        //    return $"{DateTime.UtcNow.Year}{DateTime.UtcNow.Month}{DateTime.UtcNow.Day}_{DateTime.UtcNow.Hour}{DateTime.UtcNow.Minute}{DateTime.UtcNow.Second}_{DateTime.UtcNow.Millisecond}";
        //}

        private void CreateYoProjectOnDisc(string batchFileToBeOpened)//, string args)
        {
            var processStartInfo = new ProcessStartInfo()
            {
                //Arguments = args,
                CreateNoWindow = false,
                FileName = batchFileToBeOpened,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Normal, 
            };

            // No need for a try/catch here - any exceptions are caught by the calling UserForm and displayed in the dialog to user
            using (var process = Process.Start(processStartInfo)) { }
        }
    }
}