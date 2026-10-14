using Dapper;
using SME.SGP.Infra;
using System.Data;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.NotasFechamentoConselho
{
    // Referência independente anterior à implementação: 70930e2d21 (produto igual a abe4ac44e2).
    // Strings SQL preservadas; não usar consultas do working tree como oráculo.
    internal static class FontesNotasFechamentoConselhoReferenciaAnterior
    {
        private const string Fechamento = @"select * from (
                select pe.bimestre, 
                       fn.disciplina_id as ComponenteCurricularCodigo, 
                       fn.conceito_id as ConceitoId, 
                       fn.nota as Nota,
                       fa.aluno_codigo as AlunoCodigo,
                       t.turma_id TurmaCodigo,
                       row_number() over (partition by t.id, fa.aluno_codigo, pe.id, fn.disciplina_id order by fn.id desc) sequencia
                  from fechamento_turma ft
                  left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                 inner join turma t on t.id = ft.turma_id 
                 inner join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id
                 inner join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
                 inner join fechamento_nota fn on fn.fechamento_aluno_id = fa.id 
                 where t.turma_id = @turmaCodigo
                   {condicaoTipoCalendario}
                   {condicaoBimestre}
                   {condicaoDataMatricula}
                   {condicaoDataSituacao}
                   {condicaoAlunoCodigo}
            ) x where sequencia = 1";
        private const string Conselho = @"select distinct * from (
                select pe.bimestre,
                    ccn.componente_curricular_codigo as ComponenteCurricularCodigo,
                    cc.id as ConselhoClasseId,
                    ccn.id as ConselhoClasseNotaId,
                    ccn.conceito_id as ConceitoId,
                    ccn.nota as Nota,
                    cca.aluno_codigo as AlunoCodigo,
                    t.turma_id as TurmaCodigo
                  from fechamento_turma ft
                  left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                 inner join turma t on t.id = ft.turma_id 
                 inner join conselho_classe cc on cc.fechamento_turma_id = ft.id
                 inner join conselho_classe_aluno cca on cca.conselho_classe_id  = cc.id
                 inner join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id and not ccn.excluido
                 where t.turma_id = @turmaCodigo
                   {condicaoTipoCalendario}
                   {condicaoBimestre}
                   {condicaoDataMatricula}
                   {condicaoDataSituacao}
                   {condicaoAlunoCodigo}
            ) x ";

        internal static async Task<NotasFechamentoConselhoDto> ObterAsync(IDbConnection conexao, string turmaCodigo,
            int bimestre, long? tipoCalendario, string alunoCodigo)
        {
            string Montar(string sql, bool conselho)
            {
                var filtroBimestre = conselho
                    ? (bimestre == 0 ? "" : bimestre > 0 ? "and bimestre = @bimestre" : "and ft.periodo_escolar_id is null")
                    : (bimestre > 0 ? "and bimestre = @bimestre" : "");
                return sql.Replace("{condicaoBimestre}", filtroBimestre)
                    .Replace("{condicaoTipoCalendario}", tipoCalendario.HasValue ? "and (pe.tipo_calendario_id =@tipoCalendario or pe.tipo_calendario_id is null)" : "")
                    .Replace("{condicaoAlunoCodigo}", string.IsNullOrEmpty(alunoCodigo) ? "" : conselho ? "and cca.aluno_codigo = @alunoCodigo" : "and fa.aluno_codigo = @alunoCodigo")
                    .Replace("{condicaoDataMatricula}", "").Replace("{condicaoDataSituacao}", "");
            }
            var parametros = new { turmaCodigo, bimestre, tipoCalendario, alunoCodigo };
            return new NotasFechamentoConselhoDto {
                Fechamento = await SqlMapper.QueryAsync<NotaConceitoBimestreComponenteDto>(conexao, Montar(Fechamento, false), parametros),
                Conselho = await SqlMapper.QueryAsync<NotaConceitoBimestreComponenteDto>(conexao, Montar(Conselho, true), parametros)
            };
        }
    }
}
