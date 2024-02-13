using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverRegistroColetivoCommandHandler : IRequestHandler<RemoverRegistroColetivoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRegistroColetivo repositorio;
        private readonly IUnitOfWork unitOfWork;

        public RemoverRegistroColetivoCommandHandler(IMediator mediator,
                                                     IRepositorioRegistroColetivo repositorio,
                                                     IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(RemoverRegistroColetivoCommand request, CancellationToken cancellationToken)
        {
            var registroColetivo = await repositorio.ObterPorIdAsync(request.RegistroColetivoId);

            if (registroColetivo.EhNulo())
                throw new NegocioException(MensagemNegocioComuns.REGISTRO_COLETIVO_NAO_ENCONTRADA);

            unitOfWork.IniciarTransacao();
            try
            {
                if (registroColetivo.Anexos.NaoEhNulo() && registroColetivo.Anexos.Any())
                {
                    await RemoverArquivos(registroColetivo.Anexos);
                    await mediator.Send(new RemoverRegistroColetivoAnexoCommand(registroColetivo.Id));
                }

                await mediator.Send(new RemoverRegistroColetivoUeCommand(registroColetivo.Id));

                await repositorio.RemoverAsync(registroColetivo);

                unitOfWork.PersistirTransacao();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
            }

            return true;
        }

        private async Task RemoverArquivos(IEnumerable<Arquivo> arquivos)
        {
            foreach (var arquivo in arquivos)
                await mediator.Send(new ExcluirArquivoPorIdCommand(arquivo.Id));
        }
    }
}
