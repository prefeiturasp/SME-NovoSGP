using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterValidacaoPodePersistirTurmaNasDatasQueryHandlerFake : IRequestHandler<ObterValidacaoPodePersistirTurmaNasDatasQuery, List<PodePersistirNaDataRetornoEolDto>>
    {
        public async Task<List<PodePersistirNaDataRetornoEolDto>> Handle(ObterValidacaoPodePersistirTurmaNasDatasQuery request, CancellationToken cancellationToken)
        {
            return new List<PodePersistirNaDataRetornoEolDto>()
            {
                new PodePersistirNaDataRetornoEolDto()
                {
                    PodePersistir = false
                }
            };
        }
    }
}
