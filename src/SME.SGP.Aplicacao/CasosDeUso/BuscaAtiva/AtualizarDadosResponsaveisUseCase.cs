using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarDadosResponsaveisUseCase : AbstractUseCase, IAtualizarDadosResponsaveisUseCase
    {
        public AtualizarDadosResponsaveisUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(AtualizarDadosResponsavelDto param)
        {
            var dadosResponsaveis = await mediator.Send(new ObterDadosResponsavelQuery(param.CPF));

            if (dadosResponsaveis.EhNulo() || !dadosResponsaveis.Any())
                return false;

            foreach(var dadosResponsavel in dadosResponsaveis) 
            {
                await mediator.Send(new AtualizarDadosResponsavelAlunoEolCommand(new DadosResponsavelAlunoBuscaAtivaDto(dadosResponsavel, param)));
                await mediator.Send(new AtualizarDadosResponsavelAlunoProdamCommand(new DadosResponsavelAlunoProdamDto(dadosResponsavel, param)));
            }

            return true;
        }
    }
}
