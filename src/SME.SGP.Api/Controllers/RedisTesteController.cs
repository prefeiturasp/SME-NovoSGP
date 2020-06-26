using Microsoft.AspNetCore.Mvc;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/redis")]
    public class RedisTesteController : ControllerBase
    {
        private readonly IRepositorioCache repositorioCache;

        public RedisTesteController(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache;
        }

        [HttpPost]
        public async Task<IActionResult> InserirDados()
        {
            await RemoverChaves();

            var i = 1111111;
            while (i != 4444444)
            {
                string randomValoresRandomicos = ObterValor();
                var chave = $"teste-redis-{i}";
                await repositorioCache.SalvarAsync(chave, randomValoresRandomicos);
                i++;
            }

            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeletarDados()
        {

           await  RemoverChaves();

            return Ok();
        }
        private async Task RemoverChaves()
        {
            var i = 1111111;
            while (i != 4444444)
            {
                var chave = $"teste-redis-{i}";
                await repositorioCache.RemoverAsync(chave);
                i++;
            }
        }
        [HttpGet("id")]
        public async Task<IActionResult> BuscarDados(int id)
        {
            var chave = $"teste-redis-{id}";
            var retorno = await repositorioCache.ObterAsync(chave);
            if (string.IsNullOrEmpty(retorno))
            {
                retorno = ObterValor();
                await repositorioCache.SalvarAsync(chave, retorno);
            }
                  

            return Ok(retorno);
        }
        private string ObterValor()
        {
            var str = new StringBuilder();

            for (int i = 0; i < 20; i++)
            {
                str.Append(Guid.NewGuid().ToString());
            }
            return str.ToString();
        }
    }
}
