using AppShopping.Libraries.Enums;
using AppShopping.Libraries.Helpers.MVVM;
using AppShopping.Models;
using AppShopping.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppShopping.ViewModels
{
    public abstract class EstablishmentViewModel:BaseViewModel
    {
        public string SearchWord { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand DetailCommand { get; set; }
        private readonly EstablishmentType _establishmentType;


        private List<Establishment> _establishments;
        public List<Establishment> Establishments
        {
            get { return _establishments; }
            set
            {
                SetProperty(ref _establishments, value);
            }
        }

        private readonly List<Establishment> _allEstablishments;

        public EstablishmentViewModel(EstablishmentType establishmentType)
        {
            _establishmentType = establishmentType;
            SearchCommand = new Command(Search);
            DetailCommand = new Command<Establishment>(Detail);

            var allEstablishment = new EstablishmentService().GetEstablishments();
            var allStores = allEstablishment.Where(a => a.Type == _establishmentType).ToList();

            Establishments = allStores;
            _allEstablishments = allStores;
        }

        private void Search()
        {
            Establishments = _allEstablishments.Where(a => a.Name.ToLower().Contains(SearchWord.ToLower())).ToList();
        }

        private void Detail(Establishment establishment)
        {
            string establishmentSerialized = JsonConvert.SerializeObject(establishment);
            Shell.Current.GoToAsync($"establishment/detail?establishmentSerialized={Uri.EscapeDataString(establishmentSerialized)}");
        }
    }
}
