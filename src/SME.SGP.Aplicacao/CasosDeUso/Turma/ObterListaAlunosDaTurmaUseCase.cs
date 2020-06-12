using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Turma
{
    public class ObterListaAlunosDaTurmaUseCase : AbstractUseCase, IObterListaAlunosDaTurmaUseCase
    {
        public ObterListaAlunosDaTurmaUseCase(IMediator mediator): base(mediator)
        {
        }

        public Task<IEnumerable<AlunoSimplesDto>> Executar(FiltroListagemAlunosDto param)
        {
            throw new NotImplementedException();
        }
    }
}
