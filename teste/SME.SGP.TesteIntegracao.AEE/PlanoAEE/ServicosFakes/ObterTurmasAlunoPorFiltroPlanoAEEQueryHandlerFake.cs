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
    public class ObterTurmasAlunoPorFiltroPlanoAEEQueryHandlerFake  : IRequestHandler<ObterTurmasAlunoPorFiltroQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly string ATIVO = "Ativo";
        private readonly string RESPONSAVEL = "RESPONSAVEL";
        private readonly string TIPO_RESPONSAVEL_4 = "4";
        private readonly string CELULAR_RESPONSAVEL = "11111111111";
        public Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterTurmasAlunoPorFiltroQuery request, CancellationToken cancellationToken)
        {
            var lista = new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta
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
                }
            };

            return Task.FromResult(lista.Where(x => x.CodigoAluno == request.CodigoAluno));
        }
    }
}