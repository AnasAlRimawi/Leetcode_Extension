using Microsoft.VisualStudio.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace VSIXProject2.QuestionDetails
{
    public class QuestionDetailsWindow : BaseToolWindow<QuestionDetailsWindow>
    {
        public override string GetTitle(int toolWindowId) => "Question Descripiton";

        public override Type PaneType => typeof(Pane);

        public override Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
        {
            return Task.FromResult<FrameworkElement>(new QuestionDetailsWindowControl());
        }

        [Guid("2faccf89-0bd8-49aa-b1fd-abc02e35ef98")]
        internal class Pane : ToolkitToolWindowPane
        {
            public Pane()
            {
                BitmapImageMoniker = KnownMonikers.ToolWindow;
            }
        }
    }
}
