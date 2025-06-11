using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using MediatR;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EncaminhamentoNAAPA
{
    public class AtualizarEnderecoDoEncaminhamentoNAAPAUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AtualizarEnderecoDoEncaminhamentoNAAPAUseCase _useCase;

        public AtualizarEnderecoDoEncaminhamentoNAAPAUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new AtualizarEnderecoDoEncaminhamentoNAAPAUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Atualizar_Resposta_Quando_Endereco_E_Diferente()
        {
            // Arrange
            var alunoCodigo = "123456";
            var encaminhamentoId = 99L;

            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(new EncaminhamentoNAAPADto
                {
                    Id = encaminhamentoId,
                    AlunoCodigo = alunoCodigo
                })
            };

            var enderecoEol = new AlunoEnderecoRespostaDto
            {
                Endereco = new EnderecoRespostaDto
                {
                    Bairro = "Centro",
                    Complemento = "Bloco A",
                    Logradouro = "Rua das Flores",
                    Nro = "100",
                    Tipologradouro = "Rua"
                }
            };

            var respostaAntiga = new RespostaEncaminhamentoNAAPA
            {
                Id = 1,
                Texto = JsonConvert.SerializeObject(new List<RespostaEnderecoResidencialEncaminhamentoNAAPADto>
            {
                new RespostaEnderecoResidencialEncaminhamentoNAAPADto
                {
                    bairro = "Antigo Bairro",
                    complemento = "Antigo Complemento",
                    logradouro = "Antiga Rua",
                    numero = "99",
                    tipoLogradouro = "Avenida"
                }
            })
            };

            var questao = new QuestaoEncaminhamentoNAAPA
            {
                QuestaoId = 42,
                Respostas = new List<RespostaEncaminhamentoNAAPA> { respostaAntiga }
            };

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterAlunoEnderecoEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(enderecoEol);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterQuestaoEnderecoAlunoEncaminhamentoNAAPAPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questao);

            _mediatorMock.Setup(x => x.Send(It.IsAny<AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await _useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
            _mediatorMock.Verify(x => x.Send(It.IsAny<AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
