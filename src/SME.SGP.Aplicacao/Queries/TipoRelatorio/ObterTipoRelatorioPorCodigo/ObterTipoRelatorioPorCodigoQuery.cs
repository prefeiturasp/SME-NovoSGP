using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoRelatorioPorCodigoQuery : IRequest<int>
    {
        public ObterTipoRelatorioPorCodigoQuery(string codigoRelatorio)
        {
            CodigoRelatorio = codigoRelatorio;
        }

        public string CodigoRelatorio { get; }
    }
}