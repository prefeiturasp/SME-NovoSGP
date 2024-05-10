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

namespace SME.SGP.TesteIntegracao.Fechamento.ConselhoDeClasse.ServicosFakes
{
    public class ObterAlunosDentroPeriodoQueryHandlerFake : IRequestHandler<ObterAlunosDentroPeriodoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private const string ALUNO_CODIGO_1 = "1";
        private const string ALUNO_CODIGO_2 = "2";
        private const string ALUNO_CODIGO_3 = "3";
        private const string ALUNO_CODIGO_4 = "4";
        private const string ALUNO_CODIGO_5 = "5";
        private const string ALUNO_CODIGO_6 = "6";
        private const string ALUNO_CODIGO_7 = "7";
        private const string ALUNO_CODIGO_8 = "8";
        private const string ALUNO_CODIGO_9 = "9";
        private const string ALUNO_CODIGO_10 = "10";
        private const string ALUNO_CODIGO_11 = "11";
        private const string ALUNO_CODIGO_12 = "12";
        private const string ALUNO_CODIGO_13 = "13";

        private const string ATIVO = "Ativo";
        private const string RESPONSAVEL = "RESPONSAVEL";
        private const string TIPO_RESPONSAVEL_4 = "4";
        private const string CELULAR_RESPONSAVEL = "11111111111";
        private const string NAO_COMPARECEU = "Não Compareceu";
        private const string DESISTENTE = "Desistente";
        private const string VINCULO_INDEVIDO = "Vínculo indevido";
        private const string FALECIDO = "Falecido";
        private const string DESLOCAMENTO = "Deslocamento";
        private const string CESSADO = "Cessado";
        private const string RECLASSIFICADO_SAIDA = "Reclassificado saída";

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosDentroPeriodoQuery request, CancellationToken cancellationToken)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().AddYears(-1);

            var alunos = new List<AlunoPorTurmaResposta>();

            alunos.Add(new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = ALUNO_CODIGO_1,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                CodigoTurma = Convert.ToInt64(request.CodigoTurma),
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = dataReferencia.AddDays(-5),
                DataMatricula = dataReferencia.AddDays(-120),
                NomeAluno = ALUNO_CODIGO_1,
                NumeroAlunoChamada = 0,
                SituacaoMatricula = ATIVO,
                NomeResponsavel = RESPONSAVEL,
                TipoResponsavel = TIPO_RESPONSAVEL_4,
                CelularResponsavel = CELULAR_RESPONSAVEL,
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01)
            });

            alunos.Add(new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = ALUNO_CODIGO_2,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                CodigoTurma = Convert.ToInt64(request.CodigoTurma),
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = dataReferencia.AddDays(-7),
                DataMatricula = dataReferencia.AddDays(-130),
                NomeAluno = ALUNO_CODIGO_2,
                NumeroAlunoChamada = 0,
                SituacaoMatricula = ATIVO,
                NomeResponsavel = RESPONSAVEL,
                TipoResponsavel = TIPO_RESPONSAVEL_4,
                CelularResponsavel = CELULAR_RESPONSAVEL,
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01)
            });

            alunos.Add(new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = ALUNO_CODIGO_3,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                CodigoTurma = Convert.ToInt64(request.CodigoTurma),
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = dataReferencia.AddDays(-25),
                DataMatricula = dataReferencia.AddDays(-130),
                NomeAluno = ALUNO_CODIGO_3,
                NumeroAlunoChamada = 0,
                SituacaoMatricula = ATIVO,
                NomeResponsavel = RESPONSAVEL,
                TipoResponsavel = TIPO_RESPONSAVEL_4,
                CelularResponsavel = CELULAR_RESPONSAVEL,
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01)
            });

            alunos.Add(new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = ALUNO_CODIGO_4,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                CodigoTurma = Convert.ToInt64(request.CodigoTurma),
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = dataReferencia.AddDays(-20),
                DataMatricula = dataReferencia.AddDays(-130),
                NomeAluno = ALUNO_CODIGO_4,
                NumeroAlunoChamada = 0,
                SituacaoMatricula = ATIVO,
                NomeResponsavel = RESPONSAVEL,
                TipoResponsavel = TIPO_RESPONSAVEL_4,
                CelularResponsavel = CELULAR_RESPONSAVEL,
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01)
            });

            alunos.Add(new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = ALUNO_CODIGO_5,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.NaoCompareceu,
                CodigoTurma = Convert.ToInt64(request.CodigoTurma),
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = dataReferencia.AddDays(-7),
                DataMatricula = dataReferencia.AddDays(-130),
                NomeAluno = ALUNO_CODIGO_5,
                NumeroAlunoChamada = 0,
                SituacaoMatricula = NAO_COMPARECEU,
                NomeResponsavel = RESPONSAVEL,
                TipoResponsavel = TIPO_RESPONSAVEL_4,
                CelularResponsavel = CELULAR_RESPONSAVEL,
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01)
            });

            alunos.Add(new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = ALUNO_CODIGO_6,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                CodigoTurma = Convert.ToInt64(request.CodigoTurma),
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = dataReferencia.AddDays(-8),
                DataMatricula = dataReferencia.AddDays(-130),
                NomeAluno = ALUNO_CODIGO_6,
                NumeroAlunoChamada = 0,
                SituacaoMatricula = DESISTENTE,
                NomeResponsavel = RESPONSAVEL,
                TipoResponsavel = TIPO_RESPONSAVEL_4,
                CelularResponsavel = CELULAR_RESPONSAVEL,
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            alunos.Add(new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = ALUNO_CODIGO_7,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.VinculoIndevido,
                CodigoTurma = Convert.ToInt64(request.CodigoTurma),
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = dataReferencia.AddDays(-6),
                DataMatricula = dataReferencia.AddDays(-130),
                NomeAluno = ALUNO_CODIGO_7,
                NumeroAlunoChamada = 0,
                SituacaoMatricula = VINCULO_INDEVIDO,
                NomeResponsavel = RESPONSAVEL,
                TipoResponsavel = TIPO_RESPONSAVEL_4,
                CelularResponsavel = CELULAR_RESPONSAVEL,
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01)
            });

            alunos.Add(new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = ALUNO_CODIGO_8,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Falecido,
                CodigoTurma = Convert.ToInt64(request.CodigoTurma),
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = new DateTime(dataReferencia.Year, 08, 1),
                DataMatricula = new DateTime(dataReferencia.Year, 04, 1),
                NomeAluno = ALUNO_CODIGO_8,
                NumeroAlunoChamada = 0,
                SituacaoMatricula = FALECIDO,
                NomeResponsavel = RESPONSAVEL,
                TipoResponsavel = TIPO_RESPONSAVEL_4,
                CelularResponsavel = CELULAR_RESPONSAVEL,
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01)
            });

            alunos.Add(new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = ALUNO_CODIGO_9,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Deslocamento,
                CodigoTurma = Convert.ToInt64(request.CodigoTurma),
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = dataReferencia.AddDays(-1),
                DataMatricula = dataReferencia.AddDays(-130),
                NomeAluno = ALUNO_CODIGO_9,
                NumeroAlunoChamada = 0,
                SituacaoMatricula = DESLOCAMENTO,
                NomeResponsavel = RESPONSAVEL,
                TipoResponsavel = TIPO_RESPONSAVEL_4,
                CelularResponsavel = CELULAR_RESPONSAVEL,
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01)
            });

            alunos.Add(new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = ALUNO_CODIGO_10,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Cessado,
                CodigoTurma = Convert.ToInt64(request.CodigoTurma),
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = dataReferencia.AddDays(-10),
                DataMatricula = dataReferencia.AddDays(-130),
                NomeAluno = ALUNO_CODIGO_10,
                NumeroAlunoChamada = 0,
                SituacaoMatricula = CESSADO,
                NomeResponsavel = RESPONSAVEL,
                TipoResponsavel = TIPO_RESPONSAVEL_4,
                CelularResponsavel = CELULAR_RESPONSAVEL,
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01)
            });

            alunos.Add(new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = ALUNO_CODIGO_11,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.ReclassificadoSaida,
                CodigoTurma = Convert.ToInt64(request.CodigoTurma),
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = dataReferencia.AddDays(-10),
                DataMatricula = dataReferencia.AddDays(-130),
                NomeAluno = ALUNO_CODIGO_11,
                NumeroAlunoChamada = 0,
                SituacaoMatricula = RECLASSIFICADO_SAIDA,
                NomeResponsavel = RESPONSAVEL,
                TipoResponsavel = TIPO_RESPONSAVEL_4,
                CelularResponsavel = CELULAR_RESPONSAVEL,
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01)
            });

            alunos.Add(new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = ALUNO_CODIGO_12,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                CodigoTurma = Convert.ToInt64(request.CodigoTurma),
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = dataReferencia.AddDays(-290),
                DataMatricula = dataReferencia.AddDays(-39),
                NomeAluno = ALUNO_CODIGO_13,
                NumeroAlunoChamada = 0,
                SituacaoMatricula = NAO_COMPARECEU,
                NomeResponsavel = RESPONSAVEL,
                TipoResponsavel = TIPO_RESPONSAVEL_4,
                CelularResponsavel = CELULAR_RESPONSAVEL,
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01)
            });

            alunos.Add(new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = ALUNO_CODIGO_13,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.NaoCompareceu,
                CodigoTurma = Convert.ToInt64(request.CodigoTurma),
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = dataReferencia.AddDays(-289),
                DataMatricula = dataReferencia.AddDays(-39),
                NomeAluno = ALUNO_CODIGO_13,
                NumeroAlunoChamada = 0,
                SituacaoMatricula = NAO_COMPARECEU,
                NomeResponsavel = RESPONSAVEL,
                TipoResponsavel = TIPO_RESPONSAVEL_4,
                CelularResponsavel = CELULAR_RESPONSAVEL,
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01)
            });

            return await Task.FromResult(alunos.Where(x => x.CodigoTurma == Convert.ToInt64(request.CodigoTurma)));
        }
    }
}
