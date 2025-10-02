using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaEscolaDados;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasProficienciaEscolaDadosUseCase : IConsultasProficienciaEscolaDadosUseCase
    {
        private readonly IMediator mediator;

        public ConsultasProficienciaEscolaDadosUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<PainelEducacionalProficienciaEscolaDadosDto> ObterProficienciaEscolaDados(string codigoUe)
        {
            if (string.IsNullOrWhiteSpace(codigoUe))
                throw new NegocioException("Informe a unidade escolar");

            var proficienciaEscolaDados = await mediator.Send(new ObterProficienciaEscolaDadosQuery(codigoUe));

            return proficienciaEscolaDados;
        }
    }
}
