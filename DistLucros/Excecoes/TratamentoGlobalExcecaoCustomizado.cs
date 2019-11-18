using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistLucros.Excecoes
{
    ///<summary>
    ///Classe feita para fazer tratamento de erro que foge ao tratamento padrao, dando uma ideia ao cliente de qual eh o erro, possibilitando ao mesmo corrigi-lo    
    ///</summary>
    ///<remarks>
    ///Num ambiente de producao seria interessante nao fornecer qualquer informacao de excecao pois pode ser utilizada a ataques, 
    ///contudo em ambiente de desenvolvimento essa informacao eh bem util
    ///Eventualmente, caso seja do interesse eh possivel dar parse no stack trace todo, coloquei apenas a primeira linha por opcao.
    ///</remarks>
    public class TratamentoGlobalExcecaoCustomizado : IMiddleware
    {
        private readonly ILogger<TratamentoGlobalExcecaoCustomizado> _logger;

        public TratamentoGlobalExcecaoCustomizado(ILogger<TratamentoGlobalExcecaoCustomizado> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro inesperado: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            const int codigoStatus = StatusCodes.Status400BadRequest;

            Dictionary<string, string> detalhado = new Dictionary<string, string>();                       
            detalhado.Add("Type", exception.GetType().ToString());
            detalhado.Add("Message",exception.Message);
            if (exception.StackTrace.Contains("\r"))
            {
                detalhado.Add("StackTrace", exception.StackTrace.Substring(0, exception.StackTrace.IndexOf("\r")));
            }

            var json = JsonConvert.SerializeObject(new
            {
                codigoStatus,
                mensagem = "Houve um erro ao processar sua requisicao.",
                detalhado = detalhado
            }, new JsonSerializerSettings());

            context.Response.StatusCode = codigoStatus;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(json);
        }
    }

    public static class TratamentoGlobalExcecaoCustomizadoExtension
    {
        public static IServiceCollection AddGlobalExceptionHandlerMiddleware(this IServiceCollection services)
        {
            return services.AddTransient<TratamentoGlobalExcecaoCustomizado>();
        }

        public static void UseGlobalExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<TratamentoGlobalExcecaoCustomizado>();
        }

    }
}