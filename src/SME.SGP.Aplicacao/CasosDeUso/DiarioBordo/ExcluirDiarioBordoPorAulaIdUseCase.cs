using System;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirDiarioBordoPorAulaIdUseCase : AbstractUseCase, IExcluirDiarioBordoPorAulaIdUseCase
    {
        public ExcluirDiarioBordoPorAulaIdUseCase(IMediator mediator) : base(mediator)
        {}

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroIdDto>();
            return (await mediator.Send(new ExcluirDiarioBordoDaAulaCommand(filtro.Id)));           
        }
    }
}
