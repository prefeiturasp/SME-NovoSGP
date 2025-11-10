using FluentValidation.TestHelper;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Bimestre
{
    public class ObterBimestrePorModalidadeUseCaseTeste
    {
        [Fact]
        public void Constructor_Null_Mediator_Throws_Argument_Null_Exception()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ObterBimestrePorModalidadeUseCase(null));
            Assert.Equal("mediator", ex.ParamName);
        }

        [Fact]
        public async Task Executar_Com_Retorno_Deve_Retornar_Lista()
        {
            var mockMediator = new Mock<IMediator>();
            var esperado = new List<FiltroBimestreDto>
            {
                new FiltroBimestreDto { Valor = 1, Descricao = "1º Bimestre" },
                new FiltroBimestreDto { Valor = 2, Descricao = "2º Bimestre" }
            };

            mockMediator
                .Setup(m => m.Send(It.IsAny<ObterBimestrePorModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(esperado);

            var useCase = new ObterBimestrePorModalidadeUseCase(mockMediator.Object);

            var result = await useCase.Executar(true, false, Modalidade.Fundamental);

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].Valor);
            Assert.Equal("1º Bimestre", result[0].Descricao);

            mockMediator.Verify(m => m.Send(
                It.Is<ObterBimestrePorModalidadeQuery>(q =>
                    q.OpcaoTodos == true &&
                    q.OpcaoFinal == false &&
                    q.Modalidade == Modalidade.Fundamental
                ),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Executar_Sem_Retorno_Deve_Retornar_Lista_Vazia()
        {
            var mockMediator = new Mock<IMediator>();

            mockMediator
                .Setup(m => m.Send(It.IsAny<ObterBimestrePorModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FiltroBimestreDto>()); 

            var useCase = new ObterBimestrePorModalidadeUseCase(mockMediator.Object);

            var result = await useCase.Executar(false, true, Modalidade.Medio);

            Assert.Empty(result);
            mockMediator.Verify(m => m.Send(It.IsAny<ObterBimestrePorModalidadeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    public class ObterBimestrePorModalidadeQueryValidatorTests
    {
        private readonly ObterBimestrePorModalidadeQueryValidator _validator = new ObterBimestrePorModalidadeQueryValidator();

        [Fact]
        public void Validator_Deve_Falhar_Quando_Modalidade_Nao_Informada()
        {
            var query = new ObterBimestrePorModalidadeQuery(true, false, 0); 
            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(q => q.Modalidade)
                .WithErrorMessage("A Modalidade deve ser informada para a consulta de bimestres.");
        }

        [Fact]
        public void Validator_Deve_Passar_Quando_Modalidade_Valida()
        {
            var query = new ObterBimestrePorModalidadeQuery(false, true, Modalidade.Fundamental);
            var result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
