using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DistLucros.Models;
using Newtonsoft.Json;

namespace DistLucros.Controllers
{
    [Route("api/vdisp")]
    [ApiController]
    [Consumes("application/json")]
    public class ValorDisponibilizadoController : ControllerBase
    {



        #region snippet_post
        [HttpPost]
        public Resposta Post(ValorDisponibilizado v)
        {
            ArmazenaValorDisponibilizado.valorDisponibilizado = v.valorDisponibilizado;
            return new Resposta("Recebido com sucesso!");
        }
        #endregion
    
        #region snippet_get
        [HttpGet]
        public String Get()
        {
            return "Webservice desafio."+Environment.NewLine+" O valor Disponibilizado atualmente é de " + RegraNegocio.DoubleEmReal(ArmazenaValorDisponibilizado.valorDisponibilizado);

        }
        #endregion
    }
}