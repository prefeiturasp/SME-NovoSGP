using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFake
{
    public class ObterAlunosEolPorCodigosQueryHandlerFake_filtro : IRequestHandler<ObterAlunosEolPorCodigosQuery, IEnumerable<TurmasDoAlunoDto>>
    {
        private const int ALUNO_1 = 1;
        private const int ALUNO_2 = 2;

        public async Task<IEnumerable<TurmasDoAlunoDto>> Handle(ObterAlunosEolPorCodigosQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<TurmasDoAlunoDto>()
            {
                new TurmasDoAlunoDto
                {
                    CodigoAluno = ALUNO_1,
                    NomeAluno = $"Nome do Aluno {ALUNO_1} ",
                    NomeSocialAluno = $"Nome Social do Aluno {ALUNO_1}",
                    CodigoSituacaoMatricula = (int)SituacaoMatriculaAluno.Ativo,
                    DataSituacao = DateTimeExtension.HorarioBrasilia().AddDays(-5),
                    NumeroAlunoChamada = 1,
                    CodigoTurma = 1,
                    AnoLetivo= DateTimeExtension.HorarioBrasilia().Year,
                    CodigoTipoTurma= (int)TipoTurma.Regular,
                    SituacaoMatricula = "Ativo"
                },
                new TurmasDoAlunoDto
                {
                    CodigoAluno = ALUNO_2,
                    NomeAluno = $"Nome do Aluno {ALUNO_2} ",
                    NomeSocialAluno = $"Nome Social do Aluno {ALUNO_2}",
                    CodigoSituacaoMatricula = (int)SituacaoMatriculaAluno.Desistente,
                    DataSituacao = DateTimeExtension.HorarioBrasilia(),
                    NumeroAlunoChamada = 1,
                    CodigoTurma = 1,
                    AnoLetivo= DateTimeExtension.HorarioBrasilia().Year,
                    CodigoTipoTurma= (int)TipoTurma.Regular,
                    SituacaoMatricula = "Desistente"
                },
                new TurmasDoAlunoDto
                {
                    CodigoAluno = ALUNO_2,
                    NomeAluno = $"Nome do Aluno {ALUNO_2} ",
                    NomeSocialAluno = $"Nome Social do Aluno {ALUNO_2}",
                    CodigoSituacaoMatricula = (int)SituacaoMatriculaAluno.Ativo,
                    DataSituacao = DateTimeExtension.HorarioBrasilia().AddDays(-5),
                    NumeroAlunoChamada = 1,
                    CodigoTurma = 1,
                    AnoLetivo= DateTimeExtension.HorarioBrasilia().Year,
                    CodigoTipoTurma= (int)TipoTurma.Regular,
                    SituacaoMatricula = "Ativo"
                },
            }.Where(x => x.CodigoAluno == request.CodigosAluno.FirstOrDefault()));
        }
    }
}
