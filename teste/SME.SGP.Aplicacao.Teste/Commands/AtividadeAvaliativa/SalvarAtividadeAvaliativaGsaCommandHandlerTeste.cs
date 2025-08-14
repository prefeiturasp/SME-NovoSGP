using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.AtividadeAvaliativa
{
    public class SalvarAtividadeAvaliativaGsaCommandHandlerTeste
    {
        private readonly Mock<IRepositorioAtividadeAvaliativa> repositorioMock;
        private readonly Mock<IMediator> mediatorMock;
        private readonly SalvarAtividadeAvaliativaGsaCommandHandlerFake handlerFake;

        public SalvarAtividadeAvaliativaGsaCommandHandlerTeste()
        {
            repositorioMock = new Mock<IRepositorioAtividadeAvaliativa>();
            mediatorMock = new Mock<IMediator>();
            handlerFake = new SalvarAtividadeAvaliativaGsaCommandHandlerFake(repositorioMock.Object, mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_Atividade_Existente_Deve_Chamar_Alterar_E_Atualizar()
        {
            var command = CriarCommand();

            var atividadeExistente = new SME.SGP.Dominio.AtividadeAvaliativa
            {
                AtividadeClassroomId = command.AtividadeClassroomId,
                NomeAvaliacao = "Old",
                DescricaoAvaliacao = "Old",
                DataAvaliacao = DateTime.Today
            };

            repositorioMock.Setup(r => r.ObterPorAtividadeClassroomId(command.AtividadeClassroomId))
                .ReturnsAsync(atividadeExistente);

            repositorioMock.Setup(r => r.SalvarAsync(It.IsAny<SME.SGP.Dominio.AtividadeAvaliativa>()))
                .ReturnsAsync(123);

            await handlerFake.Executar(command);

            repositorioMock.Verify(r => r.ObterPorAtividadeClassroomId(command.AtividadeClassroomId), Times.Once);
            repositorioMock.Verify(r => r.SalvarAsync(It.Is<SME.SGP.Dominio.AtividadeAvaliativa>(a =>
                a.NomeAvaliacao == command.Titulo &&
                a.DescricaoAvaliacao == command.Descricao &&
                a.DataAvaliacao == command.DataAula
            )), Times.Once);

            mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<object>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Atividade_Nao_Existente_Deve_Chamar_Inserir_E_Executar_Fluxo_Completo()
        {
            var command = CriarCommand();

            repositorioMock.Setup(r => r.ObterPorAtividadeClassroomId(command.AtividadeClassroomId))
                .ReturnsAsync((SME.SGP.Dominio.AtividadeAvaliativa)null);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Valor = "1" });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoAvaliacaoPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(10L);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCodigosDreUePorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DreUeDaTurmaDto { DreCodigo = "100", UeCodigo = "200" });

            repositorioMock.Setup(r => r.SalvarAsync(It.IsAny<SME.SGP.Dominio.AtividadeAvaliativa>()))
                .ReturnsAsync(99L);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarAtividadeAvaliativaDisciplinaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            await handlerFake.Executar(command);

            repositorioMock.Verify(r => r.ObterPorAtividadeClassroomId(command.AtividadeClassroomId), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTipoAvaliacaoPorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterCodigosDreUePorTurmaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            repositorioMock.Verify(r => r.SalvarAsync(It.IsAny<SME.SGP.Dominio.AtividadeAvaliativa>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarAtividadeAvaliativaDisciplinaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Parametro_Tipo_Atividade_Nao_Encontrado_Deve_Lancar_Negocio_Exception()
        {
            var command = CriarCommand();

            repositorioMock.Setup(r => r.ObterPorAtividadeClassroomId(command.AtividadeClassroomId))
                .ReturnsAsync((SME.SGP.Dominio.AtividadeAvaliativa)null);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ParametrosSistema)null);

            Func<Task> act = async () => await handlerFake.Executar(command);

            await act.Should().ThrowAsync<NegocioException>()
                .WithMessage($"Parametro de tipo de atividade avaliativa do classroom não localizado para o ano {command.DataCriacao.Year}");
        }

        private SalvarAtividadeAvaliativaGsaCommand CriarCommand()
        {
            var dto = new AtividadeGsaDto
            {
                UsuarioRf = "usuario123",
                TurmaId = "turma1",
                ComponenteCurricularId = 5,
                Titulo = "Titulo atividade",
                Descricao = "Descricao atividade",
                DataCriacao = new DateTime(2025, 1, 1),
                DataAlteracao = null,
                AtividadeClassroomId = 1234
            };

            return new SalvarAtividadeAvaliativaGsaCommand(DateTime.Today, dto);
        }

        public class SalvarAtividadeAvaliativaGsaCommandHandlerFake : SalvarAtividadeAvaliativaGsaCommandHandler
        {
            public SalvarAtividadeAvaliativaGsaCommandHandlerFake(IRepositorioAtividadeAvaliativa repositorio, IMediator mediator)
                : base(repositorio, mediator)
            {
            }

            public async Task Executar(SalvarAtividadeAvaliativaGsaCommand command)
            {
                await base.Handle(command, CancellationToken.None);
            }
        }
    }
}