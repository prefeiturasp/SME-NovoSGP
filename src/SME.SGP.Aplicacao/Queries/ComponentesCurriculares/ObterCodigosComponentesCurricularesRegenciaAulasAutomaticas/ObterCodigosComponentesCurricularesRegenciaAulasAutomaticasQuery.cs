using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosComponentesCurricularesRegenciaAulasAutomaticasQuery : IRequest<string[]>
    {
        public ObterCodigosComponentesCurricularesRegenciaAulasAutomaticasQuery(Modalidade modalidade)
        {
            Modalidade = modalidade;
        }

        public Modalidade Modalidade { get; set; }
    }
}
