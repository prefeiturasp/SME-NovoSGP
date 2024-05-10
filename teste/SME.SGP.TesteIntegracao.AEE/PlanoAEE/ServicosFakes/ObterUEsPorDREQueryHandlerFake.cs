using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.AEE.PlanoAEE.ServicosFakes
{
    public class ObterUEsPorDREQueryHandlerFake : IRequestHandler<ObterUEsPorDREQuery, IEnumerable<AbrangenciaUeRetorno>>
    {
        public async Task<IEnumerable<AbrangenciaUeRetorno>> Handle(ObterUEsPorDREQuery request, CancellationToken cancellationToken)
        {
            var abrangenciaLista = new List<AbrangenciaUeRetorno>();

            abrangenciaLista.Add(new AbrangenciaUeRetorno
            {
                Id = 2,
                Codigo = "2",
                NomeSimples = "NOME DA UE",
            });

            abrangenciaLista.Add(new AbrangenciaUeRetorno
            {
                Id = 3,
                Codigo = "3",
                NomeSimples = "NOME DA UE",
            });

            return await Task.FromResult(abrangenciaLista);
        }
    }
}
