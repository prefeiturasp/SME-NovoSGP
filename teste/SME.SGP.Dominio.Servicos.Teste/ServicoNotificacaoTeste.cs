using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoNotificacaoTeste
    {
        private const string CODIGO_UE = "019691";

        private readonly Mock<IRepositorioSupervisorEscolaDre> _repositorioSupervisorEscolaDreMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ServicoNotificacao _servico;

        public ServicoNotificacaoTeste()
        {
            _repositorioSupervisorEscolaDreMock = new Mock<IRepositorioSupervisorEscolaDre>();
            _mediatorMock = new Mock<IMediator>();

            _servico = new ServicoNotificacao(
                Mock.Of<IRepositorioNotificacao>(),
                Mock.Of<IRepositorioNotificacaoConsulta>(),
                _repositorioSupervisorEscolaDreMock.Object,
                _mediatorMock.Object);
        }

        [Fact(DisplayName = "ServicoNotificacao - sem supervisor escolar na UE deve escalar para o Supervisor Técnico")]
        public async Task ObterFuncionariosPorNivelAsync_SemSupervisorEscolar_DeveEscalarParaSupervisorTecnico()
        {
            _repositorioSupervisorEscolaDreMock
                .Setup(r => r.ObtemSupervisoresPorUeAsync(CODIGO_UE))
                .ReturnsAsync(Enumerable.Empty<SupervisorEscolasDreDto>());

            _mediatorMock
                .Setup(m => m.Send(It.Is<ObterFuncionariosPorCargoUeQuery>(q => q.UeId == CODIGO_UE && q.CargoId == (long)Cargo.SupervisorTecnico),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<UsuarioEolRetornoDto> { new() { CodigoRf = "1234567", EstaAfastado = false } });

            var retorno = (await _servico.ObterFuncionariosPorNivelAsync(CODIGO_UE, Cargo.Supervisor, true, true)).ToList();

            retorno.Should().ContainSingle();
            retorno.Single().Cargo.Should().Be(Cargo.SupervisorTecnico);
            retorno.Single().Id.Should().Be("1234567");
        }

        [Fact(DisplayName = "ServicoNotificacao - com supervisor escolar na UE deve notificar o supervisor sem escalar")]
        public async Task ObterFuncionariosPorNivelAsync_ComSupervisorEscolar_DeveRetornarSupervisorSemEscalar()
        {
            _repositorioSupervisorEscolaDreMock
                .Setup(r => r.ObtemSupervisoresPorUeAsync(CODIGO_UE))
                .ReturnsAsync(new List<SupervisorEscolasDreDto>
                {
                    new() { SupervisorId = "7654321", TipoAtribuicao = (int)TipoResponsavelAtribuicao.SupervisorEscolar }
                });

            var retorno = (await _servico.ObterFuncionariosPorNivelAsync(CODIGO_UE, Cargo.Supervisor, true, true)).ToList();

            retorno.Should().ContainSingle();
            retorno.Single().Cargo.Should().Be(Cargo.Supervisor);
            retorno.Single().Id.Should().Be("7654321");
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFuncionariosPorCargoUeQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
