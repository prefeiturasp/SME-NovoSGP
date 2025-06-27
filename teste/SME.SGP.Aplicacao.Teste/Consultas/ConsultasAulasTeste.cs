using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasAulasTeste
    {
        private readonly ConsultasAula consultas;
        private readonly Mock<IConsultasDisciplina> consultasDisciplinas;
        private readonly Mock<IConsultasPeriodoEscolar> consultasPeriodoEscolar;
        private readonly Mock<IConsultasTipoCalendario> consultasTipoCalendario;
        private readonly Mock<IConsultasTurma> consultasTurma;
        private readonly Mock<IRepositorioAulaConsulta> repositorioAula;
        private readonly Mock<IServicoUsuario> servicoUsuario;
        private readonly Mock<IRepositorioTipoCalendarioConsulta> repositorio;
        private readonly Mock<IMediator> mediator;

        public ConsultasAulasTeste()
        {
            repositorioAula = new Mock<IRepositorioAulaConsulta>();
            servicoUsuario = new Mock<IServicoUsuario>();
            consultasPeriodoEscolar = new Mock<IConsultasPeriodoEscolar>();
            consultasDisciplinas = new Mock<IConsultasDisciplina>();
            consultasTurma = new Mock<IConsultasTurma>();
            consultasTipoCalendario = new Mock<IConsultasTipoCalendario>();
            repositorio = new Mock<IRepositorioTipoCalendarioConsulta>();
            mediator = new Mock<IMediator>();

            consultas = new ConsultasAula(repositorioAula.Object, consultasPeriodoEscolar.Object, consultasTipoCalendario.Object, servicoUsuario.Object, consultasDisciplinas.Object, consultasTurma.Object, mediator.Object);

            Setup();
        }

        [Fact]
        public async Task DeveObterDatasDeAulasPorCalendarioTurmaEDisciplina()
        {
            SetupObterDatasDeAulas();
            var aulas = await consultas.ObterDatasDeAulasPorCalendarioTurmaEDisciplina(DateTime.Now.Year, "123", "7");

            Assert.NotNull(aulas);
            Assert.True(aulas.Count() >= 1);
        }

        [Fact]
        public async Task DeveObterQuantidadeAulas()
        {
            var qtd = await consultas.ObterQuantidadeAulasTurmaSemanaProfessor("123", "7", 3, null);

            Assert.True(qtd == 4);
        }

        private void SetupObterDatasDeAulas()
        {
            var usuario = new Usuario()
            {
                CodigoRf = "111111",
                PerfilAtual = Perfis.PERFIL_CJ
            };

            var disciplina = new DisciplinaDto()
            {
                Nome = "Portugues",
                CodigoComponenteCurricular = 7,
                TurmaCodigo = "123"
            };

            var turma = new Turma()
            {
                CodigoTurma = "123",
                AnoLetivo = DateTime.Now.Year,
                ModalidadeCodigo = Modalidade.Medio,
                Semestre = 1
            };

            var tipoCalendarioCompleto = new TipoCalendarioCompletoDto()
            {
                Id = 1,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Semestre = 1,
                AnoLetivo = DateTime.Now.Year
            };

            var tipoCalendario = new TipoCalendario()
            {
                Id = 1,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Semestre = 1,
                AnoLetivo = DateTime.Now.Year
            };

            var atribuicaoCJ = new AtribuicaoCJ()
            {
                DisciplinaId = 7,
                TurmaId = "123",
                CriadoRF = "111111"
            };

            var listaAtribuicaoCJ = new List<AtribuicaoCJ>
            {
                atribuicaoCJ
            };

            var periodoEscolarDtoLista = new List<PeriodoEscolarDto>();

            var periodoEscolarDto = new PeriodoEscolarDto()
            {
                Id = 1,
                Bimestre = 1,
                PeriodoInicio = DateTime.Now,
                PeriodoFim = DateTime.Now.AddDays(2)
            };

            periodoEscolarDtoLista.Add(periodoEscolarDto);

            var periodoEscolarListaDto = new PeriodoEscolarListaDto()
            {
                TipoCalendario = 40,
                Periodos = periodoEscolarDtoLista
            };

            var aula = new Aula()
            {
                Id = 1,
                DataAula = DateTime.Now.AddDays(1),
                ProfessorRf = usuario.CodigoRf,
                Quantidade = 3,
                Turma = turma
            };

            var listaAula = new List<Aula>
            {
                aula
            };

            IEnumerable<long> periodosId = periodoEscolarListaDto.Periodos.Select(x => x.Id);

            var componentesCurriculares = new List<ComponenteCurricularEol>
            {
                new ComponenteCurricularEol
                {
                    Codigo = 7,
                    Descricao = "Portugues",
                    Regencia = false,
                    TerritorioSaber = false
                }
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(componentesCurriculares);

            repositorioAula.Setup(c => c.ObterDatasDeAulasPorAnoTurmaEDisciplina(It.IsAny<IEnumerable<long>>(),
                                                                                    It.IsAny<int>(),
                                                                                    It.IsAny<string>(),
                                                                                    It.IsAny<string>(),
                                                                                    It.IsAny<string>(),
                                                                                    null,
                                                                                    null,
                                                                                    It.IsAny<bool>()))
                .Returns(listaAula);

            servicoUsuario.Setup(c => c.ObterUsuarioLogado())
                .Returns(Task.FromResult(usuario));

            consultasDisciplinas.Setup(c => c.ObterDisciplina(atribuicaoCJ.DisciplinaId))
                .Returns(Task.FromResult(disciplina));

            repositorio.Setup(c => c.BuscarPorAnoLetivoEModalidade(DateTime.Now.Year, ModalidadeTipoCalendario.FundamentalMedio, 1))
                .Returns(Task.FromResult(tipoCalendario));

            consultasTipoCalendario.Setup(c => c.BuscarPorAnoLetivoEModalidade(DateTime.Now.Year, ModalidadeTipoCalendario.FundamentalMedio, 1))
                .Returns(Task.FromResult(tipoCalendarioCompleto));

            consultasPeriodoEscolar.Setup(c => c.ObterPorTipoCalendario(tipoCalendarioCompleto.Id))
                .Returns(Task.FromResult(periodoEscolarListaDto));

            mediator.Setup(x => x.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(disciplina);

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesDoProfessorCJNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaAtribuicaoCJ);

        }
        private void Setup()
        {
            // Mock para testar o metodo ObterPorId
            var aula = new Aula()
            {
                Id = 1,
                DataAula = new DateTime(2019, 11, 15),
                ProfessorRf = "123",
                Quantidade = 3
            };

            repositorioAula.Setup(c => c.ObterPorId(It.IsAny<long>()))
                .Returns(aula);

            // Mock das aulas por turma e disciplina
            IEnumerable<AulasPorTurmaDisciplinaDto> aulas = new List<AulasPorTurmaDisciplinaDto>()
            {
                new AulasPorTurmaDisciplinaDto() { ProfessorId = "1", Quantidade = 1, DataAula = new System.DateTime(2019,11,12) },
                new AulasPorTurmaDisciplinaDto() { ProfessorId = "1", Quantidade = 3, DataAula = new System.DateTime(2019,11,15) },
            };

            repositorioAula.Setup(c => c.ObterAulasTurmaDisciplinaSemanaProfessor(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<int>(), null))
                .Returns(Task.FromResult(aulas));
        }
    }
}