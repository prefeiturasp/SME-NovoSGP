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
    public class Ao_lancar_compensacao_ausencia_bimestre : Ao_lancar_compensacao_ausencia_bimestre_base
    {
        public Ao_lancar_compensacao_ausencia_bimestre(CollectionFixture collectionFixture) : base(collectionFixture)
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
        public async Task Deve_lancar_compensacao_ausencia_bimestre_encerrado_sem_reabertura()
        {
            var compensacaoDeAusencia = await ObterCompensacaoDeAusencia(ObterPerfilProfessor(),
                BIMESTRE_1,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                ANO_7,
                QUANTIDADE_AULAS_22,
                true,
                false,
                true,
                true);
            
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
                var compensacaoAluno = listaDaCompensacaoAluno.Find(aluno => aluno.CodigoAluno == compensacaoAusenciaAluno.Item1);
                compensacaoAluno.ShouldNotBeNull();

                compensacaoAluno.QuantidadeFaltasCompensadas.ShouldBe(compensacaoAusenciaAluno.Item2);                
            }            
        }

        [Fact]
        public async Task Deve_bloquear_lancar_compensacao_ausencia_ano_anterior_sem_reabertura_periodo()
        {
            var compensacaoDeAusencia = await ObterCompensacaoDeAusencia(ObterPerfilProfessor(),
                BIMESTRE_1,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                ANO_7,
                QUANTIDADE_AULAS_22,
                true,
                false,
                false,
                true);
            
            var comando = ServiceProvider.GetService<IComandosCompensacaoAusencia>();
            comando.ShouldNotBeNull();            
            
            var compensacaoAusenciaDosAlunos = await LancarCompensacaoAusenciasAlunos(compensacaoDeAusencia);

            async Task DoExecutarInserir()
            {
                await comando.Inserir(compensacaoAusenciaDosAlunos);
            }

            await Should.ThrowAsync<NegocioException>(DoExecutarInserir);
        }
        
        // TODO: Comentado devido ao problema de inclusão (Bulk Insert)
        //[Fact]
        public async Task Deve_lancar_compensacao_ausencia_ano_anterior_com_reabertura_periodo()
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
                true);
            
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
                var compensacaoAluno = listaDaCompensacaoAluno.Find(aluno => aluno.CodigoAluno == compensacaoAusenciaAluno.Item1);
                compensacaoAluno.ShouldNotBeNull();

                compensacaoAluno.QuantidadeFaltasCompensadas.ShouldBe(compensacaoAusenciaAluno.Item2);                
            }
        }
        
        [Fact]
        public async Task Deve_bloquear_lancar_compensacao_ausencia_componente_que_nao_lanca_frequencia()
        {
            var compensacaoDeAusencia = await ObterCompensacaoDeAusencia(ObterPerfilProfessor(),
                BIMESTRE_1,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_AULA_COMPARTILHADA.ToString(),
                ANO_7,
                QUANTIDADE_AULAS_22,
                true,
                false,
                true,
                false);
            
            var comando = ServiceProvider.GetService<IComandosCompensacaoAusencia>();
            comando.ShouldNotBeNull();            
            
            var compensacaoAusenciaDosAlunos = await LancarCompensacaoAusenciasAlunos(compensacaoDeAusencia);

            async Task DoExecutarInserir()
            {
                await comando.Inserir(compensacaoAusenciaDosAlunos);
            }

            await Should.ThrowAsync<NegocioException>(DoExecutarInserir);
        }
    }
}