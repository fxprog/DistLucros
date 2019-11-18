using System;
using System.Collections.Generic;
using DistLucros.Models;
using Microsoft.AspNetCore.Mvc;

namespace DistLucrosBudget.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    ///<summary>
    ///Classe que implementa o controle que realiza a tarefa pedida no desafio proposto
    ///porem num approach diferente, ao inves de simplesmente aceitar que faltaria dinheiro,
    ///mantendo a proporcionalidade da regra original, distribui todo dinheiro disponivel
    ///</summary>
    public class DistLucrosBudgetController : ControllerBase
    {

        #region snippet_get
        // GET: api/DistLucrosBudget
        [HttpGet]
        public String Get()
        {
            return "Webservice desafio, approach alternativo";
        }
        #endregion


        #region snippet_post
        [HttpPost]
        public DistribuicaoLucros Post(IEnumerable<Funcionario> l)
        {
            return new DistribuicaoLucros(l, ArmazenaValorDisponibilizado.valorDisponibilizado, true);
        }
        #endregion


    }
}
