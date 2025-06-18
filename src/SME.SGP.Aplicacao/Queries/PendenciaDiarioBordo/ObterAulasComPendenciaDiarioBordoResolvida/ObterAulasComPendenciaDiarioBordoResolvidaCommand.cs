using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommand : 
        IRequest<IEnumerable<PendenciaDiarioBordoParaExcluirDto>>
    {
        public string TurmaId { get; set; }

        public ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommand(string turmaId)
        {
            TurmaId = turmaId;
        }
    }
}