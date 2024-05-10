using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterEstudanteTurmasProgramaUseCase : AbstractUseCase, IObterEstudanteTurmasProgramaUseCase
    {
        public ObterEstudanteTurmasProgramaUseCase(IMediator mediator) : base(mediator)
        {}

        public async Task<IEnumerable<AlunoTurmaProgramaDto>> Executar(string codigoAluno, int? anoLetivo, bool filtrarSituacaoMatricula) => await mediator.Send(new ObterTurmasProgramaAlunoQuery(codigoAluno, anoLetivo, filtrarSituacaoMatricula));

    }
}
