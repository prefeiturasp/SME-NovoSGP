using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes.ServicosFakes
{
    public class ObterAlunosAtivosTurmaProgramaPapEolQueryMapEstudanteHandlerFake : IRequestHandler<ObterAlunosAtivosTurmaProgramaPapEolQuery, IEnumerable<AlunosTurmaProgramaPapDto>>
    {
        public ObterAlunosAtivosTurmaProgramaPapEolQueryMapEstudanteHandlerFake()
        { }

        public async Task<IEnumerable<AlunosTurmaProgramaPapDto>> Handle(ObterAlunosAtivosTurmaProgramaPapEolQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<AlunosTurmaProgramaPapDto> {
                new AlunosTurmaProgramaPapDto
                {
                      CodigoAluno = 1,
                },
                new AlunosTurmaProgramaPapDto
                {
                    CodigoAluno = 2
                }
            });
        }
    }
}
