using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_gravar_conselho_ano_anterior : ConselhoDeClasseTesteBase
    {
        public Ao_gravar_conselho_ano_anterior(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>), typeof(ProfessorPodePersistirTurmaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        //[Fact]
        public async Task Deve_gravar_conselho_final_sem_existencia_do_conselho_4_bimestre_fundamental()
        {
            await CriarBase(TipoNota.Nota, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, ANO_7);

            var fechamentoTurma = ObterTodos<FechamentoTurma>().FirstOrDefault();
            fechamentoTurma.ShouldNotBeNull();
            
            var conselhoClasseNota = new ConselhoClasseNotaDto
            {
                Conceito = null,
                Justificativa = "Gravar conselho classe ano anterior - fundamental.",
                Nota = NOTA_8,
                CodigoComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };

            var salvarConselhoClasseAlunoNota = new SalvarConselhoClasseAlunoNotaDto
            {
                Bimestre = BIMESTRE_FINAL,
                CodigoAluno = ALUNO_CODIGO_1,
                CodigoTurma = TURMA_CODIGO_1,
                ConselhoClasseId = 0,
                FechamentoTurmaId = fechamentoTurma.Id,
                ConselhoClasseNotaDto = conselhoClasseNota
            };
            
            var useCase = ServiceProvider.GetService<ISalvarConselhoClasseAlunoNotaUseCase>();
            useCase.ShouldNotBeNull();

            var conselhoClasseNotaRetorno = await useCase.Executar(salvarConselhoClasseAlunoNota);
            conselhoClasseNotaRetorno.ConselhoClasseId.ShouldBe(1);
            conselhoClasseNotaRetorno.FechamentoTurmaId.ShouldBe(1);
            conselhoClasseNotaRetorno.EmAprovacao.ShouldBeTrue();
        }
        
        //[Fact]
        public async Task Deve_gravar_conselho_final_sem_existencia_do_conselho_2_bimestre_eja()
        {
            await CriarBase(TipoNota.Nota, Modalidade.EJA, ModalidadeTipoCalendario.EJA, ANO_3);
            
            var fechamentoTurma = ObterTodos<FechamentoTurma>().FirstOrDefault();
            fechamentoTurma.ShouldNotBeNull();
            
            var conselhoClasseNota = new ConselhoClasseNotaDto
            {
                Conceito = null,
                Justificativa = "Gravar conselho classe ano anterior - eja.",
                Nota = NOTA_7,
                CodigoComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };

            var salvarConselhoClasseAlunoNota = new SalvarConselhoClasseAlunoNotaDto
            {
                Bimestre = BIMESTRE_FINAL,
                CodigoAluno = ALUNO_CODIGO_1,
                CodigoTurma = TURMA_CODIGO_1,
                ConselhoClasseId = 0,
                FechamentoTurmaId = fechamentoTurma.Id,
                ConselhoClasseNotaDto = conselhoClasseNota
            };
            
            var useCase = ServiceProvider.GetService<ISalvarConselhoClasseAlunoNotaUseCase>();
            useCase.ShouldNotBeNull();

            var conselhoClasseNotaRetorno = await useCase.Executar(salvarConselhoClasseAlunoNota);
            conselhoClasseNotaRetorno.ConselhoClasseId.ShouldBe(1);
            conselhoClasseNotaRetorno.FechamentoTurmaId.ShouldBe(1);
            conselhoClasseNotaRetorno.EmAprovacao.ShouldBeTrue();            
        }

        private async Task CriarBase(TipoNota tipoNota, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario,
            string anoTurma)
        {
            var dataAula = DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1);
            
            var filtroConselhoClasse = new FiltroConselhoClasseDto
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = modalidade,
                TipoCalendario = tipoCalendario,
                Bimestre = BIMESTRE_FINAL,
                AnoTurma = anoTurma,
                TipoNota = tipoNota,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento,
                ConsiderarAnoAnterior = true,
                ComponenteCurricular = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                CriarPeriodoAbertura = false,
                CriarPeriodoReabertura = true,
                DataAula = dataAula,
                CriarPeriodoEscolar = true,
                CriarFechamentoDisciplinaAlunoNota = true
            };

            await CriarDadosBase(filtroConselhoClasse);
        }
    }
}