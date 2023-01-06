using AeropuertoMovil.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace AeropuertoMovil.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}