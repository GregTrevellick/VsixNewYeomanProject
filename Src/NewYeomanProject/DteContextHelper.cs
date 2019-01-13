using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;

namespace NewYeomanProject
{
    public class DteContextHelper : IDisposable
    {
        public readonly string ProjectsLocation;

        public DteContextHelper(DTE dte)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            ProjectsLocation = (string)dte.Properties["Environment", "ProjectsAndSolution"].Item("ProjectsLocation").Value;
        }

        public void Dispose()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
        }
    }
}
