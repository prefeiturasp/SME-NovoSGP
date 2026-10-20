using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_consultar_fechamento_turma_disciplina : TesteBaseComuns
    {
        public Ao_consultar_fechamento_turma_disciplina(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_retornar_um_aluno_com_multiplas_notas_e_componente_da_nota()
        {
            var (turmaId, periodoId) = await PrepararBase();
            var fechamentoTurmaId = await CriarFechamentoTurma(turmaId, periodoId);
            var fechamentoId = await CriarFechamentoDisciplina(fechamentoTurmaId, 1105);
            var alunoId = await CriarFechamentoAluno(fechamentoId);
            await CriarNota(alunoId, 138);
            await CriarNota(alunoId, 138);
            await CriarNota(alunoId, 139);

            var repositorio = ServiceProvider.GetRequiredService<IRepositorioFechamentoTurmaDisciplinaConsulta>();
            var retorno = (await repositorio.ObterFechamentosTurmaDisciplinas(turmaId, new long[] { 138 }, 1, 1)).ToList();

            Assert.Single(retorno);
            Assert.Equal(fechamentoId, retorno[0].Id);
            Assert.Equal(SISTEMA_NOME, retorno[0].CriadoPor);
            Assert.Equal(SISTEMA_CODIGO_RF, retorno[0].CriadoRF);
            Assert.Equal(fechamentoTurmaId, retorno[0].FechamentoTurma.Id);
            Assert.Equal(1, retorno[0].FechamentoTurma.PeriodoEscolar.Bimestre);
            Assert.Single(retorno[0].FechamentoAlunos);
            Assert.Equal(alunoId, retorno[0].FechamentoAlunos[0].Id);
        }

        [Fact]
        public async Task Deve_selecionar_fechamento_mais_recente_sem_nota()
        {
            var (turmaId, periodoId) = await PrepararBase();
            var fechamentoTurmaId = await CriarFechamentoTurma(turmaId, periodoId);
            var antigoId = await CriarFechamentoDisciplina(fechamentoTurmaId, 138);
            await CriarFechamentoAluno(antigoId);
            var recenteId = await CriarFechamentoDisciplina(fechamentoTurmaId, 138);
            await CriarFechamentoAluno(recenteId);

            var repositorio = ServiceProvider.GetRequiredService<IRepositorioFechamentoTurmaDisciplinaConsulta>();
            var retorno = (await repositorio.ObterFechamentosTurmaDisciplinas(turmaId, new long[] { 138 }, 1, 1)).ToList();

            Assert.Single(retorno);
            Assert.Equal(recenteId, retorno[0].Id);
            Assert.Single(retorno[0].FechamentoAlunos);
        }

        [Theory]
        [InlineData("fechamento_disciplina")]
        [InlineData("fechamento_turma")]
        [InlineData("fechamento_aluno")]
        [InlineData("fechamento_nota")]
        public async Task Deve_desconsiderar_registro_excluido_em_cada_entidade(string entidadeExcluida)
        {
            var (turmaId, periodoId) = await PrepararBase();
            var fechamentoTurmaId = await CriarFechamentoTurma(turmaId, periodoId, entidadeExcluida == "fechamento_turma");
            var fechamentoId = await CriarFechamentoDisciplina(fechamentoTurmaId, 1105, entidadeExcluida == "fechamento_disciplina");
            var alunoId = await CriarFechamentoAluno(fechamentoId, entidadeExcluida == "fechamento_aluno");
            await CriarNota(alunoId, 138, entidadeExcluida == "fechamento_nota");

            var repositorio = ServiceProvider.GetRequiredService<IRepositorioFechamentoTurmaDisciplinaConsulta>();
            var retorno = await repositorio.ObterFechamentosTurmaDisciplinas(turmaId, new long[] { 138 }, 1, 1);

            Assert.Empty(retorno);
        }

        [Fact]
        public async Task Deve_desconsiderar_nota_excluida_na_leitura_em_lote()
        {
            var (turmaId, periodoId) = await PrepararBase();
            var fechamentoTurmaId = await CriarFechamentoTurma(turmaId, periodoId);
            var fechamentoId = await CriarFechamentoDisciplina(fechamentoTurmaId, 138);
            var alunoId = await CriarFechamentoAluno(fechamentoId);
            await CriarNota(alunoId, 138);
            await CriarNota(alunoId, 139, true);

            var repositorio = ServiceProvider.GetRequiredService<IRepositorioFechamentoTurmaDisciplinaConsulta>();
            var retorno = (await repositorio.ObterNotasBimestrePorCodigosAlunosIdsFechamentos(
                new[] { ALUNO_CODIGO_1111111 }, new[] { fechamentoId })).ToList();

            Assert.Single(retorno);
            Assert.Equal(138, retorno[0].DisciplinaId);
        }

        private async Task<(long turmaId, long periodoId)> PrepararBase()
        {
            await CriarDreUePerfilComponenteCurricular();
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarTurma(Modalidade.Fundamental);
            await CriarPeriodoEscolar(DateTime.Today.AddDays(-30), DateTime.Today.AddDays(30), 1);

            return (ObterTodos<SME.SGP.Dominio.Turma>().First().Id, ObterTodos<PeriodoEscolar>().First().Id);
        }

        private Task<long> CriarFechamentoTurma(long turmaId, long periodoId, bool excluido = false)
            => InserirNaBaseAsync(new FechamentoTurma
            {
                TurmaId = turmaId,
                PeriodoEscolarId = periodoId,
                Excluido = excluido,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

        private Task<long> CriarFechamentoDisciplina(long fechamentoTurmaId, long disciplinaId, bool excluido = false)
            => InserirNaBaseAsync(new FechamentoTurmaDisciplina
            {
                FechamentoTurmaId = fechamentoTurmaId,
                DisciplinaId = disciplinaId,
                Situacao = SituacaoFechamento.ProcessadoComSucesso,
                Excluido = excluido,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

        private Task<long> CriarFechamentoAluno(long fechamentoId, bool excluido = false)
            => InserirNaBaseAsync(new FechamentoAluno
            {
                FechamentoTurmaDisciplinaId = fechamentoId,
                AlunoCodigo = ALUNO_CODIGO_1111111,
                Excluido = excluido,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

        private Task<long> CriarNota(long alunoId, long disciplinaId, bool excluido = false)
            => InserirNaBaseAsync(new FechamentoNota
            {
                FechamentoAlunoId = alunoId,
                DisciplinaId = disciplinaId,
                Nota = 7,
                Excluido = excluido,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
    }
}
