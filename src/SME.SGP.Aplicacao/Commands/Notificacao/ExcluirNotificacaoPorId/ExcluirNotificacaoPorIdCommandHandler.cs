using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoPorIdCommandHandler : IRequestHandler<ExcluirNotificacaoPorIdCommand, bool>
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public ExcluirNotificacaoPorIdCommandHandler(IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
        }

        public async Task<bool> Handle(ExcluirNotificacaoPorIdCommand request, CancellationToken cancellationToken)
        {
            repositorioNotificacao.Remover(request.Id);
            return true;
        }
    }
}
