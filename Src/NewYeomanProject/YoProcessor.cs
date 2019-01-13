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

            RunYoBatchFile(yoBatchFile, args, generationDirectory);
        }

        private static string GetYoBatchFileFullPath()
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);

            var yoBatchFile = $@"{assemblyDirectory}\yo.bat";
            return yoBatchFile;
        }

        private void RunYoBatchFile(string batchFileToBeOpened, string args, string generationDirectory)
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
            
            // Wait for exit or time out.
            process.WaitForExit();

            // Check to see if process still running
            if (process.HasExited == false)
            {
                // Check if process is hung up
                if (process.Responding)
                {
                    process.CloseMainWindow();
                }
                else
                {
                    process.Kill();
                }

                MessageBox.Show(
                    $"An unexpected error has occurred.{Environment.NewLine}{Environment.NewLine}Process {process?.ProcessName} with id {process?.Id} ended with exit code {process?.ExitCode}.",//gregt dedupe
                    "New Yeoman Project Error",//gregt dedupe
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                switch (process.ExitCode)
                {
                    // all good
                    case 0:
                        MessageBox.Show(
                            $"Yeoman project was successfully created at {generationDirectory}",//gregt dedupe
                            "New Yeoman Project Success",//gregt dedupe
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                        break;
                    // yo command not found
                    case 1:
                        MessageBox.Show(
                            $"Yeoman project was not created.{Environment.NewLine}{Environment.NewLine}Process {process.Id} ended with exit code {process.ExitCode}.",//gregt dedupe
                            "New Yeoman Project Error",//gregt dedupe
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        break;
                    // user closed cmd prompt prematurely
                    case -1073741510:
                        MessageBox.Show(
                            $"Yeoman project was not created.",//gregt dedupe
                            "New Yeoman Project Warning",//gregt dedupe
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        break;
                    default:
                        MessageBox.Show(
                            $"An unexpected error has occurred.{Environment.NewLine}{Environment.NewLine}Process {process?.ProcessName} with id {process?.Id} ended with exit code {process?.ExitCode}.",//gregt dedupe
                            "New Yeoman Project Error",//gregt dedupe
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        break;          
                }
            }
        }

        public void Dispose()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
        }
    }
}