using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace NewYeomanProject
{
    public class YoProcessor
    {
        public void Generate(string newProjectDirectory)
        {
            //var newProjectDirectory = GetNewProjectDirectory();
            GenerateYeomanProject(newProjectDirectory);
        }

        //private static string GetNewProjectDirectory()
        //{
        //    var newProjectDictionary = replacementsDictionary["$solutiondirectory$"];

        //    var newProjectDirectoryInfo = new DirectoryInfo(newProjectDictionary);
        //    var newProjectDirectoryFullName = newProjectDirectoryInfo.Parent.FullName;

        //    return newProjectDirectoryFullName;
        //}

        private void GenerateYeomanProject(string generationDirectory)
        {
            var yoBatchFile = GetYoBatchFileFullPath();

            // Wrap each arg in quotes so that batch file caters for spaces in path names etc
            var args = $"\"{generationDirectory}\"";

            RunYoBatchFile(yoBatchFile, args);
        }

        private static string GetYoBatchFileFullPath()
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);

            var yoBatchFile = $@"{assemblyDirectory}\yo.bat";
            return yoBatchFile;
        }

        private void RunYoBatchFile(string batchFileToBeOpened, string args)
        {
            var processStartInfo = new ProcessStartInfo()
            {
                Arguments = args,
                CreateNoWindow = false,
                FileName = batchFileToBeOpened,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Normal,
            };

            // No need for a try/catch here - any exceptions are caught by the calling UserForm and displayed in the dialog to user
            using (var process = Process.Start(processStartInfo)) { }
        }
    }
}