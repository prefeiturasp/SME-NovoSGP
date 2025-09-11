﻿using Bogus;
using FluentAssertions;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PendenciaDiarioBordo
{
    public class ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommandHandlerTeste
    {
        private readonly Mock<IRepositorioPendenciaDiarioBordoConsulta> _repositorioMock;
        private readonly ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommandHandler _commandHandler;
        private readonly Faker _faker;

        public ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommandHandlerTeste()
        {
            _repositorioMock = new Mock<IRepositorioPendenciaDiarioBordoConsulta>();
            _commandHandler = new ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommandHandler(_repositorioMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve chamar o repositório e retornar as pendências encontradas")]
        public async Task Handle_DeveChamarRepositorio_E_RetornarAsPendencias()
        {
            // Organização
            var turmaId = _faker.Random.AlphaNumeric(10);
            var comando = new ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommand(turmaId);

            var pendenciasEsperadas = new List<PendenciaDiarioBordoParaExcluirDto>
            {
                new PendenciaDiarioBordoParaExcluirDto { AulaId = _faker.Random.Long(1, 1000), ComponenteCurricularId = _faker.Random.Long(1, 100) }
            };

            _repositorioMock.Setup(r => r.ListarPendenciaDiarioBordoParaExcluirPorIdTurma(turmaId))
                            .ReturnsAsync(pendenciasEsperadas);

            // Ação
            var resultado = await _commandHandler.Handle(comando, default);

            // Verificação
            resultado.Should().BeEquivalentTo(pendenciasEsperadas);
            _repositorioMock.Verify(r => r.ListarPendenciaDiarioBordoParaExcluirPorIdTurma(turmaId), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar uma lista vazia quando o repositório não encontrar pendências")]
        public async Task Handle_QuandoNaoHouverPendencias_DeveRetornarListaVazia()
        {
            // Organização
            var turmaId = _faker.Random.AlphaNumeric(10);
            var comando = new ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommand(turmaId);

            var listaVazia = new List<PendenciaDiarioBordoParaExcluirDto>();

            _repositorioMock.Setup(r => r.ListarPendenciaDiarioBordoParaExcluirPorIdTurma(turmaId))
                            .ReturnsAsync(listaVazia);

            // Ação
            var resultado = await _commandHandler.Handle(comando, default);

            // Verificação
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
            _repositorioMock.Verify(r => r.ListarPendenciaDiarioBordoParaExcluirPorIdTurma(turmaId), Times.Once);
        }
    }
}
