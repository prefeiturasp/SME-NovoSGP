using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class CadastrarDevolutivaPAAIPlanoAEEUseCase : ICadastrarDevolutivaPAAIPlanoAEEUseCase
    {
        private readonly IMediator mediator;

        public CadastrarDevolutivaPAAIPlanoAEEUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(long planoAEEId, PlanoAEECadastroDevolutivaDto planoDto)
        {
            var retorno = await mediator.Send(new CadastrarDevolutivaPAAICommand(planoAEEId, planoDto.Parecer));
            return retorno;
        }
    }
}