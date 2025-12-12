using Moq;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.DiarioBordo
{
    public class RecuperarDiarioBordoComAulasExcluidasQueryHandlerTeste
    {
        private readonly Mock<IRepositorioDiarioBordoConsulta> repositorioDiarioBordo;
        private readonly RecuperarDiarioBordoComAulasExcluidasQueryHandler query;

        public RecuperarDiarioBordoComAulasExcluidasQueryHandlerTeste()
        {
            repositorioDiarioBordo = new Mock<IRepositorioDiarioBordoConsulta>();
            query = new RecuperarDiarioBordoComAulasExcluidasQueryHandler(repositorioDiarioBordo.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Houver_Diarios_Bordo()
        {
            repositorioDiarioBordo.Setup(a => a.ObterIdDiarioBordoAulasExcluidas(
                It.IsAny<string>(),
                It.IsAny<string[]>(),
                It.IsAny<long>(),
                It.IsAny<DateTime[]>()))
                .ReturnsAsync(Enumerable.Empty<Dominio.DiarioBordo>());

            var retorno = await query.Handle(
                new RecuperarDiarioBordoComAulasExcluidasQuery(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<long>(), It.IsAny<DateTime[]>()),
                new CancellationToken());

            repositorioDiarioBordo.Verify(c => c.ObterIdDiarioBordoAulasExcluidas(
                It.IsAny<string>(),
                It.IsAny<string[]>(),
                It.IsAny<long>(),
                It.IsAny<DateTime[]>()), Times.Once);

            Assert.NotNull(retorno);
            Assert.Empty(retorno);
        }

        [Fact]
        public async Task Deve_Retornar_Diarios_Bordo_Com_Aulas_Excluidas()
        {
            var diariosBordo = new List<Dominio.DiarioBordo>
            {
                new Dominio.DiarioBordo { Id = 1 },
                new Dominio.DiarioBordo { Id = 2 }
            };

            repositorioDiarioBordo.Setup(a => a.ObterIdDiarioBordoAulasExcluidas(
                It.IsAny<string>(),
                It.IsAny<string[]>(),
                It.IsAny<long>(),
                It.IsAny<DateTime[]>()))
                .ReturnsAsync(diariosBordo);

            var retorno = await query.Handle(
                new RecuperarDiarioBordoComAulasExcluidasQuery(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<long>(), It.IsAny<DateTime[]>()),
                new CancellationToken());

            repositorioDiarioBordo.Verify(c => c.ObterIdDiarioBordoAulasExcluidas(
                It.IsAny<string>(),
                It.IsAny<string[]>(),
                It.IsAny<long>(),
                It.IsAny<DateTime[]>()), Times.Once);

            Assert.NotNull(retorno);
            Assert.Equal(diariosBordo.Count, retorno.Count());
        }

        [Fact]
        public async Task Deve_Chamar_Repositorio_Com_Parametros_Corretos()
        {
            var codigoTurma = "TURMA123";
            var codigosDisciplinas = new[] { "DISC1", "DISC2" };
            var tipoCalendarioId = 10L;
            var datasConsideradas = new[] { new DateTime(2024, 1, 1), new DateTime(2024, 1, 2) };

            repositorioDiarioBordo.Setup(a => a.ObterIdDiarioBordoAulasExcluidas(
                codigoTurma,
                codigosDisciplinas,
                tipoCalendarioId,
                datasConsideradas))
                .ReturnsAsync(Enumerable.Empty<Dominio.DiarioBordo>());

            await query.Handle(
                new RecuperarDiarioBordoComAulasExcluidasQuery(codigoTurma, codigosDisciplinas, tipoCalendarioId, datasConsideradas),
                new CancellationToken());

            repositorioDiarioBordo.Verify(c => c.ObterIdDiarioBordoAulasExcluidas(
                codigoTurma,
                codigosDisciplinas,
                tipoCalendarioId,
                datasConsideradas), Times.Once);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Repositorio_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new RecuperarDiarioBordoComAulasExcluidasQueryHandler(null));
        }
    }
}
