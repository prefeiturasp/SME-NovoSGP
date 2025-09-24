using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao.Queries.UE.ObterUePorCodigoEolEscola
{
    public class ObterUePorCodigoEolEscolaQuery : IRequest<Ue>
    {
        public ObterUePorCodigoEolEscolaQuery(string codigoEolEscola)
        {
            CodigoEolEscola = codigoEolEscola;
        }
        public string CodigoEolEscola { get; }
    }
}
