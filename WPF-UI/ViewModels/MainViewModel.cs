using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF_UI.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {

        public MainViewModel()
        {
            searchBtn = new RelayCommand(search);
        }

        public ICommand searchBtn { get; }

        [ObservableProperty]
        public string barcode;


        private void search()
        {

        }


    }
}
