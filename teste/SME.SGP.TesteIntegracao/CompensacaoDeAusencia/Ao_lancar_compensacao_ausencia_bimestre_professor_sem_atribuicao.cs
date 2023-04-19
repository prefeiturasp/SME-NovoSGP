using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_lancar_compensacao_ausencia_bimestre_professor_sem_atribuicao : Ao_lancar_compensacao_ausencia_bimestre_base
    {
        public Ao_lancar_compensacao_ausencia_bimestre_professor_sem_atribuicao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>),
                typeof(ProfessorPodePersistirTurmaQueryHandlerSemPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Compensação de Ausência - Deve bloquear compensações ausência para professor sem atribuição")]
        public async Task Deve_bloquear_lancar_compensacao_ausencia_para_professor_sem_atribuicao()
        {
            var compensacaoDeAusencia = await ObterCompensacaoDeAusencia(ObterPerfilProfessor(),
                BIMESTRE_1,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                ANO_7,
                QUANTIDADE_AULAS_22,
                true,
                true,
                true,
                false);
            
            var casoDeUso = ServiceProvider.GetService<ISalvarCompensacaoAusenciaUseCase>();
            casoDeUso.ShouldNotBeNull();            
            
            var compensacaoAusenciaDosAlunos = await LancarCompensacaoAusenciasAlunos(compensacaoDeAusencia);

            async Task DoExecutarInserir()
            {
                await casoDeUso.Executar(0, compensacaoAusenciaDosAlunos);
            }

            await Should.ThrowAsync<NegocioException>(DoExecutarInserir);       
        }
    }
}