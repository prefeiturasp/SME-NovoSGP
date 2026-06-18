using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEstudanteTurmasProgramaUseCase : AbstractUseCase, IObterEstudanteTurmasProgramaUseCase
    {
        public ObterEstudanteTurmasProgramaUseCase(IMediator mediator) : base(mediator)
        { }

        public async Task<IEnumerable<AlunoTurmaProgramaDto>> Executar(string codigoAluno, int? anoLetivo, bool filtrarSituacaoMatricula) => await mediator.Send(new ObterTurmasProgramaAlunoQuery(codigoAluno, anoLetivo, filtrarSituacaoMatricula));

    }
}
