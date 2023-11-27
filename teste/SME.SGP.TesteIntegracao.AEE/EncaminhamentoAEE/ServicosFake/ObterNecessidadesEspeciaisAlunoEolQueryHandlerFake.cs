using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAee.ServicosFake
{
    public class ObterNecessidadesEspeciaisAlunoEolQueryHandlerFake : IRequestHandler<ObterNecessidadesEspeciaisAlunoEolQuery, InformacoesEscolaresAlunoDto>
    {
        public async Task<InformacoesEscolaresAlunoDto> Handle(ObterNecessidadesEspeciaisAlunoEolQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new InformacoesEscolaresAlunoDto()
            {
                CodigoAluno = "1",
                FrequenciaGlobal = "95",
                DescricaoNecessidadeEspecial = "Cego",
                FrequenciaAlunoPorBimestres = new List<FrequenciaBimestreAlunoDto>()
                {
                    new FrequenciaBimestreAlunoDto()
                    {
                        CodigoAluno = "1",
                        Bimestre = 1,
                        Frequencia = 100,
                        TotalAulas = 30
                    }
                }
            });
        }
    }
}
