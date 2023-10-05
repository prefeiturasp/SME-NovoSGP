using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterTurmaCodigosAlunoPorAnoLetivoUeTipoTurmaQueryFake  : IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoUeTipoTurmaQuery, string[]>
    {
        public async Task<string[]> Handle(ObterTurmaCodigosAlunoPorAnoLetivoUeTipoTurmaQuery request, CancellationToken cancellationToken)
        {
            var lista = new List<string>()
            {
                "1","2"
            };
            return lista.ToArray();
        }
    }
}