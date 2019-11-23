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

        public async Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaDisciplinaSemana(string turma, string disciplina, string semana)
        {
            var query = @"select professor_rf, quantidade, data_aula
                 from aula
                where not excluido
                  and turma_id = @turma
                  and disciplina_id = @disciplina
                  and to_char(data_aula, 'IW') = @semana";

            return await database.Conexao.QueryAsync<AulasPorTurmaDisciplinaDto>(query, new
            {
                turma,
                disciplina,
                semana
            });
        }

        public IEnumerable<AulaConsultaDto> ObterDatasDeAulasPorAnoTurmaEDisciplina(int anoLetivo, string turmaId, string disciplinaId, long usuarioId, string usuarioRF, Guid perfil)
        {
            var query = new StringBuilder("select distinct a.*");
            query.AppendLine("from aula a ");
            query.AppendLine("inner join v_abrangencia v on ");
            query.AppendLine("a.turma_id = v.turma_id ");
            query.AppendLine("inner join tipo_calendario t on ");
            query.AppendLine("a.tipo_calendario_id = t.id ");
            query.AppendLine("where ");
            query.AppendLine("not a.excluido ");
            query.AppendLine("and v.usuario_id = @usuarioId ");
            query.AppendLine("and v.usuario_perfil = @perfil ");
            query.AppendLine("and a.turma_id = @turmaId ");
            query.AppendLine("and a.disciplina_id = @disciplinaId ");
            query.AppendLine("and t.ano_letivo = @anoLetivo");

            if (!string.IsNullOrWhiteSpace(usuarioRF))
            {
                query.AppendLine("and a.professor_rf = @usuarioRF ");
            }

            return database.Conexao.Query<AulaConsultaDto>(query.ToString(), new
            {
                usuarioRF,
                usuarioId,
                perfil,
                anoLetivo,
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