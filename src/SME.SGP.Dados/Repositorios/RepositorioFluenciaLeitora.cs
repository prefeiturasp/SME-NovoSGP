using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFluenciaLeitora : RepositorioBase<FluenciaLeitora>, IRepositorioFluenciaLeitora
    {
        public RepositorioFluenciaLeitora(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<FluenciaLeitora> ObterRegistroFluenciaLeitoraAsync(int anoLetivo, string codigoEOLTurma, string codigoEOLAluno, int tipoAvaliacao)
        {
            string query = @"select * from fluencia_leitora
                            where ano_letivo = @anoLetivo
                            and codigo_eol_turma = @codigoEOLTurma
                            and codigo_eol_aluno = @codigoEOLAluno
                            and tipo_avaliacao = @tipoAvaliacao;";

            return database.Conexao.QueryFirstOrDefault<FluenciaLeitora>(query, new
            {
                anoLetivo,
                codigoEOLTurma,
                codigoEOLAluno,
                tipoAvaliacao
            });
        }

        public async Task<IEnumerable<PainelEducacionalRegistroFluenciaLeitoraDto>> ObterRegistroFluenciaLeitoraGeralAsync()
        {
            int anoMinimoConsulta = 2019;

            string query = @"SELECT 
                                 fl.id as IdFluencia,
                                 fl.fluencia as codigofluencia, 
                                 fl.ano_letivo as anoletivo, 
                                 fl.tipo_avaliacao as Periodo,
                                 u.nome as UeNome,
                                 u.ue_id as UeCodigo,
                                 d.nome as DreNome,
                                 d.dre_id as DreCodigo,
                                 t.nome as TurmaNome
                           FROM fluencia_leitora fl
                           LEFT JOIN turma t on t.turma_id = fl.codigo_eol_turma
                           LEFT JOIN ue u on t.ue_id = u.ue_id::integer
                           LEFT JOIN dre d on d.id::integer = u.dre_id
                           WHERE fl.ano_letivo > @anoMinimoConsulta ";

            return await database.Conexao.QueryAsync<PainelEducacionalRegistroFluenciaLeitoraDto>(query, new { anoMinimoConsulta });
        }
    }
}
