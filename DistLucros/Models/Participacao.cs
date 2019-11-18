using Newtonsoft.Json;

namespace DistLucros.Models
{
    ///<summary>
    ///Essa classe funciona como container na resposta, armazenando a participacao de cada um dos funcionarios
    ///</summary>
    public class Participacao
    {
        public Participacao(int matricula, string nome, double valorParticipacao)
        {
            this.matricula = matricula.ToString("D7");
            this.nome = nome;
            this.valorParticipacao = valorParticipacao;
            
            this.valorParticipacaoReais = RegraNegocio.DoubleEmReal(valorParticipacao);
        }
        public string matricula { get; set; }
        
        public string nome { get; set; }
        [JsonProperty("valor_da_participacao")]
        public string valorParticipacaoReais { get; set; }
        [JsonIgnore]
        public double valorParticipacao { get; set; }
    }
}
