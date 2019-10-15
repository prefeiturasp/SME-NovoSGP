using Microsoft.AspNetCore.Mvc;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        [HttpGet("{filtro}")]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaFiltroRetorno>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterAbnragenciaAutoComplete(string filtro)
        {
            if (filtro.Length < 2)
                return StatusCode(204);

            var registros = new List<AbrangenciaFiltroRetorno>();

            GeraRegistrosMockFiltros(registros);

            filtro = filtro.ToUpper();

            var retorno = registros.Where(a => a.NomeTurma.Contains(filtro) || a.NomeUe.Contains(filtro)).ToList();
            if (retorno.Count > 0)
                return Ok(retorno);
            else return StatusCode(204);
        }

        [HttpGet("anos-letivos")]
        [ProducesResponseType(typeof(int[]), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterAnosLetivos()
        {
            return Ok(new int[] { 2019 });
        }

        [HttpGet("dres")]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaDreRetorno>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterDres()
        {
            return Ok(ListaDres);
        }

        [HttpGet("modalidades")]
        [ProducesResponseType(typeof(IEnumerable<EnumeradoRetornoDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterModalidades()
        {
            var retorno = Modalidade.GetValues(typeof(Modalidade)).Cast<Modalidade>().Select(v => new EnumeradoRetornoDto
            {
                Descricao = v.GetAttribute<DisplayAttribute>().Name,
                Id = (int)v
            }).ToList();

            return Ok(retorno);
        }

        [HttpGet("semestres")]
        [ProducesResponseType(typeof(int[]), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterSemestres()
        {
            return Ok(new int[] { 0, 1, 2 });
        }

        [HttpGet("dres/ues/{codigoUe}/turmas")]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaTurmaRetorno>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterTurmas(string codigoUe)
        {
            IList<AbrangenciaTurmaRetorno> listaTurmas = new List<AbrangenciaTurmaRetorno>();

            switch (codigoUe)
            {
                case "000191":
                    listaTurmas.Add(new AbrangenciaTurmaRetorno() { Ano = 1, AnoLetivo = 2019, Codigo = "191", CodigoModalidade = 3, Nome = "191-A", Semestre = 1 });
                    break;

                case "000477":
                    listaTurmas.Add(new AbrangenciaTurmaRetorno() { Ano = 1, AnoLetivo = 2019, Codigo = "477", CodigoModalidade = 3, Nome = "477-A", Semestre = 1 });
                    break;

                case "000281":
                    listaTurmas.Add(new AbrangenciaTurmaRetorno() { Ano = 1, AnoLetivo = 2019, Codigo = "281", CodigoModalidade = 3, Nome = "281-A", Semestre = 1 });
                    break;

                case "000311":
                    listaTurmas.Add(new AbrangenciaTurmaRetorno() { Ano = 1, AnoLetivo = 2019, Codigo = "311", CodigoModalidade = 3, Nome = "311-A", Semestre = 1 });
                    break;

                case "000566":
                    listaTurmas.Add(new AbrangenciaTurmaRetorno() { Ano = 1, AnoLetivo = 2019, Codigo = "566", CodigoModalidade = 3, Nome = "566-A", Semestre = 1 });
                    break;

                default:
                    break;
            }

            return Ok(listaTurmas);
        }

        [HttpGet("dres/{codigoDre}/ues")]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaUeRetorno>), 200)]
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

        private static void GeraRegistrosMockFiltros(List<AbrangenciaFiltroRetorno> registros)
        {
            registros.Add(new AbrangenciaFiltroRetorno()
            {
                AnoLetivo = 2019,
                CodigoDre = "108100",
                CodigoModalidade = (int)Modalidade.EJA,
                CodigoTurma = "191",
                CodigoUe = "000191",
                NomeDre = "DIRETORIA REGIONAL DE EDUCACAO BUTANTA",
                NomeModalidade = Modalidade.EJA.GetAttribute<DisplayAttribute>().Name,
                NomeTurma = "191-A",
                NomeUe = "ALIPIO CORREA NETO, PROF.",
                Semestre = 1,
                AbreviacaoModalidade = Modalidade.EJA.GetAttribute<DisplayAttribute>().ShortName
            });

            registros.Add(new AbrangenciaFiltroRetorno()
            {
                AnoLetivo = 2019,
                CodigoDre = "108100",
                CodigoModalidade = (int)Modalidade.EJA,
                CodigoTurma = "477",
                CodigoUe = "000477",
                NomeDre = "DIRETORIA REGIONAL DE EDUCACAO BUTANTA",
                NomeModalidade = Modalidade.EJA.GetAttribute<DisplayAttribute>().Name,
                NomeTurma = "477-A",
                NomeUe = "EDA TEREZINHA CHICA MEDEIROS, PROFA.",
                Semestre = 1,
                AbreviacaoModalidade = Modalidade.EJA.GetAttribute<DisplayAttribute>().ShortName
            });

            registros.Add(new AbrangenciaFiltroRetorno()
            {
                AnoLetivo = 2019,
                CodigoDre = "108200",
                CodigoModalidade = (int)Modalidade.EJA,
                CodigoTurma = "281",
                CodigoUe = "000281",
                NomeDre = "DIRETORIA REGIONAL DE EDUCACAO CAMPO LIMPO",
                NomeModalidade = Modalidade.EJA.GetAttribute<DisplayAttribute>().Name,
                NomeTurma = "281-A",
                NomeUe = "VERA LUCIA FUSCO BORBA, PROFA.",
                Semestre = 1,
                AbreviacaoModalidade = Modalidade.EJA.GetAttribute<DisplayAttribute>().ShortName
            });

            registros.Add(new AbrangenciaFiltroRetorno()
            {
                AnoLetivo = 2019,
                CodigoDre = "108200",
                CodigoModalidade = (int)Modalidade.EJA,
                CodigoTurma = "311",
                CodigoUe = "000311",
                NomeDre = "DIRETORIA REGIONAL DE EDUCACAO CAMPO LIMPO",
                NomeModalidade = Modalidade.EJA.GetAttribute<DisplayAttribute>().Name,
                NomeTurma = "311-A",
                NomeUe = "ANTONIO ALVES DA SILVA, SG.",
                Semestre = 1,
                AbreviacaoModalidade = Modalidade.EJA.GetAttribute<DisplayAttribute>().ShortName
            });

            registros.Add(new AbrangenciaFiltroRetorno()
            {
                AnoLetivo = 2019,
                CodigoDre = "108200",
                CodigoModalidade = (int)Modalidade.EJA,
                CodigoTurma = "566",
                CodigoUe = "000566",
                NomeDre = "DIRETORIA REGIONAL DE EDUCACAO CAMPO LIMPO",
                NomeModalidade = Modalidade.EJA.GetAttribute<DisplayAttribute>().Name,
                NomeTurma = "566-A",
                NomeUe = "TERESA MARGARIDA DA SILVA E ORTA",
                Semestre = 1,
                AbreviacaoModalidade = Modalidade.EJA.GetAttribute<DisplayAttribute>().ShortName
            });
        }
    }
}