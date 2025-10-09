using MediatR;
using SME.SGP.Infra.Dtos.Escola;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosEscolaPorUeEolQuery : IRequest<DadosEscolaDto>
    {
        public ObterDadosEscolaPorUeEolQuery(string ueCodigo)
        {
            UeCodigo = ueCodigo;
        }
        public string UeCodigo { get; set; }
    }
}
