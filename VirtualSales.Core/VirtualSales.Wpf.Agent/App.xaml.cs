using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ReactiveUI;
using Splat;
using VirtualSales.Core;
using VirtualSales.Core.Infrastructure;
using System.Windows.Documents;
using VirtualSales.Core.SignalR;

namespace VirtualSales.Wpf.Agent
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            var agentConnection = Locator.GetService<IAgentConnection>();
            agentConnection.EndMeeting(Guid.Empty);

            base.OnExit(e);
        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var logger = new RxUiLogger();
            Splat.Locator.CurrentMutable.RegisterConstant(logger, typeof(ILogger));

            Locator = new WpfViewModelLocator(true);
            Current.Resources["Locator"] = Locator;

            FlowDocument.FontSizeProperty.OverrideMetadata(
                typeof(FlowDocument),
                new FrameworkPropertyMetadata(14.0));

            TextElement.FontSizeProperty.OverrideMetadata(
                typeof(TextElement),
                new FrameworkPropertyMetadata(16.0));

            TextBlock.FontSizeProperty.OverrideMetadata(
                typeof(TextBlock),
                new FrameworkPropertyMetadata(16.0));
            await Locator.MainService.StartApp();
        }

        public ViewModelLocator Locator
        {
            get;
            private set;
        }
    }
}
