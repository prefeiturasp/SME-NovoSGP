using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.Listao
{
    public class ObterTodosAlunosNaTurmaQueryHandlerFakeListao : IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterTodosAlunosNaTurmaQuery request, CancellationToken cancellationToken)
        {
            var dataRefencia = DateTimeExtension.HorarioBrasilia();
              
            return await Task.FromResult(new List<AlunoPorTurmaResposta>()
            { 
                new() { 
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(-5),
                    CodigoAluno = "1",
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "ATIVO",
                    NomeAluno = "NOME ALUNO 1",
                    NomeResponsavel = "Responsavel 1"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(-7),
                    CodigoAluno = "2",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "ATIVO",
                    NomeAluno = "NOME ALUNO 2",
                    NomeResponsavel = "Responsavel 2"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(-25),
                    CodigoAluno = "3",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "ATIVO",
                    NomeAluno = "NOME ALUNO 3",
                    NomeResponsavel = "Responsavel 3"
                },   
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(-20),
                    CodigoAluno = "4",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "ATIVO",
                    NomeAluno = "NOME ALUNO 4",
                    NomeResponsavel = "Responsavel 4"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(7),
                    DataMatricula = dataRefencia.AddDays(-14),
                    CodigoAluno = "5",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.NaoCompareceu,
                    SituacaoMatricula = "NÃO COMPARECEU",
                    NomeAluno = "NOME ALUNO 5",
                    NomeResponsavel = "Responsavel 5"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(8),
                    DataMatricula = dataRefencia.AddDays(-10),
                    CodigoAluno = "6",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = "DESISTENTE",
                    NomeAluno = "NOME ALUNO 6",
                    NomeResponsavel = "Responsavel 6"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(6),
                    DataMatricula = dataRefencia.AddDays(-12),
                    CodigoAluno = "7",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.VinculoIndevido,
                    SituacaoMatricula = "VINCULO INDEVIDO",
                    NomeAluno = "NOME ALUNO 7",
                    NomeResponsavel = "Responsavel 7"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(5),
                    DataMatricula = dataRefencia.AddDays(-12),
                    CodigoAluno = "8",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Falecido,
                    SituacaoMatricula = "FALECIDO",
                    NomeAluno = "NOME ALUNO 8",
                    NomeResponsavel = "Responsavel 8"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(9),
                    DataMatricula = dataRefencia.AddDays(-10),
                    CodigoAluno = "9",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Deslocamento,
                    SituacaoMatricula = "DESLOCAMENTO",
                    NomeAluno = "NOME ALUNO 9",
                    NomeResponsavel = "Responsavel 9"
                },  
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(10),
                    DataMatricula = dataRefencia.AddDays(-11),
                    CodigoAluno = "10",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Cessado,
                    SituacaoMatricula = "CESSADO",
                    NomeAluno = "NOME ALUNO 10",
                    NomeResponsavel = "Responsavel 10"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(12),
                    DataMatricula = dataRefencia.AddDays(-10),
                    CodigoAluno = "11",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.ReclassificadoSaida,
                    SituacaoMatricula = "RECLASSIFICADO SAIDA",
                    NomeAluno = "NOME ALUNO 11",
                    NomeResponsavel = "Responsavel 11"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(-54),
                    DataMatricula = dataRefencia.AddDays(-120),
                    CodigoAluno = "12",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = "DESISTENTE",
                    NomeAluno = "NOME ALUNO 12",
                    NomeResponsavel = "Responsavel 12"
                }, 
                new() { 
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(-55),
                    DataMatricula = dataRefencia.AddDays(-130),
                    CodigoAluno = "13",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.NaoCompareceu,
                    SituacaoMatricula = "NÃO COMPARECEU",
                    NomeAluno = "NOME ALUNO 13",
                    NomeResponsavel = "Responsavel 13"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddYears(-1),
                    DataMatricula = dataRefencia.AddDays(-10),
                    CodigoAluno = "14",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.ReclassificadoSaida,
                    SituacaoMatricula = "RECLASSIFICADO SAIDA",
                    NomeAluno = "NOME ALUNO 14",
                    NomeResponsavel = "Responsavel 14"
                }
            }); 
        }
    }
}