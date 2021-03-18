using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAcompanhamentoAlunoUseCase : AbstractUseCase, IObterAcompanhamentoAlunoUseCase
    {
        public ObterAcompanhamentoAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AcompanhamentoAlunoTurmaSemestreDto>> Executar(FiltroAcompanhamentoTurmaAlunoSemestreDto filtro)
        {

            var resultadoAcompanhamentosAlunoTurmaSemestre = new List<AcompanhamentoAlunoTurmaSemestreDto>();
            var acompanhamentosAlunoTurmaSemestre = await mediator.Send(new ObterAcompanhamentoPorAlunoTurmaESemestreQuery(filtro.TurmaId, filtro.AlunoId, filtro.Semestre));

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(filtro.TurmaId));

            if (turma == null)
                throw new NegocioException("Não foi possível localizar a turma informada!");

            var periodosAbertos = await mediator.Send(new ObterPeriodoEscolarAtualPorTurmaQuery(turma, DateTime.Now));

            VerificaSePodeEditarAcompanhamentoAluno(resultadoAcompanhamentosAlunoTurmaSemestre, acompanhamentosAlunoTurmaSemestre, periodosAbertos);

            return resultadoAcompanhamentosAlunoTurmaSemestre;
        }

        private static void VerificaSePodeEditarAcompanhamentoAluno(List<AcompanhamentoAlunoTurmaSemestreDto> resultadoAcompanhamentosAlunoTurmaSemestre, IEnumerable<AcompanhamentoAlunoTurmaSemestreDto> acompanhamentosAlunoTurmaSemestre, PeriodoEscolar periodosAbertos)
        {
            foreach (var acompanhamento in acompanhamentosAlunoTurmaSemestre)
            {
                acompanhamento.PodeEditar = periodosAbertos != null ? true : false;
                resultadoAcompanhamentosAlunoTurmaSemestre.Add(acompanhamento);
            }
        }
    }
}
