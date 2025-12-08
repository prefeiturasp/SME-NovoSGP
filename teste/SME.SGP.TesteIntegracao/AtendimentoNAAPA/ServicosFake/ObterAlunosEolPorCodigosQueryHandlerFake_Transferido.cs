using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFake
{
    public class ObterAlunosEolPorCodigosQueryHandlerFake_Transferido : IRequestHandler<ObterAlunosEolPorCodigosQuery, IEnumerable<TurmasDoAlunoDto>>
    {
        private const int ALUNO_1 = 1;

        public async Task<IEnumerable<TurmasDoAlunoDto>> Handle(ObterAlunosEolPorCodigosQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<TurmasDoAlunoDto>()
            {
                new TurmasDoAlunoDto
                {
                    CodigoAluno = ALUNO_1,
                    NomeAluno = $"Nome do Aluno {ALUNO_1} ",
                    NomeSocialAluno = $"Nome Social do Aluno {ALUNO_1}",
                    CodigoSituacaoMatricula = (int)SituacaoMatriculaAluno.Transferido,
                    DataSituacao = DateTimeExtension.HorarioBrasilia(),
                    NumeroAlunoChamada = 1,
                    CodigoTurma = 1,
                    AnoLetivo= DateTimeExtension.HorarioBrasilia().Year,
                    CodigoTipoTurma= (int)TipoTurma.Regular,
                },
                new TurmasDoAlunoDto
                {
                    CodigoAluno = ALUNO_1,
                    NomeAluno = $"Nome do Aluno {ALUNO_1} ",
                    NomeSocialAluno = $"Nome Social do Aluno {ALUNO_1}",
                    CodigoSituacaoMatricula = (int)SituacaoMatriculaAluno.Ativo,
                    DataSituacao = DateTimeExtension.HorarioBrasilia(),
                    NumeroAlunoChamada = 1,
                    CodigoTurma = 2,
                    AnoLetivo= DateTimeExtension.HorarioBrasilia().Year,
                    CodigoTipoTurma= (int)TipoTurma.Regular,
                },
                new TurmasDoAlunoDto
                {
                    CodigoAluno = ALUNO_1,
                    NomeAluno = $"Nome do Aluno {ALUNO_1} ",
                    NomeSocialAluno = $"Nome Social do Aluno {ALUNO_1}",
                    CodigoSituacaoMatricula = (int)SituacaoMatriculaAluno.Ativo,
                    DataSituacao = DateTimeExtension.HorarioBrasilia(),
                    NumeroAlunoChamada = 1,
                    CodigoTurma = 1,
                    AnoLetivo= DateTimeExtension.HorarioBrasilia().Year,
                    CodigoTipoTurma= (int)TipoTurma.EdFisica,
                },
            });
        }
    }
}
