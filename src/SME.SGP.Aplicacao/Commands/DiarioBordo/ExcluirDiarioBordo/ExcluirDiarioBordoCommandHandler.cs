using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using Minio.DataModel;

namespace SME.SGP.Aplicacao
{
    public class ExcluirDiarioBordoCommandHandler : IRequestHandler<ExcluirDiarioBordoCommand, bool>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;
        private readonly IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao;
        private readonly IMediator mediator;
        public readonly IUnitOfWork unitOfWork;

        public ExcluirDiarioBordoCommandHandler(IRepositorioDiarioBordo repositorioDiarioBordo, 
                                                IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao, 
                                                IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
            this.repositorioDiarioBordoObservacao = repositorioDiarioBordoObservacao ?? throw new ArgumentNullException(nameof(repositorioDiarioBordoObservacao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(ExcluirDiarioBordoCommand request, CancellationToken cancellationToken)
        {
            var diarioBordo = await repositorioDiarioBordo.ObterPorIdAsync(request.DiarioBordoId);
            if (diarioBordo.Excluido) return false;

            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var observacoesDiarioBordo = await repositorioDiarioBordoObservacao.ListarPorDiarioBordoAsync(request.DiarioBordoId, usuario.Id);
            unitOfWork.IniciarTransacao();
            try
            {
                foreach (var obs in observacoesDiarioBordo)
                    await mediator.Send(new ExcluirObservacaoDiarioBordoCommand(obs.Id));
                await repositorioDiarioBordo.RemoverLogico(request.DiarioBordoId);
                await mediator.Send(new SalvarPendenciaAoExcluirDiarioBordoCommand(request.DiarioBordoId));
                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
            return true;
        }
    }
}
