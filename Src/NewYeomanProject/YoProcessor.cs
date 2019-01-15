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
        private string _checkGenerationDirectory { get { return $"Check {_newProjectDirectory} to see if your project was created successfully."; } }
        private int _exitCode;
        private readonly string _newProjectDirectory;
        private int _processId;
        private string _processName;
        private readonly string _unexpectedError = "An unexpected error has occurred.";
        private readonly int _yoCommandTimeOutSeconds = 15 * 60;
        private int _yoCommandTimeOutMilliSeconds { get { return _yoCommandTimeOutSeconds * 1000; } }

        public string LineBreak = $"{Environment.NewLine}{Environment.NewLine}";
        public string ProjectNotCreated = "Yeoman project was not created.";

        public YoProcessor()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
        }
        
        public YoProcessor(string newProjectDirectory)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _newProjectDirectory = newProjectDirectory;
        }

        public void Generate()
        {
            GenerateYeomanProject(_newProjectDirectory);
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

            var startTime = DateTime.UtcNow;
            var process = Process.Start(processStartInfo);

            _processId = process.Id;
            _processName = process.ProcessName;

            process.WaitForExit(_yoCommandTimeOutMilliSeconds);

            _exitCode = process.ExitCode;

            if (process.HasExited)
            {
                HandleNormalExit(generationDirectory);
            }
            else
            {
                HandleAbnormalExit(generationDirectory, startTime, process);
            }
        }

        private void HandleNormalExit(string generationDirectory)
        {
            switch (_exitCode)
            {
                // Happy path
                case 0:
                    ShowMessageBoxSuccess($"Yeoman project was successfully created at {generationDirectory}");
                    break;

                // TO DEBUG TEST: change yo.bat from 'call yo' to 'call yo2'
                case 1:
                    ShowMessageBoxError($"{ProjectNotCreated}{LineBreak}{GetProcessDetails()}");
                    break;

                // TO DEBUG TEST: manually close command prompt (cross in top RHS corner)
                case -1073741510:
                    ShowMessageBoxWarning($"{ProjectNotCreated}");
                    break;

                // TO DEBUG TEST: add 'exit /b 2' as first command in yo.bat
                default:
                    ShowMessageBoxError($"{_unexpectedError}{LineBreak}{GetProcessDetails()}");
                    break;
            }
        }

        private void HandleAbnormalExit(string generationDirectory, DateTime startTime, Process process)
        {
            if (process.Responding)
            {
                HandleResponsiveAbnormalExit(generationDirectory, startTime, process);
            }
            else
            {
                HandleNonResponsiveAbnormalExit(process);
            }
        }

        private void HandleResponsiveAbnormalExit(string generationDirectory, DateTime startTime, Process process)
        {
            process.CloseMainWindow();

            var endTime = DateTime.UtcNow;
            var duration = (endTime - startTime).TotalMilliseconds;

            if (duration > _yoCommandTimeOutMilliSeconds)
            {
                // TO DEBUG TEST: set the timeout to, say, 10 secs, wait for 10 secs to expire (no need to create a project)
                var yoCommandTimeOutText = _yoCommandTimeOutSeconds <= 60 ?
                    $"{_yoCommandTimeOutSeconds} seconds" :
                    $"{_yoCommandTimeOutSeconds / 60} minutes";

                ShowMessageBoxWarning($"Creation of your Yeoman project was interupted as it exceeded the timeout of {yoCommandTimeOutText}.{LineBreak}{_checkGenerationDirectory }");
            }
            else
            {
                // TO DEBUG TEST: as above but drag cursor to here
                ShowMessageBoxError($"Creation of your Yeoman project unexpectedly ended. Please try again.{LineBreak}{_checkGenerationDirectory }");
            }
        }

        private void HandleNonResponsiveAbnormalExit(Process process)
        {
            process.Kill();

            // TO DEBUG TEST: as above but drag cursor to here
            ShowMessageBoxError($"{_unexpectedError} {GetProcessDetails()}");
        }

        private string GetProcessDetails()
        {
            return $"Process '{_processName}' with id {_processId} ended with exit code {_exitCode}.";
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