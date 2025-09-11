using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoDevolutivas
{
    public class ConsolidarDevolutivasPorTurmaInfantilUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsolidarDevolutivasPorTurmaInfantilUseCase useCase;

        public ConsolidarDevolutivasPorTurmaInfantilUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsolidarDevolutivasPorTurmaInfantilUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Publicar_Comando_Para_Turmas_E_Atualizar_DataExecucao()
        {
            var anoAtual = DateTime.Now.Year;
            long ueId = 1234;
            var mensagem = new MensagemRabbit
            {
                Mensagem = ueId.ToString()
            };

            var turmas = new List<DevolutivaTurmaDTO>
            {
                new DevolutivaTurmaDTO { TurmaId = 1 },
                new DevolutivaTurmaDTO { TurmaId = 2 }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turmas);

            mediatorMock.Setup(m => m.Send(It.IsAny<ExisteConsolidacaoDevolutivaTurmaPorAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true); // Ignora histórico

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new ParametrosSistema { Tipo = TipoParametroSistema.ExecucaoConsolidacaoDevolutivasTurma, Ano = anoAtual });

            mediatorMock.Setup(m => m.Send(It.IsAny<AtualizarParametroSistemaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1L);

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            resultado.Should().BeTrue();

            mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(
                cmd => cmd.Rota == RotasRabbitSgp.ConsolidarDevolutivasPorTurma), It.IsAny<CancellationToken>()), Times.Exactly(2));

            mediatorMock.Verify(m => m.Send(It.IsAny<AtualizarParametroSistemaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Turmas_Nulas_Ou_Vazias()
        {
            long ueId = 1234;
            var mensagem = new MensagemRabbit { Mensagem = ueId.ToString() };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((IEnumerable<DevolutivaTurmaDTO>)null); 

            Func<Task> acao = async () => await useCase.Executar(mensagem);

            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage("Não foi possível localizar turmas para consolidar dados de devolutivas");
        }

        [Fact]
        public async Task Executar_Deve_Salvar_Log_Quando_Erro_Ocorre_Ao_Publicar_Comando()
        {
            long ueId = 1234;
            int anoAtual = DateTime.Now.Year;
            var mensagem = new MensagemRabbit { Mensagem = ueId.ToString() };

            var turmas = new List<DevolutivaTurmaDTO> { new DevolutivaTurmaDTO { TurmaId = 10 } };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turmas);

            mediatorMock.Setup(m => m.Send(It.IsAny<ExisteConsolidacaoDevolutivaTurmaPorAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true); // ignora histórico

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                        .ThrowsAsync(new Exception("Erro ao publicar"));

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new ParametrosSistema { Tipo = TipoParametroSistema.ExecucaoConsolidacaoDevolutivasTurma, Ano = anoAtual });

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            resultado.Should().BeTrue();

            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Consolidar_Historico_Se_Nao_Houver_Consolidacao_Anterior()
        {
            long ueId = 1234;
            var mensagem = new MensagemRabbit { Mensagem = ueId.ToString() };
            var anoAtual = DateTime.Now.Year;

            var turmas = new List<DevolutivaTurmaDTO> { new DevolutivaTurmaDTO { TurmaId = 20 } };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turmas);

            mediatorMock.Setup(m => m.Send(It.Is<ExisteConsolidacaoDevolutivaTurmaPorAnoQuery>(q => q.Ano < anoAtual), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(false);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new ParametrosSistema { Tipo = TipoParametroSistema.ExecucaoConsolidacaoDevolutivasTurma, Ano = It.IsAny<int>() });

            mediatorMock.Setup(m => m.Send(It.IsAny<AtualizarParametroSistemaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1L);

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            resultado.Should().BeTrue();

            mediatorMock.Verify(m => m.Send(It.Is<ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQuery>(
                x => x.AnoLetivo < anoAtual), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }
    }
}
