using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries
{
    public class ObterDatasAulasPorProfessorEComponenteQueryHandlerTeste
    {
        private readonly ObterDatasAulasPorProfessorEComponenteQueryHandler query;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioAula> repositorio;
        private readonly Mock<IRepositorioTurma> repositorioTurma;
        private readonly Mock<IRepositorioAula> repositorioAula;

        public ObterDatasAulasPorProfessorEComponenteQueryHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            repositorio = new Mock<IRepositorioAula>();
            repositorioTurma = new Mock<IRepositorioTurma>();
            repositorioAula = new Mock<IRepositorioAula>();

            query = new ObterDatasAulasPorProfessorEComponenteQueryHandler(mediator.Object, repositorio.Object, repositorioTurma.Object, repositorioAula.Object);
        }

        [Fact]
        public async Task Deve_Obter_Datas_Aulas()
        {
            //Arrange
            repositorioTurma.Setup(x => x.ObterPorCodigo(It.IsAny<string>()))
                .ReturnsAsync(new Dominio.Turma()
                {
                    AnoLetivo = 2020,
                    CodigoTurma = "123",
                });

            var aula1 = new Aula() { DataAula = new DateTime(2020, 08, 05), Id = 1 };
            var aula2 = new Aula() { DataAula = new DateTime(2020, 08, 05), Id = 2 };
            var aula3 = new Aula() { DataAula = new DateTime(2020, 08, 06), Id = 3 };

            repositorio.Setup(x => x.ObterDatasDeAulasPorAnoTurmaEDisciplina(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(new List<Aula>()
                {
                    aula1, aula2, aula3
                });

            repositorioAula.Setup(x => x.ObterPorId(1))
                .Returns(aula1);

            repositorioAula.Setup(x => x.ObterPorId(2))
                .Returns(aula2);

            repositorioAula.Setup(x => x.ObterPorId(3))
                .Returns(aula3);

            mediator.Setup(x => x.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolar>()
                {
                    new PeriodoEscolar() { Id = 1, Bimestre = 1},
                });

            var usuario = new Usuario();
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
