using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorAlunosDaTurma;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorAlunosDaTurma
{
    public class ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IServicoAluno> servicoAlunoMock;
        private readonly ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaUseCase useCase;

        public ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            servicoAlunoMock = new Mock<IServicoAluno>();
            useCase = new ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaUseCase(mediatorMock.Object, servicoAlunoMock.Object);
        }

        private static FiltroDadosDeLeituraDeComunicadosPorAlunosTurmaDto CriarFiltro() =>
            new FiltroDadosDeLeituraDeComunicadosPorAlunosTurmaDto { ComunicadoId = 1, CodigoTurma = 12345 };

        [Fact]
        public async Task Executar_Deve_Retornar_Dados_Ordenados_Quando_Todos_Os_Dados_Forem_Validos()
        {
            var filtro = CriarFiltro();
            var leituraDtos = new List<DadosLeituraAlunosComunicadoPorTurmaDto>
            {
                new DadosLeituraAlunosComunicadoPorTurmaDto { CodigoAluno = 1, NomeAluno = "Carlos" },
                new DadosLeituraAlunosComunicadoPorTurmaDto { CodigoAluno = 2, NomeAluno = "Ana" }
            };

            var turma = new Turma { AnoLetivo = 2025, ModalidadeCodigo = Modalidade.Fundamental };
            var periodo = new PeriodoEscolar { PeriodoInicio = DateTime.Now.AddDays(-5), PeriodoFim = DateTime.Now.AddDays(5), Bimestre = 1 };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaQuery>(), default))
                .ReturnsAsync(leituraDtos);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), default))
                .ReturnsAsync(turma);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUltimoPeriodoEscolarPorDataQuery>(), default))
                .ReturnsAsync(periodo);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunoPorCodigoEolQuery>(), default))
                .ReturnsAsync(new AlunoPorTurmaResposta { CodigoAluno = "1", DataNascimento = new DateTime(2010, 1, 1), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo });
            mediatorMock.Setup(m => m.Send(It.IsAny<VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery>(), default))
                .ReturnsAsync(false);

            servicoAlunoMock.Setup(s => s.ObterMarcadorAluno(It.IsAny<AlunoPorTurmaResposta>(), It.IsAny<PeriodoEscolar>(), false))
                .Returns(new MarcadorFrequenciaDto { Tipo = TipoMarcadorFrequencia.Novo, Descricao = "Frequente" });

            var resultado = await useCase.Executar(filtro);

            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(2);
            resultado.First().NomeAluno.Should().Be("Ana"); 
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Turma_Nao_For_Encontrada()
        {
            var filtro = CriarFiltro();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), default))
                .ReturnsAsync((Turma)null);

            Func<Task> act = async () => await useCase.Executar(filtro);

            await act.Should().ThrowAsync<NegocioException>()
                .WithMessage("Não foi possível localizar a turma");
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Periodo_Escolar_Nao_For_Encontrado()
        {
            var filtro = CriarFiltro();
            var leituraDtos = new List<DadosLeituraAlunosComunicadoPorTurmaDto>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaQuery>(), default))
                .ReturnsAsync(leituraDtos);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), default))
                .ReturnsAsync(new Turma { AnoLetivo = 2025, ModalidadeCodigo = Modalidade.Fundamental });
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUltimoPeriodoEscolarPorDataQuery>(), default))
                .ReturnsAsync((PeriodoEscolar)null);

            Func<Task> act = async () => await useCase.Executar(filtro);

            await act.Should().ThrowAsync<NegocioException>()
                .WithMessage("Não foi possível localizar o periodo escolar");
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Aluno_Nao_For_Encontrado()
        {
            var filtro = CriarFiltro();
            var leituraDtos = new List<DadosLeituraAlunosComunicadoPorTurmaDto>
            {
                new DadosLeituraAlunosComunicadoPorTurmaDto { CodigoAluno = 1, NomeAluno = "João" }
            };

            var turma = new Turma { AnoLetivo = 2025, ModalidadeCodigo = Modalidade.Fundamental };
            var periodo = new PeriodoEscolar { PeriodoInicio = DateTime.Now, PeriodoFim = DateTime.Now.AddDays(30), Bimestre = 1 };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaQuery>(), default))
                .ReturnsAsync(leituraDtos);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), default))
                .ReturnsAsync(turma);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUltimoPeriodoEscolarPorDataQuery>(), default))
                .ReturnsAsync(periodo);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunoPorCodigoEolQuery>(), default))
                .ReturnsAsync((AlunoPorTurmaResposta)null);

            Func<Task> act = async () => await useCase.Executar(filtro);

            await act.Should().ThrowAsync<NegocioException>()
                .WithMessage("Não foi possível localizar o aluno");
        }
    }
}
