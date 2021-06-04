using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasPorAlunosAtividadesAvaliativasQuery : IRequest<IEnumerable<NotaConceito>>
    {
        public ObterNotasPorAlunosAtividadesAvaliativasQuery(long[] atividadesAvaliativasId, string[] alunosId, string componenteCurricularId)
        {
            AtividadesAvaliativasId = atividadesAvaliativasId;
            AlunosId = alunosId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long[] AtividadesAvaliativasId { get; set; }
        public string[] AlunosId { get; set; }
        public string ComponenteCurricularId { get; set; }

    }
}
