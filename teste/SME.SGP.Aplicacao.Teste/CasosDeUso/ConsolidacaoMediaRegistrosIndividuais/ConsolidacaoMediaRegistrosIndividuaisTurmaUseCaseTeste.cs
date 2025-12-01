using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoMediaRegistrosIndividuais
{
    public class ConsolidacaoMediaRegistrosIndividuaisTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidacaoMediaRegistrosIndividuaisTurmaUseCase _useCase;

        public ConsolidacaoMediaRegistrosIndividuaisTurmaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsolidacaoMediaRegistrosIndividuaisTurmaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Parametro_De_Execucao_Estiver_Inativo_Deve_Retornar_False()
        {
            var parametroInativo = new ParametrosSistema { Ativo = false };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroInativo);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            Assert.False(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<LimparConsolidacaoMediaRegistroIndividualCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Parametro_De_Execucao_Nao_For_Encontrado_Deve_Retornar_False()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ParametrosSistema)null);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            Assert.False(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmasComRegistrosIndividuaisPorModalidadeEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Nao_Houver_Turmas_Deve_Executar_Sem_Publicar_Mensagens()
        {          
            var parametroAtivo = new ParametrosSistema { Ativo = true, Valor = "" };

            _mediatorMock.SetupSequence(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroAtivo) 
                .ReturnsAsync(parametroAtivo); 

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasComRegistrosIndividuaisPorModalidadeEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<RegistroIndividualDTO>()); 

            var resultado = await _useCase.Executar(new MensagemRabbit());

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.IsAny<LimparConsolidacaoMediaRegistroIndividualCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<AtualizarParametroSistemaCommand>(), It.IsAny<CancellationToken>()), Times.Once); 
        }

        [Fact]
        public async Task Executar_Quando_Execucao_For_Permitida_E_Turmas_Encontradas_Deve_Executar_Com_Sucesso()
        {
            var anoAtual = DateTime.Now.Year;
            var parametroAtivo = new ParametrosSistema { Ativo = true, Valor = "" };
            var turmas = new List<RegistroIndividualDTO>
        {
            new RegistroIndividualDTO { TurmaId = 1, AnoLetivo = anoAtual },
            new RegistroIndividualDTO { TurmaId = 2, AnoLetivo = anoAtual }
        };

            _mediatorMock.SetupSequence(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroAtivo) 
                .ReturnsAsync(parametroAtivo); 

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmasComRegistrosIndividuaisPorModalidadeEAnoQuery>(q => q.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmas);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<LimparConsolidacaoMediaRegistroIndividualCommand>(c => c.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgp.ConsolidarMediaRegistrosIndividuais), It.IsAny<CancellationToken>()), Times.Exactly(turmas.Count));
            _mediatorMock.Verify(m => m.Send(It.IsAny<AtualizarParametroSistemaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Ocorrer_Erro_Ao_Publicar_Mensagem_Deve_Registrar_Log_E_Continuar()
        {
            var anoAtual = DateTime.Now.Year;
            var parametroAtivo = new ParametrosSistema { Ativo = true, Valor = "" };
            var turmas = new List<RegistroIndividualDTO>
    {
        new RegistroIndividualDTO { TurmaId = 1, AnoLetivo = anoAtual }, 
        new RegistroIndividualDTO { TurmaId = 2, AnoLetivo = anoAtual }  
    };

            _mediatorMock.SetupSequence(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroAtivo)
                .ReturnsAsync(parametroAtivo);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasComRegistrosIndividuaisPorModalidadeEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmas);

            _mediatorMock.Setup(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => ((FiltroMediaRegistroIndividualTurmaDTO)c.Filtros).TurmaId == 1), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro de publicação"));

            var resultado = await _useCase.Executar(new MensagemRabbit());

            Assert.True(resultado); 

            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgp.ConsolidarMediaRegistrosIndividuais), It.IsAny<CancellationToken>()), Times.Exactly(turmas.Count));

            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(c => c.Observacao == "Erro de publicação"), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<AtualizarParametroSistemaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Parametro_Para_Atualizacao_Nao_For_Encontrado_Deve_Completar_Sem_Atualizar()
        {
            var anoAtual = DateTime.Now.Year;
            var parametroAtivo = new ParametrosSistema { Ativo = true };
            var turmas = new List<RegistroIndividualDTO>
        {
            new RegistroIndividualDTO { TurmaId = 1, AnoLetivo = anoAtual }
        };

            _mediatorMock.SetupSequence(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroAtivo) 
                .ReturnsAsync((ParametrosSistema)null); 

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasComRegistrosIndividuaisPorModalidadeEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmas);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<AtualizarParametroSistemaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
