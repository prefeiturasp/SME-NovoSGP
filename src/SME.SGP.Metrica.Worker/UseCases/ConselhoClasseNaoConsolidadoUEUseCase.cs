using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class ConselhoClasseNaoConsolidadoUEUseCase : IConselhoClasseNaoConsolidadoUEUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioConselhoClasseNaoConsolidado repositorioConselhoNaoConsolidado;
        private readonly IMediator mediator;

        public ConselhoClasseNaoConsolidadoUEUseCase(IRepositorioSGPConsulta repositorioSGP,
                                                     IRepositorioConselhoClasseNaoConsolidado repositorioConselhoNaoConsolidado,
                                                     IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioConselhoNaoConsolidado = repositorioConselhoNaoConsolidado ?? throw new System.ArgumentNullException(nameof(repositorioConselhoNaoConsolidado));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var ue = mensagem.ObterObjetoMensagem<FiltroIdDto>();

            var conselhosNaoConsolidados = await repositorioSGP.ObterConselhosClasseNaoConsolidados(ue.Id);
            foreach (var conselhoNaoConsolidado in conselhosNaoConsolidados)
                await repositorioConselhoNaoConsolidado.InserirAsync(conselhoNaoConsolidado);

            foreach (var conselhoDivergenteReconsolidar in conselhosNaoConsolidados.GroupBy(cc => new { cc.TurmaId, cc.AlunoCodigo, cc.Bimestre }))
            {
                var mensagemConsolidacaoConselhoClasseAluno = new MensagemConsolidacaoConselhoClasseAlunoDto(conselhoDivergenteReconsolidar.Key.AlunoCodigo,
                                                                                                         conselhoDivergenteReconsolidar.Key.TurmaId,
                                                                                                         conselhoDivergenteReconsolidar.Key.Bimestre,
                                                                                                         false);

                await mediator.Send(new PublicarFilaCommand(RotasRabbitSgpFechamento.ConsolidarTurmaConselhoClasseAlunoTratar, mensagemConsolidacaoConselhoClasseAluno));
            }    


            return true;
        }
    }
}
