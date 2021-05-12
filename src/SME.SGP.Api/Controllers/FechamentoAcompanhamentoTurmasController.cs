﻿using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api
{
    [ApiController]
    [Route("api/v1/fechamentos/acompanhamentos")]
    //[Authorize("Bearer")]
    public class FechamentoAcompanhamentoTurmasController : ControllerBase
    {
        [HttpGet("turmas")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ListaTurmas([FromQuery] FiltroAcompanhamentoFechamentoTurmasDto filtro, [FromServices] IObterTurmasFechamentoAcompanhamentoUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("turmas/{turmaId}/fechamentos/bimestres/{bimestre}")]
        [ProducesResponseType(typeof(IEnumerable<StatusTotalFechamentoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ListaTotalStatusFechamentos(long turmaId, int bimestre, [FromServices] IObterFechamentoConsolidadoPorTurmaBimestreUseCase useCase)
        {
            var listaStatus = await useCase.Executar(new FiltroFechamentoConsolidadoTurmaBimestreDto(turmaId, bimestre));

            return Ok(listaStatus);
        }
        [HttpGet("turmas/{turmaId}/conselho-classe/bimestres/{bimestre}")]
        [ProducesResponseType(typeof(IEnumerable<StatusTotalFechamentoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ListaTotalStatusConselhosClasse(long turmaId, int bimestre, [FromServices] IObterConselhoClasseConsolidadoPorTurmaBimestreUseCase useCase)
        {
            var listaStatus = await useCase.Executar(new FiltroConselhoClasseConsolidadoTurmaBimestreDto(turmaId, bimestre));

            return Ok(listaStatus);
        }


        //TODO: REMOVER ANTES DA STORY IR PARA DEV!
        //[HttpPost]
        ////public async Task<IActionResult> TestarFila([FromServices] IExecutarConsolidacaoTurmaConselhoClasseUseCase executarConsolidacaoTurmaConselhoClasseUseCase)
        ////{

        ////    var obj = new ConsolidacaoTurmaConselhoClasseDto() { Bimestre = 1, TurmaId = 639036 };
        ////    var mensagem = JsonConvert.SerializeObject(obj); 
        ////    var msgRabbit = new MensagemRabbit(mensagem);
            
        ////    await executarConsolidacaoTurmaConselhoClasseUseCase.Executar(msgRabbit);

        ////    return Ok();
        //}
    }
}
