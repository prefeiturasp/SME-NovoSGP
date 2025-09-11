using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Comunicados.SolicitarExclusaoComunicados
{
    public class SolicitarExclusaoComunicadosEscolaAquiUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ISolicitarExclusaoComunicadosEscolaAquiUseCase useCase;

        public SolicitarExclusaoComunicadosEscolaAquiUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new SolicitarExclusaoComunicadosEscolaAquiUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Executar_Com_Sucesso_Quando_Ids_Forem_Validos()
        {
            var ids = new long[] { 1, 2, 3 };
            mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirComunicadoCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(ids);

            resultado.Should().BeTrue();
            mediatorMock.Verify(m => m.Send(It.Is<ExcluirComunicadoCommand>(cmd => cmd.Ids == ids), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Ids_Estiverem_Vazios()
        {
            var ids = Array.Empty<long>();

            var validator = new ExcluirComunicadoCommandValidator();
            var resultadoValidacao = validator.Validate(new ExcluirComunicadoCommand(ids));

            resultadoValidacao.IsValid.Should().BeFalse();
            resultadoValidacao.Errors.Should().ContainSingle(e => e.ErrorMessage == "Pelo menos um comunicado deve ser informado.");
        }
    }
}
