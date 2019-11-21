using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAula : RepositorioBase<Aula>, IRepositorioAula
    {
        public RepositorioAula(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaDisciplina(string turma, string disciplina)
        {
            var query = @"select professor_rf, quantidade, data_aula 
                 from aula 
                where not excluido 
                  and turma_id = @turma 
                  and disciplina_id = @disciplina";

            return await database.Conexao.QueryAsync<AulasPorTurmaDisciplinaDto>(query, new
            {
                turma,
                disciplina
            });
        }

        public IEnumerable<AulaConsultaDto> ObterDatasDeAulasPorCalendarioTurmaEDisciplina(long calendarioId, string turmaId, string disciplinaId, long usuarioId, Guid perfil)
        {
            var query = @"select distinct
	                        a.*
                        from
	                        aula a
                        inner join v_abrangencia v on
	                        a.turma_id = v.turma_id
                        where
	                        not a.excluido
	                        and v.usuario_id = @usuarioId
	                        and v.usuario_perfil = @perfil
	                        and a.turma_id = @turmaId
	                        and a.disciplina_id = @disciplinaId
                            and a.tipo_calendario_id = @calendarioId";

            return database.Conexao.Query<AulaConsultaDto>(query, new
            {
                usuarioId,
                perfil,
                calendarioId,
                turmaId,
                disciplinaId
            });
        }

        public bool UsuarioPodeCriarAulaNaUeTurmaEModalidade(Aula aula, ModalidadeTipoCalendario modalidade)
        {
            var query = new StringBuilder("select 1 from v_abrangencia where turma_id = @turmaId and ue_codigo = @ueId ");

            if (modalidade == ModalidadeTipoCalendario.EJA)
            {
                query.AppendLine($"and modalidade_codigo = {(int)Modalidade.EJA} ");
            }
            else
            {
                query.AppendLine($"and (modalidade_codigo = {(int)Modalidade.Fundamental} or modalidade_codigo = {(int)Modalidade.Medio}) ");
            }

            return database.Conexao.QueryFirstOrDefault<bool>(query.ToString(), new
            {
                aula.TurmaId,
                aula.UeId
            });
        }
    }
}