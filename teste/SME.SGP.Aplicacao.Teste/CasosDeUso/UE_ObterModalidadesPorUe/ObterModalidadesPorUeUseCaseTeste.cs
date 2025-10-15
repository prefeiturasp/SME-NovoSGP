using MediatR;
using Moq;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.UE_ObterModalidadesPorUe
{
    public class ObterModalidadesPorUeUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IObterModalidadesPorUeUseCase _useCase;

        public ObterModalidadesPorUeUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterModalidadesPorUeUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Com_Novas_Modalidades_Deve_Consultar_E_Filtra_Corretamente()
        {
            string ueCodigo = "123456";
            int anoLetivo = 2024;
            bool consideraNovasModalidades = true;

            var novasModalidades = new List<Modalidade> { Modalidade.CMCT, Modalidade.ETEC };
            var modalidadesRetorno = new List<ModalidadeRetornoDto>
            {
                new ModalidadeRetornoDto { Id = (int)Modalidade.Fundamental, Nome = "Ensino Fundamental" },
                new ModalidadeRetornoDto { Id = (int)Modalidade.Medio, Nome = "Ensino Médio" }
            };

            _mediatorMock
                .Setup(x => x.Send(
                    It.Is<ObterNovasModalidadesPorAnoQuery>(q => q.AnoLetivo == anoLetivo && q.ConsideraNovasModalidades == consideraNovasModalidades),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(novasModalidades)
                .Verifiable();

            _mediatorMock
                .Setup(x => x.Send(
                    It.Is<ObterModalidadesPorUeEAnoLetivoQuery>(q =>
                        q.CodigoUe == ueCodigo &&
                        q.AnoLetivo == anoLetivo &&
                        q.ModadlidadesQueSeraoIgnoradas.Count() == novasModalidades.Count()),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(modalidadesRetorno)
                .Verifiable();

            var resultado = await _useCase.Executar(ueCodigo, anoLetivo, consideraNovasModalidades);

            _mediatorMock.VerifyAll();
            Assert.Equal(modalidadesRetorno.Count, resultado.Count());
            Assert.Equal(modalidadesRetorno.First().Id, resultado.First().Id);
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Sem_Novas_Modalidades_Deve_Consultar_E_Filtra_Corretamente()
        {
            string ueCodigo = "123456";
            int anoLetivo = 2024;
            bool consideraNovasModalidades = false;

            var novasModalidadesVazias = new List<Modalidade>();
            var modalidadesRetorno = new List<ModalidadeRetornoDto>
            {
                new ModalidadeRetornoDto { Id = (int)Modalidade.Fundamental, Nome = "Ensino Fundamental" },
                new ModalidadeRetornoDto { Id = (int)Modalidade.Medio, Nome = "Ensino Médio" }
            };

            _mediatorMock
                .Setup(x => x.Send(
                    It.Is<ObterNovasModalidadesPorAnoQuery>(q => q.AnoLetivo == anoLetivo && q.ConsideraNovasModalidades == consideraNovasModalidades),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(novasModalidadesVazias)
                .Verifiable();

            _mediatorMock
                .Setup(x => x.Send(
                    It.Is<ObterModalidadesPorUeEAnoLetivoQuery>(q =>
                        q.CodigoUe == ueCodigo &&
                        q.AnoLetivo == anoLetivo &&
                        q.ModadlidadesQueSeraoIgnoradas.Count() == 0),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(modalidadesRetorno)
                .Verifiable();

            var resultado = await _useCase.Executar(ueCodigo, anoLetivo, consideraNovasModalidades);

            _mediatorMock.VerifyAll();
            Assert.Equal(modalidadesRetorno.Count, resultado.Count());
            Assert.Equal(modalidadesRetorno.First().Id, resultado.First().Id);
        }
    }
}
