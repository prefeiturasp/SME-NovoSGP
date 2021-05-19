using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CarregarDresConsolidacaoMatriculaUseCase : ICarregarDresConsolidacaoMatriculaUseCase
    {
        private readonly IMediator mediator;

        public CarregarDresConsolidacaoMatriculaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar()
        {
            var dres = await mediator.Send(new ObterIdsDresQuery());
            foreach (var dreId in dres)
            {
                var dre = new DreMatriculaDto(dreId);
                try
                {
                    var publicarTratamentoCiclo = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.SincronizarDresMatriculasTurmas, dre, new Guid(), null, fila: RotasRabbit.SincronizarDresMatriculasTurmas));
                    if (!publicarTratamentoCiclo)
                    {
                        var mensagem = $"Não foi possível inserir a dre : {publicarTratamentoCiclo} na fila de sync.";
                        SentrySdk.CaptureMessage(mensagem);
                    }
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                }
            }
            return true;
        }
    }
}
