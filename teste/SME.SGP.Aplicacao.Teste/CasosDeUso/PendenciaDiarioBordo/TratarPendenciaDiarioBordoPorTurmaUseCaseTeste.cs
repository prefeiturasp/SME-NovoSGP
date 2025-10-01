using Bogus;
using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PendenciaDiarioBordo
{
    public class TratarPendenciaDiarioBordoPorTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TratarPendenciaDiarioBordoPorTurmaUseCase _useCase;
        private readonly Faker _faker;

        public TratarPendenciaDiarioBordoPorTurmaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new TratarPendenciaDiarioBordoPorTurmaUseCase(_mediatorMock.Object);
            _faker = new Faker("pt_BR");
        }

        #region Cenários de Teste do Método Executar

        [Fact(DisplayName = "Não deve gerar novas pendências quando a turma não possuir professores")]
        public async Task Executar_QuandoNaoEncontrarProfessores_NaoDeveProcessarNovasPendencias()
        {
            // Organização
            var turmaId = _faker.Random.Guid().ToString();
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(turmaId));

            // Configura a busca de professores para retornar uma lista vazia
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterProfessoresTitularesDisciplinasEolQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<ProfessorTitularDisciplinaEol>());

            // Garante que a busca de pendências para exclusão não retorne nada.
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<PendenciaDiarioBordoParaExcluirDto>());

            // Ação
            var resultado = await _useCase.Executar(mensagem);

            // Verificação
            resultado.Should().BeTrue();

            // Verifica que a query de pendências para salvar não foi chamada
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterPendenciasDiarioBordoQuery>(), It.IsAny<CancellationToken>()), Times.Never);

            // Verifica que a busca para exclusão foi chamada
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve processar a exclusão de pendências mesmo quando não há novas para gerar")]
        public async Task Executar_DeveProcessarExclusaoDePendencias()
        {
            // Organização
            var turmaId = _faker.Random.Guid().ToString();
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(turmaId));

            var pendenciasParaExcluir = new List<PendenciaDiarioBordoParaExcluirDto>
            {
                new PendenciaDiarioBordoParaExcluirDto { AulaId = 1, ComponenteCurricularId = 10 }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterProfessoresTitularesDisciplinasEolQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<ProfessorTitularDisciplinaEol>());

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommand>(c => c.TurmaId == turmaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(pendenciasParaExcluir);

            // Ação
            await _useCase.Executar(mensagem);

            // Verificação
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgpAula.RotaExcluirPendenciasDiarioBordo), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Cenários de Teste do Método BuscaPendenciaESalva

        [Fact(DisplayName = "Deve gerar pendências para todos os componentes quando a aula não possui registro")]
        public async Task BuscaPendenciaESalva_QuandoAulaNaoTemComponente_DeveGerarPendenciaParaTodos()
        {
            // Organização
            var turma = CriarTurmaFake();
            var professoresEComponentes = CriarProfessoresEComponentesFake(); // RF1 com Comp1 e Comp2

            var aulasPendentes = new List<AulaComComponenteDto>
            {
                new AulaComComponenteDto { Id = 999, ComponenteId = 0 } // Aula sem componente
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciasDiarioBordoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aulasPendentes);

            FiltroPendenciaDiarioBordoTurmaAulaDto filtroCapturado = null;
            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, _) => filtroCapturado = (cmd as PublicarFilaSgpCommand).Filtros as FiltroPendenciaDiarioBordoTurmaAulaDto);

            // Ação
            await _useCase.BuscaPendenciaESalva(turma, professoresEComponentes);

            // Verificação
            filtroCapturado.Should().NotBeNull();
            filtroCapturado.AulasProfessoresComponentesCurriculares.Should().HaveCount(2); // Deve gerar para os 2 componentes do professor
            filtroCapturado.AulasProfessoresComponentesCurriculares.Should().Contain(c => c.ComponenteCurricularId == 1 && c.AulaId == 999);
            filtroCapturado.AulasProfessoresComponentesCurriculares.Should().Contain(c => c.ComponenteCurricularId == 2 && c.AulaId == 999);
        }

        [Fact(DisplayName = "Deve gerar pendências apenas para componentes faltantes em aulas já registradas")]
        public async Task BuscaPendenciaESalva_QuandoAulaTemComponente_DeveGerarPendenciaParaComponentesFaltantes()
        {
            // Organização
            var turma = CriarTurmaFake();
            var professoresEComponentes = CriarProfessoresEComponentesFake(); // RF1 com Comp1 e Comp2

            var aulasPendentes = new List<AulaComComponenteDto>
            {
                new AulaComComponenteDto { Id = 999, ComponenteId = 1 } // Aula já possui registro para o componente 1
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciasDiarioBordoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aulasPendentes);

            FiltroPendenciaDiarioBordoTurmaAulaDto filtroCapturado = null;
            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, _) => filtroCapturado = (cmd as PublicarFilaSgpCommand).Filtros as FiltroPendenciaDiarioBordoTurmaAulaDto);

            // Ação
            await _useCase.BuscaPendenciaESalva(turma, professoresEComponentes);

            // Verificação
            filtroCapturado.Should().NotBeNull();
            filtroCapturado.AulasProfessoresComponentesCurriculares.Should().HaveCount(1);
            filtroCapturado.AulasProfessoresComponentesCurriculares.First().ComponenteCurricularId.Should().Be(2); // Deve gerar pendência apenas para o componente 2
            filtroCapturado.AulasProfessoresComponentesCurriculares.First().AulaId.Should().Be(999);
        }

        [Fact(DisplayName = "Deve salvar log de erro quando uma exceção ocorrer")]
        public async Task BuscaPendenciaESalva_QuandoOcorrerException_DeveSalvarLog()
        {
            // Organização
            var turma = CriarTurmaFake();
            var professoresEComponentes = CriarProfessoresEComponentesFake();
            var excecao = new Exception("Erro ao buscar pendências");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciasDiarioBordoQuery>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(excecao);

            // Ação
            await _useCase.BuscaPendenciaESalva(turma, professoresEComponentes);

            // Verificação
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(log =>
                log.Nivel == LogNivel.Critico &&
                log.Observacao == excecao.Message), It.IsAny<CancellationToken>()), Times.Once);

            // Garante que não tentou publicar na fila em caso de erro
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region Métodos de Apoio

        private Turma CriarTurmaFake()
        {
            var ue = new Ue { Nome = "ESCOLA TESTE", TipoEscola = TipoEscola.EMEF, Dre = new SME.SGP.Dominio.Dre { Abreviacao = "DRE-TE" } };
            var turma = new Turma { CodigoTurma = "123", Nome = "1A", ModalidadeCodigo = Modalidade.Fundamental };
            turma.AdicionarUe(ue);
            return turma;
        }

        private List<ProfessorEComponenteInfantilDto> CriarProfessoresEComponentesFake()
        {
            return new List<ProfessorEComponenteInfantilDto>
            {
                new ProfessorEComponenteInfantilDto { CodigoRf = "RF1", DisciplinaId = 1, DescricaoComponenteCurricular = "Comp1" },
                new ProfessorEComponenteInfantilDto { CodigoRf = "RF1", DisciplinaId = 2, DescricaoComponenteCurricular = "Comp2" }
            };
        }

        #endregion
    }
}