using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaEscolaDados
{
    public class ObterProficienciaEscolaDadosQuery : IRequest<PainelEducacionalProficienciaEscolaDadosDto>
    {
        public ObterProficienciaEscolaDadosQuery(string codigoUe)
        {
            CodigoUe = codigoUe;
        }
        public string CodigoUe { get; set; }
    }
}
