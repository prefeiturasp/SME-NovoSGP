using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Devolutiva
{
    public class ExistePendenciaDiarioBordoQueryTest
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioDiarioBordo> repositorio;
        private readonly ExistePendenciaDiarioBordoQueryHandler queryHandler;
        public ExistePendenciaDiarioBordoQueryTest()
        {
            mediator = new Mock<IMediator>();
            repositorio = new Mock<IRepositorioDiarioBordo>();
            queryHandler = new ExistePendenciaDiarioBordoQueryHandler(repositorio.Object, mediator.Object);
        }

        [Fact]
        public async Task Existe_Diario_Bordo_Sem_Devolutiva_Com_Menos_De_25_Dias()
        {
            //Arrange
            var dadosRepositorio = new List<DiarioBordoSemDevolutivaDto>
            {
                new DiarioBordoSemDevolutivaDto()
                {
                    Bimestre = 1,
                    PeriodoInicio = DateTime.Now.AddDays(-60),
                    PeriodoFim = DateTime.Now.AddDays(-50),
                    DataAula   = new DateTime(2022,01,04)
        },
                new DiarioBordoSemDevolutivaDto()
                {
                    Bimestre = 2,
                    PeriodoInicio = DateTime.Now.AddDays(-10),
                    PeriodoFim = DateTime.Now.AddDays(-20),
                    DataAula   = new DateTime(2022,01,04)
                }
            };

            var parametroSistema = new ParametrosSistema()
            {
                Id = 1,
                Nome = "PeriodoDeDiasDevolutiva",
                Tipo = TipoParametroSistema.PeriodoDeDiasDevolutiva,
                Ano = 2022,
                Ativo = true,
                Descricao = "Periodo considerado para consolidação das devolutivas",
                Valor = "25"
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(parametroSistema);
            repositorio.Setup(x => x.DiarioBordoSemDevolutiva(856521, "512")).ReturnsAsync(dadosRepositorio);

            // Act
            var consulta = await queryHandler.Handle(new ExistePendenciaDiarioBordoQuery(856521, "512"), new CancellationToken());

            // Assert
            Assert.False(consulta, "Não Existe Diário de Bordo sem devolutiva com menos de 25 dias");
        }

        [Fact]
        public async Task Existe_Retornar_Diario_Bordo_Sem_Devolutiva_Com_Mais_De_25_Dias()
        {
            //Arrange
            var dadosRepositorio = new List<DiarioBordoSemDevolutivaDto>
            {
                new DiarioBordoSemDevolutivaDto()
                {
                    Bimestre = 1,
                    PeriodoInicio = new DateTime(2022,02,07),
                    PeriodoFim = new DateTime(2022,04,29),
                    DataAula   = new DateTime(2022,01,04)
                },
                new DiarioBordoSemDevolutivaDto()
                {
                    Bimestre = 2,
                    PeriodoInicio = new DateTime(2022,05,02),
                    PeriodoFim = new DateTime(2022,07,22),
                    DataAula   = new DateTime(2022,01,04)
                }
            };

            var parametroSistema = new ParametrosSistema()
            {
                Id = 1,
                Nome = "PeriodoDeDiasDevolutiva",
                Tipo = TipoParametroSistema.PeriodoDeDiasDevolutiva,
                Ano = 2022,
                Ativo = true,
                Descricao = "Periodo considerado para consolidação das devolutivas",
                Valor = "25"
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(parametroSistema);
            repositorio.Setup(x => x.DiarioBordoSemDevolutiva(856521, "512")).ReturnsAsync(dadosRepositorio);

            // Act
            var consulta = await queryHandler.Handle(new ExistePendenciaDiarioBordoQuery(856521, "512"), new CancellationToken());

            // Assert
            Assert.True(consulta, "Não Existe Diário de Bordo sem devolutiva com mais de 25 dias");
        }

        [Fact]
        public async Task Todos_Diario_Bordo_Tem_Devolutiva()
        {
            //Arrange
            var dadosRepositorio = new List<DiarioBordoSemDevolutivaDto>();
            repositorio.Setup(x => x.DiarioBordoSemDevolutiva(856521, "512")).ReturnsAsync(dadosRepositorio);

            // Act
            var consulta = await queryHandler.Handle(new ExistePendenciaDiarioBordoQuery(856521, "512"), new CancellationToken());

            // Assert
            Assert.False(consulta, "Existe Diário de Bordo sem devolutiva");
        }
    }
}
