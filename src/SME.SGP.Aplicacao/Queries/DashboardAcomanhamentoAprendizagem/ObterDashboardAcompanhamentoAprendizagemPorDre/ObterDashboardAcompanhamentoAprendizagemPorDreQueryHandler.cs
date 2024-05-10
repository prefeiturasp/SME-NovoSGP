using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardAcompanhamentoAprendizagemPorDreQueryHandler : IRequestHandler<ObterDashboardAcompanhamentoAprendizagemPorDreQuery, IEnumerable<DashboardAcompanhamentoAprendizagemPorDreDto>>
    {
        private readonly IRepositorioConsolidacaoAcompanhamentoAprendizagemAluno repositorioConsolidacaoAcompanhamentoAprendizagemAluno;

        public ObterDashboardAcompanhamentoAprendizagemPorDreQueryHandler(IRepositorioConsolidacaoAcompanhamentoAprendizagemAluno repositorioConsolidacaoAcompanhamentoAprendizagemAluno)
        {
            this.repositorioConsolidacaoAcompanhamentoAprendizagemAluno = repositorioConsolidacaoAcompanhamentoAprendizagemAluno ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoAcompanhamentoAprendizagemAluno));
        }

        public async Task<IEnumerable<DashboardAcompanhamentoAprendizagemPorDreDto>> Handle(ObterDashboardAcompanhamentoAprendizagemPorDreQuery request, CancellationToken cancellationToken)
            => await repositorioConsolidacaoAcompanhamentoAprendizagemAluno.ObterConsolidacaoPorDre(request.AnoLetivo, request.Semestre);
    }
}
