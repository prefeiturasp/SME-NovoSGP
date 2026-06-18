using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasPendenciaSemFechamentoTurmaDiscplinaProcessadoQuery : IRequest<IEnumerable<Aula>>
    {
        public ObterAulasPendenciaSemFechamentoTurmaDiscplinaProcessadoQuery(IEnumerable<Aula> aulasPendencia)
        {
            AulasPendencia = aulasPendencia;
        }

        public IEnumerable<Aula> AulasPendencia { get; set; }
    }
}
