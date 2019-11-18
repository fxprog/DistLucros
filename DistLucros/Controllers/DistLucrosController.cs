using System;
using System.Collections.Generic;
using DistLucros.Models;
using Microsoft.AspNetCore.Mvc;

namespace DistLucros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]

    ///<summary>
    ///Classe que implementa o controle que realiza a tarefa pedida no desafio proposto
    ///</summary>
    public class DistLucrosController : ControllerBase
    {

        #region snippet_get
        // GET: api/DistLucros
        [HttpGet]
        public String Get()
        {
           return "Webservice desafio";
           
        }
        #endregion

        #region snippet_post
        [HttpPost]
        public DistribuicaoLucros Post(IEnumerable<Funcionario> l)
        {
            return new DistribuicaoLucros(l, ArmazenaValorDisponibilizado.valorDisponibilizado);
        }
        #endregion

    }
}
