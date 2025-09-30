using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Comunicados.ObterComunicadosPaginadosAluno
{
    public class ObterComunicadosPaginadosAlunoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterComunicadosPaginadosAlunoUseCase useCase;

        public ObterComunicadosPaginadosAlunoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterComunicadosPaginadosAlunoUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Comunicados_Quando_Dados_Forem_Validos()
        {
            var filtro = new FiltroTurmaAlunoSemestreDto(turmaId: 1, alunoCodigo: 123, semestre: 1);

            var turma = new Turma
            {
                Id = 1,
                CodigoTurma = "TURMA123",
                AnoLetivo = 2025,
                Ue = new Ue
                {
                    CodigoUe = "UE01",
                    Dre = new SME.SGP.Dominio.Dre
                    {
                        CodigoDre = "DRE01"
                    }
                }
            };

            var aluno = new AlunoReduzidoDto
            {
                CodigoAluno = "12345",
                Nome = "Aluno Teste"
            };

            var tipoCalendarioId = 99L;

            var periodosEscolares = new List<Dominio.PeriodoEscolar>
            {
                new Dominio.PeriodoEscolar { Bimestre = 1, PeriodoInicio = new DateTime(2025, 2, 1), PeriodoFim = new DateTime(2025, 3, 31) },
                new Dominio.PeriodoEscolar { Bimestre = 2, PeriodoInicio = new DateTime(2025, 4, 1), PeriodoFim = new DateTime(2025, 5, 31) },
                new Dominio.PeriodoEscolar { Bimestre = 3, PeriodoInicio = new DateTime(2025, 6, 1), PeriodoFim = new DateTime(2025, 7, 31) },
                new Dominio.PeriodoEscolar { Bimestre = 4, PeriodoInicio = new DateTime(2025, 8, 1), PeriodoFim = new DateTime(2025, 10, 31) }
            };

            var comunicadosEsperados = new PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>
            {
                TotalPaginas = 1,
                TotalRegistros = 1,
                Items = new List<ComunicadoAlunoReduzidoDto>
                {
                    new ComunicadoAlunoReduzidoDto
                    {
                        ComunicadoId = 100,
                        DataEnvio = DateTime.Today,
                        Titulo = "Comunicado Teste",
                        Categoria = TipoComunicado.DRE,
                        CategoriaNome = "Geral",
                        StatusLeitura = "Não Lido"
                    }
                }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), default))
                .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunoPorCodigoEAnoQuery>(), default))
                .ReturnsAsync(aluno);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), default))
                .ReturnsAsync(tipoCalendarioId);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>(), default))
                .ReturnsAsync(periodosEscolares);

            mediatorMock.Setup(m => m.Send(It.Is<ListarComunicadosPaginadosQuery>(
                    q => q.DRECodigo == "DRE01" &&
                         q.UECodigo == "UE01" &&
                         q.TurmaCodigo == "TURMA123" &&
                         q.AlunoCodigo == "123"), default))
                .ReturnsAsync(comunicadosEsperados);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Items);
            Assert.Equal(1, resultado.TotalPaginas);
            Assert.Equal(1, resultado.TotalRegistros);
            Assert.Equal("Comunicado Teste", resultado.Items.First().Titulo);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Turma_Nao_Encontrada()
        {
            var filtro = new FiltroTurmaAlunoSemestreDto(1, 123, 1);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), default))
                .ReturnsAsync((Turma)null); 

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(filtro));
            Assert.Equal("A Turma informada não foi encontrada", ex.Message);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Aluno_Nao_Encontrado()
        {
            var filtro = new FiltroTurmaAlunoSemestreDto(1, 123, 1);

            var turma = new Turma
            {
                Id = 1,
                AnoLetivo = 2025,
                CodigoTurma = "TURMA123",
                Ue = new Ue
                {
                    CodigoUe = "UE01",
                    Dre = new SME.SGP.Dominio.Dre { CodigoDre = "DRE01" }
                }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), default))
                .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunoPorCodigoEAnoQuery>(), default))
                .ReturnsAsync((AlunoReduzidoDto)null); 

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(filtro));
            Assert.Equal("O Aluno informado não foi encontrado", ex.Message);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_TipoCalendario_Nao_Encontrado()
        {
            var filtro = new FiltroTurmaAlunoSemestreDto(1, 123, 1);

            var turma = new Turma
            {
                Id = 1,
                AnoLetivo = 2025,
                CodigoTurma = "TURMA123",
                Ue = new Ue
                {
                    CodigoUe = "UE01",
                    Dre = new SME.SGP.Dominio.Dre { CodigoDre = "DRE01" }
                }
            };

            var aluno = new AlunoReduzidoDto { CodigoAluno = "123456" };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), default))
                .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunoPorCodigoEAnoQuery>(), default))
                .ReturnsAsync(aluno);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), default))
                .ReturnsAsync(0L); 

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(filtro));
            Assert.Equal("O tipo de calendário da turma não foi encontrado.", ex.Message);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Periodos_Escolares_Nao_Encontrados()
        {
            var filtro = new FiltroTurmaAlunoSemestreDto(1, 123, 1);

            var turma = new Turma
            {
                Id = 1,
                AnoLetivo = 2025,
                CodigoTurma = "TURMA123",
                Ue = new Ue
                {
                    CodigoUe = "UE01",
                    Dre = new SME.SGP.Dominio.Dre { CodigoDre = "DRE01" }
                }
            };

            var aluno = new AlunoReduzidoDto { CodigoAluno = "123456" };
            var tipoCalendarioId = 99L;

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), default))
                .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunoPorCodigoEAnoQuery>(), default))
                .ReturnsAsync(aluno);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), default))
                .ReturnsAsync(tipoCalendarioId);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>(), default))
                .ReturnsAsync((IEnumerable<Dominio.PeriodoEscolar>)null);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(filtro));
            Assert.Equal("Não foi possível encontrar o período escolar da turma.", ex.Message);
        }
    }
}
