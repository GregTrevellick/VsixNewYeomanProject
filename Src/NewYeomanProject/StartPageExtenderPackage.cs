using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace NewYeomanProject
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(PackageGuids.startPageExtenderPackageString)]
    [ProvideAutoLoad(PackageGuids.startPageToolWindowString, PackageAutoLoadFlags.BackgroundLoad)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class StartPageExtenderPackage : AsyncPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            if (VsVersion < 15)
            {
                // Doesn't work with Visual Studio 2015
                return;
            }

            await JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = (EnvDTE.DTE)await GetServiceAsync(typeof(EnvDTE.DTE));
            var vsUIShell = (IVsUIShell7)await GetServiceAsync(typeof(SVsUIShell));
            StartPageExtender.Initialize(vsUIShell, dte);
        }

        static int VsVersion => Process.GetCurrentProcess().MainModule.FileVersionInfo.FileMajorPart;
    }
}
