using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao;
using SME.SGP.TesteIntegracao.RelatorioPAP;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class Validar_periodo_escolar : TesteBaseComuns
    {
        private readonly ComandosPeriodoEscolar comandosPeriodoEscolar;
        private readonly Mock<IRepositorioPeriodoEscolar> repositorioPeriodo;
        private readonly Mock<IServicoPeriodoEscolar> servicoPeriodoEscolar;
        private readonly Mock<IMediator> mediatorMock;

        public Validar_periodo_escolar(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            servicoPeriodoEscolar = new Mock<IServicoPeriodoEscolar>();
            repositorioPeriodo = new Mock<IRepositorioPeriodoEscolar>();
            mediatorMock = new Mock<IMediator>();
            comandosPeriodoEscolar = new ComandosPeriodoEscolar(repositorioPeriodo.Object, servicoPeriodoEscolar.Object, ServiceProvider.GetService<IMediator>());
        }

        [Fact(DisplayName = "Deve_Disparar_Excecao_Ao_Instanciar_Sem_Dependencias")]
        public void Deve_Disparar_Excecao_Ao_Instanciar_Sem_Dependencias()
        {
            Assert.Throws<ArgumentNullException>(() => new ComandosPeriodoEscolar(null, servicoPeriodoEscolar.Object, mediatorMock.Object));
            Assert.Throws<ArgumentNullException>(() => new ComandosPeriodoEscolar(repositorioPeriodo.Object, null, mediatorMock.Object));
            Assert.Throws<ArgumentNullException>(() => new ComandosPeriodoEscolar(repositorioPeriodo.Object, servicoPeriodoEscolar.Object, null));
        }

        [Fact(DisplayName = "Deve_Salvar_Periodo_Escolar")]
        public async Task Deve_Salvar_Periodo_Escolar()
        {
            servicoPeriodoEscolar.Setup(x => x.SalvarPeriodoEscolar(It.IsAny<IEnumerable<PeriodoEscolar>>(), It.IsAny<long>()));

            await comandosPeriodoEscolar.Salvar(new PeriodoEscolarListaDto
            {
                TipoCalendario = 1,
                Periodos = new List<PeriodoEscolarDto>
                {
                    new PeriodoEscolarDto
                    {
                        Bimestre = 1,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia(),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(1),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 2,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddMinutes(2),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(3),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 3,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddMinutes(4),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(5),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 4,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddMinutes(6),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(7),
                    }
                }
            });
        }

        [Fact(DisplayName = "Nao_Deve_Salvar_Sem_Tipo_Calendario")]
        public async Task Nao_Deve_Salvar_Sem_Tipo_Calendario()
        {
            servicoPeriodoEscolar.Setup(x => x.SalvarPeriodoEscolar(It.IsAny<IEnumerable<PeriodoEscolar>>(), It.IsAny<long>()));

            await Assert.ThrowsAsync<NegocioException>(() =>
             comandosPeriodoEscolar.Salvar(new PeriodoEscolarListaDto
             {
                 TipoCalendario = 0,
                 Periodos = new List<PeriodoEscolarDto>
                 {
                    new PeriodoEscolarDto
                    {
                        Bimestre = 1,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia(),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(1),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 2,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddMinutes(2),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(3),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 3,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddMinutes(4),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(5),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 4,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddMinutes(6),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(7),
                    }
                 }
             }));
        }

        [Fact(DisplayName = "Deve_Salvar_Periodo_Escolar_Replicando_Periodos_Relatorios_PAP")]
        public async Task Deve_Salvar_Periodo_Escolar_Fundamental_Replicando_Periodos_Relatorios_PAP()
        {
            var comandoPeriodoEscolar = ServiceProvider.GetService<IComandosPeriodoEscolar>();
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarConfiguracaoRelatorioPAP();
            var parametro = new PeriodoEscolarListaDto
            {
                TipoCalendario = 1,
                Periodos = new List<PeriodoEscolarDto>
                {
                    new PeriodoEscolarDto
                    {
                        Bimestre = 1,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia(),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(1),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 2,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddMinutes(2),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(3),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 3,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddMinutes(4),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(5),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 4,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddMinutes(6),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(7),
                    }
                }
            };
            await comandoPeriodoEscolar.Salvar(parametro);
            var periodosRelatorioPAP = ObterTodos<PeriodoRelatorioPAP>();
            var periodosEscolaresRelatorioPAP = ObterTodos<PeriodoEscolarRelatorioPAP>();
            periodosRelatorioPAP.Count().ShouldBe(4);
            periodosEscolaresRelatorioPAP.Count().ShouldBe(4);
        }

        [Fact(DisplayName = "Deve_Salvar_Periodo_Escolar_Nao_Replicando_Periodos_Relatorios_PAP")]
        public async Task Deve_Salvar_Periodo_Escolar_Fundamental_Nao_Replicando_Periodos_Relatorios_PAP()
        {
            var comandoPeriodoEscolar = ServiceProvider.GetService<IComandosPeriodoEscolar>();
            await CriarTipoCalendario(ModalidadeTipoCalendario.Infantil);
            await CriarConfiguracaoRelatorioPAP();
            var parametro = new PeriodoEscolarListaDto
            {
                TipoCalendario = 1,
                Periodos = new List<PeriodoEscolarDto>
                {
                    new PeriodoEscolarDto
                    {
                        Bimestre = 1,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia(),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(1),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 2,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddMinutes(2),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(3),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 3,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddMinutes(4),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(5),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 4,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddMinutes(6),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(7),
                    }
                }
            };
            await comandoPeriodoEscolar.Salvar(parametro);
            var periodosRelatorioPAP = ObterTodos<PeriodoRelatorioPAP>();
            var periodosEscolaresRelatorioPAP = ObterTodos<PeriodoEscolarRelatorioPAP>();
            periodosRelatorioPAP.Count().ShouldBe(0);
            periodosEscolaresRelatorioPAP.Count().ShouldBe(0);
        }

        protected async Task CriarTipoCalendario(ModalidadeTipoCalendario tipoCalendario, bool considerarAnoAnterior = false, int semestre = SEMESTRE_1)
        {
            await InserirNaBase(new TipoCalendario
            {
                AnoLetivo = considerarAnoAnterior ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL,
                Nome = considerarAnoAnterior ? NOME_TIPO_CALENDARIO_ANO_ANTERIOR : NOME_TIPO_CALENDARIO_ANO_ATUAL,
                Periodo = Periodo.Anual,
                Modalidade = tipoCalendario,
                Situacao = true,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                Migrado = false,
                Semestre = tipoCalendario.EhEjaOuCelp() ? semestre : null
            });
        }

        protected async Task CriarConfiguracaoRelatorioPAP(bool considerarAnoAnterior = false)
        {
            var AnoLetivo = considerarAnoAnterior ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL;
            await InserirNaBase(new ConfiguracaoRelatorioPAP()
            {
                InicioVigencia = new(AnoLetivo, 01, 01),
                FimVigencia = new(AnoLetivo, 12, 31),
                TipoPeriocidade = ConstantesTestePAP.TIPO_PERIODICIDADE_BIMESTRAL,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}