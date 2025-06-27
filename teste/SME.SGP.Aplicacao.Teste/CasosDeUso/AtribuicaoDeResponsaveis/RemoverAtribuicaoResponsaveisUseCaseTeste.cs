using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class RemoverAtribuicaoResponsaveisUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly RemoverAtribuicaoResponsaveisUseCase useCase;
        private readonly string[] dres = new string[] { "1", "2" };

        public RemoverAtribuicaoResponsaveisUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new RemoverAtribuicaoResponsaveisUseCase(mediator.Object);
        }

       [Fact]
        public async Task Deve_Retornar_True_Ao_Executar_Com_Sucesso()
        {
            var mensagem = new MensagemRabbit();
            
            mediator.Setup(x => x.Send(ObterCodigosDresQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Deve_Obter_Lista_De_Dres_Ao_Executar()
        {
            var mensagem = new MensagemRabbit();
            
            mediator.Setup(x => x.Send(ObterCodigosDresQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);

            await useCase.Executar(mensagem);

            mediator.Verify(x => x.Send(ObterCodigosDresQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Processar_Todas_Atribuicoes_Para_Cada_Dre()
        {
           
            var mensagem = new MensagemRabbit();
            
            mediator.Setup(x => x.Send(ObterCodigosDresQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);

            await useCase.Executar(mensagem);

            // Verifica se chamou para cada DRE (2 DREs x 3 tipos de responsáveis = 6 chamadas)
            mediator.Verify(x => x.Send(
                It.Is<PublicarFilaSgpCommand>(c => 
                    c.Rota == RotasRabbitSgp.RemoverAtribuicaoResponsaveisSupervisorPorDre ||
                    c.Rota == RotasRabbitSgp.RemoverAtribuicaoResponsaveisPAAIPorDre ||
                    c.Rota == RotasRabbitSgp.RemoverAtribuicaoResponsaveisASPPorDre),
                It.IsAny<CancellationToken>()), Times.Exactly(6));
        }

        [Fact]
        public async Task Deve_Processar_Atribuicao_Supervisor_Para_Cada_Dre()
        {
           
            var mensagem = new MensagemRabbit();
            
            mediator.Setup(x => x.Send(ObterCodigosDresQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);

            await useCase.Executar(mensagem);

            // Verifica se chamou a rota de supervisor para cada DRE
            mediator.Verify(x => x.Send(
                It.Is<PublicarFilaSgpCommand>(c => 
                    c.Rota == RotasRabbitSgp.RemoverAtribuicaoResponsaveisSupervisorPorDre),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Deve_Processar_Atribuicao_PAAI_Para_Cada_Dre()
        {
           
            var mensagem = new MensagemRabbit();
            
            mediator.Setup(x => x.Send(ObterCodigosDresQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);

            await useCase.Executar(mensagem);

            // Verifica se chamou a rota de PAAI para cada DRE
            mediator.Verify(x => x.Send(
                It.Is<PublicarFilaSgpCommand>(c => 
                    c.Rota == RotasRabbitSgp.RemoverAtribuicaoResponsaveisPAAIPorDre),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Deve_Processar_Atribuicao_ASPP_Para_Cada_Dre()
        {
           
            var mensagem = new MensagemRabbit();
            
            mediator.Setup(x => x.Send(ObterCodigosDresQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);

            await useCase.Executar(mensagem);

            // Verifica se chamou a rota de ASPP para cada DRE
            mediator.Verify(x => x.Send(
                It.Is<PublicarFilaSgpCommand>(c => 
                    c.Rota == RotasRabbitSgp.RemoverAtribuicaoResponsaveisASPPorDre),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Nao_Deve_Processar_Atribuicoes_Se_Nao_Houver_Dres()
        {
            string[] dresEmpty = new string[0];
            var mensagem = new MensagemRabbit();
            
            mediator.Setup(x => x.Send(ObterCodigosDresQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresEmpty);

            await useCase.Executar(mensagem);

            mediator.Verify(x => x.Send(
                It.IsAny<PublicarFilaSgpCommand>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}