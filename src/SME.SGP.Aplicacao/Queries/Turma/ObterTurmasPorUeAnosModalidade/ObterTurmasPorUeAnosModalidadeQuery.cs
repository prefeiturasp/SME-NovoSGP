using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorUeAnosModalidadeQuery : IRequest<IEnumerable<long>>
    {
        public ObterTurmasPorUeAnosModalidadeQuery(string ueCodigo, int anoLetivo, string[] anos, int modalidadeId)
        {
            UeCodigo = ueCodigo;
            AnoLetivo = anoLetivo;
            Anos = anos;
            ModalidadeId = modalidadeId;
        }
        public string UeCodigo { get; set; }
        public int AnoLetivo { get; set; }
        public string[] Anos { get; set; }
        public int ModalidadeId { get; set; }

    }
}
