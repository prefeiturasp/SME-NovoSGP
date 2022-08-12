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

namespace SME.SGP.Aplicacao.Teste.Queries
{
    public class ObterDatasAulasPorProfessorEComponenteQueryHandlerTeste
    {
        private readonly ObterDatasAulasPorProfessorEComponenteQueryHandler query;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioAulaConsulta> repositorioAulaConsulta;
        private readonly Mock<IRepositorioTurmaConsulta> repositorioTurmaConsulta;

        public ObterDatasAulasPorProfessorEComponenteQueryHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            repositorioAulaConsulta = new Mock<IRepositorioAulaConsulta>();
            repositorioTurmaConsulta = new Mock<IRepositorioTurmaConsulta>();

            query = new ObterDatasAulasPorProfessorEComponenteQueryHandler(mediator.Object, repositorioAulaConsulta.Object, repositorioTurmaConsulta.Object);
        }

        [Fact]
        public async Task Deve_Obter_Datas_Aulas()
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { AnoLetivo = 2020, CodigoTurma = "123" });

            var aula1 = new Aula() { DataAula = new DateTime(2020, 08, 05), Id = 1, ProfessorRf = "1" };
            var aula2 = new Aula() { DataAula = new DateTime(2020, 08, 05), Id = 2, ProfessorRf = "1" };
            var aula3 = new Aula() { DataAula = new DateTime(2020, 08, 06), Id = 3, ProfessorRf = "1" };

            var aulas = new List<Aula>() { aula1, aula2, aula3 };

            var listaAulas = new List<AulaPossuiFrequenciaAulaRegistradaDto>()
            {
                new AulaPossuiFrequenciaAulaRegistradaDto()
                {
                    Id = aula1.Id,
                    DataAula = aula1.DataAula,
                    AulaCJ = aula1.AulaCJ,
                    ProfessorRf = aula1.ProfessorRf,
                    CriadoPor = aula1.CriadoPor,
                    TipoAula = aula1.TipoAula
                },
                new AulaPossuiFrequenciaAulaRegistradaDto()
                {
                    Id = aula2.Id,
                    DataAula = aula2.DataAula,
                    AulaCJ = aula2.AulaCJ,
                    ProfessorRf = aula2.ProfessorRf,
                    CriadoPor = aula2.CriadoPor,
                    TipoAula = aula2.TipoAula
                },
                new AulaPossuiFrequenciaAulaRegistradaDto()
                {
                    Id = aula3.Id,
                    DataAula = aula3.DataAula,
                    AulaCJ = aula3.AulaCJ,
                    ProfessorRf = aula3.ProfessorRf,
                    CriadoPor = aula3.CriadoPor,
                    TipoAula = aula3.TipoAula
                },
            };

            repositorioAulaConsulta.Setup(x => x.ObterDatasDeAulasPorAnoTurmaEDisciplinaVerificandoSePossuiFrequenciaAulaRegistrada(It.IsAny<IEnumerable<long>>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<bool>()))
                .ReturnsAsync(listaAulas);

            mediator.Setup(x => x.Send(It.IsAny<ObterAulasPorIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulas);

            mediator.Setup(x => x.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolar>() { new PeriodoEscolar() { Id = 1, Bimestre = 1 } });

            var usuario = new Usuario() { CodigoRf = "1" };
            usuario.DefinirPerfis(new List<PrioridadePerfil>());
            usuario.DefinirPerfilAtual(Perfis.PERFIL_DIRETOR);
            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            // Act
            var datasAulas = await query.Handle(new ObterDatasAulasPorProfessorEComponenteQuery("123", "123", "1105", false, false), new CancellationToken());

            //// Assert
            Assert.NotNull(datasAulas);

            Assert.True(datasAulas.Count() == 2, "O retorno deve conter duas datas");
            Assert.True(datasAulas.First().Aulas.Count() == 2, "O primeiro dia deve conter duas aulas");
        }
    }
}
