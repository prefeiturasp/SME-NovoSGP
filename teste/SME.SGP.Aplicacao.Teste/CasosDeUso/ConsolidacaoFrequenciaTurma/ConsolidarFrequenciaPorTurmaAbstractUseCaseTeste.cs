using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoFrequenciaTurma
{
    public class ConsolidarFrequenciaPorTurmaUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Consolidar_Frequencia_Quando_Turma_Valida()
        {
            var mediatorMock = new Mock<IMediator>();
            var turmaId = 123;
            var codigoTurma = "TURMA123";

            var turma = new Turma
            {
                Id = turmaId,
                CodigoTurma = codigoTurma,
                AnoLetivo = 2025,
                ModalidadeCodigo = Modalidade.Fundamental
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turma);

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new ParametrosSistema { Valor = "75" });

            var frequencias = new List<FrequenciaAlunoDto>
            {
                new FrequenciaAlunoDto { AlunoCodigo = "123", TotalAulas = 100, TotalAusencias = 10, TotalCompensacoes = 0, TotalPresencas = 90 },
                new FrequenciaAlunoDto { AlunoCodigo = "456", TotalAulas = 100, TotalAusencias = 40, TotalCompensacoes = 0, TotalPresencas = 60 }
            };

            var useCase = new ConsolidarFrequenciaPorTurmaFakeUseCase(mediatorMock.Object, frequencias, TipoConsolidadoFrequencia.Anual);

            var filtro = new FiltroConsolidacaoFrequenciaTurma(turmaId, codigoTurma, 0, DateTime.Today);
            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonSerializer.Serialize(filtro)
            };

            var resultado = await useCase.Executar(mensagem);

            resultado.Should().BeTrue();

            mediatorMock.Invocations.Should().ContainSingle(i =>
            i.Method.Name == nameof(IMediator.Send) &&
            i.Arguments[0] is RegistraConsolidacaoFrequenciaTurmaCommand
             ).Which.Arguments[0].Should().BeOfType<RegistraConsolidacaoFrequenciaTurmaCommand>().Subject
               .Should().Match<RegistraConsolidacaoFrequenciaTurmaCommand>(cmd =>
                 cmd.TurmaId == turmaId &&
                 cmd.QuantidadeAcimaMinimoFrequencia == 1 &&
                 cmd.QuantidadeAbaixoMinimoFrequencia == 1
             );

            mediatorMock.Verify(x => x.Send(It.Is<RegistraConsolidacaoFrequenciaTurmaCommand>(cmd =>
                cmd.TurmaId == turmaId &&
                cmd.QuantidadeAcimaMinimoFrequencia == 1 &&
                cmd.QuantidadeAbaixoMinimoFrequencia == 1 &&
                cmd.TotalAulas == 100 &&
                (cmd.TotalFrequencias == 100 || cmd.TotalFrequencias == 95)
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    public class ConsolidarFrequenciaPorTurmaFakeUseCase : ConsolidarFrequenciaPorTurmaAbstractUseCase
    {
        private readonly IEnumerable<FrequenciaAlunoDto> frequencias;
        private readonly TipoConsolidadoFrequencia tipo;

        public ConsolidarFrequenciaPorTurmaFakeUseCase(IMediator mediator,
            IEnumerable<FrequenciaAlunoDto> frequencias,
            TipoConsolidadoFrequencia tipo) : base(mediator)
        {
            this.frequencias = frequencias;
            this.tipo = tipo;
        }

        protected override (DateTime? DataInicio, DateTime? DataFim) Periodos => (new DateTime(2025, 1, 1), new DateTime(2025, 12, 31));

        protected override TipoConsolidadoFrequencia TipoConsolidado => tipo;

        protected override Task<IEnumerable<FrequenciaAlunoDto>> ObterFrequenciaConsideradas(string codigoTurma)
        {
            return Task.FromResult(frequencias);
        }
    }
}
