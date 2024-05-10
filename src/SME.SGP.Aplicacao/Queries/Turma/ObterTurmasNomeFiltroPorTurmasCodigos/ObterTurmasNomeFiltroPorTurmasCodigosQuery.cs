using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasNomeFiltroPorTurmasCodigosQuery : IRequest<IEnumerable<RetornoConsultaTurmaNomeFiltroDto>>
    {
        public ObterTurmasNomeFiltroPorTurmasCodigosQuery(string[] turmasCodigos)
        {
            TurmasCodigos = turmasCodigos;
        }

        public string[] TurmasCodigos { get; set; }
    }
}
