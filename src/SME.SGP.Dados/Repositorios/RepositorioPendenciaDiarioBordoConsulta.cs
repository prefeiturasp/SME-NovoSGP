using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaDiarioBordoConsulta : IRepositorioPendenciaDiarioBordoConsulta
    {
        private readonly ISgpContextConsultas database;
        public RepositorioPendenciaDiarioBordoConsulta(ISgpContextConsultas database)
        {
            this.database = database;
        }
        public async Task<long> ExisteIdPendenciaDiarioBordo(long aulaId, long componenteCurricularId)
        {
            var sql = "select pendencia_id from pendencia_diario_bordo where aula_id = @aulaId and componente_curricular_id = @componenteCurricularId ";
            return await database.Conexao.QueryFirstOrDefaultAsync<long>(sql, new { aulaId, componenteCurricularId }, commandTimeout: 60);
        }

        public async Task<IEnumerable<PendenciaUsuarioDto>> ObterIdPendenciaDiarioBordoPorAulaId(long aulaId)
        {
            var sql = @"select pdb.pendencia_id as PendenciaId, u.id as UsuarioId 
                                from pendencia_diario_bordo pdb
                                inner join usuario u on u.rf_codigo = pdb.professor_rf
                                where pdb.aula_id = @aulaId";
            return await database.Conexao.QueryAsync<PendenciaUsuarioDto>(sql, new { aulaId}, commandTimeout: 60);
        }

        public async Task<IEnumerable<PendenciaDiarioBordoDescricaoDto>> ObterPendenciasDiarioPorPendencia(long pendenciaId, string codigoRf)
        {
            var query = @"select distinct a.data_aula as DataAula, coalesce(cc.descricao_infantil , cc.descricao_sgp, cc.descricao) as ComponenteCurricular, 
                                pe.bimestre, pdb.pendencia_id as PendenciaId, t.modalidade_codigo ModalidadeCodigo, t.nome NomeTurma, (a.tipo_aula = @tipoAulaReposicao) ehReposicao 
                        from pendencia_diario_bordo pdb
                        join aula a on a.id = pdb.aula_id
                        join componente_curricular cc on cc.id = pdb.componente_curricular_id
                        join periodo_escolar pe on pe.tipo_calendario_id = a.tipo_calendario_id and pe.periodo_inicio <= a.data_aula and pe.periodo_fim >= a.data_aula
                        join turma t on t.turma_id = a.turma_id 
                        where pdb.pendencia_id = @pendenciaId and pdb.professor_rf = @codigoRf
                        order by a.data_aula desc";

            return await database.Conexao.QueryAsync<PendenciaDiarioBordoDescricaoDto>(query, new { pendenciaId, codigoRf, tipoAulaReposicao = (int)TipoAula.Reposicao });
        }
    }
}
