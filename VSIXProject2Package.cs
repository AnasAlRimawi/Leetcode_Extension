global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;
using System.Runtime.InteropServices;
using System.Threading;
using VSIXProject2.QuestionDetails;
using VSIXProject2.QuestionTest;

namespace VSIXProject2
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideToolWindow(typeof(MyToolWindow.Pane), Style = VsDockStyle.Tabbed, Window = WindowGuids.SolutionExplorer)]
    [ProvideToolWindow(typeof(QuestionDetailsWindow.Pane), Style = VsDockStyle.Tabbed, Window = WindowGuids.MainWindow, Transient = true)]
    [ProvideToolWindow(typeof(QuestionTestWindow.Pane), Style = VsDockStyle.Tabbed, Window = WindowGuids.MainWindow, Transient = true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.VSIXProject2String)]
    public sealed class VSIXProject2Package : ToolkitPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.RegisterCommandsAsync();

            this.RegisterToolWindows();
        }
    }
}