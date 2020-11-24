using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorAnoModalidadeQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasPorAnoModalidadeQuery(int anoLetivo, int modalidadeId)
        {
            AnoLetivo = anoLetivo;
            ModalidadeId = modalidadeId;
        }
        public int AnoLetivo { get; set; }
        public int ModalidadeId { get; set; }

    }
}
