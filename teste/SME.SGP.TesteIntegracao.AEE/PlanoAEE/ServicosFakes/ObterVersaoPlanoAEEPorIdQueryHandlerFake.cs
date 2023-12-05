using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes
{
    public class ObterVersaoPlanoAEEPorIdQueryHandlerFake : IRequestHandler<ObterVersaoPlanoAEEPorIdQuery, PlanoAEEVersaoDto>
    {
        public async Task<PlanoAEEVersaoDto> Handle(ObterVersaoPlanoAEEPorIdQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new PlanoAEEVersaoDto()
            {
                Id = 1,
                Numero = 1,
                AlteradoEm = null,
                AlteradoPor = null,
                AlteradoRF = null,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                PlanoAEEId = 1
            });
        }
    }
}