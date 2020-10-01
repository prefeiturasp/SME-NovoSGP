using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanejamentoAnualUseCase : ISalvarPlanejamentoAnualUseCase
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public SalvarPlanejamentoAnualUseCase(IMediator mediator,
                                              IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<PlanejamentoAnualAuditoriaDto> Executar(long turmaId, long componenteCurricularId, SalvarPlanejamentoAnualDto dto)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            var turma = await mediator.Send(new ObterTurmaSimplesPorIdQuery(turmaId));
            foreach (var periodoEscolar in dto.PeriodosEscolares)
            {
                var periodo = await mediator.Send(new ObterPeriodoEscolarePorIdQuery(periodoEscolar.PeriodoEscolarId));
                var temAtribuicao = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQuery(componenteCurricularId, turma.Codigo, usuario.CodigoRf, periodo.PeriodoInicio.Date, periodo.PeriodoFim.Date));
                if (!temAtribuicao)
                    throw new NegocioException($"Você não está atribuido ao período de {periodo.PeriodoInicio:dd/MM/yyyy} à {periodo.PeriodoFim:dd/MM/yyyy} do {periodo.Bimestre}° Bimestre.");
            }
            
            unitOfWork.IniciarTransacao();

            var auditoria = await mediator.Send(new SalvarPlanejamentoAnualCommand()
            {
                TurmaId = turmaId,
                ComponenteCurricularId = componenteCurricularId,
                PeriodosEscolares = dto.PeriodosEscolares
            });

            unitOfWork.PersistirTransacao();

            return auditoria;
        }
    }
}
