using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoEscolaPorCodigoQuery : IRequest<TipoEscolaEol>
    {
        public ObterTipoEscolaPorCodigoQuery(long codigo)
        {
            Codigo = codigo;
        }

        public long Codigo { get; set; }
    }
}
