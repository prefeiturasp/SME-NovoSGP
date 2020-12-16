using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

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
            var processoNaFila = await repositorioProcessoExecutando.ObterProcessoCalculoFrequenciaAsync(request.TurmaId, request.DisciplinaId, request.Bimestre,
                Dominio.TipoProcesso.CalculoFrequenciaFilaRabbit);

            if (processoNaFila == null)
            {
                 await repositorioProcessoExecutando.SalvarAsync(new Dominio.ProcessoExecutando()
                {
                    Bimestre = request.Bimestre,
                    DisciplinaId = request.DisciplinaId,
                    TipoProcesso = Dominio.TipoProcesso.CalculoFrequenciaFilaRabbit,
                    TurmaId = request.TurmaId
                });

                var comando = new CalcularFrequenciaPorTurmaCommand(request.Alunos, request.DataAula, request.TurmaId, request.DisciplinaId, request.Bimestre);
                
                //var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.RotaCalculoFrequenciaPorTurmaComponente, comando, Guid.NewGuid(), null));
            }

            return true;
        }
    }
}