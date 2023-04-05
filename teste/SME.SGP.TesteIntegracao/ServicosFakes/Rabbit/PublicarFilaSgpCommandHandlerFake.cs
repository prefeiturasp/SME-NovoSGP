using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class PublicarFilaSgpCommandHandlerFake : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IMediator mediator;
        public readonly IUnitOfWork unitOfWork;
        
        public PublicarFilaSgpCommandHandlerFake(IMediator mediator,IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        
        public async Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            string usuarioLogadoNomeCompleto = request.Usuario?.Nome;
            string usuarioLogadoRf = request.Usuario?.CodigoRf;
            Guid? perfilUsuario = request.Usuario?.PerfilAtual;
            var administrador = await mediator.Send(new ObterAdministradorDoSuporteQuery());
            
            if (request.Usuario == null)
            {
                var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

                usuarioLogadoNomeCompleto = usuario.Nome;
                usuarioLogadoRf = usuario.CodigoRf;
                perfilUsuario = usuario.PerfilAtual;
            }

            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(request.Filtros),
                request.CodigoCorrelacao,
                usuarioLogadoNomeCompleto,
                usuarioLogadoRf,
                perfilUsuario,
                request.NotificarErroUsuario,
                administrador.Login);

            //Foram mapeadas somente algumas rotas para testes
            switch (request.Rota)
            {
                case RotasRabbitSgpFrequencia.RotaConsolidacaoDashBoardFrequencia:
                    var useCaseConsolidacaoDashBoardFrequenciaPorDataETipoUseCase = new ConsolidacaoDashBoardFrequenciaPorDataETipoUseCase(mediator);
                    useCaseConsolidacaoDashBoardFrequenciaPorDataETipoUseCase.Executar(mensagem);
                    break;
                
                case RotasRabbitSgpFrequencia.RotaCalculoFrequenciaPorTurmaComponente:
                    var useCaseCalculoFrequenciaTurmaDisciplinaUseCase = new CalculoFrequenciaTurmaDisciplinaUseCase(mediator);
                    useCaseCalculoFrequenciaTurmaDisciplinaUseCase.Executar(mensagem);
                    break;
                
                case RotasRabbitSgpFrequencia.RotaConsolidacaoFrequenciaAlunoPorTurmaMensal:
                    var useCaseConsolidarFrequenciaAlunoPorTurmaEMesUseCase = new ConsolidarFrequenciaAlunoPorTurmaEMesUseCase(mediator, unitOfWork);
                    useCaseConsolidarFrequenciaAlunoPorTurmaEMesUseCase.Executar(mensagem);
                    break;
                
                case RotasRabbitSgpFrequencia.RotaValidacaoAusenciaConciliacaoFrequenciaTurma:
                    var useCaseValidacaoAusenciaConcolidacaoFrequenciaTurmaUseCase = new ValidacaoAusenciaConcolidacaoFrequenciaTurmaUseCase(mediator);
                    useCaseValidacaoAusenciaConcolidacaoFrequenciaTurmaUseCase.Executar(mensagem);
                    break;
            }

            return true;
        }
    }
}