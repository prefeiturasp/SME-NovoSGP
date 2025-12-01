using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries
{
    public class ObterDatasAulasPorProfessorEComponenteQueryHandlerTeste
    {
        private readonly ObterDatasAulasPorProfessorEComponenteQueryHandler query;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioAulaConsulta> repositorioAulaConsulta;

        public ObterDatasAulasPorProfessorEComponenteQueryHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            repositorioAulaConsulta = new Mock<IRepositorioAulaConsulta>();
            query = new ObterDatasAulasPorProfessorEComponenteQueryHandler(mediator.Object, repositorioAulaConsulta.Object);
        }

        [Fact]
        public async Task Deve_Obter_Datas_Aulas()
        {
            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { AnoLetivo = 2020, CodigoTurma = "123" });

            var aula1 = new SME.SGP.Dominio.Aula() { DataAula = new DateTime(2020, 08, 05, 0, 0, 0, DateTimeKind.Local), Id = 1 };
            var aula2 = new SME.SGP.Dominio.Aula() { DataAula = new DateTime(2020, 08, 05, 0, 0, 0, DateTimeKind.Local), Id = 2 };
            var aula3 = new SME.SGP.Dominio.Aula() { DataAula = new DateTime(2020, 08, 06, 0, 0, 0, DateTimeKind.Local), Id = 3 };

            var listaAulas = new List<SME.SGP.Dominio.Aula>()
            {
                aula1, aula2, aula3
            };

            repositorioAulaConsulta.Setup(x => x.ObterDatasDeAulasPorAnoTurmaEDisciplina(It.IsAny<IEnumerable<long>>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<bool>()))
                .Returns(listaAulas);


            var aulaPossuiFrequenciaAulaRegistradaDto = new List<AulaPossuiFrequenciaAulaRegistradaDto>()
            {
                new AulaPossuiFrequenciaAulaRegistradaDto
                {
                    Id = 1,
                    DataAula = new DateTime(2020, 08, 05),
                    AulaCJ = false,
                    ProfessorRf = "123",
                    CriadoPor = "Sistema",
                    TipoAula = TipoAula.Normal,
                    PossuiFrequenciaRegistrada = true
                },
                new AulaPossuiFrequenciaAulaRegistradaDto
                {
                    Id = 2,
                    DataAula = new DateTime(2020, 08, 05),
                    AulaCJ = false,
                    ProfessorRf = "123",
                    CriadoPor = "Sistema",
                    TipoAula = TipoAula.Normal,
                    PossuiFrequenciaRegistrada = true
                },
                new AulaPossuiFrequenciaAulaRegistradaDto
                {
                    Id = 3,
                    DataAula = new DateTime(2020, 08, 06),
                    AulaCJ = true,
                    ProfessorRf = "456",
                    CriadoPor = "Sistema",
                    TipoAula = TipoAula.Normal,
                    PossuiFrequenciaRegistrada = true
                }
            };

            repositorioAulaConsulta.Setup(x => x.ObterDatasDeAulasPorAnoTurmaEDisciplinaVerificandoSePossuiFrequenciaAulaRegistrada(It.IsAny<IEnumerable<long>>(), It.IsAny<int>(),
                It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<bool>())).ReturnsAsync(aulaPossuiFrequenciaAulaRegistradaDto);

            mediator.Setup(x => x.Send(It.IsAny<ObterAulasPorIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaAulas);

            mediator.Setup(x => x.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolar>() { new PeriodoEscolar() { Id = 1, Bimestre = 1 } });

            var usuario = new Usuario();
            usuario.DefinirPerfis(new List<PrioridadePerfil>());
            usuario.DefinirPerfilAtual(Perfis.PERFIL_DIRETOR);
            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            var datasAulas = await query.Handle(new ObterDatasAulasPorProfessorEComponenteQuery("123", "1105"), new CancellationToken());

            Assert.NotNull(datasAulas);
            Assert.True(datasAulas.Count() == 2, "O retorno deve conter duas datas");
            Assert.True(datasAulas.First().Aulas.Count() == 2, "O primeiro dia deve conter duas aulas");
        }
    }
}