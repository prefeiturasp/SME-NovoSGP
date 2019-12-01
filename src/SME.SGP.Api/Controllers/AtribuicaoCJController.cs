using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/atribuicoes/cjs")]
    [Authorize("Bearer")]
    [ValidaDto]
    public class AtribuicaoCJController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]AtribuicaoCJListaFiltroDto atribuicaoCJListaFiltroDto, [FromServices]IConsultasAtribuicaoCJ consultasAtribuicaoCJ)
        {
            return Ok(await consultasAtribuicaoCJ.Listar(atribuicaoCJListaFiltroDto));
        }

        [HttpGet("ues/{ueId}/modalidades/{modalidadeId}/turmas/{turmaId}/professores/{professorId}/")]
        public IActionResult ObterAtribuicaoDeProfessores(string ueId, string turmaId,
            string professorId, Modalidade modalidadeId)
        {
            var retorno = new AtribuicaoCJTitularesRetornoDto()
            {
                AlteradoEm = new System.DateTime(2019, 12, 1),
                AlteradoPor = "Marcos Lobo",
                AlteradoRF = "123",
                CriadoEm = new System.DateTime(2019, 11, 1),
                CriadoPor = "Marcos Lobo",
                CriadoRF = "123"
            };

            retorno.Itens.Add(new AtribuicaoCJTitularesRetornoItemDto()
            {
                Disciplina = "Matemática",
                DisciplinaId = "1",
                ProfessorTitular = "Sávio da Silveira Santos",
                Substituir = true,
                ProfessorTitularRf = "123"
            });

            retorno.Itens.Add(new AtribuicaoCJTitularesRetornoItemDto()
            {
                Disciplina = "Geografia",
                DisciplinaId = "2",
                ProfessorTitular = "João Paulo da La Penha",
                Substituir = false,
                ProfessorTitularRf = "12"
            });

            retorno.Itens.Add(new AtribuicaoCJTitularesRetornoItemDto()
            {
                Disciplina = "História",
                DisciplinaId = "3",
                ProfessorTitular = "Iranilda Junqueira",
                Substituir = false,
                ProfessorTitularRf = "111"
            });

            return Ok(retorno);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AtribuicaoCJPersistenciaDto atribuicaoCJPersistenciaDto, [FromServices] IComandosAtribuicaoCJ comandosAtribuicaoCJ)
        {
            await comandosAtribuicaoCJ.Salvar(atribuicaoCJPersistenciaDto);
            return Ok();
        }
    }
}