using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Nito.AsyncEx;
using ReactiveUI;
using Splat;
using VirtualSales.Core.AppServices;
using VirtualSales.Core.SignalRx;

namespace VirtualSales.Core.SignalR
{
    public abstract class SignalRClient : ISignalRClient, IEnableLogger
    {
        private readonly ReplaySubject<DateTimeOffset> _heartbeatSubject = new ReplaySubject<DateTimeOffset>(1);
        private readonly Subject<string> _pdfAvailableSubject = new Subject<string>();
        private readonly AsyncManualResetEvent _manualReset = new AsyncManualResetEvent(false);
        protected readonly ISettings _settings;

        private readonly HubConnection _connection;
        private static string _WhiteBrandSignalRHostUrl;

        protected SignalRClient(ISettings settings)
        {
            _settings = settings;
            _WhiteBrandSignalRHostUrl = "WhiteBrandSignalRHostUrl";
            _connection = new HubConnection(_settings.GetValue(_WhiteBrandSignalRHostUrl));

            Proxy = _connection.CreateHubProxy("MeetingHub");
            Proxy.Observe<DateTimeOffset>("onHeartbeat").Log(this, "Heartbeat").Subscribe(_heartbeatSubject);
            Proxy.Observe<string>("onPdfAvailable").Log(this, "PDF Available ").Subscribe(_pdfAvailableSubject);
        }

        protected Task ConnectedTask
        {
            get { return _manualReset.WaitAsync(); }
        }

        protected IHubProxy Proxy { get; private set; }

        /// <summary>
        ///     Stream of heartbeat messages. Will always contain the last heartbeat
        /// </summary>
        public IObservable<DateTimeOffset> Heartbeat
        {
            get { return _heartbeatSubject; }
        }

        public IObservable<string> PdfAvailable
        {
            get { return _pdfAvailableSubject; }
        }

        public async Task Connect()
        {
            await _connection.Start();
            _manualReset.Set();
        }

        public async Task<byte[]> GetIllustrationPdfAsync(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_settings.GetValue(_WhiteBrandSignalRHostUrl));
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("api/pdfdoc/" + id);
                var result = await response.Content.ReadAsByteArrayAsync();
                return result;
            }
        }
    }
}