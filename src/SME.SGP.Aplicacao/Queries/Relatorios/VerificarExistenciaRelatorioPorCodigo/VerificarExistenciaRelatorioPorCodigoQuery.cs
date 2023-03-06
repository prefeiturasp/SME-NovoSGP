using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class VerificarExistenciaRelatorioPorCodigoQuery : IRequest<bool>
    {
        public VerificarExistenciaRelatorioPorCodigoQuery(Guid codigoRelatorio)
        {
            CodigoRelatorio = codigoRelatorio;
        }

        public Guid CodigoRelatorio { get; set; }
    }
}
