using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    /// <summary>
    /// Characterization: ConsultasFechamentoTurmaDisciplina deve buscar o Conceito uma única vez por
    /// nota-com-conceito. NotaConceito devolve o próprio Id do conceito (não Conceito.Valor);
    /// ConceitoDescricao devolve Conceito.Valor (não Conceito.Descricao).
    ///
    /// Usa BIMESTRE_4 (não BIMESTRE_1): o fake padrão de alunos
    /// (ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeValidarAlunos) tem DataMatricula/DataSituacao
    /// relativas a "hoje", e os períodos "não-válidos" de InserirPeriodoEscolarCustomizado só cobrem
    /// essas datas no bimestre mais recente (o 4º).
    /// </summary>
    public class Ao_obter_conceito_no_fechamento_turma_disciplina : NotaFechamentoBimestreTesteBase
    {
        private const long FECHAMENTO_TURMA_ID_1 = 1;
        private const long FECHAMENTO_TURMA_DISCIPLINA_ID_1 = 1;
        private const long CONCEITO_ORFAO = 999999; // não existe na tabela conceito

        public Ao_obter_conceito_no_fechamento_turma_disciplina(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IServicoTelemetria), typeof(ContadorQueriesTelemetriaFake), ServiceLifetime.Singleton));
        }

        [Fact]
        public async Task Deve_disparar_uma_unica_consulta_get_por_nota_com_conceito()
        {
            await MontarCenario();
            var conceitoId = await InserirNaBaseAsync(CriarConceito("Bom", ativo: true));
            await CriarFechamentoComNotaConceito(CODIGO_ALUNO_1, conceitoId);

            var consultas = ServiceProvider.GetService<IConsultasFechamentoTurmaDisciplina>();

            ContadorQueriesTelemetriaFake.Limpar();
            await consultas.ObterNotasFechamentoTurmaDisciplina(TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, BIMESTRE_4, SEMESTRE_0);
            var buscasDeConceito = ContadorQueriesTelemetriaFake.ContarPorTrecho("from conceito_valores");

            buscasDeConceito.ShouldBe(1);
        }

        [Fact]
        public async Task Deve_retornar_id_como_notaconceito_e_valor_como_descricao()
        {
            await MontarCenario();
            var conceitoId = await InserirNaBaseAsync(CriarConceito("Bom", ativo: true));
            await CriarFechamentoComNotaConceito(CODIGO_ALUNO_1, conceitoId);

            var consultas = ServiceProvider.GetService<IConsultasFechamentoTurmaDisciplina>();
            var retorno = await consultas.ObterNotasFechamentoTurmaDisciplina(TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, BIMESTRE_4, SEMESTRE_0);

            var nota = retorno.Alunos.First(a => a.CodigoAluno == CODIGO_ALUNO_1).Notas.Single();
            nota.NotaConceito.ShouldBe((double)conceitoId);
            nota.ConceitoDescricao.ShouldBe("Bom");
        }

        [Fact]
        public async Task Deve_retornar_zero_e_vazio_quando_conceito_referenciado_nao_existe_mais()
        {
            await MontarCenario();
            // Propositalmente não insere nenhum Conceito com id = CONCEITO_ORFAO.
            await CriarFechamentoComNotaConceito(CODIGO_ALUNO_1, CONCEITO_ORFAO);

            var consultas = ServiceProvider.GetService<IConsultasFechamentoTurmaDisciplina>();
            var retorno = await consultas.ObterNotasFechamentoTurmaDisciplina(TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, BIMESTRE_4, SEMESTRE_0);

            var nota = retorno.Alunos.First(a => a.CodigoAluno == CODIGO_ALUNO_1).Notas.Single();
            nota.NotaConceito.ShouldBe(0d);
            nota.ConceitoDescricao.ShouldBe(string.Empty);
        }

        [Fact]
        public async Task Deve_retornar_valores_mesmo_quando_conceito_esta_inativo()
        {
            await MontarCenario();
            var conceitoId = await InserirNaBaseAsync(CriarConceito("Descontinuado", ativo: false));
            await CriarFechamentoComNotaConceito(CODIGO_ALUNO_1, conceitoId);

            var consultas = ServiceProvider.GetService<IConsultasFechamentoTurmaDisciplina>();
            var retorno = await consultas.ObterNotasFechamentoTurmaDisciplina(TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, BIMESTRE_4, SEMESTRE_0);

            var nota = retorno.Alunos.First(a => a.CodigoAluno == CODIGO_ALUNO_1).Notas.Single();
            nota.NotaConceito.ShouldBe((double)conceitoId);
            nota.ConceitoDescricao.ShouldBe("Descontinuado");
        }

        private async Task MontarCenario()
        {
            // Os alunos do fake padrão (ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeValidarAlunos)
            // têm DataMatricula/DataSituacao relativas a "hoje" — os períodos escolares fixos de
            // CriarPeriodoEscolar() não cobrem essas datas. Por isso, como já feito em
            // Ao_obter_frequencia_dos_alunos.CriarDadosBaseFrequencia, os períodos são desativados no
            // filtro e criados manualmente via InserirPeriodoEscolarCustomizado (relativos a "hoje").
            var filtro = new FiltroFechamentoNotaDto
            {
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ConsiderarAnoAnterior = false,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = ANO_7,
                TipoFrequenciaAluno = TipoFrequenciaAluno.PorDisciplina,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                CriarPeriodoEscolar = false,
                CriarPeriodoEscolarCustomizado = false
            };

            await InserirPeriodoEscolarCustomizado();
            await CriarDadosBase(filtro);
        }

        private Dominio.Conceito CriarConceito(string valor, bool ativo)
            => new Dominio.Conceito
            {
                Descricao = valor + " (descrição)",
                Valor = valor,
                Aprovado = true,
                Ativo = ativo,
                InicioVigencia = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 1, 1),
                FimVigencia = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 12, 31),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            };

        private async Task CriarFechamentoComNotaConceito(string codigoAluno, long conceitoId)
        {
            await InserirNaBase(new FechamentoTurma
            {
                TurmaId = TURMA_ID_1,
                PeriodoEscolarId = ObterTodos<PeriodoEscolar>().First(p => p.Bimestre == BIMESTRE_4).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoTurmaDisciplina
            {
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var fechamentoAlunoId = await InserirNaBaseAsync(new FechamentoAluno
            {
                FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                AlunoCodigo = codigoAluno,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoNota
            {
                FechamentoAlunoId = fechamentoAlunoId,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                ConceitoId = conceitoId,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
