using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dto;

namespace SME.SGP.TesteIntegracao.Ocorrencia.ServicosFakes
{
    public class ObterUEsPorDREQueryHandlerFiltroTodasUesFake : IRequestHandler<ObterUEsPorDREQuery, IEnumerable<AbrangenciaUeRetorno>>
    {
        public async Task<IEnumerable<AbrangenciaUeRetorno>> Handle(ObterUEsPorDREQuery request, CancellationToken cancellationToken)
        {
            return new List<AbrangenciaUeRetorno>()
            {
                new AbrangenciaUeRetorno
                {
                    Codigo = "2",
                    NomeSimples = "UE 2",
                    TipoEscola = TipoEscola.EMEFM,
                    Id = 2
                },
                new AbrangenciaUeRetorno
                {
                    Codigo = "3",
                    NomeSimples = "UE 3",
                    TipoEscola = TipoEscola.EMEFM,
                    Id = 3
                },
            };
        }
    }
}