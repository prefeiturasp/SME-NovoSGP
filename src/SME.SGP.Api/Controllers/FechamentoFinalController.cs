using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        //[Permissao(Permissao.FB_C, Policy = "Bearer")]
        public IActionResult Obter([FromQuery]FechamentoFinalConsultaFiltroDto filtroFechamentoFinalConsultDto)
        {
            var ehRegencia = filtroFechamentoFinalConsultDto.EhRegencia;
            var ehNota = filtroFechamentoFinalConsultDto.TurmaCodigo != 1;
            var retorno = new FechamentoFinalConsultaRetornoDto()
            {
                EhNota = ehNota,
                EhRegencia = ehRegencia,
                EventoData = DateTime.Today,
                AuditoriaAlteracao = "Notas(ou conceitos) da avaliação ABC alterados por Nome Usuário(9999999) em 11 / 01 / 2019,às 16:00.",
                AuditoriaInclusao = "Notas (ou conceitos) da avaliação XYZ inseridos por por Nome Usuário(9999999) em 10/01/2019, às 15:00."
            };

            ///// aluno 2
            ///
            var aluno2 = new FechamentoFinalConsultaRetornoAlunoDto() { Nome = "Joselito Alves", Frequencia = 35, NumeroChamada = 2, TotalAusenciasCompensadas = 3, Informacao = "Exemplo de informação no tooltip" };

            aluno2.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = ehNota ? "7" : "PS" });

            aluno2.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = ehNota ? "3" : "PS" });

            aluno2.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = ehNota ? "2" : "PS" });

            aluno2.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 4, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = ehNota ? "3.5" : "PS" });

            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = ehNota ? "7" : "PS" });
            if (ehRegencia)
            {
                aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "12", Disciplina = "Geografia", NotaConceito = ehNota ? "8" : "PS" });
                aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "122", Disciplina = "Ciências", NotaConceito = ehNota ? "7.5" : "PS" });
                aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = ehNota ? "3" : "PS" });
                aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "12", Disciplina = "Geografia", NotaConceito = ehNota ? "6" : "PS" });
                aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "122", Disciplina = "Ciências", NotaConceito = ehNota ? "5" : "PS" });
                aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = ehNota ? "2" : "PS" });
            }

            retorno.Alunos.Add(aluno2);

            ///// aluno 1
            ///
            var aluno1 = new FechamentoFinalConsultaRetornoAlunoDto() { Nome = "Analisa Tonha", Frequencia = 1, NumeroChamada = 1, TotalAusenciasCompensadas = 10 };

            aluno1.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = ehNota ? "8.5" : "PS" });

            aluno1.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = ehNota ? "6" : "PS" });

            aluno1.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = ehNota ? "9" : "PS" });

            aluno1.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 4, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = ehNota ? "10" : "PS" });

            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = ehNota ? "4" : "PS" });

            if (ehRegencia)
            {
                aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "12", Disciplina = "Geografia", NotaConceito = ehNota ? "6" : "PS" });
                aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "122", Disciplina = "Ciências", NotaConceito = ehNota ? "9.5" : "PS" });
                aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = ehNota ? "7" : "PS" });
                aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "12", Disciplina = "Geografia", NotaConceito = ehNota ? "6.5" : "PS" });
                aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "122", Disciplina = "Ciências", NotaConceito = ehNota ? "9" : "PS" });
                aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "123", Disciplina = "Matemática", NotaConceito = ehNota ? "3" : "PS" });
            }
            retorno.Alunos.Add(aluno1);

            return Ok(retorno);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string[]), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        //[Permissao(Permissao.FB_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromBody]FechamentoFinalSalvarDto fechamentoFinalSalvarDto, [FromServices]IComandosFechamentoFinal comandosFechamentoFinal)
        {
            return Ok(await comandosFechamentoFinal.SalvarAsync(fechamentoFinalSalvarDto));
        }
    }
}