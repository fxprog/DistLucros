using System;
using System.Collections.Generic;
using System.Linq;

namespace DistLucros.Models
{
    ///<summary>
    ///Essa classe funciona como container para a devolucao da distribuicao de lucros, calculada pelas regras de negocio
    ///o parametro enforceBudget serve para forcar a distribuicao total do dinheiro disponibilizado, num approach alternativo do webservice
    ///</summary>
    public class DistribuicaoLucros
    {
        public List<Participacao> participacoes { get; set; }
        public int total_de_funcionarios { get; set; }
        public string total_distribuido { get; set; }
        public string total_disponibilizado { get; set; }
        public string saldo_total_disponibilizado { get; set; }

        public DistribuicaoLucros(IEnumerable<Funcionario> l, double totalDisponibilizado, bool enforceBudget = false)
        {
            double totalDistribuido = 0;
            total_de_funcionarios = l.Count();
            participacoes = new List<Participacao>();

            if (total_de_funcionarios == 0)
            {
                throw new Exception("Não existem funcionarios para distribuir o lucro!");
            }

            if (totalDisponibilizado <= 0)
            {
                throw new Exception("Não há lucro a ser distribuído!");
            }

            foreach (Funcionario f in l)
            {
                try
                {

                    Participacao p = new Participacao(f.matricula, f.nome, RegraNegocio.GetValorBonus(f));
                    participacoes.Add(p);
                    totalDistribuido += p.valorParticipacao;
                }
                catch
                {
                    throw new Exception("Erro ao processar o funcionario " + f.ToString());
                }
            }

            if (enforceBudget)
            {
                //tendo o que eh definido pela regra, agora eh necessario caber dentro do budget
                if (totalDistribuido > 0) //divisao por zero nao rola
                {
                    double proporcao = totalDisponibilizado / totalDistribuido;

                    totalDistribuido = 0;
                    foreach (Participacao p in participacoes)
                    {
                        p.valorParticipacao = p.valorParticipacao * proporcao;
                        p.valorParticipacaoReais = RegraNegocio.DoubleEmReal(p.valorParticipacao);
                        totalDistribuido += p.valorParticipacao;
                    }
                }
                else
                {
                    throw new Exception("Erro ao calcular o valor da participação");
                }
            }
            total_disponibilizado = RegraNegocio.DoubleEmReal(totalDisponibilizado);
            total_distribuido = RegraNegocio.DoubleEmReal(totalDistribuido);
            saldo_total_disponibilizado = RegraNegocio.DoubleEmReal(totalDisponibilizado - totalDistribuido);
        }

    }
}
