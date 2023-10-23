using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralAlunoPorTurmaEComponenteQueryHandler : IRequestHandler<ObterFrequenciaGeralAlunoPorTurmaEComponenteQuery, FrequenciaAluno>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterFrequenciaGeralAlunoPorTurmaEComponenteQueryHandler(
                                                    IMediator mediator,
                                                    IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }

        public async Task<FrequenciaAluno> Handle(ObterFrequenciaGeralAlunoPorTurmaEComponenteQuery request, CancellationToken cancellationToken)
        {
            var frequenciaAlunoPeriodos = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterFrequenciaGeralAluno(request.AlunoCodigo, request.TurmaCodigo, request.ComponenteCurricularCodigo);

            if (frequenciaAlunoPeriodos.EhNulo() || !frequenciaAlunoPeriodos.Any())
                return null;

            var frequenciaAluno = new FrequenciaAluno()
            {
                TotalAulas = frequenciaAlunoPeriodos.Sum(f => f.TotalAulas),
                TotalAusencias = frequenciaAlunoPeriodos.Sum(f => f.TotalAusencias),
                TotalCompensacoes = frequenciaAlunoPeriodos.Sum(f => f.TotalCompensacoes),
            };

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.TurmaCodigo));
            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre));
            var periodos = await mediator.Send(new ObterPeridosEscolaresPorTipoCalendarioIdQuery(tipoCalendario.Id));

            periodos.ToList().ForEach(p =>
            {
                var frequenciaCorrespondente = frequenciaAlunoPeriodos.OrderByDescending(x => x.AlteradoEm ?? x.CriadoEm).FirstOrDefault(x => x.Bimestre == p.Bimestre);
                frequenciaAluno.AdicionarFrequenciaBimestre(p.Bimestre, frequenciaCorrespondente.NaoEhNulo() ? frequenciaCorrespondente.PercentualFrequencia : 0);
            });

            return frequenciaAluno;
        }
    }
}
