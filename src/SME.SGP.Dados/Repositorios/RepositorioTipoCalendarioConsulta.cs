using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTipoCalendarioConsulta : RepositorioBase<TipoCalendario>, IRepositorioTipoCalendarioConsulta
    {
        public RepositorioTipoCalendarioConsulta(ISgpContextConsultas conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }
        public async Task<PeriodoEscolar> ObterPeriodoEscolarPorCalendarioEData(long tipoCalendarioId, DateTime dataParaVerificar)
        {
            var query = @"select pe.*, tc.* 
                            from periodo_escolar pe
                            join tipo_calendario tc on tc.id = pe.tipo_calendario_id
                          where tc.id = @tipoCalendarioId
                                and @dataParaVerificar between symmetric pe.periodo_inicio::date and pe.periodo_fim::date";

            return (await database.Conexao.QueryAsync<PeriodoEscolar, TipoCalendario, PeriodoEscolar>(query, (pe, tc) =>
            {
                pe.AdicionarTipoCalendario(tc);
                return pe;

            }, new { tipoCalendarioId, dataParaVerificar }, splitOn: "id")).FirstOrDefault();
        }

        public async Task<TipoCalendario> BuscarPorAnoLetivoEModalidade(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre = 0)
        {
            var query = $@"select id, 
                                 ano_letivo as anoLetivo,
                                 nome,
                                 periodo,
                                 modalidade,
                                 situacao,
                                 semestre,
                                 migrado,
                                 criado_em as criadoEm,
                                 criado_por as criadoPor,
                                 criado_rf as criadoRf,
                                 alterado_em as alteradoEm,
                                 alterado_por as alteradoPor,
                                 alterado_rf as alteradoRf
                        from tipo_calendario 
                        where not excluido
                            and ano_letivo = @anoLetivo
                            and modalidade = @modalidade
                            {IncluirFiltroSemestrePorModalidade(modalidade, semestre)}";

            return await database.Conexao.QueryFirstOrDefaultAsync<TipoCalendario>(query, new { anoLetivo, modalidade = (int)modalidade, semestre });
        }

        private string IncluirFiltroSemestrePorModalidade(ModalidadeTipoCalendario modalidade,int semestre)
        {
            return modalidade.EhEjaOuCelp() && semestre.EhMaiorQueZero() 
                ? ObterFiltroSemestre() 
                : string.Empty;
        }
        
        private string IncluirFiltroSemestre(int semestre)
        {
            return semestre.EhMaiorQueZero() ? ObterFiltroSemestre() : string.Empty;
        }

        private static string ObterFiltroSemestre()
        {
            return " and semestre = @semestre";
        }

        public override TipoCalendario ObterPorId(long id)
        {
            var query = $@"select id, 
                                 ano_letivo as anoLetivo,
                                 nome,
                                 periodo,
                                 modalidade,
                                 situacao,
                                 semestre,
                                 migrado,
                                 criado_em as criadoEm,
                                 criado_por as criadoPor,
                                 criado_rf as criadoRf,
                                 alterado_em as alteradoEm,
                                 alterado_por as alteradoPor,
                                 alterado_rf as alteradoRf
                        from tipo_calendario 
                        where id = @id ";

            return database.Conexao.QueryFirstOrDefault<TipoCalendario>(query, new { id });
        }

        public async Task<IEnumerable<TipoCalendario>> ObterTiposCalendario()
        {
            var query = $@"select id, 
                                 ano_letivo as anoLetivo,
                                 nome,
                                 periodo,
                                 modalidade,
                                 situacao,
                                 semestre,
                                 migrado,
                                 criado_em as criadoEm,
                                 criado_por as criadoPor,
                                 criado_rf as criadoRf,
                                 alterado_em as alteradoEm,
                                 alterado_por as alteradoPor,
                                 alterado_rf as alteradoRf
                        from tipo_calendario 
                        where not excluido";

            var retorno =  await database.Conexao.QueryAsync<TipoCalendario>(query);
            return retorno;
        }

        public async Task<bool> VerificarRegistroExistente(long id, string nome)
        {
            var query = $@"select count(id) 
                          from tipo_calendario 
                          where upper(nome) = @nomeMaiusculo 
                          and not excluido
                          {IncluirFiltroPorId(id)}";

            return (await database.Conexao.QueryFirstOrDefaultAsync<int>(query, new { id, nomeMaiusculo =  nome.ToUpper().Trim()})).EhMaiorQueZero();
        }

        private string IncluirFiltroPorId(long id)
        {
            return id.EhMaiorQueZero() ? "and id <> @id" : string.Empty;
        }

        public async Task<bool> PeriodoEmAberto(long tipoCalendarioId, DateTime dataReferencia, int bimestre = 0, bool ehAnoLetivo = false)
        {
            var query = $@"select count(pe.Id)
                             from periodo_escolar pe 
                          where pe.tipo_calendario_id = @tipoCalendarioId
                               and periodo_fim::date >= @dataReferencia::date 
                               {IncluirFiltroDataReferencia(!ehAnoLetivo)}
                               {IncluirFiltroBimestre(bimestre)}";

            return (await database.Conexao.QueryFirstAsync<int>(query.ToString(), new { tipoCalendarioId, dataReferencia, bimestre })).EhMaiorQueZero();
        }

        private string IncluirFiltroBimestre(int bimestre)
        {
            return bimestre.EhMaiorQueZero() ? " and pe.bimestre = @bimestre " : string.Empty;
        }

        private string IncluirFiltroDataReferencia(bool ehPorDataReferencia)
        {
            return ehPorDataReferencia ? " and periodo_inicio <= @dataReferencia " : string.Empty;
        }

        public async Task<long> ObterIdPorAnoLetivoEModalidadeAsync(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre = 0)
        {
            var query = $@"select id
                            from tipo_calendario t
                          where not t.excluido
                            and t.ano_letivo = @anoLetivo
                            and t.modalidade = @modalidade
                            and t.situacao 
                            {IncluirFiltroSemestrePorModalidade(modalidade, semestre)}";

            var retorno = await database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { anoLetivo, modalidade = (int)modalidade, semestre });
            return retorno;
        }

        public async Task<IEnumerable<TipoCalendario>> ListarPorAnoLetivoEModalidades(int anoLetivo, int[] modalidades, int semestre = 0)
        {
            var query = $@"select id, 
                             ano_letivo as anoLetivo,
                             nome,
                             periodo,
                             modalidade,
                             situacao,
                             semestre,
                             migrado,
                             criado_em as criadoEm,
                             criado_por as criadoPor,
                             criado_rf as criadoRf,
                             alterado_em as alteradoEm,
                             alterado_por as alteradoPor,
                             alterado_rf as alteradoRf
                    from tipo_calendario 
                    where not excluido
                          and ano_letivo = @anoLetivo
                          and modalidade = any(@modalidades) 
                          {IncluirFiltroSemestre(semestre)}";

            return await database.Conexao.QueryAsync<TipoCalendario>(query, new { anoLetivo, modalidades, semestre });
        }

        public async Task<IEnumerable<TipoCalendarioRetornoDto>> ListarPorAnoLetivoDescricaoEModalidades(int anoLetivo, string descricao, IEnumerable<int> modalidades)
        {
            var query = new StringBuilder(@"select tc.id,
                                                      tc.ano_letivo as AnoLetivo,             
                                                      tc.nome,
                                                      tc.ano_letivo ||' - '|| tc.nome as descricao,
                                                      tc.modalidade,
                                                      tc.semestre
                                              from tipo_calendario tc
                                             where not tc.excluido
                                               and tc.ano_letivo = @anoLetivo ");

            if (descricao.EstaPreenchido())
                query.AppendLine("and UPPER(ano_letivo ||' - '|| nome) like UPPER('%{descricao}%') ");

            if (modalidades.Any() && !modalidades.Any(c => c == -99))
                query.AppendLine("and modalidade = any(@modalidades)");

            return await database.Conexao.QueryAsync<TipoCalendarioRetornoDto>(query.ToString(), new { anoLetivo, descricao, modalidades });
        }

        public async Task<IEnumerable<TipoCalendarioBuscaDto>> ObterTiposCalendarioPorDescricaoAsync(string descricao)
        {
            var query = $@"select tc.id, 
                                     tc.ano_letivo,
                                     tc.nome,
                                     tc.modalidade,
                                     tc.ano_letivo ||' - '|| tc.nome as descricao,
                                     tc.migrado,
                                     tc.periodo,
                                     tc.situacao,
                                     tc.semestre,
                                     fr.aplicacao 
                                from tipo_calendario tc
                                left join fechamento_reabertura fr on tc.id = fr.tipo_calendario_id 
                               where UPPER(tc.ano_letivo ||' - '|| tc.nome) like UPPER('%{descricao}%')
                                 and not tc.excluido
                                 and not fr.excluido
                               order by descricao desc
                               limit 10";

            return await database.Conexao.QueryAsync<TipoCalendarioBuscaDto>(query);
        }

        public async Task<string> ObterNomePorId(long tipoCalendarioId)
        {
            var query = "select nome from tipo_calendario where id = @tipoCalendarioId";

            return await database.Conexao.QueryFirstAsync<string>(query, new { tipoCalendarioId });
        }

        public async Task<IEnumerable<TipoCalendarioBuscaDto>> ListarPorAnosLetivoEModalidades(int[] anosLetivo, int[] modalidades, string nome)
        {
            var query = $@"select *, ano_letivo ||' - '|| nome as descricao
                           from tipo_calendario
                           where not excluido
                                and ano_letivo = any(@anosLetivo)
                                and modalidade = any(@modalidades)
                                {IncluirFiltroPorNome(nome)}
                           order by ano_letivo desc ";

            var retorno = await database.Conexao.QueryAsync<TipoCalendarioBuscaDto>(query, new { anosLetivo, modalidades });
            return retorno;
        }

        private string IncluirFiltroPorNome(string nome)
        {
            return nome.EstaPreenchido() ? $"and upper(f_unaccent(nome)) like upper(f_unaccent('%{nome}%'))" : string.Empty;
        }

        public async Task<IEnumerable<PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto>> ObterPeriodoTipoCalendarioBimestreAsync(int anoLetivo, ModalidadeTipoCalendario modalidadeTipoCalendario, int semestre = 0)
        {
            var query = $@"select
                            pe.id as periodoEscolarId,
                            pe.bimestre,
                            pe.periodo_inicio as PeriodoInicio,
                            pe.periodo_fim as PeriodoFim
                        from tipo_calendario tc
                        join periodo_escolar pe on pe.tipo_calendario_id = tc.id
                        where
                            tc.ano_letivo = @anoLetivo
                            and tc.modalidade = @modalidadeTipoCalendario
                            and not tc.excluido 
                            {IncluirFiltroSemestrePorModalidade(modalidadeTipoCalendario, semestre)}";

            return await database.Conexao.QueryAsync<PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto>(query, new { anoLetivo, modalidadeTipoCalendario = (int)modalidadeTipoCalendario, semestre });
        }

        public async Task<long> ObterTipoCalendarioIdPorAnoLetivoModalidadeEDataReferencia(int anoLetivo, ModalidadeTipoCalendario modalidadeTipoCalendarioId, DateTime dataReferencia)
        {
            var query = @"select tc.id
                            from tipo_calendario tc 
                           inner join periodo_escolar pe on pe.tipo_calendario_id = tc.id 
                           where tc.modalidade = @modalidadeTipoCalendarioId
                             and tc.ano_letivo = @anoLetivo
                             and not tc.excluido 
                             and @dataReferencia::date between pe.periodo_inicio::date and pe.periodo_fim::date ";

            var parametros = new
            {
                anoLetivo,
                modalidadeTipoCalendarioId,
                dataReferencia
            };

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query, parametros);
        }

        public async Task<int> ObterAnoLetivoUltimoTipoCalendarioPorDataReferencia(int anoReferencia, ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            var sqlQuery = @"select ano_letivo
                                from tipo_calendario tc 
                             where ano_letivo < @anoReferencia and
                                   modalidade = @modalidadeTipoCalendario
                             order by ano_letivo desc
                             limit 1;";

            return await database.Conexao
                .QueryFirstOrDefaultAsync<int>(sqlQuery, new
                {
                    anoReferencia,
                    modalidadeTipoCalendario
                });
        }
    }
}