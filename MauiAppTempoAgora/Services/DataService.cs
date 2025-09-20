using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave= "31c207933e4f803c1b3387b25c4e58c7";

            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&appid={chave}&lang=pt_br&units=metric";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();
                    var rascunho = JObject.Parse(json);

                    DateTime sunrise = DateTimeOffset.FromUnixTimeSeconds((long)rascunho["sys"]["sunrise"]).ToLocalTime().DateTime;
                    DateTime sunset = DateTimeOffset.FromUnixTimeSeconds((long)rascunho["sys"]["sunset"]).ToLocalTime().DateTime;

                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min = (double?)rascunho["main"]["temp_min"],
                        temp_max = (double?)rascunho["main"]["temp_max"],
                        speed = (double?)rascunho["wind"]["speed"],
                        visibility = (int?)rascunho["visibility"],
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString(),
                    };
                }

            }

            return t;
        }
    }
}
