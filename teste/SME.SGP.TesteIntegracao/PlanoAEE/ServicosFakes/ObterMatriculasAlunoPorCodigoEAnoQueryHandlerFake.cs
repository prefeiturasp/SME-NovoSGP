using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes
{
    public class ObterMatriculasAlunoPorCodigoEAnoQueryHandlerFake : IRequestHandler<ObterMatriculasAlunoPorCodigoEAnoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterMatriculasAlunoPorCodigoEAnoQuery request, CancellationToken cancellationToken)
        {
            var alunoPorTurmaResposta = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta
                {
                    Ano = DateTime.Today.Year,
                    CodigoAluno = "1",
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1998, 2, 12),
                    DataSituacao = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 10),
                    DataMatricula = new DateTime(2021, 2, 12),
                    NomeSocialAluno = "Nome Social do Aluno",
                    NomeAluno = "Nome do Aluno",
                    NumeroAlunoChamada = 1,
                    SituacaoMatricula = "Ativo",
                    NomeResponsavel = "Nome do Responsavel do Aluno",
                    TipoResponsavel = "1",
                    CelularResponsavel = "9999999999",
                    DataAtualizacaoContato = DateTime.Now,
                    CodigoEscola = "111111111"
                },
                new AlunoPorTurmaResposta
                {
                    Ano = DateTime.Today.Year,
                    CodigoAluno = "1",
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1998, 2, 12),
                    DataSituacao = new DateTime(DateTime.Today.Year, DateTime.Today.Month - 1, DateTime.Today.Day),
                    DataMatricula = new DateTime(2021, 2, 12),
                    NomeSocialAluno = "Nome Social do Aluno",
                    NomeAluno = "Nome do Aluno",
                    NumeroAlunoChamada = 1,
                    SituacaoMatricula = "Desistente",
                    NomeResponsavel = "Nome do Responsavel do Aluno",
                    TipoResponsavel = "1",
                    CelularResponsavel = "9999999999",
                    DataAtualizacaoContato = DateTime.Now,
                    CodigoEscola = "111111111"
                },
                new AlunoPorTurmaResposta
                {
                    Ano = DateTime.Today.Year,
                    CodigoAluno = "1",
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Cessado,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1998, 2, 12),
                    DataSituacao = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day),
                    DataMatricula = new DateTime(2021, 2, 12),
                    NomeSocialAluno = "Nome Social do Aluno",
                    NomeAluno = "Nome do Aluno",
                    NumeroAlunoChamada = 1,
                    SituacaoMatricula = "Cessado",
                    NomeResponsavel = "Nome do Responsavel do Aluno",
                    TipoResponsavel = "1",
                    CelularResponsavel = "9999999999",
                    DataAtualizacaoContato = DateTime.Now,
                    CodigoEscola = "111111111"
                },
                new AlunoPorTurmaResposta
                {
                    Ano = DateTime.Today.Year,
                    CodigoAluno = "2",
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1998, 2, 12),
                    DataSituacao = new DateTime(DateTime.Today.Year, DateTime.Today.Month-1, DateTime.Today.Day),
                    DataMatricula = new DateTime(2021, 2, 12),
                    NomeSocialAluno = "Nome Social do Aluno",
                    NomeAluno = "Nome do Aluno",
                    NumeroAlunoChamada = 1,
                    SituacaoMatricula = "Ativo",
                    NomeResponsavel = "Nome do Responsavel do Aluno",
                    TipoResponsavel = "1",
                    CelularResponsavel = "9999999999",
                    DataAtualizacaoContato = DateTime.Now,
                    CodigoEscola = "111111111"
                },
                new AlunoPorTurmaResposta
                {
                    Ano = DateTime.Today.Year,
                    CodigoAluno = "2",
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1998, 2, 12),
                    DataSituacao = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day),
                    DataMatricula = new DateTime(2021, 2, 12),
                    NomeSocialAluno = "Nome Social do Aluno",
                    NomeAluno = "Nome do Aluno",
                    NumeroAlunoChamada = 1,
                    SituacaoMatricula = "Transferido",
                    NomeResponsavel = "Nome do Responsavel do Aluno",
                    TipoResponsavel = "1",
                    CelularResponsavel = "9999999999",
                    DataAtualizacaoContato = DateTime.Now,
                    CodigoEscola = "111111111"
                },
                new AlunoPorTurmaResposta
                {
                    Ano = DateTime.Today.Year,
                    CodigoAluno = "3",
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1998, 2, 12),
                    DataSituacao = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day-6),
                    DataMatricula = new DateTime(2021, 2, 12),
                    NomeSocialAluno = "Nome Social do Aluno",
                    NomeAluno = "Nome do Aluno",
                    NumeroAlunoChamada = 1,
                    SituacaoMatricula = "Transferido",
                    NomeResponsavel = "Nome do Responsavel do Aluno",
                    TipoResponsavel = "1",
                    CelularResponsavel = "9999999999",
                    DataAtualizacaoContato = DateTime.Now,
                    CodigoEscola = "111111111"
                },
                new AlunoPorTurmaResposta
                {
                    Ano = DateTime.Today.Year,
                    CodigoAluno = "3",
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1998, 2, 12),
                    DataSituacao = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day),
                    DataMatricula = new DateTime(2021, 2, 12),
                    NomeSocialAluno = "Nome Social do Aluno",
                    NomeAluno = "Nome do Aluno",
                    NumeroAlunoChamada = 1,
                    SituacaoMatricula = "Ativo",
                    NomeResponsavel = "Nome do Responsavel do Aluno",
                    TipoResponsavel = "1",
                    CelularResponsavel = "9999999999",
                    DataAtualizacaoContato = DateTime.Now,
                    CodigoEscola = "111111111"
                },
                new AlunoPorTurmaResposta
                {
                    Ano = DateTime.Today.Year,
                    CodigoAluno = "4",
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Concluido,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1998, 2, 12),
                    DataSituacao = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day),
                    DataMatricula = new DateTime(2021, 2, 12),
                    NomeSocialAluno = "Nome Social do Aluno",
                    NomeAluno = "Nome do Aluno",
                    NumeroAlunoChamada = 1,
                    SituacaoMatricula = "Concluido",
                    NomeResponsavel = "Nome do Responsavel do Aluno",
                    TipoResponsavel = "1",
                    CelularResponsavel = "9999999999",
                    DataAtualizacaoContato = DateTime.Now,
                    CodigoEscola = "111111111"
                },
                new AlunoPorTurmaResposta
                {
                    Ano = DateTime.Today.Year,
                    CodigoAluno = "4",
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1998, 2, 12),
                    DataSituacao = new DateTime(DateTime.Today.Year, DateTime.Today.Month-2, DateTime.Today.Day),
                    DataMatricula = new DateTime(2021, 2, 12),
                    NomeSocialAluno = "Nome Social do Aluno",
                    NomeAluno = "Nome do Aluno",
                    NumeroAlunoChamada = 1,
                    SituacaoMatricula = "Ativo",
                    NomeResponsavel = "Nome do Responsavel do Aluno",
                    TipoResponsavel = "1",
                    CelularResponsavel = "9999999999",
                    DataAtualizacaoContato = DateTime.Now,
                    CodigoEscola = "111111111"
                },
                new AlunoPorTurmaResposta
                {
                    Ano = DateTime.Today.Year,
                    CodigoAluno = "5",
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Concluido,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1998, 2, 12),
                    DataSituacao = new DateTime(DateTime.Today.Year+1, DateTime.Today.Month, DateTime.Today.Day),
                    DataMatricula = new DateTime(2022, 9, 12),
                    NomeSocialAluno = "Nome Social do Aluno",
                    NomeAluno = "Nome do Aluno",
                    NumeroAlunoChamada = 5,
                    SituacaoMatricula = "Concluido",
                    NomeResponsavel = "Nome do Responsavel do Aluno",
                    TipoResponsavel = "1",
                    CelularResponsavel = "9999999999",
                    DataAtualizacaoContato = DateTime.Now,
                    CodigoEscola = "111111111"
                },
                new AlunoPorTurmaResposta
                {
                    Ano = DateTime.Today.Year,
                    CodigoAluno = "6",
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.RemanejadoSaida,
                    CodigoTurma = 1,
                    DataNascimento = new DateTime(1998, 2, 12),
                    DataSituacao = new DateTime(DateTime.Today.Year+1, DateTime.Today.Month, DateTime.Today.Day),
                    DataMatricula = new DateTime(2022, 9, 12),
                    NomeSocialAluno = "Nome Social do Aluno",
                    NomeAluno = "Nome do Aluno",
                    NumeroAlunoChamada = 5,
                    SituacaoMatricula = "RemanejadoSaida",
                    NomeResponsavel = "Nome do Responsavel do Aluno",
                    TipoResponsavel = "1",
                    CelularResponsavel = "9999999999",
                    DataAtualizacaoContato = DateTime.Now,
                    CodigoEscola = "111111111"
                },
                new AlunoPorTurmaResposta
                {
                    Ano = DateTime.Today.Year,
                    CodigoAluno = "6",
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.TransferidoSED,
                    CodigoTurma = 2,
                    DataNascimento = new DateTime(1998, 2, 12),
                    DataSituacao = new DateTime(DateTime.Today.Year+1, DateTime.Today.Month, DateTime.Today.Day),
                    DataMatricula = new DateTime(2022, 9, 12),
                    NomeSocialAluno = "Nome Social do Aluno",
                    NomeAluno = "Nome do Aluno",
                    NumeroAlunoChamada = 5,
                    SituacaoMatricula = "TransferidoSED",
                    NomeResponsavel = "Nome do Responsavel do Aluno",
                    TipoResponsavel = "1",
                    CelularResponsavel = "9999999999",
                    DataAtualizacaoContato = DateTime.Now,
                    CodigoEscola = "111111111"
                },
            };

            return alunoPorTurmaResposta.Where(x => x.CodigoAluno == request.CodigoAluno);
        }
    }
}