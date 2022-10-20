using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaAoExcluirDiarioBordoCommandHandler : AsyncRequestHandler<SalvarPendenciaAoExcluirDiarioBordoCommand>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;
        private readonly IMediator mediator;
        
        public SalvarPendenciaAoExcluirDiarioBordoCommandHandler(IRepositorioDiarioBordo repositorioDiarioBordo, IMediator mediator)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(SalvarPendenciaAoExcluirDiarioBordoCommand request, CancellationToken cancellationToken)
        {
            var diarioBordo = await repositorioDiarioBordo.ObterDadosDiarioBordoParaPendenciaPorid(request.DiarioBordoId);
            if (diarioBordo != null && diarioBordo.DataAula < DateTimeExtension.HorarioBrasilia().Date)
            {
                var diarioBordoExistente = await repositorioDiarioBordo.ObterPorAulaId(diarioBordo.AulaId, diarioBordo.ComponenteCurricularId);
                if (diarioBordoExistente == null)
                {
                    var salvarPendenciaCommand = new SalvarPendenciaCommand
                    {
                        TipoPendencia = TipoPendencia.DiarioBordo,
                        DescricaoComponenteCurricular = diarioBordo.DescricaoComponenteCurricular,
                        TurmaAnoComModalidade = diarioBordo.RetornarTurmaComModalidade(),
                        DescricaoUeDre = diarioBordo.NomeEscola,
                        TurmaId = diarioBordo.TurmaId
                    };
                    var pendenciaId = await mediator.Send(salvarPendenciaCommand);
                    if (pendenciaId != 0)
                        await mediator.Send(new SalvarPendenciaDiarioBordoCommand()
                        {
                            AulaId = diarioBordo.AulaId,
                            PendenciaId = pendenciaId,
                            ProfessorRf = diarioBordo.ProfessorRf,
                            ComponenteCurricularId = diarioBordo.ComponenteCurricularId
                        });
                }
            }
        }

    }
}
