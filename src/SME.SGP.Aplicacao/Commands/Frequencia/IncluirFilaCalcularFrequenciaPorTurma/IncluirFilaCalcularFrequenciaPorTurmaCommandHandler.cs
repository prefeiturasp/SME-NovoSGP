using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using static SME.SGP.Dominio.DateTimeExtension;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaCalcularFrequenciaPorTurmaCommandHandler : IRequestHandler<IncluirFilaCalcularFrequenciaPorTurmaCommand, bool>
    {
        private readonly IRepositorioProcessoExecutando repositorioProcessoExecutando;
        private readonly IMediator mediator;

        public IncluirFilaCalcularFrequenciaPorTurmaCommandHandler(IRepositorioProcessoExecutando repositorioProcessoExecutando, IMediator mediator)
        {
            this.repositorioProcessoExecutando = repositorioProcessoExecutando ?? throw new ArgumentNullException(nameof(repositorioProcessoExecutando));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(IncluirFilaCalcularFrequenciaPorTurmaCommand request, CancellationToken cancellationToken)
        {
            //Verificar se já há essa turma/bimestre/disciplina
            var processoNaFila = await repositorioProcessoExecutando
                .ObterProcessoCalculoFrequenciaAsync(request.TurmaId, request.DisciplinaId, 0, TipoProcesso.CalculoFrequenciaFilaRabbit);
            
            if (processoNaFila != null)
            {
                var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.TurmaId));

                var parametroTempoValidade = await mediator
                    .Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.TempoValidadeProcessoExecutandoEmSegundos, turma.AnoLetivo));

                if (parametroTempoValidade != null)
                {
                    var tempoValidade = int.Parse(parametroTempoValidade.Valor);
                    if (processoNaFila.CriadoEm.AddSeconds(tempoValidade) < HorarioBrasilia())
                    {
                        await repositorioProcessoExecutando.RemoverAsync(processoNaFila);
                        processoNaFila = null;
                    }
                }               
            }

            if (processoNaFila == null)
            {
                await repositorioProcessoExecutando.SalvarAsync(new ProcessoExecutando()
                {
                    Bimestre = 0,
                    DisciplinaId = request.DisciplinaId,
                    TipoProcesso = TipoProcesso.CalculoFrequenciaFilaRabbit,
                    TurmaId = request.TurmaId,
                    CriadoEm = HorarioBrasilia()
                });

                var comando = new CalcularFrequenciaPorTurmaCommand(request.Alunos, request.DataAula, request.TurmaId, request.DisciplinaId, 0);

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaCalculoFrequenciaPorTurmaComponente, comando, Guid.NewGuid(), null));
            }

            return true;
        }
    }
}