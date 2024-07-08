using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes.ServicosFakes
{
    public class ObterAlunosDentroPeriodoQueryMapEstudanteHandlerFake : IRequestHandler<ObterAlunosDentroPeriodoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        public ObterAlunosDentroPeriodoQueryMapEstudanteHandlerFake()
        {
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosDentroPeriodoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<AlunoPorTurmaResposta> {
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "1"
                },
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "2"
                },
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "3"
                },
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "4"
                },
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "5"
                },
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "6"
                },
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "7"
                },
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "8"
                }
            });
        }
    }
}
