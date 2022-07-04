using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterAlunosPorTurmaQueryHandlerComRegistroFake : IRequestHandler<ObterAlunosPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosPorTurmaQuery request, CancellationToken cancellationToken)
        {
            return new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = "1",
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = new DateTime(2021, 11, 09, 17, 25, 31),
                    DataMatricula = new DateTime(2021, 11, 09, 17, 25, 31),
                    EscolaTransferencia = null,
                    NomeAluno = "Aluno teste 1",
                    NomeSocialAluno = null,
                    NumeroAlunoChamada = 1,
                    ParecerConclusivo = null,
                    PossuiDeficiencia = false,
                    SituacaoMatricula = "Ativo",
                    Transferencia_Interna = false,
                    TurmaEscola = null,
                    TurmaRemanejamento = null,
                    TurmaTransferencia = null,
                    NomeResponsavel = "João teste",
                    TipoResponsavel = "4",
                    CelularResponsavel = "11961861993",
                    DataAtualizacaoContato = new DateTime(2018, 06, 22, 19, 02, 35),
                },
                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = "2",
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = new DateTime(2021, 11, 09, 17, 25, 31),
                    DataMatricula = new DateTime(2021, 11, 09, 17, 25, 31),
                    EscolaTransferencia = null,
                    NomeAluno = "Aluno teste 2",
                    NomeSocialAluno = null,
                    NumeroAlunoChamada = 1,
                    ParecerConclusivo = null,
                    PossuiDeficiencia = false,
                    SituacaoMatricula = "Ativo",
                    Transferencia_Interna = false,
                    TurmaEscola = null,
                    TurmaRemanejamento = null,
                    TurmaTransferencia = null,
                    NomeResponsavel = "João teste",
                    TipoResponsavel = "4",
                    CelularResponsavel = "11961861993",
                    DataAtualizacaoContato = new DateTime(2018, 06, 22, 19, 02, 35),
                },
                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = "3",
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = new DateTime(2021, 11, 09, 17, 25, 31),
                    DataMatricula = new DateTime(2021, 11, 09, 17, 25, 31),
                    EscolaTransferencia = null,
                    NomeAluno = "Aluno teste 3",
                    NomeSocialAluno = null,
                    NumeroAlunoChamada = 1,
                    ParecerConclusivo = null,
                    PossuiDeficiencia = false,
                    SituacaoMatricula = "Ativo",
                    Transferencia_Interna = false,
                    TurmaEscola = null,
                    TurmaRemanejamento = null,
                    TurmaTransferencia = null,
                    NomeResponsavel = "João teste",
                    TipoResponsavel = "4",
                    CelularResponsavel = "11961861993",
                    DataAtualizacaoContato = new DateTime(2018, 06, 22, 19, 02, 35),
                },
                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = "4",
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = new DateTime(2021, 11, 09, 17, 25, 31),
                    DataMatricula = new DateTime(2021, 11, 09, 17, 25, 31),
                    EscolaTransferencia = null,
                    NomeAluno = "Aluno teste 4",
                    NomeSocialAluno = null,
                    NumeroAlunoChamada = 1,
                    ParecerConclusivo = null,
                    PossuiDeficiencia = false,
                    SituacaoMatricula = "Ativo",
                    Transferencia_Interna = false,
                    TurmaEscola = null,
                    TurmaRemanejamento = null,
                    TurmaTransferencia = null,
                    NomeResponsavel = "João teste",
                    TipoResponsavel = "4",
                    CelularResponsavel = "11961861993",
                    DataAtualizacaoContato = new DateTime(2018, 06, 22, 19, 02, 35),
                }
            };
        }
    }
}
