using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoDeNotasConceitosTeste
    {
        private readonly Mock<IConsultasAbrangencia> consultasAbrangencia;
        private readonly Mock<IRepositorioAtividadeAvaliativa> repositorioAtividadeAvaliativa;
        private readonly Mock<IRepositorioCiclo> repositorioCiclo;
        private readonly Mock<IRepositorioConceito> repositorioConceito;
        private readonly Mock<IRepositorioNotaParametro> repositorioNotaParametro;
        private readonly Mock<IRepositorioNotasConceitos> repositorioNotasConceitos;
        private readonly Mock<IRepositorioNotaTipoValor> repositorioNotaTipoValor;
        private readonly ServicoDeNotasConceitos servicoDeNotasConceitos;
        private readonly Mock<IServicoEOL> servicoEOL;
        private readonly Mock<IUnitOfWork> unitOfWork;

        public ServicoDeNotasConceitosTeste()
        {
            consultasAbrangencia = new Mock<IConsultasAbrangencia>();
            repositorioAtividadeAvaliativa = new Mock<IRepositorioAtividadeAvaliativa>();
            repositorioCiclo = new Mock<IRepositorioCiclo>();
            repositorioConceito = new Mock<IRepositorioConceito>();
            repositorioNotaParametro = new Mock<IRepositorioNotaParametro>();
            repositorioNotaTipoValor = new Mock<IRepositorioNotaTipoValor>();
            servicoEOL = new Mock<IServicoEOL>();
            repositorioNotasConceitos = new Mock<IRepositorioNotasConceitos>();
            unitOfWork = new Mock<IUnitOfWork>();

            servicoDeNotasConceitos = new ServicoDeNotasConceitos(
                repositorioAtividadeAvaliativa.Object,
                servicoEOL.Object,
                consultasAbrangencia.Object,
                repositorioNotaTipoValor.Object,
                repositorioCiclo.Object,
                repositorioConceito.Object,
                repositorioNotaParametro.Object,
                repositorioNotasConceitos.Object,
                unitOfWork.Object);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Se_Instanciar_Sem_Dependencias()
        {
            Assert.Throws<ArgumentNullException>(() => new ServicoDeNotasConceitos(null, servicoEOL.Object, consultasAbrangencia.Object, repositorioNotaTipoValor.Object, repositorioCiclo.Object, repositorioConceito.Object, repositorioNotaParametro.Object, repositorioNotasConceitos.Object, unitOfWork.Object));

            Assert.Throws<ArgumentNullException>(() => new ServicoDeNotasConceitos(repositorioAtividadeAvaliativa.Object, null, consultasAbrangencia.Object, repositorioNotaTipoValor.Object, repositorioCiclo.Object, repositorioConceito.Object, repositorioNotaParametro.Object, repositorioNotasConceitos.Object, unitOfWork.Object));

            Assert.Throws<ArgumentNullException>(() => new ServicoDeNotasConceitos(repositorioAtividadeAvaliativa.Object, servicoEOL.Object, null, repositorioNotaTipoValor.Object, repositorioCiclo.Object, repositorioConceito.Object, repositorioNotaParametro.Object, repositorioNotasConceitos.Object, unitOfWork.Object));

            Assert.Throws<ArgumentNullException>(() => new ServicoDeNotasConceitos(repositorioAtividadeAvaliativa.Object, servicoEOL.Object, consultasAbrangencia.Object, null, repositorioCiclo.Object, repositorioConceito.Object, repositorioNotaParametro.Object, repositorioNotasConceitos.Object, unitOfWork.Object));

            Assert.Throws<ArgumentNullException>(() => new ServicoDeNotasConceitos(repositorioAtividadeAvaliativa.Object, servicoEOL.Object, consultasAbrangencia.Object, repositorioNotaTipoValor.Object, null, repositorioConceito.Object, repositorioNotaParametro.Object, repositorioNotasConceitos.Object, unitOfWork.Object));

            Assert.Throws<ArgumentNullException>(() => new ServicoDeNotasConceitos(repositorioAtividadeAvaliativa.Object, servicoEOL.Object, consultasAbrangencia.Object, repositorioNotaTipoValor.Object, repositorioCiclo.Object, null, repositorioNotaParametro.Object, repositorioNotasConceitos.Object, unitOfWork.Object));

            Assert.Throws<ArgumentNullException>(() => new ServicoDeNotasConceitos(repositorioAtividadeAvaliativa.Object, servicoEOL.Object, consultasAbrangencia.Object, repositorioNotaTipoValor.Object, repositorioCiclo.Object, repositorioConceito.Object, null, repositorioNotasConceitos.Object, unitOfWork.Object));

            Assert.Throws<ArgumentNullException>(() => new ServicoDeNotasConceitos(repositorioAtividadeAvaliativa.Object, servicoEOL.Object, consultasAbrangencia.Object, repositorioNotaTipoValor.Object, repositorioCiclo.Object, repositorioConceito.Object, repositorioNotaParametro.Object, null, unitOfWork.Object));

            Assert.Throws<ArgumentNullException>(() => new ServicoDeNotasConceitos(repositorioAtividadeAvaliativa.Object, servicoEOL.Object, consultasAbrangencia.Object, repositorioNotaTipoValor.Object, repositorioCiclo.Object, repositorioConceito.Object, repositorioNotaParametro.Object, repositorioNotasConceitos.Object, null));
        }
    }
}