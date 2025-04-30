using Microsoft.VisualStudio.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace VSIXProject2.QuestionTest
{
    public class QuestionTestWindow : BaseToolWindow<QuestionTestWindow>
    {
        public override string GetTitle(int toolWindowId) => "Question Result";

        public override Type PaneType => typeof(Pane);

        public override Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
        {
            return Task.FromResult<FrameworkElement>(new QuestionTestWindowControl());
        }

        [Guid("a7014ec4-f6df-4ba2-9536-cbf52c31445b")]
        internal class Pane : ToolkitToolWindowPane
        {
            public Pane()
            {
                BitmapImageMoniker = KnownMonikers.ToolWindow;
            }
        }
    }
}
