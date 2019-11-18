using System;
using Xunit;
using DistLucros.Models;
using DistLucros;
using System.Collections.Generic;

namespace TesteUnitarioDistLucros
{
    
    public class RegraNegocioTests
    {
        static List<Funcionario> funcionarios;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
        public RegraNegocioTests()
        {
            funcionarios = new List<Funcionario>();
            funcionarios.Add(new Funcionario(0, 
                                            "Charlie Harper", 
                                            "Relacionamento com o Cliente", 
                                            "Atendente", 
                                            "R$ 989,00", 
                                            new DateTime(2018,09,21)));
        }


        [Fact]
        public void RealEmDouble_CaminhoFeliz_RetornoOk()
        {
            string real = "R$ 100.000,01";
            
            Assert.Equal(RegraNegocio.RealEmDouble(real),(double)100000.01);
        } 


        [Fact]
        public void DoubleEmReal_CaminhoFeliz_RetornoOk()
        {
            double valor = 100000.01d;

            Assert.Equal("R$ 100.000,01", RegraNegocio.DoubleEmReal(valor));
        }

        
        // Obtem o peso por tempo de admissao utilizado no calculo de distribuicao de lucros.               
        // Foi estabelecido um peso por tempo de admissão:                                                  
        //    Peso 1: Até 1 ano de casa;                                                                    
        //    Peso 2: Mais de 1 ano e menos de 3 anos;                                                      
        //    Peso 3: Acima de 3 anos e menos de 8 anos;                                                    
        //    Peso 5: Mais de 8 anos                                                                        
       
        [Fact]
        public void GetPesoPorTempoDeAdmissao_CaminhoFeliz_RetornoOk()
        {
            Funcionario f = funcionarios[0];
            TimeSpan d = DateTime.Today - f.dataAdmissao;

            int anos = (int)(DateTime.Today.Subtract(f.dataAdmissao).Days / 365.2425);

            if (anos > 8)
            {
                Assert.Equal(5,RegraNegocio.GetPesoPorTempoDeAdmissao(f));
            }
            if (anos > 3)
            {
                Assert.Equal(3, RegraNegocio.GetPesoPorTempoDeAdmissao(f));
            }
            if (anos > 1)
            {
                Assert.Equal(2, RegraNegocio.GetPesoPorTempoDeAdmissao(f));
            }
            Assert.Equal(1, RegraNegocio.GetPesoPorTempoDeAdmissao(f));
        }


        [Fact]
        public void GetQtdMesesTrabalhadosAnoAnterior_CaminhoFeliz_RetornoOk()
        {
            Funcionario f = funcionarios[0];
            if (f.dataAdmissao.Year == DateTime.Today.Year) //entrou esse ano
            {
                Assert.Equal(0, RegraNegocio.GetQtdMesesTrabalhadosAnoAnterior(f));
            }

            if (f.dataAdmissao.Year < DateTime.Today.Year - 1) //entrou antes do ano passado
            {
                Assert.Equal(12, RegraNegocio.GetQtdMesesTrabalhadosAnoAnterior(f));
            }
            Assert.Equal((13 - f.dataAdmissao.Month), RegraNegocio.GetQtdMesesTrabalhadosAnoAnterior(f));
        }


        // Obtem o peso por area de atuacao utilizado no calculo de distribuicao de lucros.                   
        // Foi estabelecido um peso por área de atuação:                                                      
        //     Peso 1: Diretoria;                                                                             
        //     Peso 2: Contabilidade, Financeiro, Tecnologia;                                                 
        //     Peso 3: Serviços Gerais;                                                                       
        //     Peso 5: Relacionamento com o Cliente;                                                          
        [Fact]
        public void GetPesoPorAreaDeAtuacao_Diretoria_Retorno1()//peso por area de atuacao
        {
            Funcionario f = funcionarios[0];
            f.area = "Diretoria";
            
            Assert.Equal(1, RegraNegocio.GetPesoPorAreaDeAtuacao(f));            
        }

        [Fact]
        public void GetPesoPorAreaDeAtuacao_Contabilidade_Retorno2()
        {
            Funcionario f1 = funcionarios[0];
            f1.area = "Contabilidade";
            Assert.Equal(2, RegraNegocio.GetPesoPorAreaDeAtuacao(f1));
        }

        [Fact]
        public void GetPesoPorAreaDeAtuacao_Financeiro_Retorno2()
        {
            Funcionario f2 = funcionarios[0];
            f2.area = "Financeiro";
            Assert.Equal(2, RegraNegocio.GetPesoPorAreaDeAtuacao(f2));
        }

        [Fact]
        public void GetPesoPorAreaDeAtuacao_Tecnologia_Retorno2()
        {
            Funcionario f3 = funcionarios[0];
            f3.area = "Tecnologia";
            Assert.Equal(2, RegraNegocio.GetPesoPorAreaDeAtuacao(f3));
        }

        [Fact]
        public void GetPesoPorAreaDeAtuacao_ServicosGerais_Retorno3()//peso por area de atuacao
        {
            Funcionario f = funcionarios[0];
            f.area = "Serviços Gerais";
            Assert.Equal(3, RegraNegocio.GetPesoPorAreaDeAtuacao(f));
        }

        [Fact]
        public void GetPesoPorAreaDeAtuacao_RelacionamentoCliente_Retorno5()//peso por area de atuacao
        {
            Funcionario f = funcionarios[0];
            f.area = "Relacionamento com o Cliente";
            Assert.Equal(5, RegraNegocio.GetPesoPorAreaDeAtuacao(f));
        }

        [Fact]
        public void GetSalarioMinimo_CaminhoFeliz_RetornaValor()
        {
            double valorAtualSalarioMinimo = 998.0;
            Assert.Equal(valorAtualSalarioMinimo, RegraNegocio.GetSalarioMinimo());
        }


       // Obtem o valor do peso por faixa salarial utilizado na funcao de distribuicao de lucros.  
       // Foi estabelecido um peso por faixa salarial e uma exceção para estagiários:              
       //    Peso 5: Acima de 8 salários mínimos;                                                  
       //    Peso 3: Acima de 5 salários mínimos e menor que 8 salários mínimos;                   
       //    Peso 2: Acima de 3 salários mínimos e menor que 5 salários mínimos;                   
       //    Peso 1: Todos os estagiários e funcionários que ganham até 3 salários mínimos;        
        [Fact]
        public void GetPesoPorFaixaSalarial_Estagiario_Retorno1()
        {
            Funcionario f = funcionarios[0];
            f.cargo = "Estagiário";
            Assert.Equal(1, RegraNegocio.GetPesoPorFaixaSalarial(f));
        }

        [Fact]
        public void GetPesoPorFaixaSalarial_MaisDe8Salarios_Retorno5()
        {
            Funcionario f = funcionarios[0];
            double salarioMinimo = RegraNegocio.GetSalarioMinimo();
            f.salarioBruto = salarioMinimo * 8 + 1;
            Assert.Equal(5, RegraNegocio.GetPesoPorFaixaSalarial(f));
        }

        [Fact]
        public void GetPesoPorFaixaSalarial_5a8salarios_Retorno3()
        {
            Funcionario f = funcionarios[0];
            double salarioMinimo = RegraNegocio.GetSalarioMinimo();
            f.salarioBruto = salarioMinimo * 5 + 1;
            Assert.Equal(3, RegraNegocio.GetPesoPorFaixaSalarial(f));
        }

        [Fact]
        public void GetPesoPorFaixaSalarial_3a5salarios_Retorno2()
        {
            Funcionario f = funcionarios[0];
            double salarioMinimo = RegraNegocio.GetSalarioMinimo();
            f.salarioBruto = salarioMinimo * 3 + 1;
            Assert.Equal(2, RegraNegocio.GetPesoPorFaixaSalarial(f));
        }

        [Fact]
        public void GetPesoPorFaixaSalarial_MenosDe3Salarios_Retorno1()
        {
            Funcionario f = funcionarios[0];
            double salarioMinimo = RegraNegocio.GetSalarioMinimo();
            f.salarioBruto = salarioMinimo * 3 - 1;
            Assert.Equal(1, RegraNegocio.GetPesoPorFaixaSalarial(f));
        }


       // Calcula o valor do bonus de acordo com a formula abaixo</para><para>
       // Pelas regras estabelecidas a fórmula para se chegar ao bônus de cada funcionário é:     
       //                        (SB* PTA) + (SB* PAA)                                            
       //                    ______________________________  * 12 (Meses do Ano)                  
       //                                (PFS)                                                    
       //   Legenda SB: Salário Bruto 
       //        PTA: Peso por tempo de admissão 
       //        PAA: Peso por aréa de atuação 
       //        PFS: Peso por faixa salarial
        [Fact]
        public void GetValorBonus_CaminhoFeliz_RetornoOk()
        {
            Funcionario f = funcionarios[0];

            double valorBonus = 0;
            int PFS = RegraNegocio.GetPesoPorFaixaSalarial(f);

            valorBonus = f.salarioBruto * RegraNegocio.GetPesoPorTempoDeAdmissao(f);
            valorBonus += f.salarioBruto * RegraNegocio.GetPesoPorAreaDeAtuacao(f);
            valorBonus = (valorBonus / PFS);
            int nmeses = RegraNegocio.GetQtdMesesTrabalhadosAnoAnterior(f);
            valorBonus = valorBonus * nmeses;
            
           // Pelas regras estabelecidas a fórmula para se chegar ao bônus de cada funcionário é:     
           //                        (SB* PTA) + (SB* PAA)                                            
           //                    ______________________________  * 12 (Meses do Ano)                  
           //                                (PFS)                                                    
           //   Legenda SB: Salário Bruto 
           //        PTA: Peso por tempo de admissão 
           //        PAA: Peso por aréa de atuação 
           //        PFS: Peso por faixa salarial
           //
           //valorBonus = 989.0 * 1 = 989.0
           //valorBonus = 989.0 + 989.0 * 5 = 5934.0
           //valorBonus = (5934.0 / 1) = 5934.0
           //valorBonus = 5934.0 * 4 = 23736.0      
            double resultado = RegraNegocio.GetValorBonus(f);
            Assert.Equal((double)23736.0d, resultado);

        }
    }
}
