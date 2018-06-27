using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using VirtualSales.Models.Annotations;

namespace VirtualSales.Core.ViewModels.Annotations
{
    /// <summary>
    /// View model for then UI that allows the agent to create 
    /// </summary>
    public class AnnotationToolViewModel : ReactiveObject
    {
        private ReactiveCommand _rectCommand;
        private ReactiveCommand _ellipseCommand;
        private ReactiveCommand _lineCommand;
        private ReactiveCommand _freeDrawCommand;
        private ReactiveCommand _undoCommand;

        public ICommand RectCommand { get { return _rectCommand; } }
        public ICommand EllipseCommand { get { return _ellipseCommand; } }
        public ICommand LineCommand { get { return _lineCommand; } }
        public ICommand FreeDrawCommand { get { return _freeDrawCommand; } }
        public ICommand UndoCommand { get { return _undoCommand; } }

        private IDisposable _isEditingSub;
        private IDisposable _activeToolSub;

        private bool _lineActive, _ellipseActive, _rectActive, _freeDrawActive;
        public bool LineActive
        {
            get { return _lineActive; }
            set { this.RaiseAndSetIfChanged(ref _lineActive, value); }
        }
        public bool EllipseActive
        {
            get { return _ellipseActive; }
            set { this.RaiseAndSetIfChanged(ref _ellipseActive, value); }
        }
        public bool RectActive
        {
            get { return _rectActive; }
            set { this.RaiseAndSetIfChanged(ref _rectActive, value); }
        }
        public bool FreeDrawActive
        {
            get { return _freeDrawActive; }
            set { this.RaiseAndSetIfChanged(ref _freeDrawActive, value); }
        }

        private Nullable<AnnotationType> _annotationType;
        public Nullable<AnnotationType> Type
        {
            get { return _annotationType; }
            set { this.RaiseAndSetIfChanged(ref _annotationType, value); }
        }

        public void ClearAllActive()
        {
            RectActive = false;
            EllipseActive = false;
            FreeDrawActive = false;
            LineActive = false;
        }

        public AgentAnnotationViewModel AgentAnnotations
        {
            get;
            private set;
        }

        public AnnotationToolViewModel(AgentAnnotationViewModel agentAnnotations)
        {
            AgentAnnotations = agentAnnotations;
            _isEditingSub = AgentAnnotations.WhenAnyValue(p => p.IsEditing, p => p).Where(p => !p).Subscribe(_ => ClearAllActive());

            _activeToolSub = this.WhenAnyValue(p => p.RectActive, p => p.EllipseActive, p => p.FreeDrawActive, p => p.LineActive,
                (a, b, c, d) =>
                {
                    return !a && !b && !c && !d;
                }).Where(p => p).Subscribe(_ =>
                {
                    Type = null;
                });

            _undoCommand = new ReactiveCommand(
                agentAnnotations.WhenAny(p => p.ContainsAnnotationsForCurrentPage, p => p.Value).Select(p => p));
            _undoCommand.ObserveOn(RxApp.MainThreadScheduler).Subscribe(p =>
            {
                ClearAllActive();
                agentAnnotations.UndoAnnotation();
            });

            _rectCommand = new ReactiveCommand();
            _rectCommand.ObserveOn(RxApp.MainThreadScheduler).Subscribe(p =>
            {
                ClearAllActive();
                RectActive = true;
                Type = AnnotationType.Rectangle;
            });

            _ellipseCommand = new ReactiveCommand();
            _ellipseCommand.ObserveOn(RxApp.MainThreadScheduler).Subscribe(p =>
            {
                ClearAllActive();
                EllipseActive = true;
                Type = AnnotationType.Ellipse;
            });

            _lineCommand = new ReactiveCommand();
            _lineCommand.ObserveOn(RxApp.MainThreadScheduler).Subscribe(p =>
            {
                ClearAllActive();
                LineActive = true;
                Type = AnnotationType.Line;
            });

            _freeDrawCommand = new ReactiveCommand();
            _freeDrawCommand.ObserveOn(RxApp.MainThreadScheduler).Subscribe(p =>
            {
                ClearAllActive();
                FreeDrawActive = true;
                Type = AnnotationType.AdHoc;
            });
        }

        private bool _disposed;
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                Dispose(true);

                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _isEditingSub.Dispose();
                _activeToolSub.Dispose();
                _undoCommand.Dispose();
                _rectCommand.Dispose();
                _lineCommand.Dispose();
                _ellipseCommand.Dispose();
                _freeDrawCommand.Dispose();

                _isEditingSub = null;
                _activeToolSub = null;
                _undoCommand = null;
                _rectCommand = null;
                _lineCommand = null;
                _ellipseCommand = null;
                _freeDrawCommand = null;
            }
        }
    }

}
