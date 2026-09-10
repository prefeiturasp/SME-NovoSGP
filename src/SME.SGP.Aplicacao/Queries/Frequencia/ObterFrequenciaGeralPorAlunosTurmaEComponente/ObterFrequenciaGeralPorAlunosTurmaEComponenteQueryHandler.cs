using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralPorAlunosTurmaEComponenteQueryHandler : IRequestHandler<ObterFrequenciaGeralPorAlunosTurmaEComponenteQuery, IEnumerable<FrequenciaAluno>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterFrequenciaGeralPorAlunosTurmaEComponenteQueryHandler(
                                                    IMediator mediator,
                                                    IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }

        public async Task<IEnumerable<FrequenciaAluno>> Handle(ObterFrequenciaGeralPorAlunosTurmaEComponenteQuery request, CancellationToken cancellationToken)
        {
            var frequenciaAlunosPeriodos = await repositorioFrequenciaAlunoDisciplinaPeriodo
                .ObterFrequenciaGeralPorAlunosTurmaEComponente(request.AlunosCodigos, request.TurmaCodigo, request.ComponenteCurricularCodigo);

            if (frequenciaAlunosPeriodos.EhNulo() || !frequenciaAlunosPeriodos.Any())
                return Enumerable.Empty<FrequenciaAluno>();

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.TurmaCodigo));
            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre));
            var periodos = await mediator.Send(new ObterPeridosEscolaresPorTipoCalendarioIdQuery(tipoCalendario.Id));

            var frequenciaRetorno = new List<FrequenciaAluno>();

            foreach (var grupo in frequenciaAlunosPeriodos.GroupBy(fa => fa.CodigoAluno))
            {
                var frequenciaAluno = new FrequenciaAluno()
                {
                    CodigoAluno = grupo.Key,
                    TotalAulas = grupo.Sum(f => f.TotalAulas),
                    TotalAusencias = grupo.Sum(f => f.TotalAusencias),
                    TotalCompensacoes = grupo.Sum(f => f.TotalCompensacoes),
                };

                periodos.ToList().ForEach(p =>
                {
                    var frequenciaCorrespondente = grupo.OrderByDescending(x => x.AlteradoEm ?? x.CriadoEm).FirstOrDefault(x => x.Bimestre == p.Bimestre);
                    frequenciaAluno.AdicionarFrequenciaBimestre(p.Bimestre, frequenciaCorrespondente.NaoEhNulo() ? frequenciaCorrespondente.PercentualFrequencia : 0);
                });

                frequenciaRetorno.Add(frequenciaAluno);
            }

            return frequenciaRetorno;
        }
    }
}
