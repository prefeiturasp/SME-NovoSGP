using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
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
        }

        [Fact]
        public async Task Deve_remover_aluno_na_compensacao()
        {
            await ExecutarTesteRemoverAlunoCompensacao(ObterPerfilDiretor());
        }

        //bulk insert
        //[Fact]
        public async Task Deve_adicionar_aluno_na_compensacao()
        {
            await ExecutarTesteAdicionarAlunoCompensacao(ObterPerfilDiretor());
        }

        //bulk insert
        //[Fact]
        public async Task Deve_alterar_quantidade_ausencias_compensadas()
        {
            await ExecutarTesteAlterar(ObterPerfilDiretor());
        }

        private async Task ExecutarTesteAdicionarAlunoCompensacao(string perfil)
        {
            var dtoDadoBase = ObtenhaDtoDadoBase(perfil, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            await ExecuteTeste(dtoDadoBase);
        }

        private async Task ExecutarTesteRemoverAlunoCompensacao(string perfil)
        {
            var dtoDadoBase = ObtenhaDtoDadoBase(perfil, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await CriarDadosBase(dtoDadoBase);
            await CriaFrequenciaAlunos(dtoDadoBase);
            await CriaCompensacaoAusencia(dtoDadoBase);
            await CriaCompensacaoAusenciaAluno();
            await CriaRegistroDeFrequencia();

            var comando = ServiceProvider.GetService<IComandosCompensacaoAusencia>();
            var listaIds = new long[] { COMPENSACAO_AUSENCIA_ID_1 };

            await comando.Excluir(listaIds);

            var compensacaoAusenciaAlunos = ObterTodos<CompensacaoAusenciaAluno>();
            compensacaoAusenciaAlunos.ForEach(x => x.Excluido.ShouldBeTrue());
        }

        private List<CompensacaoAusenciaAlunoDto> ObterListaDeAlunos()
        {
            return new List<CompensacaoAusenciaAlunoDto>()
            {
                new CompensacaoAusenciaAlunoDto()
                {
                    Id = CODIGO_ALUNO_1,
                    QtdFaltasCompensadas = QUANTIDADE_AULA
                },
                new CompensacaoAusenciaAlunoDto()
                {
                    Id = CODIGO_ALUNO_2,
                    QtdFaltasCompensadas = QUANTIDADE_AULA_2
                },
                new CompensacaoAusenciaAlunoDto()
                {
                    Id = CODIGO_ALUNO_3,
                    QtdFaltasCompensadas = QUANTIDADE_AULA
                },
                new CompensacaoAusenciaAlunoDto()
                {
                    Id = CODIGO_ALUNO_4,
                    QtdFaltasCompensadas = QUANTIDADE_AULA
                }
            };
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
            await RegistroFrequenciaAluno(CODIGO_ALUNO_2, QUANTIDADE_AULA, TipoFrequencia.F);
            await RegistroFrequenciaAluno(CODIGO_ALUNO_3, QUANTIDADE_AULA, TipoFrequencia.F);
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
                DATA_03_01_INICIO_BIMESTRE_1,
                DATA_29_04_FIM_BIMESTRE_1,
                codigoAluno,
                totalPresenca,
                totalAusencia,
                PERIODO_ESCOLAR_ID_1,
                totalCompensacao);
        }

        private CompensacaoDeAusenciaDBDto ObtenhaDtoDadoBase(string perfil, string componente)
        {
            return new CompensacaoDeAusenciaDBDto()
            {
                Perfil = perfil,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = componente,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                AnoTurma = ANO_5,
                DataReferencia = DATA_03_01_INICIO_BIMESTRE_1,
                QuantidadeAula = QUANTIDADE_AULA_4
            };
        }

        private async Task ExecutarTesteAlterar(string perfil)
        {
            var dtoDadoBase = ObterDtoDadoBase(perfil, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await CriarDadosBase(dtoDadoBase);
            await CriaFrequenciaAlunos(dtoDadoBase);

            var comando = ServiceProvider.GetService<IComandosCompensacaoAusencia>();
            var dto = ObtenhaCompensacaoAusenciaDto(dtoDadoBase, ObterListaDeAlunos());

            await comando.Inserir(dto);
            var compensacaoAusencias = ObterTodos<CompensacaoAusencia>();
            var compensacaoAusenciaAlterada = compensacaoAusencias.FirstOrDefault();
            compensacaoAusenciaAlterada.Alunos.FirstOrDefault().QuantidadeFaltasCompensadas = 10;

            var compensacaoAusenciaAlunoDto = new List<CompensacaoAusenciaAlunoDto>()
            { new CompensacaoAusenciaAlunoDto()
                { Id = compensacaoAusenciaAlterada.Alunos.FirstOrDefault().Id.ToString(),
                    QtdFaltasCompensadas = compensacaoAusenciaAlterada.Alunos.FirstOrDefault().QuantidadeFaltasCompensadas
                }
            };

            var dtoAlterado = new CompensacaoAusenciaDto()
            {
                Alunos = compensacaoAusenciaAlunoDto,
                Bimestre = compensacaoAusenciaAlterada.Bimestre,
                Descricao = compensacaoAusenciaAlterada.Descricao,
                DisciplinaId = compensacaoAusenciaAlterada.DisciplinaId,
                Id = compensacaoAusenciaAlterada.Id,
                TurmaId = compensacaoAusenciaAlterada.TurmaId.ToString()
            };

            await comando.Alterar(dtoAlterado.Id, dtoAlterado);
            var listaAlterada = ObterTodos<CompensacaoAusencia>();

            listaAlterada.ToList().Exists(x => x.Alunos.Any(z => z.QuantidadeFaltasCompensadas == 10)).ShouldBeTrue();
        }

        private CompensacaoDeAusenciaDBDto ObterDtoDadoBase(string perfil, string componente)
        {
            return new CompensacaoDeAusenciaDBDto()
            {
                Perfil = perfil,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = componente,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                AnoTurma = ANO_5,
                DataReferencia = DATA_03_01_INICIO_BIMESTRE_1,
                QuantidadeAula = QUANTIDADE_AULA_4
            };
        }
    }
}