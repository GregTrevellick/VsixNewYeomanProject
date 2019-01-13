using System;
///////////////using System.IO;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace NewYeomanProject
{
    public class DteContextHelper : IDisposable
    {
        //readonly Property projectsLocation;
        public readonly string ProjectsLocation;

        public DteContextHelper(DTE dte)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            ////////var projectsLocation = dte.Properties["Environment", "ProjectsAndSolution"].Item("ProjectsLocation");
            ////////location = (string)projectsLocation.Value;
            //////////var tempPath = $@"Temp\{DateTime.Now:yyyy-MM-dd}\";
            //////////var tempLocation = Path.Combine(location, tempPath);
            //////////projectsLocation.Value = location;

            ProjectsLocation = (string)dte.Properties["Environment", "ProjectsAndSolution"].Item("ProjectsLocation").Value;
        }

        public void Dispose()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            //projectsLocation.Value = location;
        }
    }
}
