using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/avaliacoes/notas")]
    [ValidaDto]
    public class NotasConceitosController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(NotasConceitosRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NC_A, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> Get([FromQuery]ListaNotasConceitosConsultaDto consultaListaNotasConceitosDto)
        {
            var retornoMock = new NotasConceitosRetornoDto();
            retornoMock.AuditoriaAlterado = "Notas (ou conceitos) da avaliação XYZ alterados por por Nome Usuário(9999999) em 10/01/2019, às 15:00.";
            retornoMock.AuditoriaInserido = "Notas (ou conceitos) da avaliação XYZ inseridos por por Nome Usuário(9999999) em 10/01/2019, às 15:00.";

            retornoMock.NotaTipo = Dominio.TipoNota.Nota;
            retornoMock.Bimestres.Add(new NotasConceitosBimestreRetornoDto() { Numero = 1, Descricao = "1º Bimestre" });

            var segundoBimestre = new NotasConceitosBimestreRetornoDto() { Numero = 1, Descricao = "2º Bimestre" };

            var aluno1 = new NotasConceitosAlunoRetornoDto()
            {
                Id = "321",
                Nome = "Alvaros Ramos Grassi",
                NumeroChamada = 1,
                NotasAvaliacoes = new List<NotasConceitosNotaAvaliacaoRetornoDto>() { new NotasConceitosNotaAvaliacaoRetornoDto() {
                    AtividadeAvaliativaId = 1, Ausente = false,
                    NotaConceito  = string.Empty, PodeEditar = true  }, new NotasConceitosNotaAvaliacaoRetornoDto() {
                    AtividadeAvaliativaId = 2, Ausente = false,
                    NotaConceito  = string.Empty, PodeEditar = true  }, new NotasConceitosNotaAvaliacaoRetornoDto() {
                    AtividadeAvaliativaId = 3, Ausente = false,
                    NotaConceito  = string.Empty, PodeEditar = true  }, new NotasConceitosNotaAvaliacaoRetornoDto() {
                    AtividadeAvaliativaId = 4, Ausente = false,
                    NotaConceito  = string.Empty, PodeEditar = true  }
                }
            };

            segundoBimestre.Alunos.Add(aluno1);

            var aluno2 = new NotasConceitosAlunoRetornoDto()
            {
                Id = "1241",
                Nome = "Aline Grassi",
                NumeroChamada = 2,
                NotasAvaliacoes = new List<NotasConceitosNotaAvaliacaoRetornoDto>() { new NotasConceitosNotaAvaliacaoRetornoDto() {
                    AtividadeAvaliativaId = 1, Ausente = false,
                    NotaConceito  = string.Empty, PodeEditar = true  }, new NotasConceitosNotaAvaliacaoRetornoDto() {
                    AtividadeAvaliativaId = 2, Ausente = true,
                    NotaConceito  = string.Empty, PodeEditar = true  }, new NotasConceitosNotaAvaliacaoRetornoDto() {
                    AtividadeAvaliativaId = 3, Ausente = false,
                    NotaConceito  = string.Empty, PodeEditar = true  }, new NotasConceitosNotaAvaliacaoRetornoDto() {
                    AtividadeAvaliativaId = 4, Ausente = false,
                    NotaConceito  = string.Empty, PodeEditar = true  }
                }
            };

            segundoBimestre.Alunos.Add(aluno2);

            var aluno3 = new NotasConceitosAlunoRetornoDto()
            {
                Id = "424",
                Nome = "Bianca Grassi",
                NumeroChamada = 3,
                NotasAvaliacoes = new List<NotasConceitosNotaAvaliacaoRetornoDto>() { new NotasConceitosNotaAvaliacaoRetornoDto() {
                    AtividadeAvaliativaId = 1, Ausente = false,
                    NotaConceito  = string.Empty, PodeEditar = true  }, new NotasConceitosNotaAvaliacaoRetornoDto() {
                    AtividadeAvaliativaId = 2, Ausente = false,
                    NotaConceito  = string.Empty, PodeEditar = true  }, new NotasConceitosNotaAvaliacaoRetornoDto() {
                    AtividadeAvaliativaId = 3, Ausente = false,
                    NotaConceito  = string.Empty, PodeEditar = true  }, new NotasConceitosNotaAvaliacaoRetornoDto() {
                    AtividadeAvaliativaId = 4, Ausente = false,
                    NotaConceito  = string.Empty, PodeEditar = true  }
                }
            };

            segundoBimestre.Alunos.Add(aluno3);
            segundoBimestre.Avaliacoes.Add(new NotasConceitosAvaliacaoRetornoDto() { Data = new System.DateTime(2019, 10, 7), Descricao = "Pesquisa", Nome = "Avaliação 1", Id = 1 });
            segundoBimestre.Avaliacoes.Add(new NotasConceitosAvaliacaoRetornoDto() { Data = new System.DateTime(2019, 10, 28), Descricao = "Seminário", Nome = "Avaliação 2", Id = 2 });
            segundoBimestre.Avaliacoes.Add(new NotasConceitosAvaliacaoRetornoDto() { Data = new System.DateTime(2019, 11, 1), Descricao = "Trabalho em grupo", Nome = "Avaliação 3", Id = 3 });
            segundoBimestre.Avaliacoes.Add(new NotasConceitosAvaliacaoRetornoDto() { Data = new System.DateTime(2019, 11, 9), Descricao = "Teste", Nome = "Avaliação 4", Id = 4 });
            retornoMock.Bimestres.Add(segundoBimestre);

            return Ok(retornoMock);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NC_A, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> Post([FromBody]NotaConceitoListaDto notaConceitoListaDto, [FromServices]IComandosNotasConceitos comandosNotasConceitos)
        {
            await comandosNotasConceitos.Salvar(notaConceitoListaDto);

            return Ok();
        }
    }
}