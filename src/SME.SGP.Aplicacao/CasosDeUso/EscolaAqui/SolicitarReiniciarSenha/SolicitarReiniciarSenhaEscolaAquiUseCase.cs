using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.SolicitarReiniciarSenha;
using SME.SGP.Infra.Dtos.EscolaAqui;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SolicitarReiniciarSenhaEscolaAquiUseCase : ISolicitarReiniciarSenhaEscolaAquiUseCase
    {
        private readonly IMediator mediator;

        public SolicitarReiniciarSenhaEscolaAquiUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<RespostaSolicitarReiniciarSenhaEscolaAquiDto> Executar(string cpf)
        {
            return await mediator.Send(new SolicitarReiniciarSenhaEscolaAquiCommand(cpf));
        }
    }
}
