using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesUseCase : AbstractUseCase, IObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesUseCase
    {
        public ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<TipoCalendarioRetornoDto>> Executar(int anoLetivo, IEnumerable<Modalidade> modalidades, string descricao)
            => await mediator.Send(new ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesQuery(anoLetivo,
                                                                                                  modalidades,
                                                                                                  descricao));
    }
}
