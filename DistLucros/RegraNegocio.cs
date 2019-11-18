using System;
using DistLucros.Models;

namespace DistLucros
{
    ///<summary>
    ///Essa classe foi feita para centralizar toda a regra de negocio da aplicacao
    ///</summary>
    public static class RegraNegocio
    {
        ///<summary>
        ///Converte valores monetários formatados em reais no tipo double
        ///</summary>
        public static double RealEmDouble(string real)
        {
            return Double.Parse(real, System.Globalization.NumberStyles.Currency);
        }

        ///<summary>
        ///Converte double em valores monetários formatados em reais
        ///</summary>
        public static string DoubleEmReal(double valor)
        {
                return valor.ToString("C");
        }

        /// <summary><para>
        /// Obtem o peso por tempo de admissao utilizado no calculo de distribuicao de lucros.                   </para><para>
        /// Foi estabelecido um peso por tempo de admissão:                                                      </para><para>
        ///    Peso 1: Até 1 ano de casa;                                                                        </para><para>
        ///    Peso 2: Mais de 1 ano e menos de 3 anos;                                                          </para><para>
        ///    Peso 3: Acima de 3 anos e menos de 8 anos;                                                        </para><para>
        ///    Peso 5: Mais de 8 anos                                                                            </para>
        /// </summary>
        public static int GetPesoPorTempoDeAdmissao(Funcionario f)//peso por tempo de admissao
        {
            TimeSpan d = DateTime.Today - f.dataAdmissao;

            int anos = (int)(DateTime.Today.Subtract(f.dataAdmissao).Days / 365.2425);

            if( anos > 8 )
            {
                return 5;
            }
            if (anos > 3)
            {
                return 3;
            }
            if (anos > 1)
            {
                return 2;
            }
            return 1;
        }

        /// <summary>
        /// obtem numero de meses trabalhado no anterior.
        /// </summary>
        public static int GetQtdMesesTrabalhadosAnoAnterior(Funcionario f)
        {
            if (f.dataAdmissao.Year == DateTime.Today.Year) //entrou esse ano
            {
                return 0;
            }

            if (f.dataAdmissao.Year < DateTime.Today.Year -1 ) //entrou antes do ano passado
            {
                return 12; //trabalhou o ano passado todo, assumindo que todos os funcionarios que estao recebendo hoje continuam empregados
            }

            return 13 - f.dataAdmissao.Month; //entrou no meio do ano passado
        }


        /// <summary><para>
        /// Obtem o peso por area de atuacao utilizado no calculo de distribuicao de lucros.                    </para><para>
        /// Foi estabelecido um peso por área de atuação:                                                       </para><para>
        ///     Peso 1: Diretoria;                                                                              </para><para>
        ///     Peso 2: Contabilidade, Financeiro, Tecnologia;                                                  </para><para>
        ///     Peso 3: Serviços Gerais;                                                                        </para><para>
        ///     Peso 5: Relacionamento com o Cliente;                                                           </para>
        /// </summary>                                                                                          
        public static int GetPesoPorAreaDeAtuacao(Funcionario f)//peso por area de atuacao
        {            
            if (f.area.Equals("Diretoria"))
            {
                return 1;
            }
            if(f.area.Equals("Contabilidade") ||
                    f.area.Equals("Financeiro") ||
                    f.area.Equals("Tecnologia"))
            {
                return 2;
            }
            if(f.area.Equals("Serviços Gerais"))
            {
                return 3;
            }
            if (f.area.Equals("Relacionamento com o Cliente"))
            {
                return 5;
            }

            throw new Exception("Area de atuacao invalida!");
        }

        /// <summary>
        /// Obtem o valor do salario minimo utilizado na funcao de distribuicao de lucros.        
        /// </summary>        
        /// <remarks>
        /// Quando mudar o salario minimo ou caso disponibilizem algum servico com esse dado  eh so alterar aqui =]
        /// </remarks>
        public static double GetSalarioMinimo()
        {
            return 998;
        }

        /// <summary><para>
        /// Obtem o valor do peso por faixa salarial utilizado na funcao de distribuicao de lucros.  </para><para>
        /// Foi estabelecido um peso por faixa salarial e uma exceção para estagiários:              </para><para>
        ///    Peso 5: Acima de 8 salários mínimos;                                                  </para><para>
        ///    Peso 3: Acima de 5 salários mínimos e menor que 8 salários mínimos;                   </para><para>
        ///    Peso 2: Acima de 3 salários mínimos e menor que 5 salários mínimos;                   </para><para>
        ///    Peso 1: Todos os estagiários e funcionários que ganham até 3 salários mínimos;        </para>
        /// </summary>
        public static int GetPesoPorFaixaSalarial(Funcionario f)
        {
            if(f.cargo.Equals("Estagiário")) 
            {
                return 1;
            }

            double salarioMinimo = GetSalarioMinimo();            

            if (f.salarioBruto > salarioMinimo*8)
            {
                return 5;
            }
            if (f.salarioBruto > salarioMinimo * 5)
            {
                return 3;
            }
            if (f.salarioBruto > salarioMinimo * 3)
            {
                return 2;
            }
            return 1;            
        }

        /// <summary><para>
        /// Calcula o valor do bonus de acordo com a formula abaixo</para><para>
        /// Pelas regras estabelecidas a fórmula para se chegar ao bônus de cada funcionário é:      </para><para>
        ///                        (SB* PTA) + (SB* PAA)                                             </para><para>
        ///                    ______________________________  * 12 (Meses do Ano)                   </para><para>
        ///                                (PFS)                                                     </para><para></para>
        ///   Legenda SB: Salário Bruto 
        ///        PTA: Peso por tempo de admissão 
        ///        PAA: Peso por aréa de atuação 
        ///        PFS: Peso por faixa salarial
        /// </summary>
        public static double GetValorBonus(Funcionario f)
        {
            double valorBonus = 0;
            int PFS = GetPesoPorFaixaSalarial(f);

            valorBonus = f.salarioBruto * GetPesoPorTempoDeAdmissao(f);
            valorBonus += f.salarioBruto * GetPesoPorAreaDeAtuacao(f);
            valorBonus = (valorBonus / PFS);
            valorBonus = valorBonus * GetQtdMesesTrabalhadosAnoAnterior(f);
            

            //bonus negativo ninguem merece, neh champs? ai seria onus rs
            if (valorBonus < 0)
            {
                valorBonus = 0;
            }

            return valorBonus;
        }

    }
}
