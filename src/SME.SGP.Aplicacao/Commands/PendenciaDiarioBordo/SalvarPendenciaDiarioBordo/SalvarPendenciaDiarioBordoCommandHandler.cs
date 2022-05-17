using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaDiarioBordoCommandHandler : AsyncRequestHandler<SalvarPendenciaDiarioBordoCommand>
    {
        private readonly IRepositorioPendenciaDiarioBordo repositorioDiarioBordo;
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public SalvarPendenciaDiarioBordoCommandHandler(IRepositorioPendenciaDiarioBordo repositorioDiarioBordo, IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override async Task Handle(SalvarPendenciaDiarioBordoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(request.ProfessorComponente.CodigoRf));

                await SalvarPendenciaDiario(request.Aula.Id, request.ProfessorComponente.CodigoRf, request.ProfessorComponente.DisciplinaId, request.PendenciaId, usuarioId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task SalvarPendenciaDiario(long aulaId, string codigoRf, long disciplinaId, long pendenciaId, long usuarioId)
        {
            try
            {
                unitOfWork.IniciarTransacao();

                var existePendenciaUsuario = await mediator.Send(new ObterPendenciasUsuarioPorPendenciaUsuarioIdQuery(pendenciaId, usuarioId));

                if (!existePendenciaUsuario)
                    await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, usuarioId));

                var pendenciaDiarioBordo = new PendenciaDiarioBordo()
                {
                    PendenciaId = pendenciaId,
                    ProfessorRf = codigoRf,
                    ComponenteId = disciplinaId,
                    AulaId = aulaId
                };
                await repositorioDiarioBordo.SalvarAsync(pendenciaDiarioBordo);

                unitOfWork.PersistirTransacao();
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                throw ex;
            }
        }
    }
}
