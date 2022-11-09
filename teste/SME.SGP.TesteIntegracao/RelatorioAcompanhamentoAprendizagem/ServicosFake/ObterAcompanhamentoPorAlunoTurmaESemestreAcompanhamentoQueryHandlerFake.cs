using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem.ServicosFake
{
    public class ObterAcompanhamentoPorAlunoTurmaESemestreAcompanhamentoQueryHandlerFake : IRequestHandler<ObterAcompanhamentoPorAlunoTurmaESemestreQuery, AcompanhamentoAlunoSemestre>
    {
        public async Task<AcompanhamentoAlunoSemestre> Handle(ObterAcompanhamentoPorAlunoTurmaESemestreQuery request, CancellationToken cancellationToken)
        {
            return new AcompanhamentoAlunoSemestre
            {
                AcompanhamentoAluno = null,
                AcompanhamentoAlunoId = 1,
                Semestre = request.Semestre,
                Observacoes = "OBS Teste de Percurso",
                PercursoIndividual = "Teste de Percurso",
                Excluido = false
            };
        }
    }
}