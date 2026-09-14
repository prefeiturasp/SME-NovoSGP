using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    /// <summary>
    /// Infraestrutura compartilhada pelos testes de caracterização do N+1 de notas em
    /// RetornaListagemAlunosFechamentoBimestreEspecifico (ListarFechamentoTurmaBimestreUseCase.cs:216):
    /// Ao_obter_notas_dos_alunos e Ao_obter_notas_dos_alunos_com_aprovacao_pendente.
    /// </summary>
    public abstract class NotasDosAlunosTesteBase : NotaFechamentoBimestreTesteBase
    {
        protected const long FECHAMENTO_TURMA_ID_1 = 1;
        protected const long FECHAMENTO_TURMA_DISCIPLINA_ID_1 = 1;
        protected const string TRECHO_SQL_FECHAMENTO_NOTA = "fechamento_nota";

        protected NotasDosAlunosTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected static AlunosFechamentoNotaConceitoTurmaDto ObterAlunoDoFechamento(FechamentoNotaConceitoTurmaDto retorno, string codigoAluno)
        {
            var aluno = retorno.Alunos.FirstOrDefault(a => a.CodigoAluno == codigoAluno);

            aluno.ShouldNotBeNull($"Aluno {codigoAluno} não retornado. Alunos: {string.Join(", ", retorno.Alunos.Select(a => a.CodigoAluno))}");

            return aluno;
        }

        protected static FechamentoConsultaNotaConceitoTurmaListaoDto ObterUnicaNota(FechamentoNotaConceitoTurmaDto retorno, string codigoAluno)
        {
            var aluno = ObterAlunoDoFechamento(retorno, codigoAluno);

            aluno.NotasConceitoBimestre.ShouldNotBeNull();
            aluno.NotasConceitoBimestre.Count.ShouldBe(1);

            return aluno.NotasConceitoBimestre.First();
        }

        protected async Task CriarFechamentoTurmaDisciplina(PeriodoEscolar periodoEscolar)
        {
            await InserirNaBase(new FechamentoTurma
            {
                TurmaId = TURMA_ID_1,
                PeriodoEscolarId = periodoEscolar.Id,
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

            foreach (var codigoAluno in new[] { CODIGO_ALUNO_1, CODIGO_ALUNO_2, CODIGO_ALUNO_3 })
            {
                await InserirNaBase(new FechamentoAluno
                {
                    FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                    AlunoCodigo = codigoAluno,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
            }
        }

        protected async Task<FechamentoNotaConceitoTurmaDto> ExecutarTeste(int bimestre = BIMESTRE_1)
        {
            var useCase = ServiceProvider.GetService<IListarFechamentoTurmaBimestreUseCase>();

            return await useCase.Executar(TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, bimestre, SEMESTRE_0);
        }
    }
}
