﻿
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/abae")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class ABAEController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(CadastroAcessoABAEDto), 200)]
        [Permissao(Permissao.ABA_I, Policy = "Bearer")]
        public async Task<IActionResult> Incluir([FromBody] CadastroAcessoABAEDto cadastroAcessoAbaeDto, [FromServices] ISalvarCadastroAcessoABAEUseCase salvarCadastroAcessoAbaeUse)
        {
            return Ok(salvarCadastroAcessoAbaeUse.Executar(cadastroAcessoAbaeDto));
        }
        
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(CadastroAcessoABAEDto), 200)]
        [Permissao(Permissao.ABA_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromBody] CadastroAcessoABAEDto cadastroAcessoAbaeDto, [FromServices] ISalvarCadastroAcessoABAEUseCase salvarCadastroAcessoAbaeUse)
        {
            return Ok(salvarCadastroAcessoAbaeUse.Executar(cadastroAcessoAbaeDto));
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(CadastroAcessoABAEDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ABA_C, Policy = "Bearer")]
        public async Task<IActionResult> BuscarPorId(int id, [FromServices] IObterCadastroAcessoABAEUseCase obterCadastroAcessoAbaeUse)
        {
            return Ok(obterCadastroAcessoAbaeUse.Executar(id));
        }
        
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ABA_E, Policy = "Bearer")]
        public IActionResult Excluir([FromBody] long id, [FromServices] IExcluirCadastroAcessoABAEUseCase excluirCadastroAcessoABAEUseCase)
        {
            return Ok(excluirCadastroAcessoABAEUseCase.Executar(id));
        }
    }
}