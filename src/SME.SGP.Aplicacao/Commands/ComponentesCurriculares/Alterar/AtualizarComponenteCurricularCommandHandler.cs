using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class AtualizarComponenteCurricularCommandHandler : IRequestHandler<AtualizarComponenteCurricularCommand, bool>
    {
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;

        public AtualizarComponenteCurricularCommandHandler(IRepositorioComponenteCurricular repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }

        public async Task<bool> Handle(AtualizarComponenteCurricularCommand request, CancellationToken cancellationToken)
        {
            await repositorioComponenteCurricular.Atualizar(request.ComponenteCurricular);

            return true;
        }
    }
}
