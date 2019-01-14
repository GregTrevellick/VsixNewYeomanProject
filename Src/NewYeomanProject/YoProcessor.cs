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
        public string ProjectNotCreated = "Yeoman project was not created.";
        private int yoCommandTimeOutMilliSeconds = 20_000;//900_000; // === 15 minutes
        private string _unexpectedError = "An unexpected error has occurred.";

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
            var startTime = DateTime.UtcNow;

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
            process.WaitForExit(yoCommandTimeOutMilliSeconds);

            if (process.HasExited == false)
            {
                if (process.Responding)
                {
                    //If we hit here most likely a timeout
                    process.CloseMainWindow();

                    var endTime = DateTime.UtcNow;
                    var duration = (endTime - startTime).TotalMilliseconds;
                    if (duration > yoCommandTimeOutMilliSeconds)
                    {
                        // TO DEBUG TEST: set the timeout to, say, 30 secs, create a project and wait for 30 secs to expire
                        ShowMessageBoxWarning($"Creation of your Yeoman project was interupted as it exceeded the timeout of {yoCommandTimeOutMilliSeconds}.{Environment.NewLine}{Environment.NewLine}Your project was probably created successfully at {generationDirectory}.");
                    }
                    else
                    {
                        // TO DEBUG TEST: as above but drag cursor to here
                        ShowMessageBoxWarning($"Creation of your Yeoman project unexpectedly ended. Please try again.{Environment.NewLine}{Environment.NewLine}Alternatively check {generationDirectory} to see if your project was created successfully.");
                    }
                }
                else
                {
                    //If we hit here most likely a more serious error - so tell user the details 
                    process.Kill();

                    #region TO DEBUG TEST: make this the first command in yo.bat
                    //for / L %% n in (1, 0, 10) do (
                    //    echo do stuff
                    //    exit / b
                    //    call: stop
                    //)
                    //:stop
                    //call :__stop 2 > nul
                    //:__stop
                    //() creates a syntax error, quits the batch
                    #endregion
                    ShowMessageBoxError($"{_unexpectedError} {processDetails(false)}");
                }
            }
            else
            {
                switch (process.ExitCode)
                {
                    // Happy path
                    case 0:
                        ShowMessageBoxSuccess($"Yeoman project was successfully created at {generationDirectory}");
                        break;

                    // TO DEBUG TEST: change yo.bat from 'call yo' to 'call yo2'
                    case 1:
                        ShowMessageBoxError($"{ProjectNotCreated}{Environment.NewLine}{Environment.NewLine}{processDetails(false)}");
                        break;

                    // TO DEBUG TEST: manually close command prompt (top RHS)
                    case -1073741510:
                        ShowMessageBoxWarning($"{ProjectNotCreated}");
                        break;

                    // TO DEBUG TEST: add 'exit /b 2' as first command in yo.bat
                    default:
                        ShowMessageBoxError($"{_unexpectedError}{Environment.NewLine}{Environment.NewLine}{processDetails(false)}");
                        break;          
                }
            }

            string processDetails(bool includeProcessName)
            {
                var processName = includeProcessName ? process.ProcessName : string.Empty;
                return $"Process {processName} with id {process?.Id} ended with exit code {process?.ExitCode}.";
            }
        }

        public void ShowMessageBoxError(string messageBoxText)
        {
            ShowMessageBox(messageBoxText, "Error", MessageBoxImage.Error);
        }

        private void ShowMessageBoxSuccess(string messageBoxText)
        {
            ShowMessageBox(messageBoxText, "Success", MessageBoxImage.Information);
        }

        private void ShowMessageBoxWarning(string messageBoxText)
        {
            ShowMessageBox(messageBoxText, "Warning", MessageBoxImage.Warning);
        }

        private void ShowMessageBox(string messageBoxText, string caption, MessageBoxImage messageBoxImage)
        {
            MessageBox.Show(
                messageBoxText,
                $"New Yeoman Project {caption}",//gregt get this string from resx #110
                MessageBoxButton.OK,
                messageBoxImage);
        }

        public void Dispose()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
        }
    }
}