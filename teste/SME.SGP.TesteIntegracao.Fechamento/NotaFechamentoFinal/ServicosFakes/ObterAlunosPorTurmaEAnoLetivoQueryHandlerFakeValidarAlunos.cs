using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.NotaFechamento.ServicosFakes
{
    public class ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeValidarAlunos: IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly string ALUNO_CODIGO_1 = "1";
        private readonly string ALUNO_CODIGO_2 = "2";
        private readonly string ALUNO_CODIGO_3 = "3";
        private readonly string ALUNO_CODIGO_4 = "4";
        private readonly string ALUNO_CODIGO_5 = "5";
        private readonly string ALUNO_CODIGO_6 = "6";
        private readonly string ALUNO_CODIGO_7 = "7";
        private readonly string ALUNO_CODIGO_8 = "8";
        private readonly string ALUNO_CODIGO_9 = "9";
        private readonly string ALUNO_CODIGO_10 = "10";
        private readonly string ALUNO_CODIGO_11 = "11";
        private readonly string ALUNO_CODIGO_12 = "12";
        private readonly string ALUNO_CODIGO_13 = "13";

        private readonly string ATIVO = "Ativo";
        private readonly string RESPONSAVEL = "RESPONSAVEL";
        private readonly string TIPO_RESPONSAVEL_4 = "4";
        private readonly string CELULAR_RESPONSAVEL = "11111111111";
        private readonly string NAO_COMPARECEU = "Não Compareceu";
        private readonly string DESISTENTE = "Desistente";
        private readonly string VINCULO_INDEVIDO = "Vínculo indevido";
        private readonly string FALECIDO = "Falecido";
        private readonly string DESLOCAMENTO = "Deslocamento";
        private readonly string CESSADO = "Cessado";
        private readonly string RECLASSIFICADO_SAIDA = "Reclassificado saída";

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosPorTurmaEAnoLetivoQuery request, CancellationToken cancellationToken)
        {
              var dataRefencia = DateTimeExtension.HorarioBrasilia();

            var alunos = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_1,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = int.Parse(request.CodigoTurma),
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = dataRefencia.AddDays(-5),
                    DataMatricula = dataRefencia.AddDays(-120),
                    NomeAluno = ALUNO_CODIGO_1,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = ATIVO,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel = CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                },

                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_2,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = int.Parse(request.CodigoTurma),
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = dataRefencia.AddDays(-7),
                    DataMatricula = dataRefencia.AddDays(-130),
                    NomeAluno = ALUNO_CODIGO_2,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = ATIVO,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel = CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                },

                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_3,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = int.Parse(request.CodigoTurma),
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = dataRefencia.AddDays(-25),
                    DataMatricula = dataRefencia.AddDays(-130),
                    NomeAluno = ALUNO_CODIGO_3,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = ATIVO,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel = CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                },

                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_4,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = int.Parse(request.CodigoTurma),
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = dataRefencia.AddDays(-20),
                    DataMatricula = dataRefencia.AddDays(-130),
                    NomeAluno = ALUNO_CODIGO_4,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = ATIVO,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel = CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                },

                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_5,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.NaoCompareceu,
                    CodigoTurma = int.Parse(request.CodigoTurma),
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = dataRefencia.AddDays(-7),
                    DataMatricula = dataRefencia.AddDays(-130),
                    NomeAluno = ALUNO_CODIGO_5,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = NAO_COMPARECEU,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel = CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                },

                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_6,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    CodigoTurma = int.Parse(request.CodigoTurma),
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = dataRefencia.AddDays(-8),
                    DataMatricula = dataRefencia.AddDays(-130),
                    NomeAluno = ALUNO_CODIGO_6,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = DESISTENTE,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel = CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                },

                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_7,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.VinculoIndevido,
                    CodigoTurma = int.Parse(request.CodigoTurma),
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = dataRefencia.AddDays(-6),
                    DataMatricula = dataRefencia.AddDays(-130),
                    NomeAluno = ALUNO_CODIGO_7,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = VINCULO_INDEVIDO,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel = CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                },

                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_8,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Falecido,
                    CodigoTurma = int.Parse(request.CodigoTurma),
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = dataRefencia.AddDays(-3),
                    DataMatricula = dataRefencia.AddDays(-130),
                    NomeAluno = ALUNO_CODIGO_8,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = FALECIDO,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel = CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                },

                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_9,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Deslocamento,
                    CodigoTurma = int.Parse(request.CodigoTurma),
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = dataRefencia.AddDays(-1),
                    DataMatricula = dataRefencia.AddDays(-130),
                    NomeAluno = ALUNO_CODIGO_9,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = DESLOCAMENTO,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel = CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                },

                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_10,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Cessado,
                    CodigoTurma = int.Parse(request.CodigoTurma),
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = dataRefencia.AddDays(-10),
                    DataMatricula = dataRefencia.AddDays(-130),
                    NomeAluno = ALUNO_CODIGO_10,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = CESSADO,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel = CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                },

                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_11,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.ReclassificadoSaida,
                    CodigoTurma = int.Parse(request.CodigoTurma),
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = dataRefencia.AddDays(-10),
                    DataMatricula = dataRefencia.AddDays(-130),
                    NomeAluno = ALUNO_CODIGO_11,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = RECLASSIFICADO_SAIDA,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel = CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                },

                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_12,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    CodigoTurma = int.Parse(request.CodigoTurma),
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = dataRefencia.AddDays(-50),
                    DataMatricula = dataRefencia.AddDays(-130),
                    NomeAluno = ALUNO_CODIGO_13,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = NAO_COMPARECEU,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel = CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                },

                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_13,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.NaoCompareceu,
                    CodigoTurma = int.Parse(request.CodigoTurma),
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = dataRefencia.AddDays(-55),
                    DataMatricula = dataRefencia.AddDays(-130),
                    NomeAluno = ALUNO_CODIGO_13,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = NAO_COMPARECEU,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel = CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                }
            };

            return await Task.FromResult(alunos.Where(x => x.CodigoTurma.ToString() == request.CodigoTurma));
        }
    }
}
