using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConsolidadoPedagogico
{
    public class Ao_realizar_consolidado_registro_pedagogico : TesteBaseComuns
    {
        public Ao_realizar_consolidado_registro_pedagogico(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_executar_consolidado_registro_pedagogico()
        {
            await CriarItensBasicos();
            var useCase = ServiceProvider.GetService<IConsolidarRegistrosPedagogicosUseCase>();

            await useCase.Executar(null);
            var anoAnterior = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year;
            var parametros = ObterTodos<ParametrosSistema>();
            parametros.ShouldNotBeNull();
            var parametroAnterior = parametros.FirstOrDefault(c => c.Ano == anoAnterior);
            parametroAnterior.ShouldNotBeNull();
            parametroAnterior.Valor.ShouldBe(new DateTime(anoAnterior, 1, 1).ToString());
            var consolidadoAtual = parametros.FirstOrDefault(c => c.Ano == anoAnterior);
            consolidadoAtual.ShouldNotBeNull();
            consolidadoAtual.Valor.ShouldNotBe(new DateTime(DateTimeExtension.HorarioBrasilia().Year, 1, 1).ToString());
        }

        private async Task CriarItensBasicos()
        {
            await InserirNaBase(new Dre
            {
                Id = 1,
                CodigoDre = "1"
            });

            await InserirNaBase(new Ue
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 1,
                Nome = "1A",
                CodigoTurma = "1",
                Ano = "1",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 1
            });

            await CriarTipoCalendario(ModalidadeTipoCalendario.Infantil);
            await CriarPeriodoEscolarReabertura(TIPO_CALENDARIO_1);
            await CriarParametro(DateTimeExtension.HorarioBrasilia().Year);
            await CriarParametro(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year);
            await CriarComponenteCurricular();
            await InserirNaBase(new ConsolidacaoRegistrosPedagogicos
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year,
                TurmaId= 1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                PeriodoEscolarId= 1,
                QuantidadeAulas= 2,
                FrequenciasPendentes= 1
            });
        }

        private async Task CriarParametro(int ano)
        {
            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "ExecucaoConsolidacaoRegistrosPedagogicos",
                Descricao = "Data da última execução da rotina de consolidação de registros pedagógicos",
                Tipo = TipoParametroSistema.ExecucaoConsolidacaoRegistrosPedagogicos,
                Valor = new DateTime(ano, 1, 1).ToString(),
                Ano = ano,
                Ativo = true,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });
        }
    }
}
