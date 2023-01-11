using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterAlunosInativoDurantePeriodoReaberturaQueryHandlerFake : IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly string ALUNO_CODIGO_1 = "1";
        private readonly string RESPONSAVEL = "RESPONSAVEL";
        private readonly string TIPO_RESPONSAVEL_4 = "4";
        private readonly string CELULAR_RESPONSAVEL = "11111111111";
        private readonly string INATIVO = "Inativo";

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosPorTurmaEAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            var dataSituacao = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 12, DateTime.Now.Day);

            var alunos = new List<AlunoPorTurmaResposta> {
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_1,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula = SituacaoMatriculaAluno.RemanejadoSaida,
                      CodigoTurma =int.Parse(request.CodigoTurma),
                      DataNascimento =new DateTime(1959,01,16,00,00,00),
                      DataSituacao = dataSituacao,
                      DataMatricula = new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno = ALUNO_CODIGO_1,
                      NumeroAlunoChamada =1,
                      SituacaoMatricula = INATIVO,
                      NomeResponsavel = RESPONSAVEL,
                      TipoResponsavel = TIPO_RESPONSAVEL_4,
                      CelularResponsavel = CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                }
            };
            return alunos.Where(x => x.CodigoTurma.ToString() == request.CodigoTurma);
        }
    }
}