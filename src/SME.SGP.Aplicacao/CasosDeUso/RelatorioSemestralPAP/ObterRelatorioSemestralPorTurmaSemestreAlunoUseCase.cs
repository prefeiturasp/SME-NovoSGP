using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRelatorioSemestralPorTurmaSemestreAlunoUseCase
    {
        public static async Task<RelatorioSemestralAlunoDto> Executar(IMediator mediator, string alunoCodigo, string turmaCodigo, int semestre, IConsultasPeriodoEscolar consultasPeriodoEscolar)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery()
            {
                TurmaCodigo = turmaCodigo
            });
            var relatorio = await mediator.Send(new ObterRelatorioSemestralPorTurmaSemestreAlunoQuery(alunoCodigo, turmaCodigo, semestre));

            var bimestreAtual = consultasPeriodoEscolar.ObterBimestre(DateTime.Today, turma.ModalidadeCodigo, turma.Semestre);

            if (turma.ModalidadeCodigo != Modalidade.EJA)
            {
                relatorio.PodeEditar = bimestreAtual == 2 || bimestreAtual == 4;
            }
            else
            {
                relatorio.PodeEditar = true;
            }

            return relatorio;
        }
    }
}
