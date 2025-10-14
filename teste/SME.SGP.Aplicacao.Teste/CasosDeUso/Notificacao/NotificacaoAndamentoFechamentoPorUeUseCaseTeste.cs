using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacao
{
    public class NotificacaoAndamentoFechamentoPorUeUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly NotificacaoAndamentoFechamentoPorUeUseCase useCase;

        public NotificacaoAndamentoFechamentoPorUeUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new NotificacaoAndamentoFechamentoPorUeUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Executar_Teste()
        {
            var dto = new NotificacaoAndamentoFechamentoPorUeDto
                 {
                     PeriodoEscolarId = 1,
                     UeId = 2,
                     TurmasIds = new long[] { 1, 2 },
                     Componentes = new[]
                         {
                            new ComponenteCurricularDto { Codigo = "101", Descricao = "Matemática" },
                            new ComponenteCurricularDto { Codigo = "102", Descricao = "Português" }
                         }
                 };

            var periodoEscolar = new PeriodoEscolar { Id = 1, Bimestre = 1 };
            var ue = new Ue { Id = 2, Nome = "Escola Teste", CodigoUe = "UE1", DreId = 10, TipoEscola = TipoEscola.EMEF };
            var dre = new SME.SGP.Dominio.Dre { Id = 10, CodigoDre = "DRE1", Abreviacao = "DR1" };
            var turma1 = new Turma { Id = 1, Nome = "1A", ModalidadeCodigo = Modalidade.Fundamental, CodigoTurma = "T1" };
            var turma2 = new Turma { Id = 2, Nome = "2A", ModalidadeCodigo = Modalidade.Fundamental, CodigoTurma = "T2" };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodoEscolarePorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodoEscolar);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUePorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ue);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasPorIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Turma> { turma1, turma2 });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDREPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dre);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dto.Componentes);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterSituacaoFechamentoTurmaComponenteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(SituacaoFechamento.EmProcessamento);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterSituacaoConselhoClasseQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(SituacaoConselhoClasse.Concluido);

            mediatorMock.Setup(m => m.Send(It.IsAny<EnviarNotificacaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var mensagemRabbit = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(dto));

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<EnviarNotificacaoCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
        }

        [Fact]
        public async Task ObterHeaderModalidade_Quando_Chamado_Deve_Retornar_String_Teste()
        {
            var metodo = typeof(NotificacaoAndamentoFechamentoPorUeUseCase)
                .GetMethod("ObterHeaderModalidade", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var resultado = metodo.Invoke(useCase, new object[] { "Fundamental" }) as string;
            Assert.Contains("Fundamental", resultado);
        }

        [Fact]
        public async Task MontarLinhaDaTurma_Quando_Turma_Tem_Componentes_Deve_Retornar_Linha_Teste()
        {
            var turma = new Turma { Id = 1, Nome = "1A", ModalidadeCodigo = Modalidade.Fundamental, CodigoTurma = "T1" };
            var ue = new Ue { CodigoUe = "UE1" };
            var periodoEscolar = new PeriodoEscolar { Id = 1, Bimestre = 1 };
            var componentes = new List<ComponenteCurricularDto>
        {
            new ComponenteCurricularDto { Codigo = "101", Descricao = "Matemática" },
            new ComponenteCurricularDto { Codigo = "102", Descricao = "Português" }
        };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componentes);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterSituacaoFechamentoTurmaComponenteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(SituacaoFechamento.ProcessadoComSucesso);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterSituacaoConselhoClasseQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(SituacaoConselhoClasse.EmAndamento);

            var metodo = typeof(NotificacaoAndamentoFechamentoPorUeUseCase)
                .GetMethod("MontarLinhaDaTurma", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var resultado = await (Task<string>)metodo.Invoke(useCase, new object[] { turma, componentes, ue, periodoEscolar });
            Assert.Contains("Matemática", resultado);
            Assert.Contains("Português", resultado);
        }

        [Fact]
        public async Task EnviarNotificacao_Quando_Chamado_Deve_Enviar_Comando_Teste()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<EnviarNotificacaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var metodo = typeof(NotificacaoAndamentoFechamentoPorUeUseCase)
                .GetMethod("EnviarNotificacao", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            await (Task)metodo.Invoke(useCase, new object[] { "Título", "Mensagem", "DRE1", "UE1" });

            mediatorMock.Verify(m => m.Send(It.IsAny<EnviarNotificacaoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ObterSituacaoFechamento_Quando_Chamado_Deve_Retornar_Situacao_Teste()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterSituacaoFechamentoTurmaComponenteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(SituacaoFechamento.ProcessadoComSucesso);

            var turma = new Turma { Id = 1 };
            var componente = new ComponenteCurricularDto { Codigo = "101" };
            var periodoEscolar = new PeriodoEscolar { Id = 1 };

            var metodo = typeof(NotificacaoAndamentoFechamentoPorUeUseCase)
                .GetMethod("ObterSituacaoFechamento", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var resultado = await (Task<string>)metodo.Invoke(useCase, new object[] { turma, componente, periodoEscolar });
            Assert.Equal(SituacaoFechamento.ProcessadoComSucesso.Name(), resultado);
        }

        [Fact]
        public async Task ObterSituacaoConselhoClasse_Quando_Chamado_Deve_Retornar_Situacao_Teste()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterSituacaoConselhoClasseQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(SituacaoConselhoClasse.EmAndamento);

            var turma = new Turma { Id = 1 };
            var periodoEscolar = new PeriodoEscolar { Id = 1 };

            var metodo = typeof(NotificacaoAndamentoFechamentoPorUeUseCase)
                .GetMethod("ObterSituacaoConselhoClasse", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var resultado = await (Task<string>)metodo.Invoke(useCase, new object[] { turma, periodoEscolar });
            Assert.Equal(SituacaoConselhoClasse.EmAndamento.Name(), resultado);
        }

        [Fact]
        public async Task ObterComponentesDaTurma_Quando_Chamado_Deve_Retornar_Componentes_Teste()
        {
            var componentes = new List<ComponenteCurricularDto>
        {
            new ComponenteCurricularDto { Codigo = "101", Descricao = "Matemática" }
        };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componentes);

            var turma = new Turma { CodigoTurma = "T1" };
            var ue = new Ue { CodigoUe = "UE1" };

            var metodo = typeof(NotificacaoAndamentoFechamentoPorUeUseCase)
                .GetMethod("ObterComponentesDaTurma", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var resultado = await (Task<IEnumerable<ComponenteCurricularDto>>)metodo.Invoke(useCase, new object[] { turma, ue });
            Assert.Single(resultado);
        }
    }
}
