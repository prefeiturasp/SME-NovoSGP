using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_listar_notas_avaliacoes_por_turma : NotaFechamentoBimestreTesteBase
    {
        public Ao_listar_notas_avaliacoes_por_turma(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Ao listar notas avaliações por turma de componente de regência")]
        public async Task Ao_listar_notas_avaliacoes_por_turma_componente_regencia()
        {
            var filtroNota = new FiltroFechamentoNotaDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricular = COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114.ToString(),
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_LOGIN_2222222, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114.ToString(), TIPO_CALENDARIO_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, TIPO_AVALIACAO_CODIGO_1, AVALIACAO_NOME_1, true, false, filtroNota.ProfessorRf);
            await CriarAtividadeAvaliativaDisciplina(ATIVIDADE_AVALIATIVA_1, filtroNota.ComponenteCurricular);
            await CriarAtividadeAvaliativaRegencia(COMPONENTE_GEOGRAFIA_ID_8, COMPONENTE_GEOGRAFIA_NOME);
            await CriarAtividadeAvaliativaRegencia(COMPONENTE_CIENCIAS_ID_89, COMPONENTE_CIENCIAS_NOME);
            await CriarFechamentoAluno();
            var dto = new FiltroTurmaAlunoPeriodoEscolarDto(TURMA_ID_1, PERIODO_ESCOLAR_CODIGO_2, CODIGO_ALUNO_1, COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114.ToString());
            var useCase = ServiceProvider.GetService<IObterAtividadesNotasAlunoPorTurmaPeriodoUseCase>();
            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();
            var avaliacao = retorno.FirstOrDefault();
            avaliacao.Nome.ShouldBe(AVALIACAO_NOME_1);
            avaliacao.Disciplinas.ShouldNotBeNull();
            avaliacao.Disciplinas.Length.ShouldBe(2);
            avaliacao.Disciplinas.ShouldContain(COMPONENTE_GEOGRAFIA_NOME.Replace("'", string.Empty));
            avaliacao.Disciplinas.ShouldContain(COMPONENTE_CIENCIAS_NOME.Replace("'", string.Empty));
        }

        private async Task CriarNotaConceitoNaBase(FiltroFechamentoNotaDto filtroNota, string alunoCodigo, long atividadeAvaliativaId, double? nota = null, long? conceitoId = null)
        {
            await InserirNaBase(new NotaConceito()
            {
                AlunoId = alunoCodigo,
                AtividadeAvaliativaID = atividadeAvaliativaId,
                Nota = nota,
                ConceitoId = conceitoId,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                DisciplinaId = filtroNota.ComponenteCurricular,
                TipoNota = TipoNota.Nota
            });
        }

        private async Task CriarFechamentoAluno()
        {
            await InserirNaBase(new FechamentoTurma()
            {
                TurmaId = TURMA_ID_1,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoTurmaDisciplina()
            {
                DisciplinaId = COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114,
                FechamentoTurmaId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = 1,
                AlunoCodigo = CODIGO_ALUNO_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114,
                FechamentoAlunoId = 1,
                Nota = NOTA_8,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
