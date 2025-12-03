using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQueryHandler : IRequestHandler<ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQuery, IEnumerable<AtendimentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto>>
    {
        private readonly IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA;
        public ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQueryHandler(IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public Task<IEnumerable<AtendimentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto>> Handle(ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQuery request, CancellationToken cancellationToken)
        {
            return this.repositorioEncaminhamentoNAAPA.ObterInformacoesDeNotificacaoDeInatividadeDeAtendimento(request.UeId);
        }
    }
}
