using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes;

namespace SME.SGP.Aplicacao
{
    public class GravarFechamentoTurmaConselhoClasseCommandHandler : IRequestHandler<GravarFechamentoTurmaConselhoClasseCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;

        public GravarFechamentoTurmaConselhoClasseCommandHandler(
                                                IMediator mediator,
                                                IUnitOfWork unitOfWork,
                                                IRepositorioFechamentoTurma repositorioFechamentoTurma,
                                                IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }

        public async Task<bool> Handle(GravarFechamentoTurmaConselhoClasseCommand request, CancellationToken cancellationToken)
        {
            if (request.FechamentoDeTurma.PeriodoEscolarId.HasValue)
            {
                // Fechamento Bimestral
                if (!await mediator.Send(new TurmaEmPeriodoDeFechamentoQuery(request.FechamentoDeTurma.Turma, DateTime.Today, request.FechamentoDeTurma.PeriodoEscolar.Bimestre), cancellationToken))
                    throw new NegocioException($"Turma {request.FechamentoDeTurma.Turma.Nome} não esta em período de fechamento para o {request.FechamentoDeTurma.PeriodoEscolar.Bimestre}º Bimestre!");
            }
            else
            {
                // Fechamento Final
                if (request.FechamentoDeTurma.Turma.AnoLetivo != 2020 && !request.FechamentoDeTurma.Turma.Historica)
                {
                    var validacaoConselhoFinal = await mediator.Send(new ObterUltimoBimestreTurmaQuery(request.FechamentoDeTurma.Turma), cancellationToken);

                    if (!validacaoConselhoFinal.possuiConselho && request.FechamentoDeTurma.Turma.AnoLetivo == DateTime.Today.Year)
                        throw new NegocioException($"Para salvar a nota final você precisa registrar o conselho de classe do {validacaoConselhoFinal.bimestre}º bimestre");
                }
            }
            
            var consolidacaoTurma = new ConsolidacaoTurmaDto(request.FechamentoDeTurma.Turma.Id, request.Bimestre ?? 0);
            var mensagemParaPublicar = JsonConvert.SerializeObject(consolidacaoTurma);

            try
            {
                unitOfWork.IniciarTransacao();
                
                if (request.FechamentoDeTurmaDisciplina.DisciplinaId > 0)
                {
                    await repositorioFechamentoTurma.SalvarAsync(request.FechamentoDeTurma);
                    request.FechamentoDeTurmaDisciplina.FechamentoTurmaId = request.FechamentoDeTurma.Id;
                    await repositorioFechamentoTurmaDisciplina.SalvarAsync(request.FechamentoDeTurmaDisciplina);
                }
                
                unitOfWork.PersistirTransacao();
                
                await RemoverCache(string.Format(NomeChaveCache.CHAVE_FECHAMENTO_NOTA_TURMA_BIMESTRE, request.FechamentoDeTurma.Turma.CodigoTurma, request.Bimestre), cancellationToken);
                
                await RemoverCache(string.Format(NomeChaveCache.CHAVE_FECHAMENTO_NOTA_TURMA_PERIODO_COMPONENTE,
                    request.FechamentoDeTurma.TurmaId, request.FechamentoDeTurma.PeriodoEscolarId, request.FechamentoDeTurmaDisciplina.DisciplinaId), cancellationToken);

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaFechamentoSync, mensagemParaPublicar, Guid.NewGuid()), cancellationToken);
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }

            return true;
        }
        
        private async Task RemoverCache(string nomeChave, CancellationToken cancellationToken)
        {
            await mediator.Send(new RemoverChaveCacheCommand(nomeChave), cancellationToken);
        }        
    }
}
