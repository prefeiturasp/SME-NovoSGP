using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ObterSecoesPorEtapaDeEncaminhamentoNAAPAUseCase : IObterSecoesPorEtapaDeEncaminhamentoNAAPAUseCase
    {
        private readonly IMediator mediator;

        public ObterSecoesPorEtapaDeEncaminhamentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Executar(long encaminhamentoNAAPAId)
        {
            var listaEtapas = new List<int>() { (int)EtapaEncaminhamentoNAAPA.PrimeiraEtapa };
            return await mediator.Send(new ObterSecoesPorEtapaDeEncaminhamentoNAAPAQuery(listaEtapas, encaminhamentoNAAPAId));
        }
    }
}
