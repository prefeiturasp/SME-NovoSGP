using Microsoft.AspNetCore.Mvc;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/abrangencia")]
    public class AbrangenciaController : ControllerBase
    {
        public AbrangenciaController()
        {
            ListaDres = new List<AbrangenciaDreRetorno>();
            ListaDres.Add(new AbrangenciaDreRetorno()
            {
                Codigo = "108100",
                Abreviacao = "DRE - BT",
                Nome = "DIRETORIA REGIONAL DE EDUCACAO BUTANTA"
            });
            ListaDres.Add(new AbrangenciaDreRetorno()
            {
                Codigo = "108200",
                Abreviacao = "DRE - CL",
                Nome = "DIRETORIA REGIONAL DE EDUCACAO CAMPO LIMPO"
            });
        }

        public IList<AbrangenciaDreRetorno> ListaDres { get; set; }

        [HttpGet("dres")]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaDreRetorno>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterDres()
        {
            return Ok(ListaDres);
        }

        [HttpGet("dres/ues/{codigoUe}/turmas")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterTurmas(string codigoTurma)
        {
            return Ok();
        }

        [HttpGet("dres/{codigoDre}/ues")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterUes(string codigoDre)
        {
            IList<AbrangenciaUeRetorno> listaUes = new List<AbrangenciaUeRetorno>();

            if (codigoDre == "108100")
            {
                listaUes.Add(new AbrangenciaUeRetorno() { Codigo = "000191", Nome = "ALIPIO CORREA NETO, PROF." });
                listaUes.Add(new AbrangenciaUeRetorno() { Codigo = "000477", Nome = "EDA TEREZINHA CHICA MEDEIROS, PROFA." });
            }
            if (codigoDre == "108200")
            {
                listaUes.Add(new AbrangenciaUeRetorno() { Codigo = "000281", Nome = "VERA LUCIA FUSCO BORBA, PROFA." });
                listaUes.Add(new AbrangenciaUeRetorno() { Codigo = "000311", Nome = "ANTONIO ALVES DA SILVA, SG." });
                listaUes.Add(new AbrangenciaUeRetorno() { Codigo = "000566", Nome = "TERESA MARGARIDA DA SILVA E ORTA" });
            }

            return Ok(listaUes);
        }
    }
}