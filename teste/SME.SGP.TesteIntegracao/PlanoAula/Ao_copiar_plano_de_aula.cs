using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using SME.SGP.TesteIntegracao.PlanoAula.Base;
using SME.SGP.TesteIntegracao.PlanoAula.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAula
{
    public class Ao_copiar_plano_de_aula : PlanoAulaTesteBase
    {
        public Ao_copiar_plano_de_aula(CollectionFixture collectionFixture) : base(collectionFixture){}
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAbrangenciaPorTurmaEConsideraHistoricoQuery, AbrangenciaFiltroRetorno>), typeof(ObterAbrangenciaPorTurmaEConsideraHistoricoQueryHandlerFakeFundamental6A), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));

        }

        [Fact(DisplayName = "Cópia de plano de aula para outra aula da mesma turma e componente curricular")]
        public async Task Copiar_plano_para_outra_aula_da_mesma_turma_e_componente()
        {
            
        }

        [Fact(DisplayName = "Cópia de plano de aula para outra turma com o mesmo componente curricular")]
        public async Task Copiar_plano_para_outra_turma_mesmo_componente()
        {
            
        }

        [Fact(DisplayName = "Cópia de plano de aula para outra turma e componente curricular diferente (não deve permitir)")]
        public async Task Copiar_plano_para_outra_turma_de_componente_direferente()
        {
            
        }
    }
}