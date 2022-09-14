using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.PlanoAula.Base;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class PlanoAEETesteBase : TesteBaseComuns
    {
        public PlanoAEETesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorDreEolQuery, IEnumerable<UsuarioEolRetornoDto>>),
                typeof(ObterFuncionariosPorDreEolQueryHandlerFake), ServiceLifetime.Scoped));
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>),
                typeof(ObterAlunoPorCodigoEolQueryHandlerFake), ServiceLifetime.Scoped));
            
        }
        
        protected ISalvarPlanoAEEUseCase ObterServicoSalvarPlanoAEEUseCase()
        {
            return ServiceProvider.GetService<ISalvarPlanoAEEUseCase>();
        }
        
        protected IObterPlanosAEEUseCase ObterServicoObterPlanosAEEUseCase()
        {
            return ServiceProvider.GetService<IObterPlanosAEEUseCase>();
        }
        
        protected IObterPlanoAEEPorIdUseCase ObterServicoObterPlanoAEEPorIdUseCase()
        {
            return ServiceProvider.GetService<IObterPlanoAEEPorIdUseCase>();
        }
        
        protected IObterQuestoesPlanoAEEPorVersaoUseCase ObterServicoObterQuestoesPlanoAEEPorVersaoUseCase()
        {
            return ServiceProvider.GetService<IObterQuestoesPlanoAEEPorVersaoUseCase>();
        }
        
        protected IVerificarExistenciaPlanoAEEPorEstudanteUseCase ObterServicoVerificarExistenciaPlanoAEEPorEstudanteUseCase()
        {
            return ServiceProvider.GetService<IVerificarExistenciaPlanoAEEPorEstudanteUseCase>();
        }
        
        protected IObterRestruturacoesPlanoAEEPorIdUseCase ObterServicoObterRestruturacoesPlanoAEEPorIdUseCase()
        {
            return ServiceProvider.GetService<IObterRestruturacoesPlanoAEEPorIdUseCase>();
        }
        
        protected IObterVersoesPlanoAEEUseCase ObterServicoObterVersoesPlanoAEEUseCase()
        {
            return ServiceProvider.GetService<IObterVersoesPlanoAEEUseCase>();
        }
        
        protected IObterParecerPlanoAEEPorIdUseCase ObterServicoObterParecerPlanoAEEPorIdUseCase()
        {
            return ServiceProvider.GetService<IObterParecerPlanoAEEPorIdUseCase>();
        }
        
        protected IEncerrarPlanoAEEUseCase ObterServicoEncerrarPlanoAEEUseCase()
        {
            return ServiceProvider.GetService<IEncerrarPlanoAEEUseCase>();
        }
        
        protected ICadastrarParecerCPPlanoAEEUseCase ObterServicoCadastrarParecerCPPlanoAEEUseCase()
        {
            return ServiceProvider.GetService<ICadastrarParecerCPPlanoAEEUseCase>();
        }
        
        protected ICadastrarParecerPAAIPlanoAEEUseCase ObterServicoCadastrarParecerPAAIPlanoAEEUseCase()
        {
            return ServiceProvider.GetService<ICadastrarParecerPAAIPlanoAEEUseCase>();
        }
        
        protected IAtribuirResponsavelPlanoAEEUseCase ObterServicoAtribuirResponsavelPlanoAEEUseCase()
        {
            return ServiceProvider.GetService<IAtribuirResponsavelPlanoAEEUseCase>();
        }
        
        protected IDevolverPlanoAEEUseCase ObterServicoDevolverPlanoAEEUseCase()
        {
            return ServiceProvider.GetService<IDevolverPlanoAEEUseCase>();
        }
        
        protected IAtribuirResponsavelGeralDoPlanoUseCase ObterServicoAtribuirResponsavelGeralDoPlanoUseCase()
        {
            return ServiceProvider.GetService<IAtribuirResponsavelGeralDoPlanoUseCase>();
        }
        
        protected IObterPlanoAEEPorCodigoEstudanteUseCase ObterServicoObterPlanoAEEPorCodigoEstudanteUseCase()
        {
            return ServiceProvider.GetService<IObterPlanoAEEPorCodigoEstudanteUseCase>();
        }
        
        protected IObterPlanoAEEObservacaoUseCase ObterServicoObterPlanoAEEObservacaoUseCase()
        {
            return ServiceProvider.GetService<IObterPlanoAEEObservacaoUseCase>();
        }
        
        protected ICriarPlanoAEEObservacaoUseCase ObterServicoCriarPlanoAEEObservacaoUseCase()
        {
            return ServiceProvider.GetService<ICriarPlanoAEEObservacaoUseCase>();
        }
        protected IAlterarPlanoAEEObservacaoUseCase ObterServicoAlterarPlanoAEEObservacaoUseCase()
        {
            return ServiceProvider.GetService<IAlterarPlanoAEEObservacaoUseCase>();
        }
        protected IExcluirPlanoAEEObservacaoUseCase ObterServicoExcluirPlanoAEEObservacaoUseCase()
        {
            return ServiceProvider.GetService<IExcluirPlanoAEEObservacaoUseCase>();
        }

        protected async Task CriarDadosBasicos(FiltroPlanoAee filtroPlanoAee)
        {
            await CriarTipoCalendario(filtroPlanoAee.TipoCalendario);
            
            await CriarDreUePerfil();

            await CriarPeriodoEscolarTodosBimestres();
            
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
    }
}
