using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Fechamento
{
    public class GerarFechamentoTurmaEdFisica2020UseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly GerarFechamentoTurmaEdFisica2020UseCase _useCase;

        public GerarFechamentoTurmaEdFisica2020UseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new GerarFechamentoTurmaEdFisica2020UseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_RetornarFalse_QuandoParametroDesativado()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterValorParametroSistemaTipoEAnoQuery>(), default))
                         .ReturnsAsync("false");

            var mensagem = new MensagemRabbit
            {
                Mensagem = null 
            };

            var resultado = await _useCase.Executar(mensagem);

            Assert.False(resultado);
        }

        [Fact]
        public async Task Deve_ExecutarFechamentoPorTurma_QuandoCodigoAlunoVazio()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterValorParametroSistemaTipoEAnoQuery>(), default))
                         .ReturnsAsync("true");

            var fechamentos = new List<FechamentoAlunoComponenteDTO>
                {
                    new FechamentoAlunoComponenteDTO
                    {
                        CodigoAluno = 123,
                        CodigoTurma = 999
                    }
                };

            _mediatorMock.Setup(m => m.Send(ObterAlunosEdFisica2020Query.Instance, default))
                         .ReturnsAsync(fechamentos);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), default))
                         .ReturnsAsync(new Turma
                         {
                             CodigoTurma = "999",
                             TipoTurma = Dominio.Enumerados.TipoTurma.EdFisica,
                             AnoLetivo = 2020,
                             Id = 12345
                         });

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), default))
                         .ReturnsAsync(true);

            var mensagem = new MensagemRabbit
            {
                Mensagem = string.Empty
            };

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), default), Times.Once);
        }

        [Fact]
        public async Task Deve_ExecutarFechamentoPorAluno_QuandoCodigoAlunoInformado()
        {
            const long codigoAluno = 123;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterValorParametroSistemaTipoEAnoQuery>(), default))
                         .ReturnsAsync("true");

            var fechamentos = new List<FechamentoAlunoComponenteDTO>
               {
                   new FechamentoAlunoComponenteDTO
                   {
                       CodigoAluno = codigoAluno,
                       CodigoTurma = 999
                   }
               };

            _mediatorMock.Setup(m => m.Send(ObterAlunosEdFisica2020Query.Instance, default))
                         .ReturnsAsync(fechamentos);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), default))
                         .ReturnsAsync(new Turma
                         {
                             CodigoTurma = "999",
                             TipoTurma = Dominio.Enumerados.TipoTurma.EdFisica,
                             AnoLetivo = 2020,
                             Id = 12345
                         });

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), default))
                         .ReturnsAsync(true);

            var mensagem = new MensagemRabbit
            {
                Mensagem = codigoAluno.ToString() // ⬅️ Corrigido
            };

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), default), Times.Once);
        }
    }
}
