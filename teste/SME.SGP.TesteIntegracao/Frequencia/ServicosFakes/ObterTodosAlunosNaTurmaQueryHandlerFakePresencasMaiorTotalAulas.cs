using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Frequencia.ServicosFakes
{
    public class ObterTodosAlunosNaTurmaQueryHandlerFakePresencasMaiorTotalAulas : IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterTodosAlunosNaTurmaQuery request, CancellationToken cancellationToken)
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia();

            if (request.CodigoAluno.Value == 1)
            {
                return await Task.FromResult(new List<AlunoPorTurmaResposta>()
                {
                    new AlunoPorTurmaResposta()
                    {
                        CodigoAluno = "1",
                        CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                        DataMatricula = new System.DateTime(dataAtual.Year, dataAtual.Month, 10)
                    }                
                }.AsEnumerable());
            }

            return await Task.FromResult(new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = "2",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido,
                    DataMatricula = new System.DateTime(dataAtual.Year, dataAtual.Month, 10),
                    DataSituacao = new System.DateTime(dataAtual.Year, dataAtual.Month, 20)
                }
            }.AsEnumerable());
        }
    }
}
