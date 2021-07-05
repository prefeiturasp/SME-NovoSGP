using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarComunicadoCommandHandler : IRequestHandler<AlterarComunicadoCommand, bool>
    {
        private readonly IRepositorioComunicado repositorioComunicado;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public AlterarComunicadoCommandHandler(IRepositorioComunicado repositorioComunicado, IUnitOfWork unitOfWork, IMediator mediator)
        {
            this.repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(AlterarComunicadoCommand request, CancellationToken cancellationToken)
        {
            var comunicado = await mediator.Send(new ObterComunicadoSimplesPorIdQuery(request.Id));

            if (comunicado == null)
                throw new NegocioException($"Comunicado não localizado");

            MapearAlteracao(request, comunicado);

            try
            {
                unitOfWork.IniciarTransacao();

                await repositorioComunicado.SalvarAsync(comunicado);

                await mediator.Send(new AlterarNotificacaoEscolaAquiCommand(comunicado));

                unitOfWork.PersistirTransacao();

                return true;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private void MapearAlteracao(AlterarComunicadoCommand request, Comunicado comunicado)
        {
            comunicado.Descricao = request.Descricao;
            comunicado.Titulo = request.Titulo;
            comunicado.DataExpiracao = request.DataExpiracao;
            comunicado.TipoCalendarioId = request.TipoCalendarioId;
            comunicado.EventoId = request.EventoId;
        }
    }
}
