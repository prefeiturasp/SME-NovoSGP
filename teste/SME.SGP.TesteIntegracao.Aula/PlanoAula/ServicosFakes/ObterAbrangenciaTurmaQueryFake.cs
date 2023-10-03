using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PlanoAula.ServicosFakes
{
    public class ObterAbrangenciaTurmaQueryFake : IRequestHandler<ObterAbrangenciaTurmaQuery, AbrangenciaFiltroRetorno>
    {
        public async Task<AbrangenciaFiltroRetorno> Handle(ObterAbrangenciaTurmaQuery request, CancellationToken cancellationToken)
        {
            return new AbrangenciaFiltroRetorno();
        }
    }
}
