using System;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
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
            //using (new TempProjectsLocationContext(dte))
            //{
            //    dte.ExecuteCommand("File.NewProject");
            //}
            var yoproc = new YoProcessor();
            yoproc.Generate();
        }

        public static NewYeomanProjectCommand Instance
        {
            get;
            private set;
        }
    }
}
