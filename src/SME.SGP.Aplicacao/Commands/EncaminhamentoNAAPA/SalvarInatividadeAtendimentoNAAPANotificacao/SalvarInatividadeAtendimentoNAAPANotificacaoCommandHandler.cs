using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarInatividadeAtendimentoNAAPANotificacaoCommandHandler : IRequestHandler<SalvarInatividadeAtendimentoNAAPANotificacaoCommand, long>
    {
        private readonly IRepositorioInatividadeAtendimentoNAAPANotificacao repositorio;

        public SalvarInatividadeAtendimentoNAAPANotificacaoCommandHandler(IRepositorioInatividadeAtendimentoNAAPANotificacao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<long> Handle(SalvarInatividadeAtendimentoNAAPANotificacaoCommand request, CancellationToken cancellationToken)
        {
            var inatividadeAtendimentoNotificacao = new InatividadeAtendimentoNAAPANotificacao()
            {
                EncaminhamentoNAAPAId = request.EncaminhamentoNAAPAId,
                NotificacaoId = request.NotificacaoId
            };

            return await repositorio.SalvarAsync(inatividadeAtendimentoNotificacao);
        }
    }
}
