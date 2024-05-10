using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasDiarioPorPendenciaIdEProfessorQuery : IRequest<IEnumerable<PendenciaDiarioBordoDescricaoDto>>
    {
        public ObterPendenciasDiarioPorPendenciaIdEProfessorQuery(long pendenciaId, string codigoRf)
        {
            PendenciaId = pendenciaId;
            CodigoRf = codigoRf;
        }

        public long PendenciaId { get; set; }
        public string CodigoRf { get; set; }
    }
}
