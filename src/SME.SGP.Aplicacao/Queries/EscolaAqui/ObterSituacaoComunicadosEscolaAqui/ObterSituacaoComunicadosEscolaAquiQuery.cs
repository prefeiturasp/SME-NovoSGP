using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadoEvento;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterSituacaoComunicadosEscolaAquiQuery : IRequest<IEnumerable<SituacaoComunicadoEADto>>
    {
        public string AlunoCodigo { get; set; }
        public long[] ComunicadosIds { get; set; }

        public ObterSituacaoComunicadosEscolaAquiQuery(string alunoCodigo, long[] comunicadosIds)
        {
            AlunoCodigo = alunoCodigo;
            ComunicadosIds = comunicadosIds;
        }
    }
}
