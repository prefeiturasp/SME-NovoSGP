using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.ComponentesTurmas
{
    public class ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandlerTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        public ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandlerTeste()
        {
            mediatorMock = new Mock<IMediator>();
        }

        [Fact]
        public async Task Deve_Retornar_turma_com_Componente_Curricular_historica()
        {
            var turma2 = new Turma
            {
                CodigoTurma = "2",
                Historica = true,
                AnoLetivo = 2022,
                UeId = 2,
                Ano = "2",
                ModalidadeCodigo = Modalidade.Medio,
                Semestre = 1,
                Nome = "2",
                TipoTurma = TipoTurma.Regular,
                Ue = new Ue()
                {
                    CodigoUe = "2",
                    DreId = 2,
                    Nome = "2",
                    TipoEscola = TipoEscola.EMEF,
                    Dre = new Dre()
                    {
                        CodigoDre = "2",
                        Abreviacao = "2",
                        Nome = "2"
                    }
                }
            };
            var dataReferenciaInicioAnoLetivo = new DateTime(turma2.AnoLetivo, 2, 5);
            var turmas = new List<Turma> { turma2 };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasDreUePorCodigosQuery>(), CancellationToken.None))
                .ReturnsAsync(turmas);

            var componentesDaTurma = new List<RetornoConsultaListagemTurmaComponenteDto>
            {
                new RetornoConsultaListagemTurmaComponenteDto
                {
                    TurmaCodigo = 2,
                    ComponenteCurricularCodigo = 1
                },
                new RetornoConsultaListagemTurmaComponenteDto
                {
                    TurmaCodigo = 2,
                    ComponenteCurricularCodigo = 2
                }

            };
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasComComponentesQuery>(), CancellationToken.None))
                .ReturnsAsync(new PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>
                {
                    Items = componentesDaTurma
                });

            var componentesCurricularesSgp = new List<InfoComponenteCurricular>
            {
                new InfoComponenteCurricular(){ Codigo= 1, CodigoComponenteCurricularPai=null, Nome="1", EhRegencia = false, EhTerritorioSaber = false, RegistraFrequencia = true, LancaNota = true},
                new InfoComponenteCurricular(){ Codigo= 2, CodigoComponenteCurricularPai=null, Nome="2", EhRegencia = false, EhTerritorioSaber = false, RegistraFrequencia = true, LancaNota = true},
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterInfoPedagogicasComponentesCurricularesPorIdsQuery>(), CancellationToken.None))
                .ReturnsAsync(componentesCurricularesSgp);

            var handler = new ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandler(mediatorMock.Object);

            var query = new ObterInfoComponentesCurricularesESPorTurmasCodigoQuery(new string[] { "2" });

            // Act
            var resultado = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado);
            mediatorMock.Verify(x => x.Send(It.Is<ObterTurmasComComponentesQuery>(x => x.ConsideraHistorico == true && x.PeriodoEscolarInicio == dataReferenciaInicioAnoLetivo), CancellationToken.None), Times.Once);
        }

    }
}
