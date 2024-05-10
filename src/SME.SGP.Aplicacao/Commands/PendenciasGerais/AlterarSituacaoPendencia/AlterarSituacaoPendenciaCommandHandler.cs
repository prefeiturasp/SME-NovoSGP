using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarSituacaoPendenciaCommandHandler : IRequestHandler<AlterarSituacaoPendenciaCommand, bool>
    {
        private readonly IRepositorioPendencia repositorioPendencia;

        public AlterarSituacaoPendenciaCommandHandler(IRepositorioPendencia repositorioPendencia)
        {
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }

        public async Task<bool> Handle(AlterarSituacaoPendenciaCommand request, CancellationToken cancellationToken)
        {
            var pendencia = await repositorioPendencia.ObterPorIdAsync(request.PendenciaId);

            if (pendencia.EhNulo())
                return false;

            pendencia.Situacao = request.NovaSituacao;
            await repositorioPendencia.SalvarAsync(pendencia);

            return true;
        }
    }
}
