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
                foreach (var item in request.ProfessoresComponentes)
                {
                    var pendenciaIdExistente = await mediator.Send(new ObterPendenciaDiarioBordoPorComponenteTurmaCodigoQuery(item.DisciplinaId, request.TurmaCodigo));

                    var pendenciaId = pendenciaIdExistente > 0
                        ? pendenciaIdExistente
                        : await mediator.Send(MapearPendencia(TipoPendencia.DiarioBordo, item.DescricaoComponenteCurricular, request.TurmaComModalidade, request.DescricaoUeDre));

                    var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(item.CodigoRf));

                    await SalvarPendenciaDiario(request.Aula.Id, item.CodigoRf, item.DisciplinaId, pendenciaId, usuarioId);
                }
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

        private SalvarPendenciaCommand MapearPendencia(TipoPendencia tipoPendencia, string descricaoComponenteCurricular, string turmaAnoComModalidade, string descricaoUeDre)
        {
            return new SalvarPendenciaCommand
            {
                TipoPendencia = tipoPendencia,
                DescricaoComponenteCurricular = descricaoComponenteCurricular,
                TurmaAnoComModalidade = turmaAnoComModalidade,
                DescricaoUeDre = descricaoUeDre,
            };
        }
    }
}
