using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.FrequenciaDiaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Frequencia.CompensacaoDeAusencia.ServicosFake
{
    internal class ObterAlunosDentroPeriodoCompensacaoQueryHandlerFake : IRequestHandler<ObterAlunosDentroPeriodoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosDentroPeriodoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = ConstantesTeste.ALUNO_NOME_1,
                    Ano = DateTime.Now.Year,
                    DataSituacao = DateTime.Now.AddDays(-14).Date,
                    CodigoAluno = ConstantesTeste.ALUNO_CODIGO_1,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = ConstantesTeste.SITUACAO_MATRICULA_ATIVO
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = ConstantesTeste.ALUNO_NOME_2,
                    Ano = DateTime.Now.Year,
                    DataSituacao = DateTime.Now.AddDays(-14).Date,
                    CodigoAluno = ConstantesTeste.ALUNO_CODIGO_2,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = ConstantesTeste.SITUACAO_MATRICULA_ATIVO
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = ConstantesTeste.ALUNO_NOME_3,
                    Ano = DateTime.Now.Year,
                    DataSituacao = DateTime.Now.AddDays(-14).Date,
                    CodigoAluno = ConstantesTeste.ALUNO_CODIGO_3,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = ConstantesTeste.SITUACAO_MATRICULA_ATIVO
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = ConstantesTeste.ALUNO_NOME_4,
                    Ano = DateTime.Now.Year,
                    DataSituacao = DateTime.Now.AddDays(-14).Date,
                    CodigoAluno = ConstantesTeste.ALUNO_CODIGO_4,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = ConstantesTeste.SITUACAO_MATRICULA_ATIVO
                },
                 new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno Teste 5",
                    Ano = DateTime.Now.Year,
                    DataSituacao = new DateTime(DateTime.Now.Year, 05,10),
                    CodigoAluno = "5",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido,
                    SituacaoMatricula = "Transferido"
                }
            });
        }
    }
}
