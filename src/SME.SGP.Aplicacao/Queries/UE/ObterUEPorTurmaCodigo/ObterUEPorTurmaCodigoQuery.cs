using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterUEPorTurmaCodigoQuery : IRequest<Ue>
    {
        public ObterUEPorTurmaCodigoQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string TurmaCodigo { get; set; }
    }
}
