Como testar:
Compilar e executar utilizando Visual Studio (Utilizei 2017 Community, provavelmente qualquer uma a partir dessa funciona)
Utilizar algum aplicativo que consiga enviar requisições rest.(utilizei o SOAPUI 5.5.0)

1 - Verificar a porta em que o visual studio está rodando a aplicação e configurá-la no software ex: https://localhost:44332
2 - Setar o recurso que sera utilizado. ex: /api/vdisp

Enviar sempre com UTF8 e POST.

Setando o valor disponibilizando:
Enviar para /api/vdisp o json com o seguinte formato, para definir o valor disponibilizado.
exemplo:
	{
	   "valor_disponibilizado": "R$ 100.000,00",
	}


Enviar lista de funcionarios para /api/distlucros para definir quais funcionarios levar em consideração ao realizar os calculos	
Exemplo:
[
	{
		   "matricula": "0009968",
		   "nome": "Victor Wilson",
		   "area": "Diretoria",
		   "cargo": "Diretor Financeiro",
		   "salario_bruto": "R$ 12.696,20",
		   "data_de_admissao": "2012-01-05"
	},
	{      "matricula": "448",
		   "nome": "Flossie Wilson",
		   "area": "Contabilidade",
		   "cargo": "Auxiliar de Contabilidade",
		   "salario_bruto": "R$ 1.396,52",
		   "data_de_admissao": "2015-01-01"
	},
	{      "matricula": "0008175",
		   "nome": "Sherman Hodges",
		   "area": "Relacionamento com o Cliente",
		   "cargo": "Líder de Relacionamento",
		   "salario_bruto": "R$ 3.899,74",
		   "data_de_admissao": "2015-06-07"     
	}
]

Versão alternativa:
Como opção, fiz uma versão que realiza a distribuição que se adequa ao valor disponibilizado para a distribuição de lucros utilizando a mesma proporção do cálculo original.
Enviar lista de funcionarios para /api/distlucrosbudget para definir quais funcionarios levar em consideração ao realizar os calculos. Mesmo exemplo anterior vale.

