using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional.Frequencia;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ConsolidacaoFrequenciaDiaria;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ConsolidacaoFrequenciaDiaria.LimparConsolidacao;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaDiaria;
using SME.SGP.Aplicacao.Queries.UE.ObterTodasUes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Frequencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using MediatR;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao.Testes.CasosDeUso.PainelEducacional.Frequencia
{
    public class ConsolidarFrequenciaDiariaPainelEducacionalUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidarFrequenciaDiariaPainelEducacionalUseCase _useCase;

        public ConsolidarFrequenciaDiariaPainelEducacionalUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsolidarFrequenciaDiariaPainelEducacionalUseCase(_mediatorMock.Object);
        }

        private List<Dre> ObterDresFicticias()
        {
            return new List<Dre>
            {
                new Dre { Id = 1, Nome = "DRE Leste", CodigoDre = "10000" },
                new Dre { Id = 2, Nome = "DRE Oeste", CodigoDre = "20000" }
            };
        }

        private List<Ue> ObterUesFicticias()
        {
            return new List<Ue>
            {
                new Ue { Id = 101, CodigoUe = "UE001", Nome = "Escola A", DreId = 1, TipoEscola = TipoEscola.EMEF },
                new Ue { Id = 102, CodigoUe = "UE002", Nome = "Escola B", DreId = 1, TipoEscola = TipoEscola.EMEF },
                new Ue { Id = 201, CodigoUe = "UE003", Nome = "Escola C", DreId = 2, TipoEscola = TipoEscola.EMEF }
            };
        }

        private List<DadosParaConsolidarFrequenciaDiariaAlunoDto> ObterDadosFrequencia(long dreId)
        {
            if (dreId == 1)
            {
                return new List<DadosParaConsolidarFrequenciaDiariaAlunoDto>
                {
                    new DadosParaConsolidarFrequenciaDiariaAlunoDto { CodigoDre = "10000", UeId = 101, TurmaId = 1, NomeTurma = "T1", AnoLetivo = 2025, TotalPresentes = 20, TotalAusentes = 2, TotalRemotos = 3, DataAula = new DateTime(2025, 10, 1) },
                    new DadosParaConsolidarFrequenciaDiariaAlunoDto { CodigoDre = "10000", UeId = 101, TurmaId = 1, NomeTurma = "T1", AnoLetivo = 2025, TotalPresentes = 10, TotalAusentes = 1, TotalRemotos = 1, DataAula = new DateTime(2025, 10, 1) }, // Duplicata para teste de soma

                    new DadosParaConsolidarFrequenciaDiariaAlunoDto { CodigoDre = "10000", UeId = 101, TurmaId = 2, NomeTurma = "T2", AnoLetivo = 2025, TotalPresentes = 15, TotalAusentes = 5, TotalRemotos = 0, DataAula = new DateTime(2025, 10, 1) },

                    new DadosParaConsolidarFrequenciaDiariaAlunoDto { CodigoDre = "10000", UeId = 102, TurmaId = 3, NomeTurma = "T3", AnoLetivo = 2025, TotalPresentes = 25, TotalAusentes = 0, TotalRemotos = 5, DataAula = new DateTime(2025, 10, 1) },

                    new DadosParaConsolidarFrequenciaDiariaAlunoDto { CodigoDre = "10000", UeId = 101, TurmaId = 1, NomeTurma = "T1", AnoLetivo = 2025, TotalPresentes = 22, TotalAusentes = 0, TotalRemotos = 3, DataAula = new DateTime(2025, 10, 2) },
                };
            }
            if (dreId == 2)
            {
                return new List<DadosParaConsolidarFrequenciaDiariaAlunoDto>
                {
                    new DadosParaConsolidarFrequenciaDiariaAlunoDto { CodigoDre = "20000", UeId = 201, TurmaId = 4, NomeTurma = "T4", AnoLetivo = 2025, TotalPresentes = 30, TotalAusentes = 1, TotalRemotos = 0, DataAula = new DateTime(2025, 10, 1) },
                };
            }
            return new List<DadosParaConsolidarFrequenciaDiariaAlunoDto>();
        }

        [Fact]
        public async Task Executar_DeveProcessarTodasDresEsalvarConsolidacoes()
        {
            // Arrange
            var dres = ObterDresFicticias();
            var ues = ObterUesFicticias();
            var anoAtual = DateTime.Now.Year;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasDresQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasUesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ues);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterFrequenciaDiariaQuery>(q => q.DreId == 1 && q.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ObterDadosFrequencia(1));
            _mediatorMock.Setup(m => m.Send(It.Is<ObterFrequenciaDiariaQuery>(q => q.DreId == 2 && q.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ObterDadosFrequencia(2));
            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparPainelEducacionalConsolidacaoFrequenciaDiariaCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarPainelEducacionalConsolidacaoFrequenciaDiariaCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);


            // Act
            var resultado = await _useCase.Executar(new MensagemRabbit());

            // Assert
            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.IsAny<LimparPainelEducacionalConsolidacaoFrequenciaDiariaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<LimparPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterFrequenciaDiariaQuery>(q => q.DreId == 1 && q.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterFrequenciaDiariaQuery>(q => q.DreId == 2 && q.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<SalvarPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand>(c => c.Indicadores.Count() > 0), It.IsAny<CancellationToken>()), Times.Exactly(2));
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarPainelEducacionalConsolidacaoFrequenciaDiariaCommand>(c => c.Indicadores.Count() > 0), It.IsAny<CancellationToken>()), Times.Exactly(2));

            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgpPainelEducacional.ConsolidarFrequenciaSemanalPainelEducacional), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void ObterNivelFrequencia_DeveRetornarNivelCorreto()
        {
            // Act & Assert
            Assert.Equal(NivelFrequenciaEnum.Baixa, ConsolidarFrequenciaDiariaPainelEducacionalUseCase.ObterNivelFrequencia(84.9m));
            Assert.Equal(NivelFrequenciaEnum.Media, ConsolidarFrequenciaDiariaPainelEducacionalUseCase.ObterNivelFrequencia(85.0m));
            Assert.Equal(NivelFrequenciaEnum.Media, ConsolidarFrequenciaDiariaPainelEducacionalUseCase.ObterNivelFrequencia(89.9m));
            Assert.Equal(NivelFrequenciaEnum.Alta, ConsolidarFrequenciaDiariaPainelEducacionalUseCase.ObterNivelFrequencia(90.0m));
            Assert.Equal(NivelFrequenciaEnum.Alta, ConsolidarFrequenciaDiariaPainelEducacionalUseCase.ObterNivelFrequencia(100m));
            Assert.Equal(NivelFrequenciaEnum.Baixa, ConsolidarFrequenciaDiariaPainelEducacionalUseCase.ObterNivelFrequencia(0m));
        }

        [Fact]
        public async Task Executar_ConsolidacaoPorTurma_ValoresCorretos()
        {
            // Arrange
            var dres = ObterDresFicticias().Take(1).ToList(); // Apenas DRE 1
            var ues = ObterUesFicticias().Where(u => u.DreId == 1).ToList();
            var dadosDre1 = ObterDadosFrequencia(1);
            var anoAtual = DateTime.Now.Year;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasDresQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasUesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ues);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterFrequenciaDiariaQuery>(q => q.DreId == 1 && q.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosDre1);

            List<ConsolidacaoFrequenciaDiariaTurmaDto> indicadoresTurmasSalvos = null;
            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<bool>, CancellationToken>((cmd, token) =>
                {
                    indicadoresTurmasSalvos = ((SalvarPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand)cmd).Indicadores.ToList();
                })
                .ReturnsAsync(true);


            // Act
            await _useCase.Executar(new MensagemRabbit());

            Assert.NotNull(indicadoresTurmasSalvos);
            Assert.Equal(4, indicadoresTurmasSalvos.Count);

            var turma1Dia1 = indicadoresTurmasSalvos.First(x => x.Turma == "T1" && x.DataAula == new DateTime(2025, 10, 1));
            Assert.Equal("UE001", turma1Dia1.CodigoUe);
            Assert.Equal(37, turma1Dia1.TotalEstudantes);
            Assert.Equal(30, turma1Dia1.TotalPresentes);
            Assert.Equal(Math.Round(30m / 37m * 100m, 2), Math.Round(turma1Dia1.PercentualFrequencia, 2));
            Assert.Equal(NivelFrequenciaEnum.Baixa, turma1Dia1.NivelFrequencia);

            var turma2Dia1 = indicadoresTurmasSalvos.First(x => x.Turma == "T2" && x.DataAula == new DateTime(2025, 10, 1));
            Assert.Equal(20, turma2Dia1.TotalEstudantes);
            Assert.Equal(15, turma2Dia1.TotalPresentes);
            Assert.Equal(75.0m, turma2Dia1.PercentualFrequencia);
            Assert.Equal(NivelFrequenciaEnum.Baixa, turma2Dia1.NivelFrequencia);
        }

        [Fact]
        public async Task Executar_ConsolidacaoPorDre_ValoresCorretos()
        {
            // Arrange
            var dres = ObterDresFicticias().Take(1).ToList();
            var ues = ObterUesFicticias().Where(u => u.DreId == 1).ToList();
            var dadosDre1 = ObterDadosFrequencia(1);
            var anoAtual = DateTime.Now.Year;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasDresQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasUesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ues);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterFrequenciaDiariaQuery>(q => q.DreId == 1 && q.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosDre1);

            List<ConsolidacaoFrequenciaDiariaDreDto> indicadoresDreSalvos = null;
            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarPainelEducacionalConsolidacaoFrequenciaDiariaCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<bool>, CancellationToken>((cmd, token) =>
                {
                    indicadoresDreSalvos = ((SalvarPainelEducacionalConsolidacaoFrequenciaDiariaCommand)cmd).Indicadores.ToList();
                })
                .ReturnsAsync(true);

            // Act
            await _useCase.Executar(new MensagemRabbit());

            Assert.NotNull(indicadoresDreSalvos);
            Assert.Equal(3, indicadoresDreSalvos.Count);

            var ue1Dia1 = indicadoresDreSalvos.First(x => x.CodigoUe == "UE001" && x.DataAula == new DateTime(2025, 10, 1));
            Assert.Equal(NivelFrequenciaEnum.Baixa, ue1Dia1.NivelFrequencia);
            Assert.Equal(57, ue1Dia1.TotalEstudantes);
            Assert.Equal(45, ue1Dia1.TotalPresentes);
            Assert.Equal(Math.Round(45m / 57m * 100m, 2), Math.Round(ue1Dia1.PercentualFrequencia, 2));
            Assert.Equal(NivelFrequenciaEnum.Baixa, ue1Dia1.NivelFrequencia);

            var ue2Dia1 = indicadoresDreSalvos.First(x => x.CodigoUe == "UE002" && x.DataAula == new DateTime(2025, 10, 1));
            Assert.Equal(30, ue2Dia1.TotalEstudantes);
            Assert.Equal(25, ue2Dia1.TotalPresentes);
            Assert.Equal(Math.Round(25m / 30m * 100m, 2), Math.Round(ue2Dia1.PercentualFrequencia, 2));
            Assert.Equal(NivelFrequenciaEnum.Baixa, ue2Dia1.NivelFrequencia);
        }
    }
}