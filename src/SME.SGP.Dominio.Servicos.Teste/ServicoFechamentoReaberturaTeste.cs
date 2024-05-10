using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class DatasParaTestarFechamento : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
    {
        new object[] { new List<(DateTime, DateTime)> { (new DateTime(2020, 01, 01), new DateTime(2020, 01, 10)), (new DateTime(2020, 02, 01), new DateTime(2020, 02, 10)) } , new DateTime(2020, 01, 02), new DateTime(2020, 01, 15) },
        new object[] { new List<(DateTime, DateTime)> { (new DateTime(2020, 01, 10), new DateTime(2020, 01, 20)), (new DateTime(2020, 02, 10), new DateTime(2020, 02, 20)) } , new DateTime(2020, 01, 02), new DateTime(2020, 01, 01) }
    };

        public IEnumerator<object[]> GetEnumerator()
        { return _data.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }
    }

    public class ServicoFechamentoReaberturaTeste
    {
        private readonly Mock<IComandosWorkflowAprovacao> comandosWorkflowAprovacao;
        private readonly Mock<IRepositorioEvento> repositorioEvento;
        private readonly Mock<IRepositorioEventoTipo> repositorioEventoTipo;
        private readonly Mock<IRepositorioFechamentoReabertura> repositorioFechamentoReabertura;
        private readonly Mock<IServicoEOL> servicoEOL;
        private readonly Mock<IServicoEvento> servicoEvento;
        private readonly IServicoFechamentoReabertura servicoFechamentoReabertura;
        private readonly Mock<IServicoNotificacao> servicoNotificacao;
        private readonly Mock<IServicoUsuario> servicoUsuario;
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly Mock<IRepositorioSupervisorEscolaDre> repositorioSupervisorEscolaDre;

        public ServicoFechamentoReaberturaTeste()
        {
            repositorioFechamentoReabertura = new Mock<IRepositorioFechamentoReabertura>();
            servicoUsuario = new Mock<IServicoUsuario>();

            unitOfWork = new Mock<IUnitOfWork>();
            comandosWorkflowAprovacao = new Mock<IComandosWorkflowAprovacao>();
            servicoEOL = new Mock<IServicoEOL>();
            servicoNotificacao = new Mock<IServicoNotificacao>();
            repositorioEventoTipo = new Mock<IRepositorioEventoTipo>();
            servicoEvento = new Mock<IServicoEvento>();
            repositorioEvento = new Mock<IRepositorioEvento>();
            repositorioSupervisorEscolaDre = new Mock<IRepositorioSupervisorEscolaDre>();
            servicoFechamentoReabertura = new ServicoFechamentoReabertura(repositorioFechamentoReabertura.Object, unitOfWork.Object, comandosWorkflowAprovacao.Object, servicoUsuario.Object,
                servicoEOL.Object, servicoNotificacao.Object, repositorioEventoTipo.Object, servicoEvento.Object, repositorioEvento.Object, repositorioSupervisorEscolaDre.Object);
        }

        [Theory, ClassData(typeof(DatasParaTestarFechamento))]
        public async void Deve_Validar_Periodos(IEnumerable<(DateTime, DateTime)> datas, DateTime dataInicio, DateTime dataFim)
        {
            var tipoCalendario = new TipoCalendario() { Id = 1, AnoLetivo = 2020 };

            var fechamentoReabertura = new FechamentoReabertura()
            {
                Inicio = dataInicio,
                Fim = dataFim,
                Descricao = "Teste",
                DreId = 1
            };
            fechamentoReabertura.AtualizarTipoCalendario(tipoCalendario);

            var listaBimestres = new int[] { 1, 2 };
            listaBimestres.ToList().ForEach(bimestre =>
            {
                fechamentoReabertura.Adicionar(new FechamentoReaberturaBimestre()
                {
                    Bimestre = bimestre
                });
            });

            repositorioFechamentoReabertura.Setup(a => a.Listar(fechamentoReabertura.TipoCalendarioId, null, null, null)).Returns(Task.FromResult(GeraFechamentosParaValidatarData(datas)));

            await Assert.ThrowsAsync<NegocioException>(() => servicoFechamentoReabertura.SalvarAsync(fechamentoReabertura));
        }

        private IEnumerable<FechamentoReabertura> GeraFechamentosParaValidatarData(IEnumerable<(DateTime, DateTime)> datas)
        {
            var listaRetorno = new List<FechamentoReabertura>();

            datas.ToList().ForEach(a =>
            {
                listaRetorno.Add(new FechamentoReabertura() { Inicio = a.Item1, Fim = a.Item2 });
            });

            return listaRetorno;
        }
    }
}