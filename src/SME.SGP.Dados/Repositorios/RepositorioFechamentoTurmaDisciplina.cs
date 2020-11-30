﻿using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoTurmaDisciplina : RepositorioBase<FechamentoTurmaDisciplina>, IRepositorioFechamentoTurmaDisciplina
    {
        private readonly IRepositorioTurma repositorioTurma;

        public RepositorioFechamentoTurmaDisciplina(ISgpContext database, IRepositorioTurma repositorioTurma) : base(database)
        {
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosTurmaDisciplinas(long turmaId, long[] disciplinasId, int bimestre = 0)
        {
            var query = new StringBuilder(@"select f.*, fa.*
                         from fechamento_turma_disciplina f
                        inner join fechamento_turma ft on ft.id = f.fechamento_turma_id
                         left join periodo_escolar p on p.id = ft.periodo_escolar_id 
                        inner join turma t on t.id = ft.turma_id
                        inner join fechamento_aluno fa on f.id = fa.fechamento_turma_disciplina_id
                        where not f.excluido
                            and t.id = @turmaId ");

            if (disciplinasId != null && disciplinasId.Length > 0)
                query.AppendLine("and f.disciplina_id = ANY(@disciplinasId)");

            if (bimestre > 0)
                query.AppendLine("and p.bimestre = @bimestre ");
            else
                query.AppendLine("and ft.periodo_escolar_id is null");

            IList<FechamentoTurmaDisciplina> fechammentosTurmaDisciplina = new List<FechamentoTurmaDisciplina>();

            await database.Conexao.QueryAsync<FechamentoTurmaDisciplina, FechamentoAluno, FechamentoTurmaDisciplina>(query.ToString(),
                (fechamentoTurmaDiscplina, fechamentoAluno) =>
                {
                    var fechamentoTurmaDisciplinaLista = fechammentosTurmaDisciplina.FirstOrDefault(ftd => ftd.Id == fechamentoTurmaDiscplina.Id);
                    if (fechamentoTurmaDisciplinaLista == null)
                    {
                        fechamentoTurmaDisciplinaLista = fechamentoTurmaDiscplina;
                        fechammentosTurmaDisciplina.Add(fechamentoTurmaDiscplina);
                    }
                    fechamentoTurmaDisciplinaLista.FechamentoAlunos.Add(fechamentoAluno);
                    return fechamentoTurmaDiscplina;
                }, new { turmaId, disciplinasId, bimestre });

            return fechammentosTurmaDisciplina;
        }

        public async Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosTurmaDisciplinas(string turmaCodigo, long[] disciplinasId, int bimestre = 0)
        {
            var turma = await repositorioTurma.ObterPorCodigo(turmaCodigo);

            if (turma == null)
                return Enumerable.Empty<FechamentoTurmaDisciplina>();

            return await ObterFechamentosTurmaDisciplinas(turma.Id, disciplinasId, bimestre);
        }

        public async Task<FechamentoTurmaDisciplina> ObterFechamentoTurmaDisciplina(string turmaCodigo, long disciplinaId, int bimestre = 0)
        {
            var query = new StringBuilder(@"select f.*
                         from fechamento_turma_disciplina f
                        inner join fechamento_turma ft on ft.id = f.fechamento_turma_id
                         left join periodo_escolar p on p.id = ft.periodo_escolar_id
                        inner join turma t on t.id = ft.turma_id
                        where not f.excluido
                          and t.turma_id = @turmaCodigo
                          and f.disciplina_id = @disciplinaId ");
            if (bimestre > 0)
                query.AppendLine(" and p.bimestre = @bimestre ");
            else
                query.AppendLine(" and ft.periodo_escolar_id is null ");

            return await database.Conexao.QueryFirstOrDefaultAsync<FechamentoTurmaDisciplina>(query.ToString(), new { turmaCodigo, disciplinaId, bimestre });
        }

        public async Task<IEnumerable<FechamentoNotaDto>> ObterNotasBimestre(string codigoAluno, long fechamentoTurmaDisciplinaId)
        {
            var query = @"select n.disciplina_id as DisciplinaId, n.nota as Nota, n.conceito_id as ConceitoId, aluno_codigo as CodigoAluno, n.sintese_id as SinteseId
                         from fechamento_nota n
                        inner join fechamento_aluno fa on fa.id = n.fechamento_aluno_id
                        where not n.excluido
                            and fa.fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId
                            and fa.aluno_codigo = @codigoAluno ";

            return await database.Conexao.QueryAsync<FechamentoNotaDto>(query, new { codigoAluno, fechamentoTurmaDisciplinaId });
        }

        public override FechamentoTurmaDisciplina ObterPorId(long id)
        {
            var query = @"select ftd.*, ft.* 
                            from fechamento_turma_disciplina ftd
                          inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id
                          where ftd.id = @id";

            return database.Conexao.Query<FechamentoTurmaDisciplina, FechamentoTurma, FechamentoTurmaDisciplina>(query
                , (fechamentoTurmaDisciplina, fechamentoTurma) =>
                {
                    fechamentoTurmaDisciplina.FechamentoTurma = fechamentoTurma;
                    return fechamentoTurmaDisciplina;
                }
                , new { id }).FirstOrDefault();

        }
    }
}