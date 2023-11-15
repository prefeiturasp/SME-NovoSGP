using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConsolidacaoConselhoDeClasse.ServicosFakes
{
    public class ObterMatriculasAlunoNaTurmaQueryHandlerFake : IRequestHandler<ObterMatriculasAlunoNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        public ObterMatriculasAlunoNaTurmaQueryHandlerFake()
        {
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterMatriculasAlunoNaTurmaQuery request, CancellationToken cancellationToken)
        {
            var dataMatriculaAluno1 = new DateTime(2022, 10, 26);
            var dataMatriculaAluno2 = new DateTime(2023, 08, 08);
            var dataSituacaoAluno = new DateTime(2023, 10, 31);

            var lista = new List<AlunoPorTurmaResposta> {
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "1",
                    SituacaoMatricula = "Rematriculado",
                    DataSituacao = dataSituacaoAluno,
                    DataMatricula = dataMatriculaAluno1,
                    Ano = 2023,
                    CelularResponsavel = "1111111111111",
                    CodigoComponenteCurricular = 0,
                    CodigoEscola = "1",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Rematriculado,
                    CodigoTipoTurma = 0,
                    CodigoTurma = 1,
                    EscolaTransferencia = null,
                    NomeAluno = "1",
                    NumeroAlunoChamada = 1,
                    ParecerConclusivo = null,
                    TipoResponsavel = "1"
                },
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "2",
                    SituacaoMatricula = "Rematriculado",
                    DataSituacao = dataSituacaoAluno,
                    DataMatricula = dataMatriculaAluno2,
                    Ano = 2023,
                    CelularResponsavel = "2222222222222",
                    CodigoComponenteCurricular = 0,
                    CodigoEscola = "1",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Rematriculado,
                    CodigoTipoTurma = 0,
                    CodigoTurma = 1,
                    EscolaTransferencia = null,
                    NomeAluno = "2",
                    NumeroAlunoChamada = 2,
                    ParecerConclusivo = null,
                    TipoResponsavel = "1"
                }
            };
            return lista.Where(x => x.CodigoAluno.Equals(request.CodigoAluno));
        }
    }
}
