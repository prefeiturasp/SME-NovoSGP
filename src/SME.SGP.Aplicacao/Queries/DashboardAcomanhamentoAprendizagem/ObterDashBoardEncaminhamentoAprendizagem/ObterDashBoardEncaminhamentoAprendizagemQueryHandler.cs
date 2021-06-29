using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashBoardEncaminhamentoAprendizagemQueryHandler : IRequestHandler<ObterDashBoardEncaminhamentoAprendizagemQuery, IEnumerable<DashboardAcompanhamentoAprendizagemDto>>
    {
        private readonly IRepositorioConsolidacaoAcompanhamentoAprendizagemAluno repositorioConsolidacaoAcompanhamentoAprendizagemAluno;

        public ObterDashBoardEncaminhamentoAprendizagemQueryHandler(IRepositorioConsolidacaoAcompanhamentoAprendizagemAluno repositorioConsolidacaoAcompanhamentoAprendizagemAluno)
        {
            this.repositorioConsolidacaoAcompanhamentoAprendizagemAluno = repositorioConsolidacaoAcompanhamentoAprendizagemAluno ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoAcompanhamentoAprendizagemAluno));
        }

        public async Task<IEnumerable<DashboardAcompanhamentoAprendizagemDto>> Handle(ObterDashBoardEncaminhamentoAprendizagemQuery request, CancellationToken cancellationToken)
            => await repositorioConsolidacaoAcompanhamentoAprendizagemAluno.ObterConsolidacao(request.AnoLetivo, request.DreId, request.UeId, request.Semestre);
    }
}
