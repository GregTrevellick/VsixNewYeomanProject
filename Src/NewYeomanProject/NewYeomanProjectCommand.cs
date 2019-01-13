using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.Windows;
using Task = System.Threading.Tasks.Task;

namespace NewYeomanProject
{
    internal sealed class NewYeomanProjectCommand
    {
        private NewYeomanProjectCommand(IMenuCommandService commandService, DTE dte)
        {
            var menuCommandID = new CommandID(PackageGuids.guidNewYeomanProjectCommandPackageCmdSet, PackageIds.NewYeomanProjectCommandId);
            var menuItem = new MenuCommand((s, e) => Execute(dte), menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            await package.JoinableTaskFactory.SwitchToMainThreadAsync();

            var commandService = (IMenuCommandService)await package.GetServiceAsync((typeof(IMenuCommandService)));
            var dte = (DTE)await package.GetServiceAsync((typeof(DTE)));
            Instance = new NewYeomanProjectCommand(commandService, dte);
        }

        [STAThread]
        static void Execute(DTE dte)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                using (var dteContextHelper = new DteContextHelper(dte))
                using (var yoProcessor = new YoProcessor())
                {
                    yoProcessor.Generate(dteContextHelper.ProjectsLocation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Yeoman project was not created.{Environment.NewLine}{Environment.NewLine}{ex.ToString()}.",//gregt dedupe
                    "New Yeoman Project Error",//gregt dedupe
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public static NewYeomanProjectCommand Instance
        {
            get;
            private set;
        }
    }
}
