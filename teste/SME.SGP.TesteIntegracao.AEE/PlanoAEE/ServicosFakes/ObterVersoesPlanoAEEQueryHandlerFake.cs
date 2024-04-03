using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes
{
    public class ObterVersoesPlanoAEEQueryHandlerFake : IRequestHandler<ObterVersoesPlanoAEEQuery, IEnumerable<PlanoAEEVersaoDto>>
    {
        public Task<IEnumerable<PlanoAEEVersaoDto>> Handle(ObterVersoesPlanoAEEQuery request, CancellationToken cancellationToken)
        {
            var versoes = new List<PlanoAEEVersaoDto>()
            {
                new PlanoAEEVersaoDto()
                {
                    Id = 1,
                    Numero = 2,
                    AlteradoEm = null,
                    AlteradoPor = null,
                    AlteradoRF = null,
                    CriadoEm = DateTime.Now,
                    CriadoPor = "Sistema",
                    CriadoRF = "1",
                    PlanoAEEId = 1
                }
            };

            return Task.FromResult<IEnumerable<PlanoAEEVersaoDto>>(versoes);
        }
    }
}