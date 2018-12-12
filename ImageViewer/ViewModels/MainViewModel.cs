using ImageViewer.Models;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ImageViewer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region fields

        private readonly DraggedImageModel _draggedImageModel;
        private ImagesListItemViewModel _selectedImage;
        private int _selectedImageIndex;
        private DelegateCommand _showPreviousImageCommand;
        private DelegateCommand _showNextImageCommand;
        private DelegateCommand<DragEventArgs> _dropImageCommand;
        private bool _isDropAllowed = true;
        private Visibility _buttonsVisibility = Visibility.Collapsed;

        #endregion

        #region properties

        public ObservableCollection<ImagesListItemViewModel> ImageCollection { get; set; }
        public Visibility DropPanelLabelVisibility => ImageCollection.Any() ? Visibility.Collapsed : Visibility.Visible;

        public Visibility ButtonsVisibility
        {
            get => _buttonsVisibility;
            set
            {
                _buttonsVisibility = value;
                RaisePropertyChanged(nameof(ButtonsVisibility));
            }
        }

        public bool IsDropAllowed
        {
            get => _isDropAllowed;
            set
            {
                _isDropAllowed = value;
                RaisePropertyChanged(nameof(IsDropAllowed));
            }
        }

        #endregion

        #region commands

        public DelegateCommand<DragEventArgs> DropImageCommand => _dropImageCommand ??
                                                                  (_dropImageCommand = new DelegateCommand<DragEventArgs>(DropImageCommandExecute));

        public DelegateCommand ShowPreviousImageCommand => _showPreviousImageCommand ??
                                                           (_showPreviousImageCommand = new DelegateCommand(ShowPreviousImageCommandExecute));

        public DelegateCommand ShowNextImageCommand => _showNextImageCommand ??
                                                       (_showNextImageCommand = new DelegateCommand(ShowNextImageCommandExecute));



        #endregion

        public MainViewModel()
        {
            _draggedImageModel = new DraggedImageModel();
            ImageCollection = new ObservableCollection<ImagesListItemViewModel>();
            ImageCollection.CollectionChanged += (sender, args) =>
            {
                RaisePropertyChanged(nameof(ImageCollection));
                RaisePropertyChanged(nameof(DropPanelLabelVisibility));
            };
        }

        #region commands executors

        private void DropImageCommandExecute(DragEventArgs args)
        {
            var parameter = args.Data.GetData(DataFormats.FileDrop);

            if (_draggedImageModel.IsDraggedFileImage(parameter))
            {
                var droppedImagesPaths = _draggedImageModel.GetDroppedImagesPaths(parameter);

                foreach (var imagesPath in droppedImagesPaths)
                {
                    var addedImageItem = new ImagesListItemViewModel { Path = imagesPath };

                    addedImageItem.AllowDropChanged += isDropAllowed => IsDropAllowed = isDropAllowed;
                    addedImageItem.SelectedImageChanged += image =>
                    {
                        _selectedImage = image;
                        _selectedImageIndex = ImageCollection.IndexOf(_selectedImage);
                    };
                    addedImageItem.SrollButtonsVisibilityChanged += visibility => ButtonsVisibility = visibility;
                    addedImageItem.ImagesVisibilityChanged += OnImagesVisibilityChanged;

                    ImageCollection.Add(addedImageItem);
                }

            }
        }

        private void ShowPreviousImageCommandExecute()
        {
            if (_selectedImageIndex > 1)
            {
                _selectedImage.DecreaseImage();
                ImageCollection.ElementAt(_selectedImageIndex - 1).IncreaseImage();
            }
        }

        private void ShowNextImageCommandExecute()
        {
            if (_selectedImageIndex < ImageCollection.Count - 2)
            {
                _selectedImage.DecreaseImage();
                ImageCollection.ElementAt(_selectedImageIndex + 1).IncreaseImage();
            }
        }

        #endregion


        #region event handlers

        private void OnImagesVisibilityChanged(Visibility visibility, ImagesListItemViewModel unchangedImage)
        {
            foreach (var image in ImageCollection)
            {
                if (!image.Equals(unchangedImage))
                {
                    image.ImageVisibility = visibility;
                }
            }
        }

        #endregion
    }
}
