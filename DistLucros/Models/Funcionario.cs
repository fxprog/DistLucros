using Newtonsoft.Json;
using System;

namespace DistLucros.Models
{
    ///<summary>
    ///Essa classe faz o parse e armazena os dados relevantes dos funcionarios que mais tarde serao utilizados no processamento
    ///</summary>
    public class Funcionario
    {
        public int matricula { get; set; }
        public string nome { get; set; }
        public string area { get; set; }
        public string cargo { get; set; }
        [JsonProperty("salario_bruto")]
        public string salarioBrutoReais { get; set; }
        [JsonProperty("data_de_admissao")]
        public DateTime dataAdmissao { get; set; }
        [JsonIgnore]
        public double salarioBruto { get; set; }

        public Funcionario(int matricula, string nome, string area, string cargo, string salarioBrutoReais, DateTime dataAdmissao)
        {            
            string erro = verificaErros(matricula, nome, area, cargo, salarioBrutoReais, dataAdmissao);
            if(!erro.Equals(""))
            {
                throw new Exception(erro);
            }
            this.matricula = matricula;            
            this.nome = nome;
            this.area = area;
            this.cargo = cargo;
            this.dataAdmissao = dataAdmissao;
            this.salarioBrutoReais = salarioBrutoReais;
            this.salarioBruto = RegraNegocio.RealEmDouble(salarioBrutoReais);
        }

        public override string ToString()
        {
            if (nome == null)
                nome = "";

            return "matricula: " + matricula.ToString() + " nome: " + nome.ToString();
        }

        /// <summary>
        /// Feito esse metodo de verificao externo para nao poluir o construtor com verificacoes
        /// </summary>
        private string verificaErros(int matricula, string nome, string area, string cargo, string salarioBrutoReais, DateTime dataAdmissao)
        {
            double salarioBruto;
            if (salarioBrutoReais == null || !salarioBrutoReais.Equals(""))
            {
                salarioBruto = RegraNegocio.RealEmDouble(salarioBrutoReais);
            }
            else
            {
                return "Erro ao processar o funcionario matricula: " + matricula.ToString() + " nome: " + nome + ". Salario bruto nao pode ser vazio ou nulo";
            }

            if (nome == null || area == null || cargo == null || matricula < 0 || nome.Equals("") || area.Equals("") || cargo.Equals(""))
            {
                return "Erro ao processar o funcionario matricula: " + matricula.ToString() + " nome: " + nome + ", area: " + area + ", cargo: " + cargo + ". Existem campos sem preenchimento adequado";
            }

            if (dataAdmissao > DateTime.Today)
            {
                return "Erro ao processar o funcionario matricula: " + matricula.ToString() + " nome: " + nome + ". Data de admissao nao pode ser maior do que a data atual.";
            }

            if (salarioBruto < 0)
            {
                return "Erro ao processar o funcionario matricula: " + matricula.ToString() + " nome: " + nome + ". Seu salario eh nao pode ser negativo.";
            }

            return "";
        }
    }
}
