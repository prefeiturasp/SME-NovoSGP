using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_alterar_compensacao_ausencia_pelo_professor_titular : Ao_lancar_compensacao_de_ausencia_base
    {
        public Ao_alterar_compensacao_ausencia_pelo_professor_titular(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRepositorioCache), typeof(RepositorioCacheFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQuery, IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto>>), typeof(ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));            
        }

        [Fact]
        public async Task Deve_adicionar_aluno_na_compensacao_sem_aulas_selecionadas()
        {
            var dtoDadoBase = ObtenhaDtoDadoBase(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await ExecuteInserirCompensasaoAusenciaSemAulasSelecionadas(dtoDadoBase);
        }

        [Fact]
        public async Task Deve_adicionar_aluno_na_compensacao_com_aulas_selecionadas()
        {
            var dtoDadoBase = ObtenhaDtoDadoBase(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await ExecuteInserirCompensasaoAusenciaComAulasSelecionadas(dtoDadoBase);
        }

        [Fact]
        public async Task Deve_alterar_quantidade_ausencias_compensadas()
        {
            await ExecutarTesteAlterar(ObterPerfilDiretor());
        }

        [Fact]
        public async Task Deve_remover_aluno_na_compensacao()
        {
            await ExecutarTesteRemoverAlunoCompensacao(ObterPerfilDiretor());
        }

        private async Task ExecutarTesteRemoverAlunoCompensacao(string perfil)
        {
            var dtoDadoBase = ObtenhaDtoDadoBase(perfil, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await CriarDadosBase(dtoDadoBase);
            await CriaFrequenciaAlunos(dtoDadoBase);
            await CriaCompensacaoAusencia(dtoDadoBase);
            await CriaCompensacaoAusenciaAluno();
            await CriaRegistroDeFrequencia();

            var comando = ServiceProvider.GetService<IExcluirCompensacaoAusenciaUseCase>();
            var listaIds = new long[] { COMPENSACAO_AUSENCIA_ID_1 };

            await comando.Executar(listaIds);

            var compensacaoAusenciaAlunos = ObterTodos<CompensacaoAusenciaAluno>();
            compensacaoAusenciaAlunos.ForEach(x => x.Excluido.ShouldBeTrue());
        }

        private async Task CriaCompensacaoAusenciaAluno()
        {
            await CriaCompensacaoAusenciaAluno(
                    CODIGO_ALUNO_1,
                    QUANTIDADE_AULA);

            await CriaCompensacaoAusenciaAluno(
                    CODIGO_ALUNO_2,
                    QUANTIDADE_AULA_3);

            await CriaCompensacaoAusenciaAluno(
                    CODIGO_ALUNO_3,
                    QUANTIDADE_AULA_2);

            await CriaCompensacaoAusenciaAluno(
                    CODIGO_ALUNO_4,
                    QUANTIDADE_AULA);
        }

        private async Task CriaRegistroDeFrequencia()
        {
            await CrieRegistroDeFrenquencia();
            await RegistroFrequenciaAluno(CODIGO_ALUNO_1, QUANTIDADE_AULA, TipoFrequencia.F);
            await RegistroFrequenciaAluno(CODIGO_ALUNO_2, QUANTIDADE_AULA_3, TipoFrequencia.F);
            await RegistroFrequenciaAluno(CODIGO_ALUNO_3, QUANTIDADE_AULA_2, TipoFrequencia.F);
            await RegistroFrequenciaAluno(CODIGO_ALUNO_4, QUANTIDADE_AULA, TipoFrequencia.F);
        }

        private async Task CriaFrequenciaAlunos(CompensacaoDeAusenciaDBDto dtoDadoBase)
        {
            await CriaFrequenciaAlunos(
            dtoDadoBase,
            CODIGO_ALUNO_1,
            QUANTIDADE_AULA_3,
            QUANTIDADE_AULA,
            QUANTIDADE_AULA);

            await CriaFrequenciaAlunos(
            dtoDadoBase,
            CODIGO_ALUNO_2,
            QUANTIDADE_AULA,
            QUANTIDADE_AULA_3,
            QUANTIDADE_AULA_2);

            await CriaFrequenciaAlunos(
            dtoDadoBase,
            CODIGO_ALUNO_3,
            QUANTIDADE_AULA_2,
            QUANTIDADE_AULA_2,
            QUANTIDADE_AULA);

            await CriaFrequenciaAlunos(
            dtoDadoBase,
            CODIGO_ALUNO_4,
            QUANTIDADE_AULA_3,
            QUANTIDADE_AULA,
            QUANTIDADE_AULA);
        }

        private async Task CriaFrequenciaAlunos(CompensacaoDeAusenciaDBDto dtoDadoBase, string codigoAluno, int totalPresenca, int totalAusencia, int totalCompensacao)
        {
            await CriaFrequenciaAluno(
                dtoDadoBase,
                DATA_25_07_INICIO_BIMESTRE_3,
                DATA_30_09_FIM_BIMESTRE_3,
                codigoAluno,
                totalPresenca,
                totalAusencia,
                PERIODO_ESCOLAR_ID_3,
                totalCompensacao);
        }

        private CompensacaoDeAusenciaDBDto ObtenhaDtoDadoBase(string perfil, string componente)
        {
            return new CompensacaoDeAusenciaDBDto()
            {
                Perfil = perfil,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_3,
                ComponenteCurricular = componente,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                AnoTurma = ANO_5,
                DataReferencia = DATA_25_07_INICIO_BIMESTRE_3,
                QuantidadeAula = QUANTIDADE_AULA_4
            };
        }

        private async Task ExecutarTesteAlterar(string perfil)
        {
            var dtoDadoBase = ObterDtoDadoBase(perfil, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await CriarDadosBase(dtoDadoBase);
            await CriaFrequenciaAlunos(dtoDadoBase);
            await CriaRegistroDeFrequencia();

            var comando = ServiceProvider.GetService<ISalvarCompensasaoAusenciaUseCase>();
            var dto = ObtenhaCompensacaoAusenciaDto(dtoDadoBase, ObtenhaListaDeAlunosSemAulasSelecionadas());

            await comando.Executar(0, dto);
            var compensacaoAusencias = ObterTodos<CompensacaoAusencia>();
            var compensacaoAusenciaAlunos = ObterTodos<CompensacaoAusenciaAluno>();

            var compensacaoAusenciaAlterada = compensacaoAusencias.FirstOrDefault();
            compensacaoAusenciaAlterada.Alunos = compensacaoAusenciaAlunos.Where(t => t.CompensacaoAusenciaId == compensacaoAusenciaAlterada.Id);
            compensacaoAusenciaAlterada.Alunos.FirstOrDefault(t => t.CodigoAluno == CODIGO_ALUNO_2).QuantidadeFaltasCompensadas = 2;

            var compensacaoAusenciaAlunoDto = new List<CompensacaoAusenciaAlunoDto>()
            { new CompensacaoAusenciaAlunoDto()
                { Id = compensacaoAusenciaAlterada.Alunos.FirstOrDefault(t => t.CodigoAluno == CODIGO_ALUNO_2).Id.ToString(),
                    QtdFaltasCompensadas = compensacaoAusenciaAlterada.Alunos.FirstOrDefault(t => t.CodigoAluno == CODIGO_ALUNO_2).QuantidadeFaltasCompensadas
                }
            };

            var dtoAlterado = new CompensacaoAusenciaDto()
            {
                Alunos = compensacaoAusenciaAlunoDto,
                Bimestre = compensacaoAusenciaAlterada.Bimestre,
                Descricao = compensacaoAusenciaAlterada.Descricao,
                Atividade = compensacaoAusenciaAlterada.Nome,
                DisciplinaId = compensacaoAusenciaAlterada.DisciplinaId,
                Id = compensacaoAusenciaAlterada.Id,
                TurmaId = compensacaoAusenciaAlterada.TurmaId.ToString()
            };

            await comando.Executar(dtoAlterado.Id, dtoAlterado);
            var listaAlterada = ObterTodos<CompensacaoAusencia>();

            listaAlterada.ToList().Exists(x => x.Alunos.Any(z => z.QuantidadeFaltasCompensadas == 2)).ShouldBeTrue();
        }

        private CompensacaoDeAusenciaDBDto ObterDtoDadoBase(string perfil, string componente)
        {
            return new CompensacaoDeAusenciaDBDto()
            {
                Perfil = perfil,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_3,
                ComponenteCurricular = componente,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                AnoTurma = ANO_5,
                DataReferencia = DATA_25_07_INICIO_BIMESTRE_3,
                QuantidadeAula = QUANTIDADE_AULA_4
            };
        }
    }
}