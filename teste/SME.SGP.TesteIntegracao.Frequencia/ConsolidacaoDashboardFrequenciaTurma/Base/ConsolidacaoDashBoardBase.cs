
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConsolidacaoDashboardFrequenciaTurma.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;

namespace SME.SGP.TesteIntegracao.ConsolidacaoDashboardFrequenciaTurma
{
    public abstract class ConsolidacaoDashBoardBase : TesteBaseComuns
    {
        protected ConsolidacaoDashBoardBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosDentroPeriodoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosDentroPeriodoQueryHandlerFake), ServiceLifetime.Scoped));
        }    
        
        protected async Task CriarItensBasicos()
        {
            await InserirNaBase(new Dre
            {
                CodigoDre = ConstantesTeste.DRE_1_CODIGO,
                Abreviacao = ConstantesTeste.DRE_1_NOME,
                Nome = ConstantesTeste.DRE_1_NOME
            });

            await InserirNaBase(new Ue
            {
                CodigoUe = ConstantesTeste.UE_1_CODIGO,
                DreId = ConstantesTeste.DRE_1_ID,
                Nome = ConstantesTeste.UE_1_NOME,
            });

            await InserirNaBase(new Dominio.Turma
            {
                UeId = ConstantesTeste.UE_1_ID,
                Ano = ConstantesTeste.TURMA_ANO_1,
                CodigoTurma = ConstantesTeste.TURMA_CODIGO_1,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ModalidadeCodigo = Modalidade.Fundamental,
                Nome = ConstantesTeste.TURMA_NOME_1A
            });

            await InserirNaBase(new TipoCalendario()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Nome = ConstantesTeste.TIPO_CALENDARIO_NOME_1,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Situacao = true,
                Periodo = Periodo.Anual,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = ConstantesTeste.SISTEMA_NOME,
                CriadoRF = ConstantesTeste.SISTEMA_RF
            });

            await CriarPeriodoEscolarCustomizadoQuartoBimestre(true);
        }
    }
}