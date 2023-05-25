using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class VerificarSeExisteRecomendacaoPorTurmaQueryHandlerFake : IRequestHandler<VerificarSeExisteRecomendacaoPorTurmaQuery,IEnumerable<AlunoTemRecomandacaoDto>>
    {
        public async Task<IEnumerable<AlunoTemRecomandacaoDto>> Handle(VerificarSeExisteRecomendacaoPorTurmaQuery request, CancellationToken cancellationToken)
        {
            var lista = new List<AlunoTemRecomandacaoDto>()
            {
                new AlunoTemRecomandacaoDto
                {
                    AluncoCodigo = "1",
                    TemRecomendacao = true
                },
                new AlunoTemRecomandacaoDto
                {
                    AluncoCodigo = "2",
                    TemRecomendacao = false
                },
                new AlunoTemRecomandacaoDto
                {
                    AluncoCodigo = "3",
                    TemRecomendacao = false
                },
                new AlunoTemRecomandacaoDto
                {
                    AluncoCodigo = "4",
                    TemRecomendacao = false
                }
            };
           return lista;
        }
    }
}