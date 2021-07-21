using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigoNotificacaoPorIdQueryHandler : IRequestHandler<ObterCodigoNotificacaoPorIdQuery, long>
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public ObterCodigoNotificacaoPorIdQueryHandler(IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
        }

        public async Task<long> Handle(ObterCodigoNotificacaoPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioNotificacao.ObterCodigoPorId(request.NotificacaoId);
    }
}
