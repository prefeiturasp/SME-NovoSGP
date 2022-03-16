using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRegistroIndividualCommandHandler : IRequestHandler<ExcluirRegistroIndividualCommand, bool>
    {
        private readonly IRepositorioRegistroIndividual repositorioRegistroIndividual;
        private readonly IMediator mediator;

        public ExcluirRegistroIndividualCommandHandler(IRepositorioRegistroIndividual repositorioRegistroIndividual, IMediator mediator)
        {
            this.repositorioRegistroIndividual = repositorioRegistroIndividual ?? throw new ArgumentNullException(nameof(repositorioRegistroIndividual));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirRegistroIndividualCommand request, CancellationToken cancellationToken)
        {
            var registroIndividual = await repositorioRegistroIndividual.ObterPorIdAsync(request.Id);
            if (registroIndividual == null)
                throw new NegocioException("Registro individual não encontrada.");

            registroIndividual.Remover();

            await repositorioRegistroIndividual.SalvarAsync(registroIndividual);
            if (registroIndividual?.Registro != null)
            {
                await mediator.Send(new DeletarArquivoDeRegistroExcluidoCommand(registroIndividual.Registro, TipoArquivo.RegistroIndividual.Name()));
            }
            return true;
        }
    }
}
