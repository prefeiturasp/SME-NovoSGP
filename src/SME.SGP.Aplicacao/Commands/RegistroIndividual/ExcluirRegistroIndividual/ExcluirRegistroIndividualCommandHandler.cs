using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRegistroIndividualCommandHandler : IRequestHandler<ExcluirRegistroIndividualCommand, bool>
    {
        private readonly IRepositorioRegistroIndividual repositorioRegistroIndividual;

        public ExcluirRegistroIndividualCommandHandler(IRepositorioRegistroIndividual repositorioRegistroIndividual)
        {
            this.repositorioRegistroIndividual = repositorioRegistroIndividual ?? throw new ArgumentNullException(nameof(repositorioRegistroIndividual));
        }

        public async Task<bool> Handle(ExcluirRegistroIndividualCommand request, CancellationToken cancellationToken)
        {
            var registroIndividual = await repositorioRegistroIndividual.ObterPorIdAsync(request.Id);
            if (registroIndividual == null)
                throw new NegocioException("Registro individual não encontrada.");

            registroIndividual.Remover();

            await repositorioRegistroIndividual.SalvarAsync(registroIndividual);

            return true;
        }
    }
}
