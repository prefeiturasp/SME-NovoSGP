using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtendimentosNAAPAComInatividadeDeAtendimentoQueryHandler : IRequestHandler<ObterAtendimentosNAAPAComInatividadeDeAtendimentoQuery, IEnumerable<AtendimentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto>>
    {
        private readonly IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA;
        public ObterAtendimentosNAAPAComInatividadeDeAtendimentoQueryHandler(IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public Task<IEnumerable<AtendimentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto>> Handle(ObterAtendimentosNAAPAComInatividadeDeAtendimentoQuery request, CancellationToken cancellationToken)
        {
            return this.repositorioEncaminhamentoNAAPA.ObterInformacoesDeNotificacaoDeInatividadeDeAtendimento(request.UeId);
        }
    }
}
