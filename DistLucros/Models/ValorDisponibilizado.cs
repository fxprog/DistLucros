using System;
using Newtonsoft.Json;

namespace DistLucros.Models
{
    public class ValorDisponibilizado
    {
        public ValorDisponibilizado(string valorDisponibilizadoReais)
        {
            this.valorDisponibilizadoReais = valorDisponibilizadoReais ?? throw new ArgumentNullException(nameof(valorDisponibilizadoReais));
            this.valorDisponibilizado = RegraNegocio.RealEmDouble(valorDisponibilizadoReais);
        }

        [JsonProperty("valor_disponibilizado")]
        public string valorDisponibilizadoReais { get; set; }
        [JsonIgnore]
        public double valorDisponibilizado { get; set;}


    }
}
