using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

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
