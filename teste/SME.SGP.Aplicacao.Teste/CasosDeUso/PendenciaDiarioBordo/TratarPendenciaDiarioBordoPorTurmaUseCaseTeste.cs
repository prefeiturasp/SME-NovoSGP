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
            var turmaId = _faker.Random.Guid().ToString();
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(turmaId));

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterProfessoresTitularesDisciplinasEolQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<ProfessorTitularDisciplinaEol>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<PendenciaDiarioBordoParaExcluirDto>());

            var resultado = await _useCase.Executar(mensagem);

            resultado.Should().BeTrue();

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterPendenciasDiarioBordoQuery>(), It.IsAny<CancellationToken>()), Times.Never);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve processar a exclusão de pendências mesmo quando não há novas para gerar")]
        public async Task Executar_DeveProcessarExclusaoDePendencias()
        {
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

            await _useCase.Executar(mensagem);

            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgpAula.RotaExcluirPendenciasDiarioBordo), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Turma_Nao_Encontrada_Deve_Buscar_Pendencia_Para_Excluir()
        {
            var turmaId = _faker.Random.Guid().ToString();
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(turmaId));

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasDreUePorCodigosQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<Turma>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterProfessoresTitularesDisciplinasEolQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<ProfessorTitularDisciplinaEol>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<PendenciaDiarioBordoParaExcluirDto>());

            var resultado = await _useCase.Executar(mensagem);

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Professor_Rf_Nulo_Deve_Processar_Sem_Erro()
        {
            var turmaId = _faker.Random.Guid().ToString();
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(turmaId));
            var turma = CriarTurmaFake();

            var professoresTitulares = new List<ProfessorTitularDisciplinaEol>
            {
                new ProfessorTitularDisciplinaEol { ProfessorRf = null, CodigosDisciplinas = "1,2" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasDreUePorCodigosQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<Turma> { turma });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterProfessoresTitularesDisciplinasEolQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(professoresTitulares);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(CriarComponentesCurricularesFake());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<PendenciaDiarioBordoParaExcluirDto>());

            var resultado = await _useCase.Executar(mensagem);

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterPendenciasDiarioBordoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Professor_Rf_Vazio_Deve_Processar_Sem_Erro()
        {
            var turmaId = _faker.Random.Guid().ToString();
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(turmaId));
            var turma = CriarTurmaFake();

            var professoresTitulares = new List<ProfessorTitularDisciplinaEol>
            {
                new ProfessorTitularDisciplinaEol { ProfessorRf = "", CodigosDisciplinas = "1,2" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasDreUePorCodigosQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<Turma> { turma });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterProfessoresTitularesDisciplinasEolQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(professoresTitulares);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(CriarComponentesCurricularesFake());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<PendenciaDiarioBordoParaExcluirDto>());

            var resultado = await _useCase.Executar(mensagem);

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterPendenciasDiarioBordoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Professor_Rf_Com_Multiplos_Valores_Deve_Processar_Todos()
        {
            var turmaId = _faker.Random.Guid().ToString();
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(turmaId));
            var turma = CriarTurmaFake();

            var professoresTitulares = new List<ProfessorTitularDisciplinaEol>
            {
                new ProfessorTitularDisciplinaEol { ProfessorRf = "RF001,RF002,RF003", CodigosDisciplinas = "1,2" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasDreUePorCodigosQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<Turma> { turma });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterProfessoresTitularesDisciplinasEolQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(professoresTitulares);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(CriarComponentesCurricularesFake());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciasDiarioBordoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<AulaComComponenteDto>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<PendenciaDiarioBordoParaExcluirDto>());

            var resultado = await _useCase.Executar(mensagem);

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Executar_Quando_Professor_Rf_Com_Espacos_Em_Branco_Deve_Ignorar_Vazios()
        {
            var turmaId = _faker.Random.Guid().ToString();
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(turmaId));
            var turma = CriarTurmaFake();

            var professoresTitulares = new List<ProfessorTitularDisciplinaEol>
            {
                new ProfessorTitularDisciplinaEol { ProfessorRf = "RF001, , RF003", CodigosDisciplinas = "1,2" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasDreUePorCodigosQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<Turma> { turma });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterProfessoresTitularesDisciplinasEolQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(professoresTitulares);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(CriarComponentesCurricularesFake());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciasDiarioBordoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<AulaComComponenteDto>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<PendenciaDiarioBordoParaExcluirDto>());

            var resultado = await _useCase.Executar(mensagem);

            resultado.Should().BeTrue();
        }

        #endregion

        #region Cenários de Teste do Método BuscaPendenciaESalva

        [Fact(DisplayName = "Deve gerar pendências para todos os componentes quando a aula não possui registro")]
        public async Task BuscaPendenciaESalva_QuandoAulaNaoTemComponente_DeveGerarPendenciaParaTodos()
        {
            var turma = CriarTurmaFake();
            var professoresEComponentes = CriarProfessoresEComponentesFake(); 

            var aulasPendentes = new List<AulaComComponenteDto>
            {
                new AulaComComponenteDto { Id = 999, ComponenteId = 0 } 
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciasDiarioBordoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aulasPendentes);

            FiltroPendenciaDiarioBordoTurmaAulaDto filtroCapturado = null;
            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, _) => filtroCapturado = (cmd as PublicarFilaSgpCommand).Filtros as FiltroPendenciaDiarioBordoTurmaAulaDto);

            await _useCase.BuscaPendenciaESalva(turma, professoresEComponentes);

            filtroCapturado.Should().NotBeNull();
            filtroCapturado.AulasProfessoresComponentesCurriculares.Should().HaveCount(2); // Deve gerar para os 2 componentes do professor
            filtroCapturado.AulasProfessoresComponentesCurriculares.Should().Contain(c => c.ComponenteCurricularId == 1 && c.AulaId == 999);
            filtroCapturado.AulasProfessoresComponentesCurriculares.Should().Contain(c => c.ComponenteCurricularId == 2 && c.AulaId == 999);
        }

        [Fact(DisplayName = "Deve gerar pendências apenas para componentes faltantes em aulas já registradas")]
        public async Task BuscaPendenciaESalva_QuandoAulaTemComponente_DeveGerarPendenciaParaComponentesFaltantes()
        {
            var turma = CriarTurmaFake();
            var professoresEComponentes = CriarProfessoresEComponentesFake(); 

            var aulasPendentes = new List<AulaComComponenteDto>
            {
                new AulaComComponenteDto { Id = 999, ComponenteId = 1 } 
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciasDiarioBordoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aulasPendentes);

            FiltroPendenciaDiarioBordoTurmaAulaDto filtroCapturado = null;
            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, _) => filtroCapturado = (cmd as PublicarFilaSgpCommand).Filtros as FiltroPendenciaDiarioBordoTurmaAulaDto);

            await _useCase.BuscaPendenciaESalva(turma, professoresEComponentes);

            filtroCapturado.Should().NotBeNull();
            filtroCapturado.AulasProfessoresComponentesCurriculares.Should().HaveCount(1);
            filtroCapturado.AulasProfessoresComponentesCurriculares.First().ComponenteCurricularId.Should().Be(2); // Deve gerar pendência apenas para o componente 2
            filtroCapturado.AulasProfessoresComponentesCurriculares.First().AulaId.Should().Be(999);
        }

        [Fact(DisplayName = "Deve salvar log de erro quando uma exceção ocorrer")]
        public async Task BuscaPendenciaESalva_QuandoOcorrerException_DeveSalvarLog()
        {
            var turma = CriarTurmaFake();
            var professoresEComponentes = CriarProfessoresEComponentesFake();
            var excecao = new Exception("Erro ao buscar pendências");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciasDiarioBordoQuery>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(excecao);

            await _useCase.BuscaPendenciaESalva(turma, professoresEComponentes);

            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(log =>
                log.Nivel == LogNivel.Critico &&
                log.Observacao == excecao.Message), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task BuscaPendenciaESalva_Quando_Aulas_Vazias_Deve_Publicar_Filtro_Sem_Aulas()
        {
            var turma = CriarTurmaFake();
            var professoresEComponentes = CriarProfessoresEComponentesFake();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciasDiarioBordoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<AulaComComponenteDto>());

            FiltroPendenciaDiarioBordoTurmaAulaDto filtroCapturado = null;
            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, _) => filtroCapturado = (cmd as PublicarFilaSgpCommand).Filtros as FiltroPendenciaDiarioBordoTurmaAulaDto);

            await _useCase.BuscaPendenciaESalva(turma, professoresEComponentes);

            filtroCapturado.Should().NotBeNull();
            filtroCapturado.AulasProfessoresComponentesCurriculares.Should().BeEmpty();
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task BuscaPendenciaESalva_Quando_Multiplas_Aulas_Com_Mesmo_Id_Deve_Processar_Componentes_Faltantes_Corretamente()
        {
            var turma = CriarTurmaFake();
            var professoresEComponentes = new List<ProfessorEComponenteInfantilDto>
            {
                new ProfessorEComponenteInfantilDto { CodigoRf = "RF1", DisciplinaId = 1, DescricaoComponenteCurricular = "Comp1" },
                new ProfessorEComponenteInfantilDto { CodigoRf = "RF1", DisciplinaId = 2, DescricaoComponenteCurricular = "Comp2" },
                new ProfessorEComponenteInfantilDto { CodigoRf = "RF1", DisciplinaId = 3, DescricaoComponenteCurricular = "Comp3" }
            };

            var aulasPendentes = new List<AulaComComponenteDto>
            {
                new AulaComComponenteDto { Id = 999, ComponenteId = 1 },
                new AulaComComponenteDto { Id = 999, ComponenteId = 2 }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciasDiarioBordoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aulasPendentes);

            FiltroPendenciaDiarioBordoTurmaAulaDto filtroCapturado = null;
            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, _) => filtroCapturado = (cmd as PublicarFilaSgpCommand).Filtros as FiltroPendenciaDiarioBordoTurmaAulaDto);

            await _useCase.BuscaPendenciaESalva(turma, professoresEComponentes);

            filtroCapturado.Should().NotBeNull();
            filtroCapturado.AulasProfessoresComponentesCurriculares.Should().HaveCount(2);
            filtroCapturado.AulasProfessoresComponentesCurriculares.Should().OnlyContain(c => c.ComponenteCurricularId == 3 && c.AulaId == 999);
        }

        [Fact]
        public async Task BuscaPendenciaESalva_Quando_Nao_Ha_Professores_Para_Gerar_Pendencia_Deve_Pular_Aula()
        {
            var turma = CriarTurmaFake();
            var professoresEComponentes = new List<ProfessorEComponenteInfantilDto>
            {
                new ProfessorEComponenteInfantilDto { CodigoRf = "RF1", DisciplinaId = 1, DescricaoComponenteCurricular = "Comp1" }
            };

            var aulasPendentes = new List<AulaComComponenteDto>
            {
                new AulaComComponenteDto { Id = 999, ComponenteId = 1 }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciasDiarioBordoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aulasPendentes);

            FiltroPendenciaDiarioBordoTurmaAulaDto filtroCapturado = null;
            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, _) => filtroCapturado = (cmd as PublicarFilaSgpCommand).Filtros as FiltroPendenciaDiarioBordoTurmaAulaDto);

            await _useCase.BuscaPendenciaESalva(turma, professoresEComponentes);

            filtroCapturado.Should().NotBeNull();
            filtroCapturado.AulasProfessoresComponentesCurriculares.Should().BeEmpty();
        }

        #endregion

        #region Métodos de Apoio

        private Turma CriarTurmaFake()
        {
            var ue = new Ue { Nome = "ESCOLA TESTE", TipoEscola = Dominio.TipoEscola.EMEF, Dre = new SME.SGP.Dominio.Dre { Abreviacao = "DRE-TE" } };
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

        private List<ComponenteCurricularDto> CriarComponentesCurricularesFake()
        {
            return new List<ComponenteCurricularDto>
            {
                new ComponenteCurricularDto { Codigo = "1", Descricao = "Comp1" },
                new ComponenteCurricularDto { Codigo = "2", Descricao = "Comp2" }
            };
        }

        #endregion
    }
}