using Prism.Commands;
using System;
using System.Windows;
using ImageViewer.Helpers;

namespace ImageViewer.ViewModels
{
    public class ImagesListItemViewModel : BaseViewModel
    {
        private Visibility _imageVisibility = Visibility.Visible;
        private double _imageHeight;
        private double _imageWidth;
        private DelegateCommand _decreaseImageCommand;
        private DelegateCommand<RoutedEventArgs> _mouseDoubleClickCommand;
        
        public string Path { get; set; }
        public Action<bool> AllowDropChanged;
        public Action<Visibility> SrollButtonsVisibilityChanged;
        public Action<ImagesListItemViewModel> SelectedImageChanged;
        public Action<Visibility, ImagesListItemViewModel> ImagesVisibilityChanged;

        public double ImageHeight
        {
            get => _imageHeight;
            set
            {
                _imageHeight = value;
                RaisePropertyChanged(nameof(ImageHeight));
            }
        }

        public double ImageWidth
        {
            get => _imageWidth;
            set
            {
                _imageWidth = value;
                RaisePropertyChanged(nameof(ImageWidth));
            }
        }

        public Visibility ImageVisibility
        {
            get => _imageVisibility;
            set
            {
                _imageVisibility = value;
                RaisePropertyChanged(nameof(ImageVisibility));
            }
        }

        public DelegateCommand DecreaseImageCommand => _decreaseImageCommand ??
                                                       (_decreaseImageCommand = new DelegateCommand(DecreaseImageCommandExecute));

        public DelegateCommand<RoutedEventArgs> MouseDoubleClickCommand => _mouseDoubleClickCommand ??
                                                                           (_mouseDoubleClickCommand = new DelegateCommand<RoutedEventArgs>(MouseDoubleClickCommandExecute));

        public ImagesListItemViewModel()
        {
            ImageWidth = ActualSizeObserver.DefaultWidth;
            ImageHeight = ActualSizeObserver.DefaultHeight;
        }

        public void IncreaseImage()
        {
            ImageWidth = ActualSizeObserver.IncreasedImageWidth;
            ImageHeight = ActualSizeObserver.IncreasedImageHeight;
            
            AllowDropChanged?.Invoke(false);
            SelectedImageChanged?.Invoke(this);
            SrollButtonsVisibilityChanged?.Invoke(Visibility.Visible);
            ImagesVisibilityChanged?.Invoke(Visibility.Collapsed, this);
        }
        public void DecreaseImage()
        {
            ImageWidth = ActualSizeObserver.DefaultWidth;
            ImageHeight = ActualSizeObserver.DefaultHeight;

            AllowDropChanged?.Invoke(true);
            SrollButtonsVisibilityChanged?.Invoke(Visibility.Hidden);
            ImagesVisibilityChanged?.Invoke(Visibility.Visible, this);
        }
        
        private void DecreaseImageCommandExecute()
        {
            DecreaseImage();
        }

        private void MouseDoubleClickCommandExecute(RoutedEventArgs args)
        {
            IncreaseImage();
        }
    }
}