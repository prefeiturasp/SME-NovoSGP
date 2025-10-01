using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoMatriculaTurma
{
    public class CarregarDresConsolidacaoMatriculaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CarregarDresConsolidacaoMatriculaUseCase _useCase;

        public CarregarDresConsolidacaoMatriculaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new CarregarDresConsolidacaoMatriculaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Consolidacao_Inativa_Deve_Retornar_False()
        {
            var parametroInativo = new ParametrosSistema { Ativo = false };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(parametroInativo);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            Assert.False(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<LimparConsolidacaoMatriculaTurmaPorAnoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Ativo_Deve_Realizar_Consolidacao_E_Enfileirar_Dres()
        {
            var anoAtual = DateTime.Now.Year;
            var anoParaConsolidar = anoAtual - 1;
            var anoJaConsolidado = anoAtual - 2;

            var parametroAtivo = new ParametrosSistema { Ativo = true };
            _mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q => q.Ano == anoAtual), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(parametroAtivo);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q => q.Ano != anoAtual), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ParametrosSistema());

            _mediatorMock.Setup(m => m.Send(It.Is<ExisteConsolidacaoMatriculaTurmaPorAnoQuery>(q => q.AnoLetivo == anoParaConsolidar), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterQuantidadeUesPorAnoLetivoQuery>(q => q.AnoLetivo == anoParaConsolidar), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(10);

            _mediatorMock.Setup(m => m.Send(It.Is<ExisteConsolidacaoMatriculaTurmaPorAnoQuery>(q => q.AnoLetivo == anoJaConsolidado), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var idsDres = new List<long> { 1, 2 };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdsDresQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(idsDres);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<LimparConsolidacaoMatriculaTurmaPorAnoCommand>(c => c.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<AtualizarParametroSistemaCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgpInstitucional.SincronizarDresMatriculasTurmas), It.IsAny<CancellationToken>()), Times.Exactly(idsDres.Count));
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Falhar_Ao_Publicar_Deve_Salvar_Log_E_Continuar()
        {
            var anoAtual = DateTime.Now.Year;
            var parametroAtivo = new ParametrosSistema { Ativo = true };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(parametroAtivo);

            var idsDres = new List<long> { 1, 2 };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdsDresQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(idsDres);

            _mediatorMock.Setup(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgpInstitucional.SincronizarDresMatriculasTurmas), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new Exception("Erro na fila"));

            var resultado = await _useCase.Executar(new MensagemRabbit());

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(idsDres.Count));
        }
    }
}
