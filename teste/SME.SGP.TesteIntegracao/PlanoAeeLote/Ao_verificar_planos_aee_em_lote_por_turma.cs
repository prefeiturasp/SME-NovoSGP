using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAeeLote
{
    /// <summary>
    /// Characterization: a consulta em lote ObterPlanosPorAlunosEAno deve devolver, para uma turma
    /// com múltiplos alunos com plano AEE ativo simultâneo, o mesmo conjunto que a consulta unitária
    /// ObterPlanoPorEstudanteEAno devolve quando executada uma vez por aluno.
    /// </summary>
    public class Ao_verificar_planos_aee_em_lote_por_turma : TesteBase
    {
        private const long TURMA_ID = 1;
        private const string ALUNO_1 = "1111";
        private const string ALUNO_2 = "2222";
        private const string ALUNO_3 = "3333"; // sem plano AEE

        public Ao_verificar_planos_aee_em_lote_por_turma(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_retornar_um_plano_para_cada_aluno_que_possui_plano_aee_ativo()
        {
            var anoLetivo = await CarregarTurma();
            await CarregarPlanoAEE(id: 1, versaoId: 1, aluno: ALUNO_1, anoLetivo: anoLetivo);
            await CarregarPlanoAEE(id: 2, versaoId: 2, aluno: ALUNO_2, anoLetivo: anoLetivo);

            var repositorio = ServiceProvider.GetService<IRepositorioPlanoAEEConsulta>();

            var emLote = (await repositorio.ObterPlanosPorAlunosEAno(new[] { ALUNO_1, ALUNO_2, ALUNO_3 }, anoLetivo)).ToList();

            emLote.Count.ShouldBe(2);
            emLote.ShouldContain(p => p.CodigoAluno == ALUNO_1);
            emLote.ShouldContain(p => p.CodigoAluno == ALUNO_2);
        }

        [Fact]
        public async Task Deve_retornar_o_mesmo_resultado_da_consulta_unitaria_repetida_por_aluno()
        {
            var anoLetivo = await CarregarTurma();
            await CarregarPlanoAEE(id: 1, versaoId: 1, aluno: ALUNO_1, anoLetivo: anoLetivo);
            await CarregarPlanoAEE(id: 2, versaoId: 2, aluno: ALUNO_2, anoLetivo: anoLetivo);

            var repositorio = ServiceProvider.GetService<IRepositorioPlanoAEEConsulta>();

            var alunos = new[] { ALUNO_1, ALUNO_2, ALUNO_3 };
            var gabarito = alunos
                .Select(async a => (aluno: a, plano: await repositorio.ObterPlanoPorEstudanteEAno(a, anoLetivo)))
                .Select(t => t.Result)
                .Where(t => t.plano.NaoEhNulo())
                .ToList();

            var emLote = (await repositorio.ObterPlanosPorAlunosEAno(alunos, anoLetivo)).ToList();

            emLote.Count.ShouldBe(gabarito.Count);
            foreach (var (aluno, plano) in gabarito)
            {
                var doLote = emLote.FirstOrDefault(p => p.CodigoAluno == aluno);
                doLote.ShouldNotBeNull($"esperava plano AEE em lote para o aluno {aluno}");
                doLote.Id.ShouldBe(plano.Id);
            }
        }

        [Fact]
        public async Task Deve_retornar_vazio_quando_nenhum_aluno_possui_plano()
        {
            var anoLetivo = await CarregarTurma();

            var repositorio = ServiceProvider.GetService<IRepositorioPlanoAEEConsulta>();

            (await repositorio.ObterPlanosPorAlunosEAno(new[] { ALUNO_1, ALUNO_2, ALUNO_3 }, anoLetivo)).ShouldBeEmpty();
        }

        [Fact]
        public async Task Deve_ignorar_plano_com_versao_excluida()
        {
            var anoLetivo = await CarregarTurma();
            await CarregarPlanoAEE(id: 1, versaoId: 1, aluno: ALUNO_1, anoLetivo: anoLetivo, versaoExcluida: true);

            var repositorio = ServiceProvider.GetService<IRepositorioPlanoAEEConsulta>();

            (await repositorio.ObterPlanoPorEstudanteEAno(ALUNO_1, anoLetivo)).ShouldBeNull();
            (await repositorio.ObterPlanosPorAlunosEAno(new[] { ALUNO_1 }, anoLetivo)).ShouldBeEmpty();
        }

        [Fact]
        public async Task Deve_ignorar_plano_aee_marcado_como_excluido()
        {
            var anoLetivo = await CarregarTurma();
            await CarregarPlanoAEE(id: 1, versaoId: 1, aluno: ALUNO_1, anoLetivo: anoLetivo, planoExcluido: true);

            var repositorio = ServiceProvider.GetService<IRepositorioPlanoAEEConsulta>();

            (await repositorio.ObterPlanoPorEstudanteEAno(ALUNO_1, anoLetivo)).ShouldBeNull();
            (await repositorio.ObterPlanosPorAlunosEAno(new[] { ALUNO_1 }, anoLetivo)).ShouldBeEmpty();
        }

        [Fact]
        public async Task Deve_considerar_plano_de_qualquer_ano_quando_ano_informado_e_o_atual()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            await CarregarTurma(anoAtual - 1);
            // Plano criado no ano letivo anterior, mas ainda vigente/ativo.
            await CarregarPlanoAEE(id: 1, versaoId: 1, aluno: ALUNO_1, anoLetivo: anoAtual, criadoEm: new DateTime(anoAtual - 1, 6, 1));

            var repositorio = ServiceProvider.GetService<IRepositorioPlanoAEEConsulta>();

            (await repositorio.ObterPlanoPorEstudanteEAno(ALUNO_1, anoAtual)).ShouldNotBeNull();
            (await repositorio.ObterPlanosPorAlunosEAno(new[] { ALUNO_1 }, anoAtual)).ShouldNotBeEmpty();
        }

        [Fact]
        public async Task Deve_filtrar_por_ano_de_criacao_quando_ano_informado_for_diferente_do_atual()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var anoConsultado = anoAtual - 2;
            await CarregarTurma(anoConsultado);
            // Plano criado num ano diferente do ano consultado.
            await CarregarPlanoAEE(id: 1, versaoId: 1, aluno: ALUNO_1, anoLetivo: anoConsultado, criadoEm: new DateTime(anoAtual - 1, 6, 1));

            var repositorio = ServiceProvider.GetService<IRepositorioPlanoAEEConsulta>();

            (await repositorio.ObterPlanoPorEstudanteEAno(ALUNO_1, anoConsultado)).ShouldBeNull();
            (await repositorio.ObterPlanosPorAlunosEAno(new[] { ALUNO_1 }, anoConsultado)).ShouldBeEmpty();
        }

        private async Task<int> CarregarTurma(int? anoLetivo = null)
        {
            var ano = anoLetivo ?? DateTimeExtension.HorarioBrasilia().Year;

            await InserirNaBase(new Dre { Id = 1, Nome = "Dre Teste", CodigoDre = "11", Abreviacao = "DT" });
            await InserirNaBase(new Ue { Id = 1, Nome = "Ue Teste", DreId = 1, TipoEscola = TipoEscola.EMEF, CodigoUe = "22" });
            await InserirNaBase(new Dominio.Turma
            {
                Id = TURMA_ID,
                Nome = "1A",
                CodigoTurma = "1234",
                Ano = "1",
                AnoLetivo = ano,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 1
            });

            return ano;
        }

        private async Task CarregarPlanoAEE(long id, long versaoId, string aluno, int anoLetivo,
            bool versaoExcluida = false, bool planoExcluido = false, DateTime? criadoEm = null)
        {
            var dataCriacao = criadoEm ?? DateTimeExtension.HorarioBrasilia();

            await InserirNaBase(new Dominio.PlanoAEE
            {
                Id = id,
                TurmaId = TURMA_ID,
                AlunoCodigo = aluno,
                AlunoNome = $"Aluno {aluno}",
                AlunoNumero = 1,
                Situacao = SituacaoPlanoAEE.Validado,
                CriadoEm = dataCriacao,
                CriadoPor = "sistema",
                CriadoRF = "0000000"
            });

            await InserirNaBase(new Dominio.PlanoAEEVersao
            {
                Id = versaoId,
                PlanoAEEId = id,
                Numero = 1,
                Excluido = versaoExcluida,
                CriadoEm = dataCriacao,
                CriadoPor = "sistema",
                CriadoRF = "0000000"
            });

            // A entidade PlanoAEE não expõe a coluna "excluido" (mapeada só no banco), então
            // marcamos via SQL direto quando o cenário precisa de um plano excluído.
            if (planoExcluido)
                await MarcarPlanoAeeComoExcluido(id);
        }

        private async Task MarcarPlanoAeeComoExcluido(long planoAeeId)
        {
            using var cmd = new NpgsqlCommand("update plano_aee set excluido = true where id = @id", _collectionFixture.Database.Conexao);
            cmd.Parameters.AddWithValue("id", planoAeeId);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
