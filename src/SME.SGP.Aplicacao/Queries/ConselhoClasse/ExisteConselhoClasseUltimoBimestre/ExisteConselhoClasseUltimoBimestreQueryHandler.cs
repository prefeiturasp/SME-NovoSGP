using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class ExisteConselhoClasseUltimoBimestreQueryHandler : IRequestHandler<ExisteConselhoClasseUltimoBimestreQuery, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAlunoConsulta;

        public ExisteConselhoClasseUltimoBimestreQueryHandler(IMediator mediator,IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAlunoConsulta)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioConselhoClasseAlunoConsulta = repositorioConselhoClasseAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAlunoConsulta));
        }
        
        public async Task<bool> Handle(ExisteConselhoClasseUltimoBimestreQuery request, CancellationToken cancellationToken)
        {

            var periodoEscolar = await ObterPeriodoUltimoBimestre(request.Turma);

            var conselhoClasseUltimoBimestre = await repositorioConselhoClasseAlunoConsulta.ObterPorPeriodoAsync(request.AlunoCodigo, request.Turma.Id, periodoEscolar.Id);

            if (conselhoClasseUltimoBimestre.EhNulo())
                return false;

            return await mediator.Send(new VerificaNotasTodosComponentesCurricularesQuery(request.AlunoCodigo, request.Turma, periodoEscolar.Bimestre, request.Turma.Historica));
        }
        
        private async Task<PeriodoEscolar> ObterPeriodoUltimoBimestre(Turma turma)
        {
            var periodoEscolarUltimoBimestre = await mediator.Send(new ObterUltimoPeriodoEscolarPorAnoModalidadeSemestreQuery(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre));
            if (periodoEscolarUltimoBimestre.EhNulo())
                throw new NegocioException(MensagemNegocioPeriodo.NAO_FOI_ENCONTRADO_PERIODO_ULTIMO_BIMESTRE);

            return periodoEscolarUltimoBimestre;
        }
    }
}