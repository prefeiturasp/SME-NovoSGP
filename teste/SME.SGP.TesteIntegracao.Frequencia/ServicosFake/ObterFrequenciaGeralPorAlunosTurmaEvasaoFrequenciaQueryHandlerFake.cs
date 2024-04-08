using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Frequencia.ServicosFake
{
    public class ObterFrequenciaGeralPorAlunosTurmaEvasaoFrequenciaQueryHandlerFake : IRequestHandler<ObterFrequenciaGeralPorAlunosTurmaQuery, IEnumerable<Dominio.FrequenciaAluno>>
    {
        private readonly string ALUNO_CODIGO_3 = "3";
        private readonly string ALUNO_CODIGO_4 = "4";
        private readonly string ALUNO_CODIGO_5 = "5";
        private readonly string ALUNO_CODIGO_6 = "6";
        public async Task<IEnumerable<Dominio.FrequenciaAluno>> Handle(ObterFrequenciaGeralPorAlunosTurmaQuery request, CancellationToken cancellationToken)
        {
            var retorno = new List<Dominio.FrequenciaAluno>()
            {
                new Dominio.FrequenciaAluno()
                {
                    CodigoAluno = ALUNO_CODIGO_3,
                    TotalAulas = 10,
                    TotalAusencias = 6,
                    TotalPresencas = 4,
                },
                new Dominio.FrequenciaAluno()
                {
                    CodigoAluno = ALUNO_CODIGO_4,
                    TotalAulas = 10,
                    TotalAusencias = 7,
                    TotalPresencas = 3,
                },
                new Dominio.FrequenciaAluno()
                {
                    CodigoAluno = ALUNO_CODIGO_5,
                    TotalAulas = 10,
                    TotalAusencias = 10,
                    TotalPresencas = 0,
                },
                new Dominio.FrequenciaAluno()
                {
                    CodigoAluno = ALUNO_CODIGO_6,
                    TotalAulas = 10,
                    TotalAusencias = 10,
                    TotalPresencas = 0,
                },
            };
            return await Task.FromResult(retorno);
        }
    }
}
