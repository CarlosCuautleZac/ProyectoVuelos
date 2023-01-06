using ArepouertoMovil.Models;
using ArepouertoMovil.Services;
using ArepouertoMovil.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ArepouertoMovil.ViewModels
{
    public class VuelosViewModel : INotifyPropertyChanged
    {

        #region Comandos
        public Command EnviarVueloCommand { get; set; }
        public Command VerDetallesVueloCommand { get; set; }
        public Command VerNuevoVueloCommand { get; set; }
        public Command EliminarCommand { get; set; }
        public Command VerEditarCommand { get; set; }
        public Command EditarCommand { get; set; }
        #endregion

        #region Propiedades
        public Vuelo Vuelo { get; set; }
        public Vuelo Clon { get; set; }
        public Observacion Observacion { get; set; }
        public List<Observacion> Observaciones { get; set; }
        public string Error { get; set; } = "";
        public DateTime Fecha { get; set; } = DateTime.Now;
        public TimeSpan Hora { get; set; }
        public ObservableCollection<Vuelo> Vuelos { get; set; }
        public DateTime MinDate { get; set; } = DateTime.Now.Date;
        #endregion

        #region Objetos
        VueloService vueloService;
        ObservacionService observacionService;
        DetallesView detallesView;
        AgregarVueloView agregarView;
        EditarView editarView;
        #endregion

        public VuelosViewModel()
        {
            //Comandos
            EnviarVueloCommand = new Command(EnviarVuelo);
            VerDetallesVueloCommand = new Command<Vuelo>(VerDetalleVuelo);
            VerNuevoVueloCommand = new Command(VerNuevoVuelo);
            EliminarCommand = new Command(Eliminar);
            VerEditarCommand = new Command(VerEditar);
            EditarCommand = new Command(Editar);

            //Services
            vueloService = new VueloService();
            vueloService.Error += VueloService_Error;
            observacionService = new ObservacionService();

            //Listas
            Vuelo = new Vuelo() { Fecha = DateTime.Now.Date };
            Observacion = new Observacion();

            //Views
            detallesView = new DetallesView() { BindingContext = this };
            agregarView = new AgregarVueloView() { BindingContext = this };
            editarView = new EditarView() { BindingContext = this };

            //Metodos para el llenado de colecciones
            LlenarVuelos();
            LLenarObservaciones();

            Device.StartTimer(new TimeSpan(0, 0, 5), () =>
            {
                // do something every 5 seconds
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        LlenarVuelos();

                        if (Observacion == null)
                            LLenarObservaciones();
                    }
                });
                return true; 
            });
        }

        private async void Editar()
        {
            //Validar bien machin asi bien padre bien riko

            try
            {
                if (Vuelo != null && Observacion != null)
                {
                    Clon.Fecha = Fecha.Date;
                    Clon.Fecha = Clon.Fecha.Add(Hora);
                    Clon.Observacion = Observacion.Observacion1;
                    var editado = await vueloService.Update(Clon);
                    if (editado)
                    {
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                        LlenarVuelos();
                    }
                }
            }
            catch(Exception m)
            {
                await App.Current.MainPage.DisplayAlert("Alert", m.Message, "OK");
            }
        }

        private async void VerEditar()
        {
            try
            {
                if (Vuelo != null)
                {
                    Clon = new Vuelo()
                    {
                        Aerolinea = Vuelo.Aerolinea,
                        Destino = Vuelo.Destino,
                        Fecha = Vuelo.Fecha,
                        Id = Vuelo.Id,
                        Observacion = Vuelo.Observacion,
                        Puerta = Vuelo.Puerta
                    };

                    Fecha = Clon.Fecha.Date;
                    Hora = Clon.Fecha.TimeOfDay;
                    Observacion = new Observacion();
                    Observacion.Observacion1 = Clon.Observacion;
                    Actualizar("");
                    await Application.Current.MainPage.Navigation.PushAsync(editarView);
                }
            }
            catch(Exception)
            {

            }
        }

        private async void Eliminar()
        {
            try
            {
                if (Vuelo != null)
                {
                    var eliminate = vueloService.Delete(Vuelo).Result;
                    LlenarVuelos();
                    await App.Current.MainPage.Navigation.PopAsync();

                }
            }
            catch(Exception m)
            {
                await App.Current.MainPage.DisplayAlert("Alert", m.Message, "OK");
            }
        }

        private async void VerNuevoVuelo()
        {
            try
            {
                Vuelo = new Vuelo();
                Hora = DateTime.Now.TimeOfDay;
                Actualizar("");
                await Application.Current.MainPage.Navigation.PushAsync(agregarView);
            }
            catch (Exception)
            {

            }
        }

        private async void VerDetalleVuelo(Vuelo vuelo)
        {
            try
            {
                Vuelo = vuelo;
                Hora = vuelo.Fecha.TimeOfDay;
                Actualizar("");
                await Application.Current.MainPage.Navigation.PushAsync(detallesView);
            }
            catch (Exception m)
            {
                var error = m.Message;
            }
        }

        private async void VueloService_Error(List<string> errores)
        {
            Error = "";

            foreach (var e in errores)
            {
                Error += e + Environment.NewLine;
            }
            await App.Current.MainPage.DisplayAlert("Alert", Error, "OK");
        }

        private async void EnviarVuelo()
        {
            try
            {
                //Validar bien machin asi bien padre bien riko
                if (Vuelo != null && Observacion != null)
                {
                    Vuelo.Fecha = Fecha.Date;
                    Vuelo.Fecha = Vuelo.Fecha.Add(Hora);
                    Vuelo.Fecha = Vuelo.Fecha.AddHours(2);
                    Vuelo.Observacion = Observacion.Observacion1;
                    var enviado = vueloService.Insert(Vuelo).Result;

                    if (enviado)
                    {
                        LlenarVuelos();
                        await App.Current.MainPage.Navigation.PopAsync();
                    }
                }
                
            }
            catch(Exception m)
            {
                await App.Current.MainPage.DisplayAlert("Alert", m.Message, "OK");
            }

        }

        private void LLenarObservaciones()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                Observaciones = new List<Observacion>(observacionService.Get().Result);
            Actualizar("");
        }

        private void LlenarVuelos()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                Vuelos = new ObservableCollection<Vuelo>(vueloService.Get().Result);
            Actualizar("");
        }

        public void Actualizar(string nombre = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
