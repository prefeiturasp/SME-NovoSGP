using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_listar_opcoes_impressao : ConselhoDeClasseTesteBase
    {
        public Ao_listar_opcoes_impressao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_listar_4_bimestres_para_modalidade_do_ensino_fundamental_e_medio()
        {
            await CriarDados(COMPONENTE_LINGUA_PORTUGUESA_ID_138, ANO_8, Modalidade.Medio,
                ModalidadeTipoCalendario.FundamentalMedio);
            
            var useCase = ServiceProvider.GetService<IObterBimestresComConselhoClasseTurmaUseCase>();
            useCase.ShouldNotBeNull();

            var turmas = ObterTodos<Turma>();
            turmas.ShouldNotBeNull();

            var turma = turmas.FirstOrDefault(c => c.CodigoTurma == TURMA_CODIGO_1);
            turma.ShouldNotBeNull();
            
            var retorno = (await useCase.Executar(turma.Id))
                .Where(c => c.Bimestre != 0)
                .GroupBy(c => c.Bimestre);
            
            retorno.Count().ShouldBe(4);
        }
        
        [Fact]
        public async Task Deve_listar_2_bimestres_para_eja()
        {
            await CriarDados(COMPONENTE_LINGUA_PORTUGUESA_ID_138, ANO_4, Modalidade.EJA,
                ModalidadeTipoCalendario.EJA);
            
            var useCase = ServiceProvider.GetService<IObterBimestresComConselhoClasseTurmaUseCase>();
            useCase.ShouldNotBeNull();

            var turmas = ObterTodos<Turma>();
            turmas.ShouldNotBeNull();

            var turma = turmas.FirstOrDefault(c => c.CodigoTurma == TURMA_CODIGO_1);
            turma.ShouldNotBeNull();
            
            var retorno = (await useCase.Executar(turma.Id))
                .Where(c => c.Bimestre != 0)
                .GroupBy(c => c.Bimestre);
            
            retorno.Count().ShouldBe(2);
        }

        [Fact]
        public async Task Deve_exibir_opcao_final_apos_inicio_ultimo_bimestre()
        {
            await CriarDados(COMPONENTE_LINGUA_PORTUGUESA_ID_138, ANO_8, Modalidade.Medio,
                ModalidadeTipoCalendario.FundamentalMedio);
            
            var useCase = ServiceProvider.GetService<IObterBimestresComConselhoClasseTurmaUseCase>();
            useCase.ShouldNotBeNull();

            var turmas = ObterTodos<Turma>();
            turmas.ShouldNotBeNull();

            var turma = turmas.FirstOrDefault(c => c.CodigoTurma == TURMA_CODIGO_1);
            turma.ShouldNotBeNull();
            
            var retorno = (await useCase.Executar(turma.Id)).Where(c => c.Bimestre == 0);
            retorno.Any().ShouldBeTrue();
        }
        
        private async Task CriarDados(
            string componenteCurricular,
            string anoTurma, 
            Modalidade modalidade,
            ModalidadeTipoCalendario modalidadeTipoCalendario)
        {            
            var filtroNota = new FiltroNotasDto
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                ComponenteCurricular = componenteCurricular,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = false,
                DataAula = DATA_03_01_INICIO_BIMESTRE_1,
                TipoNota = TipoNota.Nota,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento,
                CriarFechamentoDisciplinaAlunoNota = true,
                CriarConselhosTodosBimestres = true
            };
            
            await CriarDadosBase(filtroNota);
        }        
    }
}