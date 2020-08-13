using System;
using System.Data;
using System.Windows;
using Catel.Data;
using TravailParserUI.Model;

namespace TravailCatel.ViewModels
{
    using Catel.MVVM;
    using System.Threading.Tasks;

    public class MainWindowViewModel : ViewModelBase
    {
        private DataService _dataService = new DataService();

        [Model]
        public DataTable TrvTable
        {
            get => GetValue<DataTable>(TrvTableProperty);
            set => SetValue(TrvTableProperty, value);
        }

        public static readonly PropertyData TrvTableProperty = RegisterProperty(nameof(TrvTable), typeof(DataTable), null);


        public Command OpenTrvCommand { get; private set; }


        public Command OnLoadedCommand { get; private set; }

        // TODO: Move code below to constructor
        // TODO: Move code above to constructor

        private bool OnOnLoadedCommandCanExecute()
        {
            return true;
        }

        private void OnOnLoadedCommandExecute()
        {
            var startupArguments = Environment.GetCommandLineArgs();

            if (startupArguments.Length > 1)
            {
                OnOpenTrvCommandExecute();
            }
        }

        
        private void OnOpenTrvCommandExecute()
        {
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        //_dialogService.ShowMessageBox(this, error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        // Report error here
                        return;
                    }

                    TrvTable = item;
                }, @"\\Vt25techtex118-\c\Documents and Settings\Administrator\Application Data\Lectra\worklib\VT25TECHTEX118-_170818.TRV");
        }

        public MainWindowViewModel()
        {
            OpenTrvCommand = new Command(OnOpenTrvCommandExecute, ()=> true);
            OnLoadedCommand = new Command(OnOnLoadedCommandExecute, OnOnLoadedCommandCanExecute);

        }

        public override string Title { get { return "TravailCatel"; } }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
