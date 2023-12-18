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
    public class ObterAlunosAtivosPorTurmaCodigoQueryHandlerFakeValidarAlunosFrequencia : IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>
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
        private readonly string NAO_COMPARECEU = "Não Compareceu";
        private readonly string DESISTENTE = "Desistente";
        private readonly string VINCULO_INDEVIDO = "Vínculo indevido";
        private readonly string FALECIDO = "Falecido";
        private readonly string DESLOCAMENTO = "Deslocamento";
        private readonly string CESSADO = "Cessado";
        private readonly string RECLASSIFICADO_SAIDA = "Reclassificado saída";

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosAtivosPorTurmaCodigoQuery request, CancellationToken cancellationToken)
        {
            var dataRefencia = DateTimeExtension.HorarioBrasilia();

            return await Task.FromResult(new List<AlunoPorTurmaResposta>()
              {
                  new AlunoPorTurmaResposta() {
                      Ano = DateTime.Now.Year ,
                      DataSituacao = dataRefencia.AddDays(-5),
                      CodigoAluno = ALUNO_CODIGO_1,
                      CodigoComponenteCurricular = 138,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      SituacaoMatricula = ATIVO,
                      NomeAluno = "NOME_ALUNO_1"
                  },
                  new AlunoPorTurmaResposta() {
                      Ano = DateTime.Now.Year ,
                      DataSituacao = dataRefencia.AddDays(-7),
                      CodigoAluno = ALUNO_CODIGO_2,
                      CodigoComponenteCurricular= 138,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      SituacaoMatricula = ATIVO,
                      NomeAluno = "NOME_ALUNO_2"
                  },
                  new AlunoPorTurmaResposta() {
                      Ano = DateTime.Now.Year ,
                      DataSituacao = dataRefencia.AddDays(-25),
                      CodigoAluno = ALUNO_CODIGO_3,
                      CodigoComponenteCurricular= 138,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      SituacaoMatricula = ATIVO,
                      NomeAluno = "NOME_ALUNO_3"
                  },
                  new AlunoPorTurmaResposta() {
                      Ano = DateTime.Now.Year ,
                      DataSituacao = dataRefencia.AddDays(-20),
                      CodigoAluno = ALUNO_CODIGO_4,
                      CodigoComponenteCurricular= 138,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      SituacaoMatricula = ATIVO,
                      NomeAluno = "NOME_ALUNO_4"
                  },
                  new AlunoPorTurmaResposta() {
                      Ano = DateTime.Now.Year ,
                      DataSituacao = dataRefencia.AddDays(7),
                      DataMatricula = dataRefencia.AddDays(-14),
                      CodigoAluno = ALUNO_CODIGO_5,
                      CodigoComponenteCurricular= 138,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.NaoCompareceu,
                      SituacaoMatricula = NAO_COMPARECEU,
                      NomeAluno = "NOME_ALUNO_5"
                  },
                  new AlunoPorTurmaResposta() {
                      Ano = DateTime.Now.Year ,
                      DataSituacao = dataRefencia.AddDays(8),
                      DataMatricula = dataRefencia.AddDays(-10),
                      CodigoAluno = ALUNO_CODIGO_6,
                      CodigoComponenteCurricular= 138,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Desistente,
                      SituacaoMatricula = DESISTENTE,
                      NomeAluno = "NOME_ALUNO_6"
                  },
                  new AlunoPorTurmaResposta() {
                      Ano = DateTime.Now.Year ,
                      DataSituacao = dataRefencia.AddDays(6),
                      DataMatricula = dataRefencia.AddDays(-12),
                      CodigoAluno = ALUNO_CODIGO_7,
                      CodigoComponenteCurricular= 138,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.VinculoIndevido,
                      SituacaoMatricula = VINCULO_INDEVIDO,
                      NomeAluno = "NOME_ALUNO_7"
                  },
                  new AlunoPorTurmaResposta() {
                      Ano = DateTime.Now.Year ,
                      DataSituacao = dataRefencia.AddDays(5),
                      DataMatricula = dataRefencia.AddDays(-12),
                      CodigoAluno = ALUNO_CODIGO_8,
                      CodigoComponenteCurricular= 138,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Falecido,
                      SituacaoMatricula = FALECIDO,
                      NomeAluno = "NOME_ALUNO_8"
                  },
                  new AlunoPorTurmaResposta() {
                      Ano = DateTime.Now.Year ,
                      DataSituacao = dataRefencia.AddDays(9),
                      DataMatricula = dataRefencia.AddDays(-10),
                      CodigoAluno = ALUNO_CODIGO_9,
                      CodigoComponenteCurricular= 138,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Deslocamento,
                      SituacaoMatricula = DESLOCAMENTO,
                      NomeAluno = "NOME_ALUNO_9"
                  },
                  new AlunoPorTurmaResposta() {
                      Ano = DateTime.Now.Year ,
                      DataSituacao = dataRefencia.AddDays(10),
                      DataMatricula = dataRefencia.AddDays(-11),
                      CodigoAluno = ALUNO_CODIGO_10,
                      CodigoComponenteCurricular= 138,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Cessado,
                      SituacaoMatricula = CESSADO,
                      NomeAluno = "NOME_ALUNO_10"
                  },
                  new AlunoPorTurmaResposta() {
                      Ano = DateTime.Now.Year ,
                      DataSituacao = dataRefencia.AddDays(12),
                      DataMatricula = dataRefencia.AddDays(-10),
                      CodigoAluno = ALUNO_CODIGO_11,
                      CodigoComponenteCurricular= 138,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.ReclassificadoSaida,
                      SituacaoMatricula = RECLASSIFICADO_SAIDA,
                      NomeAluno = "NOME_ALUNO_11"
                  },
                  new AlunoPorTurmaResposta() {
                      Ano = DateTime.Now.Year ,
                      DataSituacao = dataRefencia.AddDays(-54),
                      DataMatricula = dataRefencia.AddDays(-120),
                      CodigoAluno = ALUNO_CODIGO_12,
                      CodigoComponenteCurricular= 138,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Desistente,
                      SituacaoMatricula = DESISTENTE,
                      NomeAluno = "NOME_ALUNO_12"
                  },
                  new AlunoPorTurmaResposta() {
                      Ano = DateTime.Now.Year ,
                      DataSituacao = dataRefencia.AddDays(-55),
                      DataMatricula = dataRefencia.AddDays(-130),
                      CodigoAluno = ALUNO_CODIGO_13,
                      CodigoComponenteCurricular= 138,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.NaoCompareceu,
                      SituacaoMatricula = NAO_COMPARECEU,
                      NomeAluno = "NOME_ALUNO_13"
                  },
              });
        }
    }
}
