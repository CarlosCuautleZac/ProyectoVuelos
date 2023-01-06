using SalidasAerolinea.Models;
using SalidasAerolinea.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SalidasAerolinea.ViewModels
{
    public class AerolineaViewModel : INotifyPropertyChanged
    {
        VueloService vueloService = new VueloService();

        public ObservableCollection<Vuelo>? Vuelos { get; set; }

        public AerolineaViewModel()
        {
            CargarVuelos();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            CargarVuelos();
        }

        private async void CargarVuelos()
        {

            if (Vuelos == null)
                Vuelos = new();

            Vuelos = new ObservableCollection<Vuelo>(await vueloService.Get());
            //Vuelos.Clear();

           //var vuelosentrantes = await vueloService.Get();

            //foreach (var v in vuelosentrantes)
            //{
            //    Vuelos.Add(v);
            //}

            Actualizar(nameof(Vuelos));
        }

        public void Actualizar(string nombre = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
