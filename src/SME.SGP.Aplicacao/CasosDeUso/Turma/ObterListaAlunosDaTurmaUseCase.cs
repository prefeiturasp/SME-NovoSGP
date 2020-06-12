using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListaAlunosDaTurmaUseCase : AbstractUseCase, IObterListaAlunosDaTurmaUseCase
    {
        public ObterListaAlunosDaTurmaUseCase(IMediator mediator): base(mediator)
        {
        }

        public async Task<IEnumerable<AlunoSimplesDto>> Executar(string turmaCodigo)
            => await mediator.Send(new ObterAlunosSimplesDaTurmaQuery(turmaCodigo));
    }
}
