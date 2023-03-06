using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterValidacaoPodePersistirTurmaNasDatasQueryHandlerFake : IRequestHandler<ObterValidacaoPodePersistirTurmaNasDatasQuery, List<PodePersistirNaDataRetornoEolDto>>
    {
        public ObterValidacaoPodePersistirTurmaNasDatasQueryHandlerFake()
        {}

        public async Task<List<PodePersistirNaDataRetornoEolDto>> Handle(ObterValidacaoPodePersistirTurmaNasDatasQuery request, CancellationToken cancellationToken)
        {
            return request.DateTimes.ToList().Select(s=> new PodePersistirNaDataRetornoEolDto() 
            {
                Data = s,
                PodePersistir = true
            }).ToList();
                
        }
    }
}