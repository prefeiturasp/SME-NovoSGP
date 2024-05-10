using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterTurmaAlunoPorCodigoAlunoQueryHandlerFake : IRequestHandler<ObterTurmaAlunoPorCodigoAlunoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        const string CODIGO_ALUNO_1 = "1";
        const string CODIGO_ALUNO_2 = "2";
        const string CODIGO_ALUNO_3 = "3";
        const string CODIGO_ALUNO_4 = "4";
        const string NOME_ALUNO_1 = "Aluno teste 1";
        const string NOME_ALUNO_2 = "Aluno teste 2";
        const string NOME_ALUNO_3 = "Aluno teste 3";
        const string NOME_ALUNO_4 = "Aluno teste 4";
        const string SITUACAO_MATRICULA_ATIVA = "Ativo";

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterTurmaAlunoPorCodigoAlunoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = CODIGO_ALUNO_1,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1959, 01, 16),
                    DataSituacao = new (DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                    DataMatricula = new (DateTimeExtension.HorarioBrasilia().Year, 11, 09),
                    EscolaTransferencia = null,
                    NomeAluno = NOME_ALUNO_1,
                    NomeSocialAluno = null,
                    NumeroAlunoChamada = 1,
                    ParecerConclusivo = null,
                    PossuiDeficiencia = false,
                    SituacaoMatricula = SITUACAO_MATRICULA_ATIVA,
                    Transferencia_Interna = false,
                    TurmaEscola = null,
                    TurmaRemanejamento = null,
                    TurmaTransferencia = null,
                    NomeResponsavel = "João teste",
                    TipoResponsavel = "4",
                    CelularResponsavel = "11961861993",
                    DataAtualizacaoContato = new DateTime(2018, 06, 22),
                },
                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = CODIGO_ALUNO_2,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1959, 01, 16),
                    DataSituacao = new (DateTimeExtension.HorarioBrasilia().Year, 11, 09),
                    DataMatricula = new (DateTimeExtension.HorarioBrasilia().Year, 11, 09),
                    EscolaTransferencia = null,
                    NomeAluno = NOME_ALUNO_2,
                    NomeSocialAluno = null,
                    NumeroAlunoChamada = 1,
                    ParecerConclusivo = null,
                    PossuiDeficiencia = false,
                    SituacaoMatricula = SITUACAO_MATRICULA_ATIVA,
                    Transferencia_Interna = false,
                    TurmaEscola = null,
                    TurmaRemanejamento = null,
                    TurmaTransferencia = null,
                    NomeResponsavel = "João teste",
                    TipoResponsavel = "4",
                    CelularResponsavel = "11961861993",
                    DataAtualizacaoContato = new(DateTimeExtension.HorarioBrasilia().Year, 06, 22),
                },
                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = CODIGO_ALUNO_3,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1959, 01, 16),
                    DataSituacao = new (DateTimeExtension.HorarioBrasilia().Year, 11, 09),
                    DataMatricula = new (DateTimeExtension.HorarioBrasilia().Year, 11, 09),
                    EscolaTransferencia = null,
                    NomeAluno = NOME_ALUNO_3,
                    NomeSocialAluno = null,
                    NumeroAlunoChamada = 1,
                    ParecerConclusivo = null,
                    PossuiDeficiencia = false,
                    SituacaoMatricula = SITUACAO_MATRICULA_ATIVA,
                    Transferencia_Interna = false,
                    TurmaEscola = null,
                    TurmaRemanejamento = null,
                    TurmaTransferencia = null,
                    NomeResponsavel = "João teste",
                    TipoResponsavel = "4",
                    CelularResponsavel = "11961861993",
                    DataAtualizacaoContato = new (DateTimeExtension.HorarioBrasilia().Year, 06, 22),
                },
                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = CODIGO_ALUNO_4,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = new (DateTimeExtension.HorarioBrasilia().Year, 11, 09),
                    DataMatricula = new (DateTimeExtension.HorarioBrasilia().Year, 11, 09),
                    EscolaTransferencia = null,
                    NomeAluno = NOME_ALUNO_4,
                    NomeSocialAluno = null,
                    NumeroAlunoChamada = 1,
                    ParecerConclusivo = null,
                    PossuiDeficiencia = false,
                    SituacaoMatricula = SITUACAO_MATRICULA_ATIVA,
                    Transferencia_Interna = false,
                    TurmaEscola = null,
                    TurmaRemanejamento = null,
                    TurmaTransferencia = null,
                    NomeResponsavel = "João teste",
                    TipoResponsavel = "4",
                    CelularResponsavel = "11961861993",
                    DataAtualizacaoContato = new (DateTimeExtension.HorarioBrasilia().Year, 06, 22),
                }
            });
        }
    }
}
