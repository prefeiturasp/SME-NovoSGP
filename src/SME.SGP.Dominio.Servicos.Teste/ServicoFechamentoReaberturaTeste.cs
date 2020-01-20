using Moq;
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
        new object[] { new List<(DateTime, DateTime)> { (new DateTime(2020, 01, 01), new DateTime(2020, 01, 10)), (new DateTime(2020, 02, 01), new DateTime(2020, 02, 10)) } , new DateTime(2020, 01, 02), new DateTime(2020, 01, 03) }
    };

        public IEnumerator<object[]> GetEnumerator()
        { return _data.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }
    }

    public class ServicoFechamentoReaberturaTeste
    {
        private readonly Mock<IRepositorioFechamentoReabertura> repositorioFechamentoReabertura;
        private readonly IServicoFechamentoReabertura servicoFechamentoReabertura;

        public ServicoFechamentoReaberturaTeste()
        {
            repositorioFechamentoReabertura = new Mock<IRepositorioFechamentoReabertura>();
            servicoFechamentoReabertura = new ServicoFechamentoReabertura(repositorioFechamentoReabertura.Object);
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

            repositorioFechamentoReabertura.Setup(a => a.Listar(fechamentoReabertura.TipoCalendarioId, null, null)).Returns(Task.FromResult(GeraFechamentosParaValidatarData(datas)));

            await Assert.ThrowsAsync<NegocioException>(() => servicoFechamentoReabertura.Salvar(fechamentoReabertura));
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