using Microsoft.VisualStudio.Shell;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace NewYeomanProject
{
    public class YoProcessor : IDisposable
    {
        public YoProcessor()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
        }

        public void Generate(string newProjectDirectory)
        {
            GenerateYeomanProject(newProjectDirectory);
        }

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

            // No need for a try/catch here - any exceptions are caught by the caller and displayed in the dialog to user
            var process = Process.Start(processStartInfo);
            
            //Wait for the process to exit or time out.
            process.WaitForExit();

            //Check to see if the process is still running.
            if (process.HasExited == false)
            {
                //Process is still running. Test to see if the process is hung up.
                if (process.Responding)
                {
                    process.CloseMainWindow();
                }
                else
                {
                    process.Kill();
                }

                MessageBox.Show(
                    $"Something went wrong - unexpected error occurred. Process {process.ProcessName} with id {process.Id} ended with exit code {process.ExitCode}",
                       "Error",
                       MessageBoxButton.OK,
                       MessageBoxImage.Error);
            }
            else
            {
                if (process.ExitCode == 0)
                {
                    MessageBox.Show(
                        $"Yeoman project was successfully created at 'gregt_locn'", 
                        "SUCCESS",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(
                        $"Yeoman project was not created.{Environment.NewLine}Process {process.Id} ended with exit code {process.ExitCode}.",
                        "ERROR", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error);
                }
            }
        }

        public void Dispose()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
        }
    }
}