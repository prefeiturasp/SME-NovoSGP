using MediatR;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarDadosUsuarioUseCase : AbstractUseCase, IAtualizarDadosUsuarioUseCase
    {
        public AtualizarDadosUsuarioUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(AtualizarDadosUsuarioDto param)
        {
            var dadosResponsaveis = await mediator.Send(new ObterDadosResponsavelQuery(param.CPF));

            if (dadosResponsaveis == null || !dadosResponsaveis.Any())
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
