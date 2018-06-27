using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VirtualSales.Models;

namespace VirtualSales.Core.ViewModels
{
    public class VideoConfViewModel : ReactiveObject
    {
        // this must be unique per instance of the app
        private static readonly int _userId = Guid.NewGuid().GetHashCode(); 

        public VideoConfViewModel()
        {
            var playCommand = new ReactiveCommand(this.WhenAnyValue(vm => vm.IsPlaying,
                                                                    vm => vm.Config,
                                                                    (playing, config) => !playing && (config != null)));
            this.PlayCommand = playCommand;
            playCommand.RegisterAsyncAction(_ => OnPlay());

            var pauseCommand = new ReactiveCommand(this.WhenAnyValue(vm => vm.IsPlaying,
                                                                     (playing) => playing));
            pauseCommand.RegisterAsyncAction(_ => OnPause());
            this.PauseCommand = pauseCommand;
        }

        private VideoChatConfiguration _config;
        private bool _isPlaying;
        private bool _videoInitCompleted;

        public bool VideoInitCompleted
        {
            get { return _videoInitCompleted; }
            set { this.RaiseAndSetIfChanged(ref _videoInitCompleted, value); }
        }

        private void OnPlay()
        {
            this.IsPlaying = true;
        }
        private void OnPause()
        {
            this.IsPlaying = false;
        }

        public VideoChatConfiguration Config
        {
            get { return _config; }
            set { this.RaiseAndSetIfChanged(ref _config, value); }
        }

        public int UserId
        {
            get { return _userId; }
        }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set { this.RaiseAndSetIfChanged(ref _isPlaying, value); }
        }
        public ICommand PauseCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
    }
}
