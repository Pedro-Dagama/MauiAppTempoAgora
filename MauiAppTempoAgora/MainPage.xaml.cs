using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System.Threading.Tasks;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();
        }


        private async void Button_Clicked_Previsao(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);
                    if (t != null)
                    {
                        string dados_previsao = "";
                        dados_previsao = $"Latitude:{t.lat} \n" +
                            $"Longitude: {t.lon}\n" +
                              $"Nascer do Sol: {t.sunrise}\n" +
                                $"Por do Sol: {t.sunset}\n" +
                                  $"Temperatura Max: {t.temp_max}\n" +
                                    $"Temperatura Min: {t.temp_min}\n"+
                                     $"Descrição do Clima: {t.description}\n"+
                                     $"Velocidade do Vento: {t.speed}\n"+
                                     $"Visibilidade: {t.visibility}";

                        lbl_res.Text = dados_previsao;

                        string mapa = $"https://embed.windy.com/embed.html?" +
                            $"type=map&location=coordinates&metricRain=mm&metricTemp=°C" +
                            $"&metricWind=km/h&zoom=5&overlay=wind&product=ecmwf&level=surface" +
                            $"&lat={t.lat.ToString().Replace(",",".")}lon={t.lon.ToString().Replace(",",".")}";

                        wv_mapa.Source = mapa;
                    }
                    else
                    {
                        lbl_res.Text = "sem dados de previsao";
                    }
                }
                else
                {
                    lbl_res.Text = "preencha a cidade";
                }
            }
            catch (Exception ex)
            {

                await DisplayAlert("ops", ex.Message, ("ok"));
            }
        }

        private async void Button_Clicked_localizacao(object sender, EventArgs e)
        {
            try
            {
                GeolocationRequest request = 
                    new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));

                Location? local = await Geolocation.Default.GetLocationAsync(request);

                if (local != null) {

                    string local_disp = $"Latitude:{local.Latitude} \n" +
                                        $"Longitude{local.Longitude} ";

                    lbl_localizacao.Text = local_disp;

                    GetCidade(local.Latitude, local.Longitude);
                }
                else
                {
                    lbl_localizacao.Text = "Nenhuma localização";
                }

            }catch(FeatureNotSupportedException fsnsEx){
                await DisplayAlert("Error: Dispositivo não suporta", fsnsEx.Message, "ok");
            }catch(FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("Error: Localização desabilitada", fneEx.Message, "ok");
            }catch(PermissionException pmEx)
            {
                await DisplayAlert("Error: Permissão da localização", pmEx.Message, "ok");
            }catch(Exception ex)
            {
                await DisplayAlert("Ocorreu algum erro", ex.Message, "ok");
            }

        }

        private async void GetCidade(double lat, double lon)
        {
            try
            {
                IEnumerable<Placemark> places = await Geocoding.Default.GetPlacemarksAsync(lat, lon);
                Placemark? place = places.FirstOrDefault();

                if (place != null)
                {
                    txt_cidade.Text = place.Locality;

                }

            }
            catch (Exception ex) {
                await DisplayAlert("Erro: obtensão do nome da cidade", ex.Message, "ok");
            }
        }
    }

        
}
