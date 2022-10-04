using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.RegistroIndividual.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;

namespace SME.SGP.TesteIntegracao.RegistroIndividual
{
    public abstract class RegistroIndividualTesteBase : TesteBaseComuns
    {
        public RegistroIndividualTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
        
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<MoverArquivosTemporariosCommand, string>),
                 typeof(MoverArquivosTemporariosCommandHandlerFake), ServiceLifetime.Scoped));
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarAtualizacaoPendenciaRegistroIndividualCommand>),
                typeof(PublicarAtualizacaoPendenciaRegistroIndividualCommandHandlerFake), ServiceLifetime.Scoped));
        }
        protected IInserirRegistroIndividualUseCase ObterServicoInserirRegistroIndividualUseCase()
        {
            return ServiceProvider.GetService<IInserirRegistroIndividualUseCase>();
        }
        protected IAlterarRegistroIndividualUseCase ObterServicoAlterarRegistroIndividualUseCase()
        {
            return ServiceProvider.GetService<IAlterarRegistroIndividualUseCase>();
        }

        protected IExcluirRegistroIndividualUseCase ObterServicoExcluirRegistroUseCase()
        {
            return ServiceProvider.GetService<IExcluirRegistroIndividualUseCase>();
        }

        protected IObterRegistroIndividualPorAlunoDataUseCase ObterServicoListarRegistroIndividualUseCase()
        {
            return ServiceProvider.GetService<IObterRegistroIndividualPorAlunoDataUseCase>();
        }

        protected async Task CriarDadosBasicos(FiltroRegistroIndividualDto filtroPlanoAee)
        {
            await CriarTipoCalendario(filtroPlanoAee.TipoCalendario);

            await CriarDreUePerfil();

            if(filtroPlanoAee.BimestreEncerrado) 
                await CriarPeriodoEscolarTodosBimestresEncerrados(); 
            else await CriarPeriodoEscolarTodosBimestres();

            await CriarComponenteCurricular();

            CriarClaimUsuario(filtroPlanoAee.Perfil);

            await CriarUsuarios();

            await CriarTurma(filtroPlanoAee.Modalidade);
        }

        protected async Task CriarPeriodoEscolarTodosBimestres()
        {
            await CriarPeriodoEscolar(DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);
        }
        protected async Task CriarPeriodoEscolarTodosBimestresEncerrados()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1_ANO_ANTERIOR, DATA_29_04_FIM_BIMESTRE_1_ANO_ANTERIOR, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2_ANO_ANTERIOR, DATA_08_07_FIM_BIMESTRE_2_ANO_ANTERIOR, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3_ANO_ANTERIOR, DATA_30_09_FIM_BIMESTRE_3_ANO_ANTERIOR, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4_ANO_ANTERIOR, DATA_22_12_FIM_BIMESTRE_4_ANO_ANTERIOR, BIMESTRE_4);
        }

        protected IInserirRegistroIndividualUseCase InserirRegistroIndividualUseCase()
        {
            return ServiceProvider.GetService<IInserirRegistroIndividualUseCase>();
        }

    }
}
