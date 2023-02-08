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
    public class ObterAlunosEolPorTurmaQueryHandlerParecerConclusivo : IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private const string ALUNO_CODIGO_1 = "1";
        private const string NOME_ALUNO_CODIGO_1 = "NOME_ALUNO_CODIGO_1";
        private const string NOME_RESPONSAVEL_ALUNO_CODIGO_1 = "NOME_RESPONSAVEL_ALUNO_CODIGO_1";
        private const string TIPO_RESPONSAVEL_1 = "TIPO_RESPONSAVEL_1";

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosEolPorTurmaQuery request, CancellationToken cancellationToken)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;

            return new List<AlunoPorTurmaResposta> {

                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_1,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(request.TurmaId),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= dataReferencia,
                      DataMatricula= dataReferencia.AddDays(-2),
                      NomeAluno= NOME_ALUNO_CODIGO_1,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= "Ativo",
                      NomeResponsavel= NOME_RESPONSAVEL_ALUNO_CODIGO_1,
                      TipoResponsavel= TIPO_RESPONSAVEL_1,
                      CelularResponsavel="111111",
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                }
            };
        }
    }
}
