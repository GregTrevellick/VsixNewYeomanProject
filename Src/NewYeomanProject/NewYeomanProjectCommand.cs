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

            try
            {
                using (var dteContextHelper = new DteContextHelper(dte))
                {
                    var yoProcessor = new YoProcessor();
                    yoProcessor.Generate(dteContextHelper.ProjectsLocation);
                }
            }
            catch (Exception)
            {
                //gregt do something
                throw;
            }
        }

        public static NewYeomanProjectCommand Instance
        {
            get;
            private set;
        }
    }
}
