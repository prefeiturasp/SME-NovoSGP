using Moq;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoPeriodoEscolarTeste
    {
        private readonly Mock<IRepositorioPeriodoEscolar> repositorioPeriodoEscolar;
        private readonly Mock<IRepositorioTipoCalendario> repositorioTipoCalendario;
        private readonly IServicoPeriodoEscolar servicoPeriodoEscolar;
        private readonly Mock<IUnitOfWork> unitOfWork;

        public ServicoPeriodoEscolarTeste()
        {
            repositorioPeriodoEscolar = new Mock<IRepositorioPeriodoEscolar>();
            repositorioTipoCalendario = new Mock<IRepositorioTipoCalendario>();
            unitOfWork = new Mock<IUnitOfWork>();
            servicoPeriodoEscolar = new ServicoPeriodoEscolar(unitOfWork.Object, repositorioPeriodoEscolar.Object, repositorioTipoCalendario.Object);
        }

        [Fact]
        public void Deve_Gerar_Excecao_Quando_Instanciar_Sem_Dependencia()
        {
            Assert.Throws<ArgumentNullException>(() => new ServicoPeriodoEscolar(null, repositorioPeriodoEscolar.Object, repositorioTipoCalendario.Object));
            Assert.Throws<ArgumentNullException>(() => new ServicoPeriodoEscolar(unitOfWork.Object, null, repositorioTipoCalendario.Object));
            Assert.Throws<ArgumentNullException>(() => new ServicoPeriodoEscolar(unitOfWork.Object, repositorioPeriodoEscolar.Object, null));
        }

        //[Fact]
        //public void Deve_Gerar_Excecao_Se_Adicionar_Periodo_Para_Tipo_Com_Periodo_Cadastrado()
        //{
        //    var periodos = new List<PeriodoEscolar>();
        //    long tipoCalendario = 1;

        //    repositorioTipoCalendario.Setup(x => x.ObterPorId(It.IsAny<long>())).Returns(new TipoCalendario
        //    {
        //        Id = 1,
        //        Modalidade = ModalidadeTipoCalendario.EJA,
        //        AnoLetivo = DateTime.Now.Year
        //    });

        //    repositorioPeriodoEscolar.Setup(x => x.ObterPorTipoCalendario(It.IsAny<long>())).Returns(new List<PeriodoEscolar> { new PeriodoEscolar { Id = 1 } });

        //    periodos.Add(new PeriodoEscolar
        //    {
        //        TipoCalendarioId = 1,
        //        Bimestre = 1,
        //        PeriodoFim = DateTime.Now.AddMinutes(1),
        //        PeriodoInicio = DateTime.Now
        //    });

        //    periodos.Add(new PeriodoEscolar
        //    {
        //        TipoCalendarioId = 1,
        //        Bimestre = 2,
        //        PeriodoFim = DateTime.Now.AddMinutes(3),
        //        PeriodoInicio = DateTime.Now.AddMinutes(2)
        //    });

        //    Assert.Throws<NegocioException>(() => servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario));

        //    periodos[0].Id = 1;

        //    Assert.Throws<NegocioException>(() => servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario));

        //    periodos[1].Id = 2;

        //    servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario);
        //}

        //[Fact]
        //public void Deve_Gerar_Excecao_Se_Ids_Repetidos()
        //{
        //    var periodos = new List<PeriodoEscolar>();
        //    long tipoCalendario = 1;

        //    repositorioTipoCalendario.Setup(x => x.ObterPorId(It.IsAny<long>())).Returns(new TipoCalendario
        //    {
        //        Id = 1,
        //        Modalidade = ModalidadeTipoCalendario.EJA,
        //        AnoLetivo = DateTime.Now.Year
        //    });

        //    periodos.Add(new PeriodoEscolar
        //    {
        //        Id = 1,
        //        TipoCalendarioId = 1,
        //        Bimestre = 1,
        //        PeriodoFim = DateTime.Now.AddMinutes(1),
        //        PeriodoInicio = DateTime.Now
        //    });

        //    periodos.Add(new PeriodoEscolar
        //    {
        //        Id = 1,
        //        TipoCalendarioId = 1,
        //        Bimestre = 2,
        //        PeriodoFim = DateTime.Now.AddMinutes(3),
        //        PeriodoInicio = DateTime.Now.AddMinutes(2)
        //    });

        //    Assert.Throws<NegocioException>(() => servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario));

        //    periodos[1].Id = 2;

        //    servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario);
        //}

        //[Fact]
        //public void Deve_Validar_Ano_Base()
        //{
        //    repositorioTipoCalendario.Setup(x => x.ObterPorId(It.IsAny<long>())).Returns(new TipoCalendario
        //    {
        //        Id = 1,
        //        Modalidade = ModalidadeTipoCalendario.EJA,
        //        AnoLetivo = DateTime.Now.Year - 1
        //    });

        //    long tipoCalendario = 1;
        //    var periodos = new List<PeriodoEscolar>();

        //    periodos.Add(new PeriodoEscolar
        //    {
        //        TipoCalendarioId = 1,
        //        Bimestre = 1,
        //        PeriodoFim = DateTime.Now.AddMinutes(1),
        //        PeriodoInicio = DateTime.Now
        //    });

        //    periodos.Add(new PeriodoEscolar
        //    {
        //        TipoCalendarioId = 1,
        //        Bimestre = 2,
        //        PeriodoFim = DateTime.Now.AddMinutes(3),
        //        PeriodoInicio = DateTime.Now.AddMinutes(2)
        //    });

        //    Assert.Throws<NegocioException>(() => servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario));

        //    repositorioTipoCalendario.Setup(x => x.ObterPorId(It.IsAny<long>())).Returns(new TipoCalendario
        //    {
        //        Id = 1,
        //        Modalidade = ModalidadeTipoCalendario.EJA,
        //        AnoLetivo = DateTime.Now.Year + 1
        //    });

        //    Assert.Throws<NegocioException>(() => servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario));

        //    repositorioTipoCalendario.Setup(x => x.ObterPorId(It.IsAny<long>())).Returns(new TipoCalendario
        //    {
        //        Id = 1,
        //        Modalidade = ModalidadeTipoCalendario.EJA,
        //        AnoLetivo = DateTime.Now.Year
        //    });

        //    servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario);
        //}

        [Fact]
        public void Deve_Validar_Inicio_Apos_Fim_Periodo()
        {
            repositorioTipoCalendario.Setup(x => x.ObterPorId(It.IsAny<long>())).Returns(new TipoCalendario
            {
                Id = 1,
                Modalidade = ModalidadeTipoCalendario.EJA,
                AnoLetivo = DateTime.Now.Year
            });

            long tipoCalendario = 1;
            var periodos = new List<PeriodoEscolar>();

            periodos.Add(new PeriodoEscolar
            {
                TipoCalendarioId = 1,
                Bimestre = 1,
                PeriodoFim = DateTime.Now.AddMinutes(1),
                PeriodoInicio = DateTime.Now
            });

            periodos.Add(new PeriodoEscolar
            {
                TipoCalendarioId = 1,
                Bimestre = 2,
                PeriodoFim = DateTime.Now.AddMinutes(3),
                PeriodoInicio = DateTime.Now.AddMinutes(4)
            });

            Assert.ThrowsAsync<NegocioException>(() => servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario));
        }

        //[Fact]
        //public void Deve_Validar_Inicio_Periodo_Antes_Fim_Periodo_Anterior()
        //{
        //    repositorioTipoCalendario.Setup(x => x.ObterPorId(It.IsAny<long>())).Returns(new TipoCalendario
        //    {
        //        Id = 1,
        //        Modalidade = ModalidadeTipoCalendario.EJA,
        //        AnoLetivo = DateTime.Now.Year
        //    });

        //    long tipoCalendario = 1;
        //    var periodos = new List<PeriodoEscolar>();

        //    periodos.Add(new PeriodoEscolar
        //    {
        //        TipoCalendario = 1,
        //        Bimestre = 1,
        //        PeriodoFim = DateTime.Now.AddMinutes(1),
        //        PeriodoInicio = DateTime.Now
        //    });

        //    periodos.Add(new PeriodoEscolar
        //    {
        //        TipoCalendario = 1,
        //        Bimestre = 2,
        //        PeriodoFim = DateTime.Now,
        //        PeriodoInicio = DateTime.Now.AddMinutes(4)
        //    });

        //    Assert.Throws<NegocioException>(() => servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario));

        //    periodos = new List<PeriodoEscolar>();

        //    repositorioTipoCalendario.Setup(x => x.ObterPorId(It.IsAny<long>())).Returns(new TipoCalendario
        //    {
        //        Id = 1,
        //        Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
        //        AnoLetivo = DateTime.Now.Year
        //    });

        //    periodos.Add(new PeriodoEscolar
        //    {
        //        TipoCalendario = 1,
        //        Bimestre = 1,
        //        PeriodoFim = DateTime.Now.AddMinutes(1),
        //        PeriodoInicio = DateTime.Now
        //    });

        //    periodos.Add(new PeriodoEscolar
        //    {
        //        TipoCalendario = 1,
        //        Bimestre = 2,
        //        PeriodoFim = DateTime.Now.AddMinutes(3),
        //        PeriodoInicio = DateTime.Now.AddMinutes(2)
        //    });

        //    periodos.Add(new PeriodoEscolar
        //    {
        //        TipoCalendario = 1,
        //        Bimestre = 3,
        //        PeriodoFim = DateTime.Now.AddMinutes(3),
        //        PeriodoInicio = DateTime.Now.AddMinutes(2)
        //    });

        //    periodos.Add(new PeriodoEscolar
        //    {
        //        TipoCalendario = 1,
        //        Bimestre = 4,
        //        PeriodoFim = DateTime.Now.AddMinutes(5),
        //        PeriodoInicio = DateTime.Now.AddMinutes(4)
        //    });

        //    Assert.Throws<NegocioException>(() => servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario));

        //    periodos[2].PeriodoInicio = DateTime.Now.AddMinutes(3);
        //    periodos[2].PeriodoFim = DateTime.Now.AddMinutes(3).AddSeconds(10);

        //    servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario);
        //}

        //[Fact]
        //public void Deve_Validar_Quantidade_Bimestres_EJA()
        //{
        //    var periodos = new List<PeriodoEscolar>();
        //    long tipoCalendario = 1;

        //    repositorioTipoCalendario.Setup(x => x.ObterPorId(It.IsAny<long>())).Returns(new TipoCalendario
        //    {
        //        Id = 1,
        //        Modalidade = ModalidadeTipoCalendario.EJA,
        //        AnoLetivo = DateTime.Now.Year
        //    });

        //    Assert.Throws<NegocioException>(() => servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario));

        //    periodos.Add(new PeriodoEscolar
        //    {
        //        TipoCalendario = 1,
        //        Bimestre = 1,
        //        PeriodoFim = DateTime.Now.AddMinutes(1),
        //        PeriodoInicio = DateTime.Now
        //    });

        //    Assert.Throws<NegocioException>(() => servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario));

        //    periodos.Add(new PeriodoEscolar
        //    {
        //        TipoCalendario = 1,
        //        Bimestre = 2,
        //        PeriodoFim = DateTime.Now.AddMinutes(3),
        //        PeriodoInicio = DateTime.Now.AddMinutes(2)
        //    });

        //    servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario);
        //}

        //[Fact]
        //public void Deve_Validar_Quantidade_Bimestres_Regular()
        //{
        //    var periodos = new List<PeriodoEscolar>();
        //    int tipoCalendario = 1;

        //    repositorioTipoCalendario.Setup(x => x.ObterPorId(It.IsAny<long>())).Returns(new TipoCalendario
        //    {
        //        Id = 1,
        //        Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
        //        AnoLetivo = DateTime.Now.Year
        //    });

        //    Assert.Throws<NegocioException>(() => servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario));

        //    periodos.Add(new PeriodoEscolar
        //    {
        //        TipoCalendario = 1,
        //        Bimestre = 1,
        //        PeriodoFim = DateTime.Now.AddMinutes(1),
        //        PeriodoInicio = DateTime.Now
        //    });

        //    Assert.Throws<NegocioException>(() => servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario));

        //    periodos.Add(new PeriodoEscolar
        //    {
        //        TipoCalendario = 1,
        //        Bimestre = 2,
        //        PeriodoFim = DateTime.Now.AddMinutes(3),
        //        PeriodoInicio = DateTime.Now.AddMinutes(2)
        //    });

        //    Assert.Throws<NegocioException>(() => servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario));

        //    periodos.Add(new PeriodoEscolar
        //    {
        //        TipoCalendario = 1,
        //        Bimestre = 3,
        //        PeriodoFim = DateTime.Now.AddMinutes(5),
        //        PeriodoInicio = DateTime.Now.AddMinutes(4)
        //    });

        //    Assert.Throws<NegocioException>(() => servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario));

        //    periodos.Add(new PeriodoEscolar
        //    {
        //        TipoCalendario = 1,
        //        Bimestre = 4,
        //        PeriodoFim = DateTime.Now.AddMinutes(7),
        //        PeriodoInicio = DateTime.Now.AddMinutes(6)
        //    });

        //    servicoPeriodoEscolar.SalvarPeriodoEscolar(periodos, tipoCalendario);
        //}
    }
}