using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem.ServicosFake
{
    public class ObterFrequenciaBimestresQueryHandlerFake : IRequestHandler<ObterFrequenciaBimestresQuery, IEnumerable<FrequenciaBimestreAlunoDto>>
    {
        public async Task<IEnumerable<FrequenciaBimestreAlunoDto>> Handle(ObterFrequenciaBimestresQuery request, CancellationToken cancellationToken)
        {
            var lista = new List<FrequenciaBimestreAlunoDto>
            {
                new FrequenciaBimestreAlunoDto
                {
                    CodigoAluno = "1",
                    Bimestre = 2,
                    QuantidadeAusencias = 2,
                    QuantidadeCompensacoes = 1,
                    TotalAulas = 2,
                    Frequencia = 50.0
                }
            };

            return lista;
        }
    }
}