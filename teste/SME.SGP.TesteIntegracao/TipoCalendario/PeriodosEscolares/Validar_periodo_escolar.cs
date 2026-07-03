using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Dominio;
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
    [Collection("TesteIntegradoSGP")]
    public class Validar_periodo_escolar : TesteBaseComuns
    {
        public Validar_periodo_escolar(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Deve_Salvar_Periodo_Escolar_Replicando_Periodos_Relatorios_PAP")]
        public async Task Deve_Salvar_Periodo_Escolar_Fundamental_Replicando_Periodos_Relatorios_PAP()
        {
            var comandoPeriodoEscolar = ServiceProvider.GetService<IComandosPeriodoEscolar>();

            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarConfiguracaoRelatorioPAP();

            var parametro = MontarParametroPadrao();

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

            var parametro = MontarParametroPadrao();

            await comandoPeriodoEscolar.Salvar(parametro);

            var periodosRelatorioPAP = ObterTodos<PeriodoRelatorioPAP>();
            var periodosEscolaresRelatorioPAP = ObterTodos<PeriodoEscolarRelatorioPAP>();

            periodosRelatorioPAP.Count().ShouldBe(0);
            periodosEscolaresRelatorioPAP.Count().ShouldBe(0);
        }

        private static PeriodoEscolarListaDto MontarParametroPadrao()
        {
            return new PeriodoEscolarListaDto
            {
                TipoCalendario = 1,
                Periodos = new List<PeriodoEscolarDto>
                {
                    new() { Bimestre = 1, PeriodoInicio = DateTime.SpecifyKind(DateTimeExtension.HorarioBrasilia(), DateTimeKind.Utc), PeriodoFim = DateTime.SpecifyKind(DateTimeExtension.HorarioBrasilia().AddMinutes(1), DateTimeKind.Utc) },
                    new() { Bimestre = 2, PeriodoInicio = DateTime.SpecifyKind(DateTimeExtension.HorarioBrasilia().AddMinutes(2), DateTimeKind.Utc), PeriodoFim = DateTime.SpecifyKind(DateTimeExtension.HorarioBrasilia().AddMinutes(3), DateTimeKind.Utc) },
                    new() { Bimestre = 3, PeriodoInicio = DateTime.SpecifyKind(DateTimeExtension.HorarioBrasilia().AddMinutes(4), DateTimeKind.Utc), PeriodoFim = DateTime.SpecifyKind(DateTimeExtension.HorarioBrasilia().AddMinutes(5), DateTimeKind.Utc) },
                    new() { Bimestre = 4, PeriodoInicio = DateTime.SpecifyKind(DateTimeExtension.HorarioBrasilia().AddMinutes(6), DateTimeKind.Utc), PeriodoFim = DateTime.SpecifyKind(DateTimeExtension.HorarioBrasilia().AddMinutes(7), DateTimeKind.Utc) }
                }
            };
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
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                Migrado = false,
                Semestre = tipoCalendario.EhEjaOuCelp() ? semestre : null
            });
        }

        protected async Task CriarConfiguracaoRelatorioPAP(bool considerarAnoAnterior = false)
        {
            var anoLetivo = considerarAnoAnterior ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL;
            var inicioVigenciaUtc = new DateTime(anoLetivo, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            var fimVigenciaUtc = new DateTime(anoLetivo, 12, 31, 23, 59, 59, DateTimeKind.Utc);
            await InserirNaBase(new ConfiguracaoRelatorioPAP
            {
                InicioVigencia = inicioVigenciaUtc,
                FimVigencia = fimVigenciaUtc,
                TipoPeriocidade = ConstantesTestePAP.TIPO_PERIODICIDADE_BIMESTRAL,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}