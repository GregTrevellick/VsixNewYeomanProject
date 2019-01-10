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
        //private DirectoryInfo _solutionDirectoryInfo;
        //private string _generatorName;

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
            CreateYoProjectOnDisc();
        }

        private void CreateYoProjectOnDisc()
        {
            var processStartInfo = new ProcessStartInfo()
            {
                CreateNoWindow = false,
                FileName = "Yo",
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Normal, 
            };

            // No need for a try/catch here - any exceptions are caught by the calling UserForm and displayed in the dialog to user
            using (var process = Process.Start(processStartInfo)) { }
        }
    }
}