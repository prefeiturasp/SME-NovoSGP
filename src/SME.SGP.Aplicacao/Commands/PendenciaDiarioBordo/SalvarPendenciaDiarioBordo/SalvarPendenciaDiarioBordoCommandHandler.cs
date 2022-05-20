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

                await SalvarPendenciaDiario(request, usuarioId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task SalvarPendenciaDiario(SalvarPendenciaDiarioBordoCommand request, long usuarioId)
        {
            try
            {
                var pendenciaId = await mediator.Send(new ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQuery(request.ProfessorComponente.DisciplinaId, request.ProfessorComponente.CodigoRf, request.Aula.PeriodoEscolarId));

                unitOfWork.IniciarTransacao();

                if (pendenciaId == 0)
                    pendenciaId = await mediator.Send(MapearPendencia(TipoPendencia.DiarioBordo, request.ProfessorComponente.DescricaoComponenteCurricular, request.TurmaComModalidade, request.NomeEscola));

                var existePendenciaUsuario = await mediator.Send(new ObterPendenciasUsuarioPorPendenciaUsuarioIdQuery(pendenciaId, usuarioId));

                if (!existePendenciaUsuario)
                    await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, usuarioId));

                var pendenciaDiarioBordo = new PendenciaDiarioBordo()
                {
                    PendenciaId = pendenciaId,
                    ProfessorRf = request.ProfessorComponente.CodigoRf,
                    ComponenteId = request.ProfessorComponente.DisciplinaId,
                    AulaId = request.Aula.Id
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
