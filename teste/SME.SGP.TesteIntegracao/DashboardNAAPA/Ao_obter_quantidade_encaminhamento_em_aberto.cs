using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Autenticar;
using SME.SGP.TesteIntegracao.DashboardNAAPA.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.DashboardNAAPA
{
    public class Ao_obter_quantidade_encaminhamento_em_aberto : TesteBaseComuns
    {
        public Ao_obter_quantidade_encaminhamento_em_aberto(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            //ObterDataUltimaConsolidacaoDashboardNAAPAQueryHandlerFake
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterParametroSistemaPorTipoEAnoQuery, ParametrosSistema>), typeof(ObterParametroSistemaPorTipoEAnoQueryHandlerFakeDashNAAPA), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDataUltimaConsolicacaoDashboardNaapaQuery, DateTime?>), typeof(ObterDataUltimaConsolidacaoDashboardNAAPAQueryHandlerFake), ServiceLifetime.Scoped));
        }

            [Fact]
        public async Task Ao_obter_quantidade_encaminhamento_por_dre()
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilProfessor());

            await CriarUsuarios();

            await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Situacao = SituacaoNAAPA.AguardandoAtendimento,
                Quantidade = 4,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Situacao = SituacaoNAAPA.EmAtendimento,
                Quantidade = 4,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Situacao = SituacaoNAAPA.Rascunho,
                Quantidade = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterQuantidadeAtendimentoNAAPAEmAbertoPorDreUseCase>();
            var dto = new FiltroQuantidadeAtendimentoNAAPAEmAbertoDto() { AnoLetivo = DateTimeExtension.HorarioBrasilia().Year, DreId = DRE_ID_1 };
            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();
            retorno.TotaEncaminhamento.ShouldBe(10);
            retorno.Graficos.FirstOrDefault().Quantidade.ShouldBe(10);
        }

        [Fact]
        public async Task Ao_obter_quantidade_encaminhamento_todas_dre()
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilProfessor());

            await CriarUsuarios();

            await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Situacao = SituacaoNAAPA.AguardandoAtendimento,
                Quantidade = 4,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_2,
                Situacao = SituacaoNAAPA.EmAtendimento,
                Quantidade = 4,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterQuantidadeAtendimentoNAAPAEmAbertoPorDreUseCase>();
            var dto = new FiltroQuantidadeAtendimentoNAAPAEmAbertoDto() { AnoLetivo = DateTimeExtension.HorarioBrasilia().Year, DreId = null };
            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();
            retorno.TotaEncaminhamento.ShouldBe(8);
            var graficos = retorno.Graficos.ToList();
            graficos.Count().ShouldBe(2);
            graficos.Exists(consolidado => consolidado.Descricao == DRE_NOME_1).ShouldBeTrue();
            graficos.Exists(consolidado => consolidado.Descricao == DRE_NOME_2).ShouldBeTrue();
        }

        [Fact]
        public async Task Ao_obter_data_ultima_consolidacao_se_corresponde_ao_filtro_dash_naapa()
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilProfessor());

            await CriarUsuarios();

            await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Situacao = SituacaoNAAPA.AguardandoAtendimento,
                Quantidade = 4,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_2,
                Situacao = SituacaoNAAPA.EmAtendimento,
                Quantidade = 4,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterQuantidadeAtendimentoNAAPAEmAbertoPorDreUseCase>();
            var dto = new FiltroQuantidadeAtendimentoNAAPAEmAbertoDto() { AnoLetivo = DateTimeExtension.HorarioBrasilia().Year, DreId = null };
            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();
            var graficos = retorno.Graficos.ToList();
            graficos.Count().ShouldBe(2);
            retorno.DataUltimaConsolidacao.ShouldNotBeNull();
            retorno.DataUltimaConsolidacao.Value.Year.ShouldBe(DateTimeExtension.HorarioBrasilia().Year);
        }


        [Fact(DisplayName = "Dashboard NAAPA - Obter quantidade encaminhamento por modalidade")]
        public async Task Ao_obter_quantidade_encaminhamento_modalidade()
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilProfessor());

            await CriarUsuarios();

            await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Situacao = SituacaoNAAPA.AguardandoAtendimento,
                Quantidade = 4,
                Modalidade = Modalidade.EducacaoInfantil,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_2,
                Situacao = SituacaoNAAPA.EmAtendimento,
                Quantidade = 4,
                Modalidade = Modalidade.Fundamental,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterQuantidadeAtendimentoNAAPAEmAbertoPorDreUseCase>();
            var dto = new FiltroQuantidadeAtendimentoNAAPAEmAbertoDto() { AnoLetivo = DateTimeExtension.HorarioBrasilia().Year, DreId = null, Modalidade = Modalidade.Fundamental };
            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();
            retorno.TotaEncaminhamento.ShouldBe(4);
            var graficos = retorno.Graficos.ToList();
            graficos.Count().ShouldBe(1);
            graficos.Exists(consolidado => consolidado.Descricao == DRE_NOME_2).ShouldBeTrue();
        }
    }
}
