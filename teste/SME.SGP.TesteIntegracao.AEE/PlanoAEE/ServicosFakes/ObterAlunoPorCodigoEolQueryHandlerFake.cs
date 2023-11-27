using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes
{
    public class ObterAlunoPorCodigoEolQueryHandlerFake  : IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>
    {
        private readonly string ALUNO_CODIGO_1 = "1"; 

        private readonly string ATIVO = "Ativo";
        private readonly string RESPONSAVEL = "RESPONSAVEL";
        private readonly string TIPO_RESPONSAVEL_4 = "4";
        private readonly string CELULAR_RESPONSAVEL = "11111111111";
        public Task<AlunoPorTurmaResposta> Handle(ObterAlunoPorCodigoEolQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = request.CodigoAluno,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                CodigoTurma = 1,
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                DataMatricula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                NomeAluno = request.CodigoAluno,
                NumeroAlunoChamada = 1,
                SituacaoMatricula = ATIVO,
                NomeResponsavel = RESPONSAVEL,
                TipoResponsavel = TIPO_RESPONSAVEL_4,
                CelularResponsavel = CELULAR_RESPONSAVEL,
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });
        }
    }
}
