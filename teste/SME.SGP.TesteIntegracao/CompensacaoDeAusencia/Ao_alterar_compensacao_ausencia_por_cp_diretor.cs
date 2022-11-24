using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_alterar_compensacao_ausencia_por_cp_diretor : CompensacaoDeAusenciaTesteBase
    {
        private const string DESCRICAO_ALTERADA = "OUTRA DESCRICAO";

        public Ao_alterar_compensacao_ausencia_por_cp_diretor(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        //bulk insert
        //[Fact]
        public async Task deve_alterar_compensacao_perfil_cp()
        {
            await ExecutarComando(ObterPerfilCP());
        }

        //bulk insert
        //[Fact]
        public async Task deve_alterar_compensacao_perfil_diretor()
        {
            await ExecutarComando(ObterPerfilDiretor());
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

        private async Task CriaFrequenciaAlunos(CompensacaoDeAusenciaDBDto dtoDadoBase)
        {
            await CriaFrequenciaAlunos(
            dtoDadoBase,
            CODIGO_ALUNO_1,
            QUANTIDADE_AULA_3,
            QUANTIDADE_AULA);

            await CriaFrequenciaAlunos(
            dtoDadoBase,
            CODIGO_ALUNO_2,
            QUANTIDADE_AULA,
            QUANTIDADE_AULA_3);

            await CriaFrequenciaAlunos(
            dtoDadoBase,
            CODIGO_ALUNO_3,
            QUANTIDADE_AULA_2,
            QUANTIDADE_AULA_2);

            await CriaFrequenciaAlunos(
            dtoDadoBase,
            CODIGO_ALUNO_4,
            QUANTIDADE_AULA_3,
            QUANTIDADE_AULA);
        }

        private async Task CriaFrequenciaAlunos(CompensacaoDeAusenciaDBDto dtoDadoBase, string codigoAluno, int totalPresenca, int totalAusencia)
        {
            await CriaFrequenciaAluno(
                dtoDadoBase,
                DATA_03_01_INICIO_BIMESTRE_1,
                DATA_29_04_FIM_BIMESTRE_1,
                codigoAluno,
                totalPresenca,
                totalAusencia,
                PERIODO_ESCOLAR_ID_3);
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

        private async Task ExecutarComando(string perfil)
        {
            var dtoDadoBase = ObterDtoDadoBase(perfil, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await CriarDadosBase(dtoDadoBase);
            await CriaFrequenciaAlunos(dtoDadoBase);

            var comando = ServiceProvider.GetService<IComandosCompensacaoAusencia>();
            var dto = ObtenhaCompensacaoAusenciaDto(dtoDadoBase, ObterListaDeAlunos());

            await comando.Inserir(dto);
            var compensacaoAusencias = ObterTodos<CompensacaoAusencia>();
            var compensacaoAusenciaAlterada = compensacaoAusencias.FirstOrDefault();
            compensacaoAusenciaAlterada.Descricao = DESCRICAO_ALTERADA;

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

            listaAlterada.ToList().Exists(x => x.Descricao == DESCRICAO_ALTERADA).ShouldBeTrue();
        }
    }
}