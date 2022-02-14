using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarAulasInfantilUseCase : ICriarAulasInfantilUseCase
    {
        private readonly IMediator mediator;

        public CriarAulasInfantilUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {            
            var comando = mensagemRabbit.ObterObjetoMensagem<CriarAulasInfantilAutomaticamenteCommand>();

            await mediator
                    .Send(new SalvarLogViaRabbitCommand($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - Rotina de carregamento para manutenção de aulas do Infantil mensagem criação recebida. Turma: {comando.Turma}.", LogNivel.Informacao, LogContexto.Infantil));

            if (comando != null)
            {
                await mediator.Send(comando);
                
                await mediator
                    .Send(new SalvarLogViaRabbitCommand($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - Rotina de carregamento para manutenção de aulas do Infantil comando criação enviado Turma: {comando.Turma}.", LogNivel.Informacao, LogContexto.Infantil));

                return true;
            }
            return false;
        }
    }
}
