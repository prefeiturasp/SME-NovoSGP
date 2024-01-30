using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosAcaoCriancaEstudanteAusenteUseCase : IObterRegistrosAcaoCriancaEstudanteAusenteUseCase
    {
        private readonly IMediator mediator;

        public ObterRegistrosAcaoCriancaEstudanteAusenteUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto>> Executar(FiltroRegistrosAcaoCriancasEstudantesAusentesDto filtro)
        {
            return await mediator.Send(new ObterRegistrosAcaoCriancaEstudanteAusenteQuery(filtro.CodigoAluno, filtro.TurmaId));
        }
    }
}
