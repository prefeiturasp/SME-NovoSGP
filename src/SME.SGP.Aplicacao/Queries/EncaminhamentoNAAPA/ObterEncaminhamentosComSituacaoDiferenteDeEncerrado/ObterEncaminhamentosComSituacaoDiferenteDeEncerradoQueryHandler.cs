using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosComSituacaoDiferenteDeEncerradoQueryHandler : IRequestHandler<ObterEncaminhamentosComSituacaoDiferenteDeEncerradoQuery, IEnumerable<AtendimentoNAAPADto>>
    {
        private readonly IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNaapa;

        public ObterEncaminhamentosComSituacaoDiferenteDeEncerradoQueryHandler(IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNaapa)
        {
            this.repositorioEncaminhamentoNaapa = repositorioEncaminhamentoNaapa ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNaapa));
        }

        public Task<IEnumerable<AtendimentoNAAPADto>> Handle(ObterEncaminhamentosComSituacaoDiferenteDeEncerradoQuery request, CancellationToken cancellationToken)
        {
            return this.repositorioEncaminhamentoNaapa.ObterEncaminhamentosComSituacaoDiferenteDeEncerrado();
        }
    }
}
