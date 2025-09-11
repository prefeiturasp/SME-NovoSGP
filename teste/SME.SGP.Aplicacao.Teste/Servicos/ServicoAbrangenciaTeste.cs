using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Servicos
{
    public class ServicoAbrangenciaTeste
    {
        private readonly Mock<IRepositorioAbrangencia> _repositorioAbrangenciaMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IConsultasSupervisor> _consultasSupervisorMock;
        private readonly Mock<IRepositorioDre> _repositorioDreMock;
        private readonly Mock<IRepositorioUe> _repositorioUeMock;
        private readonly Mock<IRepositorioTurma> _repositorioTurmaMock;
        private readonly Mock<IRepositorioCicloEnsino> _repositorioCicloEnsinoMock;
        private readonly Mock<IRepositorioTipoEscola> _repositorioTipoEscolaMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioUsuarioConsulta> _repositorioUsuarioConsultaMock;
        private readonly ServicoAbrangencia _servico;

        public ServicoAbrangenciaTeste()
        {
            _repositorioAbrangenciaMock = new Mock<IRepositorioAbrangencia>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _consultasSupervisorMock = new Mock<IConsultasSupervisor>();
            _repositorioDreMock = new Mock<IRepositorioDre>();
            _repositorioUeMock = new Mock<IRepositorioUe>();
            _repositorioTurmaMock = new Mock<IRepositorioTurma>();
            _repositorioCicloEnsinoMock = new Mock<IRepositorioCicloEnsino>();
            _repositorioTipoEscolaMock = new Mock<IRepositorioTipoEscola>();
            _mediatorMock = new Mock<IMediator>();
            _repositorioUsuarioConsultaMock = new Mock<IRepositorioUsuarioConsulta>();

            _servico = new ServicoAbrangencia(
                _repositorioAbrangenciaMock.Object,
                _unitOfWorkMock.Object,
                _consultasSupervisorMock.Object,
                _repositorioDreMock.Object,
                _repositorioUeMock.Object,
                _repositorioTurmaMock.Object,
                _repositorioCicloEnsinoMock.Object,
                _repositorioTipoEscolaMock.Object,
                _mediatorMock.Object,
                _repositorioUsuarioConsultaMock.Object
            );
        }

        [Fact]
        public async Task DreEstaNaAbrangencia_DeveRetornarTrue_QuandoDreEstiverNaLista()
        {
            string login = "usuario";
            Guid perfilId = Guid.NewGuid();
            string codigoDre = "123";
            var abrangencias = new List<AbrangenciaDreRetornoDto> { new AbrangenciaDreRetornoDto { Codigo = codigoDre } };

            _repositorioAbrangenciaMock
                .Setup(r => r.ObterDres(login, perfilId, null, 0, false, 0, "", false))
                .ReturnsAsync(abrangencias);

            var resultado = await _servico.DreEstaNaAbrangencia(login, perfilId, codigoDre);

            Assert.True(resultado);
        }

        [Fact]
        public async Task DreEstaNaAbrangencia_DeveRetornarFalse_QuandoDreNaoEstiverNaLista()
        {
            string login = "usuario";
            Guid perfilId = Guid.NewGuid();
            string codigoDre = "999";
            var abrangencias = new List<AbrangenciaDreRetornoDto> { new AbrangenciaDreRetornoDto { Codigo = "123" } };

            _repositorioAbrangenciaMock
                .Setup(r => r.ObterDres(login, perfilId, null, 0, false, 0, "", false))
                .ReturnsAsync(abrangencias);

            var resultado = await _servico.DreEstaNaAbrangencia(login, perfilId, codigoDre);

            Assert.False(resultado);
        }
    }
}

