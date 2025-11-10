using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase useCase;

        public ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Listar_Sem_Ensino_Especial()
        {
            var mockObjetivos = new List<ObjetivoAprendizagemDto> {
                    new ObjetivoAprendizagemDto
                    {
                        Codigo = "(EF06C11)",
                        Ano = "fifth",
                        Descricao = "",
                        Id = 1475,
                        ComponenteCurricularEolId = 139,
                        IdComponenteCurricular = 1
                    }
                };

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Turma() { CodigoTurma = "1", EnsinoEspecial = false, ModalidadeCodigo = Modalidade.Fundamental });

            mediator.Setup(a => a.Send(It.IsAny<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockObjetivos);

            var mockJurema = new long[] { 1 };

            mediator.Setup(a => a.Send(It.IsAny<ObterJuremaIdsPorComponentesCurricularIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockJurema);

            var retorno = await useCase.Executar("5", 139, false, 1);

            mediator.Verify(x => x.Send(It.IsAny<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterJuremaIdsPorComponentesCurricularIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno.Any());
        }

        [Fact]
        public async Task Deve_Listar_Com_Ensino_Especial()
        {
            var mockObjetivos = new List<ObjetivoAprendizagemDto> {
                    new ObjetivoAprendizagemDto
                    {
                        Codigo = "(EF06C11)",
                        Ano = "fifth",
                        Descricao = "",
                        Id = 1475,
                        ComponenteCurricularEolId = 138,
                        IdComponenteCurricular = 1
                    }
                };

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Turma() { CodigoTurma = "1", EnsinoEspecial = true, ModalidadeCodigo = Modalidade.Fundamental });

            mediator.Setup(a => a.Send(It.IsAny<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockObjetivos);

            var mockJurema = new long[] { 6 };

            mediator.Setup(a => a.Send(It.IsAny<ObterJuremaIdsPorComponentesCurricularIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockJurema);

            var retorno = await useCase.Executar("5", 138, true, 1);

            mediator.Verify(x => x.Send(It.IsAny<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterJuremaIdsPorComponentesCurricularIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);

            Assert.True(retorno.Any());
        }

        [Fact]
        public void Executar_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Modalidade_Nao_Possui_Objetivos_Deve_Retornar_Null()
        {
            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Turma() { ModalidadeCodigo = Modalidade.Medio });

            var retorno = await useCase.Executar("1", 139, false, 1);

            Assert.Null(retorno);
        }

        [Fact]
        public async Task Executar_Quando_Lingua_Portuguesa_Ensino_Especial_Deve_Usar_Id_Jurema_11()
        {
            var mockObjetivos = new List<ObjetivoAprendizagemDto> {
                new ObjetivoAprendizagemDto
                {
                    Codigo = "(EF01LP01)",
                    Ano = "first",
                    Descricao = "Teste",
                    Id = 1,
                    ComponenteCurricularEolId = 138,
                    IdComponenteCurricular = 1
                }
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Turma() { ModalidadeCodigo = Modalidade.Fundamental, EnsinoEspecial = true });

            mediator.Setup(a => a.Send(It.IsAny<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockObjetivos);

            var retorno = await useCase.Executar("1", 138, true, 1);

            mediator.Verify(x => x.Send(It.IsAny<ObterJuremaIdsPorComponentesCurricularIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.True(retorno.Any());
        }

        [Fact]
        public async Task Executar_Quando_Lingua_Portuguesa_Sem_Ensino_Especial_Deve_Usar_Id_Jurema_6()
        {
            var mockObjetivos = new List<ObjetivoAprendizagemDto> {
                new ObjetivoAprendizagemDto
                {
                    Codigo = "(EF01LP01)",
                    Ano = "first",
                    Descricao = "Teste",
                    Id = 1,
                    ComponenteCurricularEolId = 138,
                    IdComponenteCurricular = 1
                }
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Turma() { ModalidadeCodigo = Modalidade.Fundamental, EnsinoEspecial = false });

            mediator.Setup(a => a.Send(It.IsAny<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockObjetivos);

            var retorno = await useCase.Executar("1", 138, false, 1);

            mediator.Verify(x => x.Send(It.IsAny<ObterJuremaIdsPorComponentesCurricularIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.True(retorno.Any());
        }       

        [Fact]
        public async Task Executar_Quando_Ensino_Especial_E_Ano_Nao_Contido_Deve_Retornar_Ordenado_Por_Enum()
        {
            var mockObjetivos = new List<ObjetivoAprendizagemDto> {
                new ObjetivoAprendizagemDto
                {
                    Codigo = "(EF05MA01)",
                    Ano = "fifth",
                    Descricao = "Teste",
                    Id = 2,
                    ComponenteCurricularEolId = 139,
                    IdComponenteCurricular = 1
                },
                new ObjetivoAprendizagemDto
                {
                    Codigo = "(EF01MA01)",
                    Ano = "first",
                    Descricao = "Teste",
                    Id = 1,
                    ComponenteCurricularEolId = 139,
                    IdComponenteCurricular = 1
                }
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Turma() { ModalidadeCodigo = Modalidade.Fundamental, EnsinoEspecial = true });

            mediator.Setup(a => a.Send(It.IsAny<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockObjetivos);

            mediator.Setup(a => a.Send(It.IsAny<ObterJuremaIdsPorComponentesCurricularIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new long[] { 1 });

            var retorno = await useCase.Executar("10", 139, true, 1);

            Assert.True(retorno.Any());
            Assert.Equal(2, retorno.Count());
        }
      
               [Fact]
        public async Task Executar_Quando_Componente_Nao_Lingua_Portuguesa_Deve_Buscar_Ids_Jurema()
        {
            var mockObjetivos = new List<ObjetivoAprendizagemDto> {
                new ObjetivoAprendizagemDto
                {
                    Codigo = "(EF01MA01)",
                    Ano = "first",
                    Descricao = "Teste",
                    Id = 1,
                    ComponenteCurricularEolId = 139,
                    IdComponenteCurricular = 1
                }
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Turma() { ModalidadeCodigo = Modalidade.Fundamental });

            mediator.Setup(a => a.Send(It.IsAny<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockObjetivos);

            mediator.Setup(a => a.Send(It.IsAny<ObterJuremaIdsPorComponentesCurricularIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new long[] { 2 });

            var retorno = await useCase.Executar("1", 139, false, 1);

            mediator.Verify(x => x.Send(It.IsAny<ObterJuremaIdsPorComponentesCurricularIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(retorno.Any());
        }
    }
}
