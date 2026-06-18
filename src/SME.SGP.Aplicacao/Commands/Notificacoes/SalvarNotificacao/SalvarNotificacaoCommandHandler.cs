using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarNotificacaoCommandHandler : IRequestHandler<SalvarNotificacaoCommand, long>
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public SalvarNotificacaoCommandHandler(IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ??
                                          throw new ArgumentNullException(nameof(repositorioNotificacao));
        }

        public async Task<long> Handle(SalvarNotificacaoCommand request, CancellationToken cancellationToken)
        {
            return await repositorioNotificacao.SalvarAsync(request.Notificacao);
        }
    }
}