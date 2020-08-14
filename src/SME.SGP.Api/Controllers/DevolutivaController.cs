using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/devolutivas")]
    //[Authorize("Bearer")]
    public class DevolutivaController : ControllerBase
    {

        [HttpGet("turmas/{turmaCodigo}/componentes-curriculares/{componenteCurricularId}")]
        //[ProducesResponseType(typeof(DiarioBordoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> Obter(string turmaCodigo, string componenteCurricularId, DateTime data, int numeroPagina, int numeroRegistros)
        {
            var text = @"[
                          {
                            id: 1,
                            periodoInicio: ""2020-07-01T00:00:00.000000"",
                            periodoFim: ""2020-07-31T00:00:00.000000"",
                            criadoEm: ""2020-07-05T00:00:00.000000"",
                            criadoPor: ""DIONE LEMOS DE SOUZA OLIVEIRA"",
                          },
                          {
                            id: 2,
                            periodoInicio: ""2020-08-01T00:00:00.000000"",
                            periodoFim: ""2020-08-14T00:00:00.000000"",
                            criadoEm: ""2020-08-05T00:00:00.000000"",
                            criadoPor: ""DIONE LEMOS DE SOUZA OLIVEIRA"",
                          },
                        ]";

            var json = JArray.Parse(text);

            return Ok(json);
        }



        [HttpGet("{devolutivaId}")]
        //[ProducesResponseType(typeof(DiarioBordoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPorId(long devolutivaId)
        {
            var text = @"{
                            id: 1,
                            devolutiva: ""devolutiva mockada"",
                            diariosIds: [1, 2, 3, 4],
                            auditoria: {
                                id: 1,
                                alteradoEm: ""2020-08-05T00:00:00.000000"",
                                alteradoPor: ""DIONE LEMOS DE SOUZA OLIVEIRA"",
                                alteradoRF: ""5793785"",
                                criadoEm: ""2020-08-05T18:20:34.13358"",
                                criadoPor: ""DIONE LEMOS DE SOUZA OLIVEIRA"",
                                criadoRF: ""5793785""
                            }
                        }";

            var json = JObject.Parse(text);

            return Ok(json);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.DDB_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromBody] TempSalvarDto model)
        {
            return Ok(true);
        }

        [HttpPut]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.DDB_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromBody] TempSalvarDto model)
        {
            return Ok(true);
        }

        [HttpDelete("{devolutivaId}")]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.DDB_A, Policy = "Bearer")]
        public async Task<IActionResult> Excluir(long devolutivaId)
        {
            return Ok(true);
        }





        [HttpGet("turmas/{turmaCodigo}/componentes-curriculares/{componenteCurricularId}/sugestao")]
        //[ProducesResponseType(typeof(DiarioBordoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> SugestaoDataInicio(string turmaCodigo, string componenteCurricularId)
        {

            return Ok(DateTime.Today.AddDays(-5));
        }
    }

    public class TempSalvarDto {
        public string Devolutiva { get; set; }

        public List<long> DiariosIds { get; set; }
    }

}
