using System.Linq;
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
    public class Ao_lancar_compensacao_ausencia_bimestre_professor_com_atribuicao : Ao_lancar_compensacao_ausencia_bimestre_base
    {
        public Ao_lancar_compensacao_ausencia_bimestre_professor_com_atribuicao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>),
                typeof(ProfessorPodePersistirTurmaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        // TODO: Comentado devido ao problema de inclusão (Bulk Insert)
        //[Fact]
        public async Task Deve_lancar_compensacao_ausencia_para_professor_com_atribuicao()
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
            
            var comando = ServiceProvider.GetService<IComandosCompensacaoAusencia>();
            comando.ShouldNotBeNull();            
            
            var compensacaoAusenciaDosAlunos = await LancarCompensacaoAusenciasAlunos(compensacaoDeAusencia);

            await comando.Inserir(compensacaoAusenciaDosAlunos);
            
            var listaDeCompensacaoAusencia = ObterTodos<CompensacaoAusencia>();
            listaDeCompensacaoAusencia.ShouldNotBeNull();

            var listaDeCompensacaoAusenciaAluno = ObterTodos<CompensacaoAusenciaAluno>();
            listaDeCompensacaoAusenciaAluno.ShouldNotBeNull();
            
            var compensacao = listaDeCompensacaoAusencia.FirstOrDefault();
            compensacao.ShouldNotBeNull();
            
            var listaDaCompensacaoAluno = listaDeCompensacaoAusenciaAluno.FindAll(aluno => aluno.CompensacaoAusenciaId == compensacao.Id);
            listaDaCompensacaoAluno.ShouldNotBeNull(); 
            
            var compensacaoAusenciasAlunos = await ObterCompensacaoAusenciasAlunos();

            foreach (var compensacaoAusenciaAluno in compensacaoAusenciasAlunos)
            {
                var compensacaoAluno = listaDaCompensacaoAluno.Find(aluno => aluno.CodigoAluno == compensacaoAusenciaAluno.CodigoAluno);
                compensacaoAluno.ShouldNotBeNull();

                compensacaoAluno.QuantidadeFaltasCompensadas.ShouldBe(compensacaoAusenciaAluno.QdadeAula);                
            }       
        }
    }
}