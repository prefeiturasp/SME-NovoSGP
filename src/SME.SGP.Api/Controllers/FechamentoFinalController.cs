using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

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
        public IActionResult Obter([FromQuery]FechamentoFinalConsultaFiltroDto filtroFechamentoFinalConsultDto)
        {
            var retorno = new FechamentoFinalConsultaRetornoDto() { EhNota = true, EhRegencia = true };

            ///// aluno 2
            ///
            var aluno2 = new FechamentoFinalConsultaRetornoAlunoDto() { Nome = "Joselito Alves", Frequencia = 35, NumeroChamada = 2, TotalAusenciasCompensadas = 3 };

            aluno2.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "123", NotaConceito = "7" });

            aluno2.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "123", NotaConceito = "3" });

            aluno2.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "123", NotaConceito = "2" });

            aluno2.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 4, DisciplinaCodigo = "123", NotaConceito = "3.5" });

            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "123", NotaConceito = "7" });
            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "12", NotaConceito = "8" });
            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "122", NotaConceito = "7.5" });

            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "123", NotaConceito = "3" });
            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "12", NotaConceito = "6" });
            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "122", NotaConceito = "5" });

            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "123", NotaConceito = "2" });
            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "12", NotaConceito = "4" });
            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "122", NotaConceito = "2.5" });

            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 4, DisciplinaCodigo = "123", NotaConceito = "3.5" });
            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 4, DisciplinaCodigo = "12", NotaConceito = "9" });
            aluno2.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 4, DisciplinaCodigo = "122", NotaConceito = "10" });

            retorno.Alunos.Add(aluno2);

            ///// aluno 1
            ///
            var aluno1 = new FechamentoFinalConsultaRetornoAlunoDto() { Nome = "Analisa Tonha", Frequencia = 1, NumeroChamada = 1, TotalAusenciasCompensadas = 10 };

            aluno1.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "123", NotaConceito = "8.5" });

            aluno1.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "123", NotaConceito = "6" });

            aluno1.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "123", NotaConceito = "9" });

            aluno1.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 4, DisciplinaCodigo = "123", NotaConceito = "10" });

            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "123", NotaConceito = "4" });
            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "12", NotaConceito = "6" });
            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 1, DisciplinaCodigo = "122", NotaConceito = "9.5" });

            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "123", NotaConceito = "7" });
            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "12", NotaConceito = "6.5" });
            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 2, DisciplinaCodigo = "122", NotaConceito = "9" });

            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "123", NotaConceito = "3" });
            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "12", NotaConceito = "4" });
            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 3, DisciplinaCodigo = "122", NotaConceito = "5" });

            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 4, DisciplinaCodigo = "123", NotaConceito = "5" });
            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 4, DisciplinaCodigo = "12", NotaConceito = "4" });
            aluno1.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = 4, DisciplinaCodigo = "122", NotaConceito = "3" });

            retorno.Alunos.Add(aluno1);

            return Ok(retorno);
        }
    }
}