using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSituacaoFechamentoTurmaComponenteQueryHandler : IRequestHandler<ObterSituacaoFechamentoTurmaComponenteQuery, SituacaoFechamento>
    {
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;

        public ObterSituacaoFechamentoTurmaComponenteQueryHandler(IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }

        public async Task<SituacaoFechamento> Handle(ObterSituacaoFechamentoTurmaComponenteQuery request, CancellationToken cancellationToken)
            => await repositorioFechamentoTurmaDisciplina.ObterSituacaoFechamento(request.TurmaId, request.ComponenteCurricularId, request.PeriodoEscolarId);
    }
}
