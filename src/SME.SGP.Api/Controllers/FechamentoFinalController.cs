using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/fechamentos/finais")]
    [Authorize("Bearer")]
    public class FechamentoFinalController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(FechamentoFinalConsultaRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public IActionResult Obter([FromQuery]FechamentoFinalConsultaFiltroDto filtroFechamentoFinalConsultDto)
        {
            var retorno = new FechamentoFinalConsultaRetornoDto()
            {
                EhNota = true,
                EhRegencia = true,
                EventoData = DateTime.Today,
                AuditoriaAlteracao = "Notas(ou conceitos) da avaliação ABC alterados por Nome Usuário(9999999) em 11 / 01 / 2019,às 16:00.",
                AuditoriaInclusao = "Notas (ou conceitos) da avaliação XYZ inseridos por por Nome Usuário(9999999) em 10/01/2019, às 15:00."
            };

            ///// aluno 2
            ///
            var aluno2 = new FechamentoFinalConsultaRetornoAlunoDto() { Nome = "Joselito Alves", Frequencia = 35, NumeroChamada = 2, TotalAusenciasCompensadas = 3 };

            aluno2.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = "7" });

            aluno2.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = "3" });

            aluno2.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = "2" });

            aluno2.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 4, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = "3.5" });

            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = "7" });
            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "12", Disciplina = "Geografia", NotaConceito = "8" });
            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "122", Disciplina = "Ciências", NotaConceito = "7.5" });

            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = "3" });
            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "12", Disciplina = "Geografia", NotaConceito = "6" });
            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "122", Disciplina = "Ciências", NotaConceito = "5" });

            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = "2" });
            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "12", Disciplina = "Geografia", NotaConceito = "4" });
            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "122", Disciplina = "Ciências", NotaConceito = "2.5" });

            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 4, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = "3.5" });
            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 4, DisciplinaCodigo = "12", Disciplina = "Geografia", NotaConceito = "9" });
            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 4, DisciplinaCodigo = "122", Disciplina = "Ciências", NotaConceito = "10" });

            retorno.Alunos.Add(aluno2);

            ///// aluno 1
            ///
            var aluno1 = new FechamentoFinalConsultaRetornoAlunoDto() { Nome = "Analisa Tonha", Frequencia = 1, NumeroChamada = 1, TotalAusenciasCompensadas = 10 };

            aluno1.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = "8.5" });

            aluno1.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = "6" });

            aluno1.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = "9" });

            aluno1.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 4, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = "10" });

            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = "4" });
            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "12", Disciplina = "Geografia", NotaConceito = "6" });
            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "122", Disciplina = "Ciências", NotaConceito = "9.5" });

            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = "7" });
            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "12", Disciplina = "Geografia", NotaConceito = "6.5" });
            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "122", Disciplina = "Ciências", NotaConceito = "9" });

            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = "3" });
            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "12", Disciplina = "Geografia", NotaConceito = "4" });
            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "122", Disciplina = "Ciências", NotaConceito = "5" });

            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 4, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = "5" });
            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 4, DisciplinaCodigo = "12", Disciplina = "Geografia", NotaConceito = "4" });
            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 4, DisciplinaCodigo = "122", Disciplina = "Ciências", NotaConceito = "3" });

            retorno.Alunos.Add(aluno1);

            return Ok(retorno);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string[]), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromBody]FechamentoFinalSalvarDto fechamentoFinalSalvarDto, [FromServices]IComandosFechamentoFinal comandosFechamentoFinal)
        {
            return Ok(await comandosFechamentoFinal.SalvarAsync(fechamentoFinalSalvarDto));
        }
    }
}