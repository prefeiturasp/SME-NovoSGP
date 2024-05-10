using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.Listao
{
    public class ObterAlunosPorTurmaEDataMatriculaQueryHandlerFakeListao : IRequestHandler<ObterAlunosPorTurmaEDataMatriculaQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosPorTurmaEDataMatriculaQuery request, CancellationToken cancellationToken)
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
                    NomeAluno = "NOME ALUNO 1"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(-7),
                    CodigoAluno = "2",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "ATIVO",
                    NomeAluno = "NOME ALUNO 2"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(-25),
                    CodigoAluno = "3",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "ATIVO",
                    NomeAluno = "NOME ALUNO 3"
                },   
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(-20),
                    CodigoAluno = "4",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "ATIVO",
                    NomeAluno = "NOME ALUNO 4"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(7),
                    DataMatricula = dataRefencia.AddDays(-14),
                    CodigoAluno = "5",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.NaoCompareceu,
                    SituacaoMatricula = "NÃO COMPARECEU",
                    NomeAluno = "NOME ALUNO 5"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(8),
                    DataMatricula = dataRefencia.AddDays(-10),
                    CodigoAluno = "6",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = "DESISTENTE",
                    NomeAluno = "NOME ALUNO 6"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(6),
                    DataMatricula = dataRefencia.AddDays(-12),
                    CodigoAluno = "7",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.VinculoIndevido,
                    SituacaoMatricula = "VINCULO INDEVIDO",
                    NomeAluno = "NOME ALUNO 7"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(5),
                    DataMatricula = dataRefencia.AddDays(-12),
                    CodigoAluno = "8",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Falecido,
                    SituacaoMatricula = "FALECIDO",
                    NomeAluno = "NOME ALUNO 8"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(9),
                    DataMatricula = dataRefencia.AddDays(-10),
                    CodigoAluno = "9",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Deslocamento,
                    SituacaoMatricula = "DESLOCAMENTO",
                    NomeAluno = "NOME ALUNO 9"
                },  
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(10),
                    DataMatricula = dataRefencia.AddDays(-11),
                    CodigoAluno = "10",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Cessado,
                    SituacaoMatricula = "CESSADO",
                    NomeAluno = "NOME ALUNO 10"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(12),
                    DataMatricula = dataRefencia.AddDays(-10),
                    CodigoAluno = "11",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.ReclassificadoSaida,
                    SituacaoMatricula = "RECLASSIFICADO SAIDA",
                    NomeAluno = "NOME ALUNO 11"
                },
                new() {
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(-54),
                    DataMatricula = dataRefencia.AddDays(-120),
                    CodigoAluno = "12",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = "DESISTENTE",
                    NomeAluno = "NOME ALUNO 12"
                }, 
                new() { 
                    Ano = DateTimeExtension.HorarioBrasilia().Year,
                    DataSituacao = dataRefencia.AddDays(-55),
                    DataMatricula = dataRefencia.AddDays(-130),
                    CodigoAluno = "13",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.NaoCompareceu,
                    SituacaoMatricula = "NÃO COMPARECEU",
                    NomeAluno = "NOME ALUNO 13"
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