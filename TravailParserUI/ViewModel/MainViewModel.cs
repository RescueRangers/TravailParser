using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using TravailParserUI.Model;

namespace TravailParserUI.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;

        /// <summary>
        /// The <see cref="TrvArray" /> property's name.
        /// </summary>
        public const string TrvArrayPropertyName = "TrvArray";

        private DataTable _trvData;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        

        public DataTable TrvData
        {
            get => _trvData;
            set => Set(ref _trvData, value);
        }

        public ICommand OpenFileCommand { get; set; }
        public ICommand ExportToExcelCommand { get; set; }
        public ICommand LoadedCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dialogService = new DialogService();

            LoadCommands();

            
        }

        private void LoadCommands()
        {
            OpenFileCommand = new RelayCommand(OpenFile, () => true);
            LoadedCommand = new RelayCommand(Loaded, () => true);
            ExportToExcelCommand = new RelayCommand(ExportToExcel, () => true);
        }

        private void ExportToExcel()
        {
            var setting = new SaveFileDialogSettings
            {
                DefaultExt = ".xlsx",
                AddExtension = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = "WorkTime.xlsx",
                Filter = "Excel files (*.xlsx)|*.xlsx",
                Title = "Save travail file as an excel sheet",
                CheckFileExists = false
            };
            
            if (_dialogService.ShowSaveFileDialog(this, setting) == true)
            {
                _dataService.SaveData(TrvData, setting.FileName);
            }
        }

        private void Loaded()
        {
            var startupArguments = Environment.GetCommandLineArgs();

            if (startupArguments.Length > 1)
            {
                OpenTrvFile(startupArguments[1]);
            }
        }

        private void OpenFile()
        {
            try
            {
                var openFileSetting = new OpenFileDialogSettings
                {
                    Multiselect = false,
                    Filter = "trv files (*.trv)|*.trv"
                };
                var results = _dialogService.ShowOpenFileDialog(this, openFileSetting);

                if (results == true)
                {
                    OpenTrvFile(openFileSetting.FileName);
                }
            }
            catch (Exception e)
            {
                _dialogService.ShowMessageBox(this, e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenTrvFile(string filePath)
        {
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        _dialogService.ShowMessageBox(this, error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        // Report error here
                        return;
                    }

                    TrvData = item;
                }, filePath);
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}