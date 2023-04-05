using System;
using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao
{
    public class PublicarFilaEmLoteSgpCommandHandlerFake : IRequestHandler<PublicarFilaEmLoteSgpCommand, bool>
    {
        private readonly IMediator mediator;
        
        public PublicarFilaEmLoteSgpCommandHandlerFake(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        public async Task<bool> Handle(PublicarFilaEmLoteSgpCommand request, CancellationToken cancellationToken)
        {
            foreach (var command in request.Commands)
            {
                string usuarioLogadoNomeCompleto = command.Usuario?.Nome;
                string usuarioLogadoRf = command.Usuario?.CodigoRf;
                Guid? perfilUsuario = command.Usuario?.PerfilAtual;
                var administrador = await mediator.Send(new ObterAdministradorDoSuporteQuery());

                if (command.Usuario == null)
                {
                    var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

                    usuarioLogadoNomeCompleto = usuario.Nome;
                    usuarioLogadoRf = usuario.CodigoRf;
                    perfilUsuario = usuario.PerfilAtual;
                }

                var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(command.Filtros),
                    command.CodigoCorrelacao,
                    usuarioLogadoNomeCompleto,
                    usuarioLogadoRf,
                    perfilUsuario,
                    command.NotificarErroUsuario,
                    administrador.Login);

                //Foram mapeadas somente algumas rotas para testes
                switch (command.Rota)
                {
                    case RotasRabbitSgpAula.NotificacoesDaAulaExcluir:
                        var useCaseExcluirNotificacoesPorAulaIdUseCase = new ExcluirNotificacoesPorAulaIdUseCase(mediator);
                        useCaseExcluirNotificacoesPorAulaIdUseCase.Executar(mensagem);
                        break;
                    
                    case RotasRabbitSgpFrequencia.FrequenciaDaAulaExcluir:
                        var useCaseExcluirFrequenciaPorAulaIdUseCase = new ExcluirFrequenciaPorAulaIdUseCase(mediator);
                        useCaseExcluirFrequenciaPorAulaIdUseCase.Executar(mensagem);
                        break;
                    
                    case RotasRabbitSgpAula.PlanoAulaDaAulaExcluir:
                        var useCaseExcluirPlanoAulaPorAulaIdUseCase = new ExcluirPlanoAulaPorAulaIdUseCase(mediator);
                        useCaseExcluirPlanoAulaPorAulaIdUseCase.Executar(mensagem);
                        break;
                    
                    case RotasRabbitSgpFrequencia.AnotacoesFrequenciaDaAulaExcluir:
                        var useCaseExcluirAnotacoesFrequenciaPorAulaIdUseCase = new ExcluirAnotacoesFrequenciaPorAulaIdUseCase(mediator);
                        useCaseExcluirAnotacoesFrequenciaPorAulaIdUseCase.Executar(mensagem);
                        break;
                    
                    case RotasRabbitSgp.DiarioBordoDaAulaExcluir:
                        var useCaseExcluirDiarioBordoPorAulaIdUseCase = new ExcluirDiarioBordoPorAulaIdUseCase(mediator);
                        useCaseExcluirDiarioBordoPorAulaIdUseCase.Executar(mensagem);
                        break;
                    
                    case RotasRabbitSgpAula.RotaExecutaExclusaoPendenciasAula:
                        var useCaseExecutarExclusaoPendenciasAulaUseCase = new ExecutarExclusaoPendenciasAulaUseCase(mediator);
                        useCaseExecutarExclusaoPendenciasAulaUseCase.Executar(mensagem);
                        break;
                    
                    case RotasRabbitSgpAula.RotaExecutaExclusaoPendenciaDiarioBordoAula:
                        var useCaseExcluirPendenciaDiarioBordoPorAulaIdUseCase = new ExcluirPendenciaDiarioBordoPorAulaIdUseCase(mediator);
                        useCaseExcluirPendenciaDiarioBordoPorAulaIdUseCase.Executar(mensagem);
                        break;
                    
                    case RotasRabbitSgp.ExclusaoCompensacaoAusenciaAlunoEAula:
                        var useCaseExcluirCompensacaoAusenciaPorAulaIdUseCase = new ExcluirCompensacaoAusenciaPorAulaIdUseCase(mediator);
                        useCaseExcluirCompensacaoAusenciaPorAulaIdUseCase.Executar(mensagem);
                        break;                
                }
            }
            return true;
        }
    }
}
