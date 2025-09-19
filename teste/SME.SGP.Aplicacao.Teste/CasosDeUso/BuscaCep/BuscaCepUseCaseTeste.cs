using FluentValidation.TestHelper;
using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.BuscaCep
{
    public class BuscaCepUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly BuscaCepUseCase useCase;

        public BuscaCepUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new BuscaCepUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Cep_Dto_Quando_Mediator_Responder()
        {
            var cep = "01001000";
            var esperado = new CepDto
            {
                Cep = cep,
                Logradouro = "Praça da Sé",
                Bairro = "Sé",
                Localidade = "São Paulo",
                UF = "SP",
                Complemento = "lado ímpar"
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterCepQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(esperado);

            var resultado = await useCase.Executar(cep);

            Assert.NotNull(resultado);
            Assert.Equal(esperado.Cep, resultado.Cep);
            Assert.Equal(esperado.Logradouro, resultado.Logradouro);
            Assert.Equal(esperado.Bairro, resultado.Bairro);
            Assert.Equal(esperado.Localidade, resultado.Localidade);
            Assert.Equal(esperado.UF, resultado.UF);
            Assert.Equal(esperado.Complemento, resultado.Complemento);

            mediatorMock.Verify(m => m.Send(It.Is<ObterCepQuery>(q => q.Cep == cep), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Obter_Cep_Query_Deve_Atribuir_Cep_Corretamente()
        {
            var cep = "12345678";

            var query = new ObterCepQuery(cep);

            Assert.Equal(cep, query.Cep);
        }

        [Fact]
        public void Obter_Cep_Query_Validator_Deve_Falhar_Quando_Cep_Nulo()
        {
            var validator = new ObterCepQueryValidator();
            var query = new ObterCepQuery(null);

            var resultado = validator.TestValidate(query);

            resultado.ShouldHaveValidationErrorFor(q => q.Cep)
                .WithErrorMessage("É necessário informar o cep para a busca das informações postais");
        }

        [Fact]
        public void Obter_Cep_Query_Validator_Deve_Passar_Quando_Cep_Valido()
        {
            var validator = new ObterCepQueryValidator();
            var query = new ObterCepQuery("01001000");

            var resultado = validator.TestValidate(query);

            resultado.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Cep_Dto_Deve_Permitir_Get_Set()
        {
            var dto = new CepDto();

            dto.Cep = "01001000";
            dto.Logradouro = "Praça da Sé";
            dto.Bairro = "Sé";
            dto.Localidade = "São Paulo";
            dto.UF = "SP";
            dto.Complemento = "lado ímpar";

            Assert.Equal("01001000", dto.Cep);
            Assert.Equal("Praça da Sé", dto.Logradouro);
            Assert.Equal("Sé", dto.Bairro);
            Assert.Equal("São Paulo", dto.Localidade);
            Assert.Equal("SP", dto.UF);
            Assert.Equal("lado ímpar", dto.Complemento);
        }
    }
}
