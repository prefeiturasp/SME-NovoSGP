using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class MigrarPlanejamentoAnualCommandHandler : AbstractUseCase, IRequestHandler<MigrarPlanejamentoAnualCommand, AuditoriaDto>
    {
        private readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar;
        private readonly IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente;
        private readonly IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem;
        public MigrarPlanejamentoAnualCommandHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual, IMediator mediator,
            IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar,
            IRepositorioTurma repositorioTurma,
            IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente,
            IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem
            ) : base(mediator)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.repositorioPlanejamentoAnualPeriodoEscolar = repositorioPlanejamentoAnualPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualPeriodoEscolar));
            this.repositorioPlanejamentoAnualComponente = repositorioPlanejamentoAnualComponente ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualComponente));
            this.repositorioPlanejamentoAnualObjetivosAprendizagem = repositorioPlanejamentoAnualObjetivosAprendizagem ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualObjetivosAprendizagem));
        }

        public async Task<AuditoriaDto> Handle(MigrarPlanejamentoAnualCommand comando, CancellationToken cancellationToken)
        {
            AuditoriaDto auditorias = new AuditoriaDto();

            var obterComponenteRegencia = await mediator.Send(new ObterComponentesCurricularesRegenciaPorTurmaUseCase());


            // Validando as turmas
            foreach(var turma in comando.Planejamento.TurmasDestinoIds)
            {
                var checarTurma = await repositorioTurma.ObterPorId(turma);
                if (checarTurma == null)
                    throw new NegocioException($"Turma não encontrada");

                foreach(var periodo in comando.Planejamento.PlanejamentoPeriodosEscolaresIds)
                {
                    var checarPeriodo = await repositorioPlanejamentoAnualPeriodoEscolar.ObterPorIdAsync(periodo);
                    if (checarPeriodo == null)
                        throw new NegocioException($"Período não encontrado");

                    var componente = await repositorioPlanejamentoAnualComponente.ObterPorPlanejamentoAnualPeriodoEscolarId(comando.Planejamento.ComponenteCurricularId, checarPeriodo.Id);

                    if(componente != null)
                    {
                        var objetivos = await repositorioPlanejamentoAnualObjetivosAprendizagem.ObterPorPlanejamentoAnualComponenteId(componente.Id);
                    }

                }
            }

            //var planoAnualOrigem  = await repositorioPlanejamentoAnual.ObterPlanejamentoSimplificadoPorTurmaEComponenteCurricular()

            return auditorias;
        }
    }
}
