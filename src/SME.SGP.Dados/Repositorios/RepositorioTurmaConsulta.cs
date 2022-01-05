using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTurmaConsulta : IRepositorioTurmaConsulta
    {
        private readonly ISgpContextConsultas contexto;

        public RepositorioTurmaConsulta(ISgpContextConsultas contexto)
        {
            this.contexto = contexto;
        }

        public async Task<Turma> ObterPorCodigo(string turmaCodigo)
        {
            return await contexto.Conexao.QueryFirstOrDefaultAsync<Turma>("select * from turma where turma_id = @turmaCodigo", new { turmaCodigo });
        }

        public async Task<string> ObterTurmaCodigoPorConselhoClasseId(long conselhoClasseId)
        {
            var query = @"select t.turma_id
                          from conselho_classe cc
                          inner join fechamento_turma ft on ft.id = cc.fechamento_turma_id
                          inner join turma t on t.id = ft.turma_id
                         where not cc.excluido and not ft.excluido
                           and cc.id = @conselhoClasseId";

            return await contexto.Conexao.QueryFirstOrDefaultAsync<string>(query, new { conselhoClasseId });
        }

        public async Task<long> ObterTurmaIdPorCodigo(string turmaCodigo)
        {
            return await contexto.QueryFirstOrDefaultAsync<long>("select id from turma where turma_id = @turmaCodigo", new { turmaCodigo });
        }

        public async Task<Turma> ObterPorId(long id)
        {
            return await contexto.QueryFirstOrDefaultAsync<Turma>("select * from turma where id = @id", new { id });
        }

        public async Task<IEnumerable<Turma>> ObterPorCodigosAsync(string[] codigos)
        {
            var query = "select * from turma t where t.turma_id = ANY(@codigos)";

            return await contexto.Conexao.QueryAsync<Turma>(query, new { codigos });
        }

        public async Task<ObterTurmaSimplesPorIdRetornoDto> ObterTurmaSimplesPorId(long id)
        {
            var query = "select t.id, t.turma_id as codigo, t.nome from turma t where t.id = @id";
            return await contexto.Conexao.QueryFirstOrDefaultAsync<ObterTurmaSimplesPorIdRetornoDto>(query, new { id });
        }

        public async Task<IEnumerable<Turma>> ObterTurmasInfantilNaoDeProgramaPorAnoLetivoAsync(int anoLetivo, string codigoTurma = null)
        {
            var modalidade = Modalidade.EducacaoInfantil;
            var turmas = new List<Turma>();
            var query = $@"select
	                            t.*,
	                            u.*,
                                d.*
                            from
	                            turma t
                            inner join ue u on
	                            u.id = t.ue_id
                            inner join dre d on
	                            u.dre_id = d.id
                            where
	                            t.modalidade_codigo = :modalidade
	                            and t.historica = false
	                            and t.ano_letivo = :anoLetivo
	                            and ano ~ E'^[0-9\.]+$'
                                {(!string.IsNullOrEmpty(codigoTurma) ? " and t.turma_id = :codigoTurma" : string.Empty)}";

            await contexto.Conexao.QueryAsync<Turma, Ue, Dre, Turma>(query, (turma, ue, dre) =>
            {
                ue.AdicionarDre(dre);
                turma.AdicionarUe(ue);

                var turmaExistente = turmas.FirstOrDefault(c => c.Id == turma.Id);
                if (turmaExistente == null)
                    turmas.Add(turma);

                return turma;
            }, new { anoLetivo, modalidade, codigoTurma });

            return turmas;
        }

        public async Task<IEnumerable<Turma>> ObterTurmasPorAnoLetivoModalidade(int anoLetivo, Modalidade[] modalidades)
        {

            var query = new StringBuilder(@"select distinct * from turma t
                                            where  t.ano_letivo = @anoLetivo and t.modalidade_codigo = ANY(@modalidades)");

            return await contexto.Conexao.QueryAsync<Turma>(query.ToString(), new { anoLetivo, modalidades = modalidades.Cast<int>().ToArray() });

        }

        public async Task<IEnumerable<Turma>> ObterTurmasCompletasPorAnoLetivoModalidade(int anoLetivo, Modalidade[] modalidades, string turmaCodigo = "")
        {
            var query = @"select turma.*, ue.*, dre.* 
                            from turma
                        inner join ue on ue.id = turma.ue_id
                        inner join dre on dre.id = ue.dre_id
                        where 
                            turma.ano_letivo = @anoLetivo
                            and turma.modalidade_codigo = any(@modalidades) ";

            if (!string.IsNullOrEmpty(turmaCodigo))
                query += " and turma.turma_id = @turmaCodigo ";

            return await contexto.QueryAsync<Turma, Ue, Dre, Turma>(query, (turma, ue, dre) =>
            {
                ue.AdicionarDre(dre);
                turma.AdicionarUe(ue);
                return turma;
            }, new { modalidades = modalidades.Cast<int>().ToArray(), anoLetivo, turmaCodigo });

        }

        public async Task<IEnumerable<Turma>> ObterTurmasComFechamentoOuConselhoNaoFinalizados(long ueId, int anoLetivo, long? periodoEscolarId, int[] modalidades, int semestre)
        {
            var joinFechamentoTurma = periodoEscolarId.HasValue ?
                "left join fechamento_turma ft on ft.turma_id = t.id and ft.periodo_escolar_id = @periodoEscolarId" :
                "left join fechamento_turma ft on ft.turma_id = t.id and ft.periodo_escolar_id is null";

            var query = $@"select distinct t.*
                          from turma t
                        inner join ue on ue.id = t.ue_id
                        inner join dre on dre.id = ue.dre_id
                         {joinFechamentoTurma}
                         left join fechamento_turma_disciplina d on d.fechamento_turma_id = ft.id
                         left join conselho_classe cc on cc.fechamento_turma_id = ft.id
                         where t.ue_id = @ueId
                           and t.ano_letivo  = @anoLetivo
                           and t.modalidade_codigo = ANY(@modalidades)
                           and t.ano between '1' and '9'
                           and (t.semestre = 0 or t.semestre = @semestre)
                           and (d.situacao in (1,2) 
   	                         or d.id is null 
   	                         or cc.id is null 
   	                         or cc.situacao = 1)";

            return await contexto.Conexao.QueryAsync<Turma>(query, new { ueId, anoLetivo, periodoEscolarId, modalidades, semestre });
        }

        public async Task<IEnumerable<Turma>> ObterTurmasComInicioFechamento(long ueId, long periodoEscolarId, int[] modalidades)
        {
            var query = @"select t.*
                          from turma t
                        inner join ue on ue.id = t.ue_id
                        inner join dre on dre.id = ue.dre_id
                        left join fechamento_turma ft on ft.turma_id = t.id and ft.periodo_escolar_id = @periodoEscolarId
                        left join fechamento_turma_disciplina d on d.fechamento_turma_id = ft.id
                        where t.ue_id = @ueId
                           and t.modalidade_codigo = ANY(@modalidades)";

            return await contexto.Conexao.QueryAsync<Turma>(query, new { ueId, periodoEscolarId, modalidades });

        }

        public async Task<IEnumerable<Turma>> ObterTurmasPorUeModalidadesAno(long ueId, int[] modalidades, int ano)
        {
            var query = @"select turma.*, ue.*, dre.* 
                         from turma
                        inner join ue on ue.id = turma.ue_id
                        inner join dre on dre.id = ue.dre_id
                        where turma.ue_id = @ueId
                          and turma.ano_letivo = @ano
                          and turma.modalidade_codigo = any(@modalidades) ";

            return await contexto.QueryAsync<Turma, Ue, Dre, Turma>(query, (turma, ue, dre) =>
            {
                ue.AdicionarDre(dre);
                turma.AdicionarUe(ue);
                return turma;
            }, new { ueId, modalidades, ano });
        }

        public async Task<IEnumerable<Turma>> ObterTurmasPorIds(long[] turmasIds)
        {
            var query = @"select *
                         from turma
                        where id = any(@turmasIds)";

            return await contexto.Conexao.QueryAsync<Turma>(query, new { turmasIds });
        }

        public async Task<Modalidade> ObterModalidadePorCodigo(string turmaCodigo)
        {
            var query = "select modalidade_codigo from turma where turma_id = @turmaCodigo";

            return await contexto.Conexao.QueryFirstOrDefaultAsync<Modalidade>(query, new { turmaCodigo });
        }

        public async Task<DreUeDaTurmaDto> ObterCodigosDreUe(string turmaCodigo)
        {
            var query = @"select ue.ue_id as ueCodigo, dre.dre_id as dreCodigo
                          from turma t 
                         inner join ue on ue.id = t.ue_id
                         inner join dre on dre.id = ue.dre_id 
                         where t.turma_id = @turmaCodigo";

            return await contexto.Conexao.QueryFirstOrDefaultAsync<DreUeDaTurmaDto>(query, new { turmaCodigo });
        }

        public async Task<DreUeDaTurmaDto> ObterCodigosDreUePorId(long turmaId)
        {
            var query = @"select ue.ue_id as ueCodigo, dre.dre_id as dreCodigo
                          from turma t 
                         inner join ue on ue.id = t.ue_id
                         inner join dre on dre.id = ue.dre_id 
                         where t.id = @turmaId";

            return await contexto.Conexao.QueryFirstOrDefaultAsync<DreUeDaTurmaDto>(query, new { turmaId });
        }

        public async Task<Turma> ObterTurmaPorAnoLetivoModalidadeTipoAsync(long ueId, int anoLetivo, TipoTurma turmaTipo)
        {
            var query = @"select * from turma t 
                            where t.ue_id = @ueId 
                            and t.ano_letivo = @anoLetivo 
                            and t.tipo_turma = @turmaTipo";

            return await contexto.Conexao.QueryFirstOrDefaultAsync<Turma>(query, new { ueId, anoLetivo, turmaTipo });
        }

        public async Task<PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>> ObterTurmasFechamentoAcompanhamento(Paginacao paginacao, long dreId, long ueId, string[] turmasCodigo, Modalidade modalidade, int semestre, int bimestre, int anoLetivo, int? situacaoFechamento, int? situacaoConselhoClasse, bool listarTodasTurmas)
        {
            var query = new StringBuilder(@"select distinct t.id as TurmaId,
                                                     t.nome       
                                                from turma t 
                                               inner join ue on ue.id = t.ue_id
                                               inner join dre on dre.id = ue.dre_id
                                               inner join tipo_calendario tc 
                                                  on t.ano_letivo = tc.ano_letivo and tc.modalidade = @modalidadeTipoCalendario
                                               left join periodo_escolar pe
	                                              on tc.id = pe.tipo_calendario_id
                                               where dre.id = @dreId
                                                 and ue.id = @ueId and t.tipo_turma in (1,2,7) ");

            if (!listarTodasTurmas)
                query.AppendLine("and t.turma_id = ANY(@turmasCodigo) ");

            if (bimestre > 0)
                query.AppendLine("and pe.bimestre = @bimestre ");

            var querySituacao = new StringBuilder();
            if (situacaoFechamento.HasValue && situacaoFechamento.Value > -99)
            {
                querySituacao.AppendLine(@"and t.id in (select turma_id from consolidado_fechamento_componente_turma 
                                   where not excluido and turma_id = t.id and status = @situacaoFechamento  ");

                if (bimestre > 0)
                    querySituacao.AppendLine("and bimestre = @bimestre ");

                querySituacao.AppendLine(")");
            }

            if (situacaoConselhoClasse.HasValue && situacaoConselhoClasse.Value > -99)
            {
                querySituacao.AppendLine(@"and t.id in (select turma_id from consolidado_conselho_classe_aluno_turma 
                                   where not excluido and turma_id = t.id and status = @situacaoConselhoClasse  ");

                if (bimestre > 0)
                    querySituacao.AppendLine("and bimestre = @bimestre ");

                querySituacao.AppendLine(")");
            }

            query.AppendLine(querySituacao.ToString());

            DateTime dataReferencia = DateTime.MinValue;
            string queryPeriodoEJA = string.Empty;
            if (modalidade == Modalidade.EJA)
            {
                var periodoReferencia = semestre == 1 ? "p.periodo_inicio < @dataReferencia" : "p.periodo_fim > @dataReferencia";
                queryPeriodoEJA = $"and exists(select 0 from periodo_escolar p where p.tipo_calendario_id = tc.id and {periodoReferencia})";
                query.AppendLine(queryPeriodoEJA);

                dataReferencia = new DateTime(anoLetivo, semestre == 1 ? 6 : 8, 1);
            }

            query.AppendLine(@" and t.modalidade_codigo = @modalidade
                                and t.ano_letivo = @anoLetivo
                                and not t.historica 
                                and t.tipo_turma in (1,2,7)
                            order by t.nome
                            OFFSET @quantidadeRegistrosIgnorados ROWS FETCH NEXT @quantidadeRegistros ROWS ONLY; ");

            query.AppendLine(@"select count(distinct (t.id))
                                    from turma t 
                                inner join ue on ue.id = t.ue_id
                                inner join dre on dre.id = ue.dre_id
                                inner join tipo_calendario tc 
                                    on t.ano_letivo = tc.ano_letivo and tc.modalidade = @modalidadeTipoCalendario
                                left join periodo_escolar pe
	                                on tc.id = pe.tipo_calendario_id
                                where dre.id = @dreId
                                    and ue.id = @ueId ");

            if (!listarTodasTurmas)
                query.AppendLine("and t.turma_id = ANY(@turmasCodigo) ");

            if (bimestre > 0)
                query.AppendLine("and pe.bimestre = @bimestre ");

            if (modalidade == Modalidade.EJA)
                query.AppendLine(queryPeriodoEJA);

            query.AppendLine(querySituacao.ToString());

            query.AppendLine(@"and t.modalidade_codigo = @modalidade 
                               and t.ano_letivo = @anoLetivo
                               and not t.historica
                               and t.tipo_turma in (1,2,7)");


            var retorno = new PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>();

            var parametros = new
            {
                quantidadeRegistrosIgnorados = paginacao.QuantidadeRegistrosIgnorados,
                quantidadeRegistros = paginacao.QuantidadeRegistros,
                turmasCodigo,
                dreId,
                ueId,
                modalidadeTipoCalendario = (int)modalidade.ObterModalidadeTipoCalendario(),
                modalidade,
                semestre,
                bimestre,
                anoLetivo,
                situacaoFechamento,
                situacaoConselhoClasse,
                dataReferencia
            };

            var multi = await contexto.Conexao.QueryMultipleAsync(query.ToString(), parametros);

            retorno.Items = multi.Read<TurmaAcompanhamentoFechamentoRetornoDto>();
            retorno.TotalRegistros = multi.ReadFirst<int>();
            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        public async Task<IEnumerable<ModalidadesPorAnoDto>> ObterModalidadesPorAnos(int anoLetivo, long dreId, long ueId, int modalidade, int semestre)
        {

            var query = new StringBuilder(@"select 
                            distinct modalidade_codigo as modalidade, 
                            ano
                         from turma t
                         inner join ue u on t.ue_id = u.id
                         inner join dre d on d.id = u.dre_id
                         where ano in ('1', '2', '3', '4', '5', '6', '7', '8', '9') and
                         ano_letivo = @anoLetivo  
                         ");

            if (dreId > 0)
                query.AppendLine(" and d.id = @dreId ");

            if (ueId > 0)
                query.AppendLine(" and u.id  = @ueId ");

            if (modalidade > 0)
                query.AppendLine(" and t.modalidade_codigo = @modalidade ");

            if (semestre >= 0)
                query.AppendLine(" and t.semestre = @semestre ");

            query.AppendLine("order by ano");

            return await contexto.Conexao.QueryAsync<ModalidadesPorAnoDto>(query.ToString(), new { anoLetivo, dreId, ueId, modalidade, semestre });
        }


        public async Task<IEnumerable<GraficoBaseDto>> ObterInformacoesEscolaresTurmasAsync(int anoLetivo, long dreId, long ueId, AnoItinerarioPrograma[] anos, Modalidade modalidade, int? semestre)
        {
            var tiposTurma = new List<int>();
            var anosCondicao = new List<string>();
            if (anos != null)
            {
                foreach (var ano in anos)
                {
                    if (ano == AnoItinerarioPrograma.Programa || ano == AnoItinerarioPrograma.Itinerario || ano == AnoItinerarioPrograma.EducacaoFisica)
                        tiposTurma.Add(ano.ObterTipoTurma());
                    else anosCondicao.Add(ano.ShortName());
                }
            }
            var sql = dreId > 0 ? QueryInformacoesEscolaresTurmasPorAno(dreId, ueId, anosCondicao, tiposTurma, semestre) :
                QueryInformacoesEscolaresTurmasPorDre(dreId, ueId, anosCondicao, tiposTurma, semestre);
            return await contexto
                .Conexao
                .QueryAsync<GraficoBaseDto>(sql, new { modalidade, dreId, ueId, anoLetivo, semestre, tiposTurma, anosCondicao });
        }

        private string CondicoesInformacoesEscolares(long dreId, long ueId, IEnumerable<string> anosCondicao, IEnumerable<int> tiposTurma, int? semestre)
        {
            var query = new StringBuilder("");
            if (semestre > 0) query.AppendLine(@"  and t.semestre = @semestre");
            if (dreId > 0) query.AppendLine(@" and dre.id = @dreId");
            if (ueId > 0) query.AppendLine(@"  and ue.id = @ueId");
            if (anosCondicao != null && anosCondicao.Any()) query.AppendLine(@"  and t.ano = ANY(@anosCondicao)");
            if (tiposTurma != null && tiposTurma.Any()) query.AppendLine(@"  and t.tipo_turma = ANY(@tiposTurma)");

            return query.ToString();
        }

        private string QueryInformacoesEscolaresTurmasPorDre(long dreId, long ueId, IEnumerable<string> anos, IEnumerable<int> tiposTurma, int? semestre)
        {
            var query = new StringBuilder(@"select dre.abreviacao as descricao,
	                                             count(t.id) as quantidade
                                            from turma t 
                                           inner join ue on ue.id = t.ue_id 
                                           inner join dre on dre.id = ue.dre_id 
                                           where t.ano is not null 
                                             and t.ano_letivo = @anoLetivo  
                                             and t.modalidade_codigo = @modalidade");
            if (semestre > 0) query.AppendLine(@"  and t.semestre = @semestre");
            if (dreId > 0) query.AppendLine(@" and dre.id = @dreId");
            if (ueId > 0) query.AppendLine(@"  and ue.id = @ueId");
            if (anos != null && anos.Any() && tiposTurma != null && tiposTurma.Any())
            {
                query.AppendLine(@"  and (t.ano = ANY(@anosCondicao) or t.tipo_turma = ANY(@tiposTurma)) ");
            }
            else
            {
                if (anos != null && anos.Any()) query.AppendLine(@"  and t.ano = ANY(@anosCondicao)");
                if (tiposTurma != null && tiposTurma.Any()) query.AppendLine(@"  and t.tipo_turma = ANY(@tiposTurma)");
            }
            query.AppendLine(@" group by dre.abreviacao 
                         order by dre.abreviacao");
            return query.ToString();
        }

        private string QueryInformacoesEscolaresTurmasPorAno(long dreId, long ueId, IEnumerable<string> anosCondicao, IEnumerable<int> tiposTurma, int? semestre)
        {
            var query = new StringBuilder(@"select * from (	
	                                            (select t.ano as descricao,
		                                               count(t.id) as quantidade
	                                              from turma t 
	                                             inner join ue on ue.id = t.ue_id 
	                                             inner join dre on dre.id = ue.dre_id 
	                                             where  t.ano is not null
	                                               and t.tipo_turma not in (2,3,7)
                                                   and t.ano_letivo = @anoLetivo
                                                   and t.modalidade_codigo = @modalidade ");

            if (tiposTurma.Any() && !anosCondicao.Any()) query.AppendLine("and t.ano = '0'");
            else query.AppendLine("and t.ano <> '0' ");

            query.AppendLine(CondicoesInformacoesEscolares(dreId, ueId, anosCondicao, null, semestre));
            query.AppendLine(@" group by t.ano
	                                             order by t.ano)
	                                             union
	                                            select case 
                                                       when t.tipo_turma = 3 then 'Turmas de programa' 
                                                       when t.tipo_turma = 2 then 'Ed. Física'
                                                       else 'Itinerário' end AS descricao,
		                                               count(t.id) as quantidade
	                                              from turma t 
	                                             inner join ue on ue.id = t.ue_id 
	                                             inner join dre on dre.id = ue.dre_id 
	                                             where t.ano is not null
	                                               and t.tipo_turma in (2,3,7) 
                                                   and t.ano_letivo = @anoLetivo
                                                   and t.modalidade_codigo = @modalidade ");
            query.AppendLine(CondicoesInformacoesEscolares(dreId, ueId, anosCondicao, tiposTurma, semestre));
            query.AppendLine(@"group by t.tipo_turma) x
                                            order by x.descricao");
            return query.ToString();
        }

        public async Task<IEnumerable<TurmaModalidadeDto>> ObterTurmasComModalidadePorAno(int ano)
        {
            var query = @"select id as TurmaId, turma_id as TurmaCodigo, modalidade_codigo as Modalidade from turma where ano_letivo = @ano";

            return await contexto.Conexao.QueryAsync<TurmaModalidadeDto>(query, new { ano });
        }

        public async Task<IEnumerable<TurmaConsolidacaoFechamentoGeralDto>> ObterTurmasConsolidacaoFechamentoGeralAsync(string turmaCodigo)
        {
            var query = @"select t.id as turmaId, t.modalidade_codigo as modalidade 
                            from turma t 
                           where t.tipo_turma in  (1,2,7) 
                             and t.modalidade_codigo  in (3,5,6) 
                             and t.ano_letivo > 2020 ";

            if (!string.IsNullOrEmpty(turmaCodigo))
                query += " and t.turma_id = @turmaCodigo";

            return await contexto.Conexao.QueryAsync<TurmaConsolidacaoFechamentoGeralDto>(query, new { turmaCodigo });
        }

        public async Task<IEnumerable<string>> ObterCodigosTurmasPorAnoModalidade(int anoLetivo, int[] modalidades, string turmaCodigo = "")
        {
            var query = @"select turma_id 
                            from turma
                           where ano_letivo = @anoLetivo
                             and modalidade_codigo = any(@modalidades) ";

            if (!string.IsNullOrEmpty(turmaCodigo))
                query += " and turma_id = @turmaCodigo ";

            return await contexto.QueryAsync<string>(query, new { anoLetivo, modalidades, turmaCodigo });
        }

        public async Task<IEnumerable<TurmaComponenteDto>> ObterTurmasComponentesPorAnoLetivo(DateTime dataReferencia)
        {
            var query = @"select a.turma_id as TurmaCodigo, a.disciplina_id as ComponenteCurricularId, pe.periodo_fim as DataReferencia from aula a 
                                inner join turma t on a.turma_id = t.turma_id 
                                inner join tipo_calendario tc on a.tipo_calendario_id = tc.id 
                                inner join periodo_escolar pe on pe.tipo_calendario_id  = tc.id 
                                    where t.ano_letivo  = @anoLetivo   
                                    and pe.periodo_inicio < @dataReferencia
                                group by a.turma_id, a.disciplina_id, a.tipo_calendario_id, pe.periodo_fim ";



            return await contexto.QueryAsync<TurmaComponenteDto>(query, new { anoLetivo = dataReferencia.Year, dataReferencia });
        }
        public async Task<Turma> ObterTurmaCompletaPorCodigo(string turmaCodigo)
        {
            var query = @"select turma.*, ue.*, dre.* 
                         from turma
                        inner join ue on ue.id = turma.ue_id
                        inner join dre on dre.id = ue.dre_id
                        where turma_id = @turmaCodigo";

            var retorno = await contexto.QueryAsync<Turma, Ue, Dre, Turma>(query, (turma, ue, dre) =>
            {
                ue.AdicionarDre(dre);
                turma.AdicionarUe(ue);
                return turma;
            }, new { turmaCodigo });

            return retorno.FirstOrDefault();
        }

        public async Task<IEnumerable<TurmaModalidadeCodigoDto>> ObterModalidadePorCodigos(string[] turmasCodigo)
        {
            var query = @"select t.turma_id as TurmaCodigo, t.modalidade_codigo as ModalidadeCodigo from turma t where t.turma_id = Any(@turmasCodigo) ";

            return await contexto.Conexao.QueryAsync<TurmaModalidadeCodigoDto>(query, new { turmasCodigo });
        }

        public async Task<IEnumerable<TurmaDTO>> ObterTurmasInfantilPorAno(int anoLetivo, string ueCodigo)
        {
            var query = @"select t.id as turmaId 
                            , t.turma_id as TurmaCodigo
                        from turma t
                       inner join ue on ue.id = t.ue_id
                       where t.modalidade_codigo = 1 
                         and ue.ue_id = @ueCodigo
                         and t.ano_letivo = @anoLetivo ";

            return await contexto.QueryAsync<TurmaDTO>(query, new { anoLetivo, ueCodigo });
        }

        public async Task<IEnumerable<TurmaAlunoBimestreFechamentoDto>> AlunosTurmaPorDreIdUeIdBimestreSemestre(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre)
        {
            var query = new StringBuilder(@"select 
                            distinct t.id as TurmaId, 
                            t.ano,
                            t.tipo_turma as TurmaTipo,
                            t.nome as TurmaNome,
                            t.modalidade_codigo as TurmaModalidade, bimestre as Bimestre, aluno_codigo as AlunoCodigo 
                        from consolidado_conselho_classe_aluno_turma cccat
                        inner join turma t on cccat.turma_id = t.id 
                        inner join ue on ue.id = t.ue_id 
                        where t.ano_letivo = @ano ");

            if (ueId > 0)
            {
                query.Append(" and t.ue_id = @ueId ");
            }

            if (dreId > 0)
            {
                query.Append(" and ue.dre_id = @dreId ");
            }

            if (modalidade > 0)
            {
                query.Append(" and t.modalidade_codigo = @modalidade ");
            }

            if (semestre > 0)
            {
                query.Append(" and t.semestre = @semestre ");
            }

            if (bimestre > 0)
            {
                query.Append(" and cccat.bimestre = @bimestre ");
            }

            return await contexto.QueryAsync<TurmaAlunoBimestreFechamentoDto>(query.ToString(), new { ueId, ano, dreId, modalidade, semestre, bimestre });
        }

        public async Task<IEnumerable<long>> ObterTurmasPorUeAnos(string ueCodigo, int anoLetivo, string[] anos, int modalidadeId)
        {
            var query = new StringBuilder(@"select t.turma_id from turma t
                                                inner join ue u on u.id  = t.ue_id 
                                            where  t.ano_letivo = @anoLetivo and t.modalidade_codigo = @modalidadeId ");

            if (!string.IsNullOrEmpty(ueCodigo))
                query.AppendLine("and u.ue_id  = @ueCodigo");

            if (anos != null && anos.Length > 0)
                query.AppendLine("and t.ano = any(@anos) ");

            return await contexto.Conexao.QueryAsync<long>(query.ToString(), new { ueCodigo, anos, anoLetivo, modalidadeId });

        }


        public async Task<bool> ObterTurmaEspecialPorCodigo(string turmaCodigo)
        {
            var query = "select ensino_especial from turma where turma_id = @turmaCodigo";

            return await contexto.Conexao.QueryFirstAsync<bool>(query, new { turmaCodigo });
        }

        public async Task<Turma> ObterTurmaComUeEDrePorId(long turmaId)
        {
            var query = @"select
	                        t.id,
	                        t.turma_id,
	                        t.ue_id,
	                        t.nome,
	                        t.ano,
	                        t.ano_letivo,
	                        t.modalidade_codigo,
	                        t.semestre,
	                        t.qt_duracao_aula,
	                        t.tipo_turno,
	                        t.data_atualizacao,
	                        u.id as UeId,
	                        u.id,
	                        u.ue_id,
	                        u.nome,
	                        u.dre_id,
	                        u.tipo_escola,
	                        u.data_atualizacao,
	                        d.id as DreId,
	                        d.id,
	                        d.nome,
	                        d.dre_id,
	                        d.abreviacao,
	                        d.data_atualizacao
                        from
	                        turma t
                        inner join ue u on
	                        t.ue_id = u.id
                        inner join dre d on
	                        u.dre_id = d.id
                        where
	                        t.id = @turmaId";
            return (await contexto.QueryAsync<Turma, Ue, Dre, Turma>(query, (turma, ue, dre) =>
            {
                ue.AdicionarDre(dre);
                turma.AdicionarUe(ue);
                return turma;
            }, new { turmaId }, splitOn: "TurmaId, UeId, DreId")).FirstOrDefault();
        }

        public async Task<Turma> ObterTurmaComUeEDrePorCodigo(string turmaCodigo)
        {
            var query = @"select
	                        t.id,
	                        t.turma_id,
	                        t.ue_id,
	                        t.nome,
	                        t.ano,
	                        t.ano_letivo,
	                        t.modalidade_codigo,
	                        t.semestre,
	                        t.qt_duracao_aula,
	                        t.tipo_turno,
	                        t.data_atualizacao,
                            t.tipo_turma,
	                        u.id as UeId,
	                        u.id,
	                        u.ue_id,
	                        u.nome,
	                        u.dre_id,
	                        u.tipo_escola,
	                        u.data_atualizacao,
	                        d.id as DreId,
	                        d.id,
	                        d.nome,
	                        d.dre_id,
	                        d.abreviacao,
	                        d.data_atualizacao
                        from
	                        turma t
                        inner join ue u on
	                        t.ue_id = u.id
                        inner join dre d on
	                        u.dre_id = d.id
                        where
	                        turma_id = @turmaCodigo";

            contexto.AbrirConexao();

            return (await contexto.QueryAsync<Turma, Ue, Dre, Turma>(query, (turma, ue, dre) =>
            {
                ue.AdicionarDre(dre);
                turma.AdicionarUe(ue);
                return turma;
            }, new { turmaCodigo }, splitOn: "TurmaId, UeId, DreId")).FirstOrDefault();
        }

        public async Task<IEnumerable<long>> ObterTurmasIdsPorUeEAnoLetivo(int anoLetivo, string ueCodigo)
        {
            var query = new StringBuilder(@"select t.id 
                                              from turma t
                                             inner join ue u on u.id = t.ue_id 
                                             where t.ano_letivo = @anoLetivo
                                               and u.ue_id = @ueCodigo
                                               and not t.historica ");

            return await contexto.Conexao.QueryAsync<long>(query.ToString(), new { anoLetivo, ueCodigo });

        }

        public Task<IEnumerable<string>> ObterCodigosTurmasPorUeAno(int anoLetivo, string ueCodigo)
        {
            var query = @"select t.turma_id 
                          from turma t
                         inner join ue on ue.id = t.ue_id 
                         where t.ano_letivo = @anoLetivo 
                           and ue.ue_id = @ueCodigo ";

            return contexto.Conexao.QueryAsync<string>(query, new { anoLetivo, ueCodigo });
        }
    }
}
