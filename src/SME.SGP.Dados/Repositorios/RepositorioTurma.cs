using Dapper;
using Dommel;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTurma : IRepositorioTurma
    {
        private const string QuerySincronizacao = @"
                    select
	                    id,
	                    turma_id,
	                    ue_id,
	                    nome,
	                    ano,
	                    ano_letivo,
	                    modalidade_codigo,
	                    semestre,
	                    qt_duracao_aula,
	                    tipo_turno,
	                    data_atualizacao,
                        ensino_especial,
                        etapa_eja,
                        data_inicio,
                        dt_fim_eol
                    from
	                    public.turma
                    where turma_id in (#ids);";

        private const string Update = @"
                    update
	                    public.turma
                    set
	                    nome = @nome,
	                    ano = @ano,
	                    ano_letivo = @anoLetivo,
	                    modalidade_codigo = @modalidadeCodigo,
	                    semestre = @semestre,
	                    qt_duracao_aula = @qtDuracaoAula,
	                    tipo_turno = @tipoTurno,
	                    data_atualizacao = @dataAtualizacao,
                        ensino_especial = @ensinoEspecial,
                        etapa_eja = @etapaEja,
                        data_inicio = @dataInicio,
                        serie_ensino = @serieEnsino,
                        dt_fim_eol = @dataFim
                    where
	                    id = @id;";

        private const string Delete = @"
                    delete from public.compensacao_ausencia_aluno
                    where compensacao_ausencia_id in (select id
                                                      from public.compensacao_ausencia
                                                      where turma_id in (#queryIdsTurmasForaListaCodigos));

                    delete from public.compensacao_ausencia
                    where turma_id in (#queryIdsTurmasForaListaCodigos);

                    delete from public.pendencia_fechamento
                    where fechamento_turma_disciplina_id in (#queryIdsFechamentoTurmaDisciplinaTurmasForaListaCodigos);

                    delete from public.wf_aprovacao_nota_fechamento
                    where fechamento_nota_id in (select id
                                                 from public.fechamento_nota
                                                 where fechamento_aluno_id in (#queryFechamentoAlunoTurmasForaListaCodigos));

                    delete from public.fechamento_nota
                    where fechamento_aluno_id in (#queryFechamentoAlunoTurmasForaListaCodigos);

                    delete from public.fechamento_aluno
                    where fechamento_turma_disciplina_id in (#queryIdsFechamentoTurmaDisciplinaTurmasForaListaCodigos);

                    delete from public.fechamento_turma_disciplina
                    where fechamento_turma_id in (#queryIdsFechamentoTurmaTurmasForaListaCodigos);

                    delete from public.conselho_classe_nota
                    where conselho_classe_aluno_id in (select id
                                                       from public.conselho_classe_aluno
                                                       where conselho_classe_id in (#queryIdsConselhoClasseTurmasForaListaCodigos));

                    delete from public.conselho_classe_aluno
                    where conselho_classe_id in (#queryIdsConselhoClasseTurmasForaListaCodigos);        

                    delete from public.conselho_classe
                    where fechamento_turma_id in (select id
                                                  from public.fechamento_turma
                                                  where turma_id in (#queryIdsTurmasForaListaCodigos));

                    delete from public.fechamento_turma
                    where turma_id in (#queryIdsTurmasForaListaCodigos);
                  
                    delete from public.frequencia_aluno
                    where turma_id in (#codigosTurmasARemover);

                    delete from public.diario_bordo
                    where aula_id in (#queryIdsAulasTurmasForaListaCodigos);         
                    
                    delete from public.notificacao_frequencia
                    where aula_id in (#queryIdsAulasTurmasForaListaCodigos);

                    delete from public.registro_frequencia
                    where aula_id in (#queryIdsAulasTurmasForaListaCodigos);

                    delete from public.aula
                    where id in (#queryIdsAulasTurmasForaListaCodigos);
                    
                    delete from public.turma
                    where turma_id in (#codigosTurmasARemover);";

        private const string QueryIdsTurmasForaListaCodigos = "select id from public.turma where turma_id in (#codigosTurmasARemover)";

        private const string QueryIdsFechamentoTurmaDisciplinaTurmasForaListaCodigos = @"select id
                                                                                         from public.fechamento_turma_disciplina
                                                                                         where fechamento_turma_id in (select id
                                                                                                                       from public.fechamento_turma
                                                                                                                       where turma_id in (#queryIdsTurmasForaListaCodigos))";

        private const string QueryIdsFechamentoTurmaTurmasForaListaCodigos = @"select id
                                                                               from public.fechamento_turma
                                                                               where turma_id in (#queryIdsTurmasForaListaCodigos)";

        private const string QueryIdsConselhoClasseTurmasForaListaCodigos = @"select id
                                                                              from public.conselho_classe
                                                                              where fechamento_turma_id in (#queryIdsFechamentoTurmaTurmasForaListaCodigos)";

        private const string QueryFechamentoAlunoTurmasForaListaCodigos = @"select id
                                                                            from public.fechamento_aluno
                                                                            where fechamento_turma_disciplina_id in (#queryIdsFechamentoTurmaDisciplinaTurmasForaListaCodigos)";

        private const string QueryAulasTurmasForaListaCodigos = @"select id from public.aula where turma_id in (#codigosTurmasARemover)";

        private const string QueryDefinirTurmaHistorica = "update public.turma set historica = true where turma_id in (#codigosTurmasParaHistorico);";

        private readonly ISgpContext contexto;

        public RepositorioTurma(ISgpContext contexto)
        {
            this.contexto = contexto;
        }

        public IEnumerable<Turma> MaterializarCodigosTurma(string[] idTurmas, out string[] codigosNaoEncontrados)
        {
            List<Turma> resultado = new List<Turma>();
            List<string> naoEncontrados = new List<string>();

            for (int i = 0; i < idTurmas.Count(); i = i + 900)
            {
                var iteracao = idTurmas.Skip(i).Take(900);

                var armazenados = contexto.Conexao.Query<Turma>(QuerySincronizacao.Replace("#ids", string.Join(",", idTurmas.Select(x => $"'{x}'"))));

                naoEncontrados.AddRange(iteracao.Where(x => !armazenados.Select(y => y.CodigoTurma).Contains(x)));

                resultado.AddRange(armazenados);
            }
            codigosNaoEncontrados = naoEncontrados.ToArray();

            return resultado;
        }

        public async Task<Turma> ObterPorCodigo(string turmaCodigo)
        {
            return await contexto.Conexao.QueryFirstOrDefaultAsync<Turma>("select * from turma where turma_id = @turmaCodigo", new { turmaCodigo });
        }

        public async Task<long> ObterTurmaIdPorCodigo(string turmaCodigo)
        {
            return await contexto.QueryFirstOrDefaultAsync<long>("select id from turma where turma_id = @turmaCodigo", new { turmaCodigo });
        }

        public async Task<Turma> ObterPorId(long id)
        {
            return await contexto.QueryFirstOrDefaultAsync<Turma>("select * from turma where id = @id", new { id });
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

        public async Task<bool> ObterTurmaEspecialPorCodigo(string turmaCodigo)
        {
            var query = "select ensino_especial from turma where turma_id = @turmaCodigo";

            return await contexto.Conexao.QueryFirstAsync<bool>(query, new { turmaCodigo });
        }

        public async Task<IEnumerable<long>> ObterTurmasPorUeAnos(string ueCodigo, int anoLetivo, string[] anos, int modalidadeId)
        {
            var query = new StringBuilder(@"select x.turma_Id from (select distinct on (2) t.turma_id , t.ano from turma t
                                                inner join ue u on u.id  = t.ue_id 
                                            where  t.ano_letivo = @anoLetivo and t.modalidade_codigo = @modalidadeId ");

            if (!string.IsNullOrEmpty(ueCodigo))
                query.AppendLine("and u.ue_id  = @ueCodigo");

            if (anos != null && anos.Length > 0)
                query.AppendLine("and t.ano = any(@anos) ");

            query.AppendLine(") x");

            return await contexto.Conexao.QueryAsync<long>(query.ToString(), new { ueCodigo, anos, anoLetivo, modalidadeId });

        }

        public async Task<IEnumerable<Turma>> SincronizarAsync(IEnumerable<Turma> entidades, IEnumerable<Ue> ues)
        {
            List<Turma> resultado = new List<Turma>();

            var anoLetivoConsiderado = (from e in entidades
                                        where !e.Extinta
                                        orderby e.AnoLetivo descending
                                        select e.AnoLetivo).Last();

            await AtualizarRemoverTurmasExtintasAsync(entidades, anoLetivoConsiderado);

            for (int i = 0; i < entidades.Count(); i = i + 900)
            {
                var iteracao = entidades.Skip(i).Take(900);

                var armazenados = (await contexto.Conexao.QueryAsync<Turma>(
                    QuerySincronizacao.Replace("#ids", string.Join(",", iteracao.Select(x => $"'{x.CodigoTurma}'"))))).ToList();

                var idsArmazenados = armazenados.Select(y => y.CodigoTurma);

                var novos = iteracao
                    .Where(x => !x.Extinta && !idsArmazenados.Contains(x.CodigoTurma))
                    .ToList();

                foreach (var item in novos)
                {
                    item.DataAtualizacao = DateTime.Today;
                    item.Ue = ues.First(x => x.CodigoUe == item.Ue.CodigoUe);
                    item.UeId = item.Ue.Id;
                    item.Id = (long)await contexto.Conexao.InsertAsync(item);
                    resultado.Add(item);
                }

                var modificados = from c in entidades
                                  join l in armazenados on c.CodigoTurma equals l.CodigoTurma
                                  where c.Nome != l.Nome ||
                                        c.Ano != l.Ano ||
                                        c.AnoLetivo != l.AnoLetivo ||
                                        c.ModalidadeCodigo != l.ModalidadeCodigo ||
                                        c.Semestre != l.Semestre ||
                                        c.QuantidadeDuracaoAula != l.QuantidadeDuracaoAula ||
                                        c.TipoTurno != l.TipoTurno ||
                                        c.EnsinoEspecial != l.EnsinoEspecial ||
                                        c.EtapaEJA != l.EtapaEJA ||
                                        c.SerieEnsino != l.SerieEnsino ||
                                        c.DataInicio.HasValue != l.DataInicio.HasValue ||
                                        (c.DataInicio.HasValue && l.DataInicio.HasValue && c.DataInicio.Value.Date != l.DataInicio.Value.Date) ||
                                        c.DataFim.HasValue != l.DataFim.HasValue ||
                                        (c.DataFim.HasValue && l.DataFim.HasValue && c.DataFim.Value.Date != l.DataFim.Value.Date)
                                  select new Turma()
                                  {
                                      Ano = c.Ano,
                                      AnoLetivo = c.AnoLetivo,
                                      CodigoTurma = c.CodigoTurma,
                                      DataAtualizacao = DateTime.Today,
                                      Id = l.Id,
                                      ModalidadeCodigo = c.ModalidadeCodigo,
                                      Nome = c.Nome,
                                      QuantidadeDuracaoAula = c.QuantidadeDuracaoAula,
                                      Semestre = c.Semestre,
                                      TipoTurno = c.TipoTurno,
                                      Ue = l.Ue,
                                      UeId = l.UeId,
                                      EnsinoEspecial = c.EnsinoEspecial,
                                      EtapaEJA = c.EtapaEJA,
                                      DataInicio = c.DataInicio,
                                      SerieEnsino = c.SerieEnsino,
                                      DataFim = c.DataFim,
                                      Extinta = c.Extinta
                                  };

                foreach (var item in modificados)
                {
                    await contexto.Conexao.ExecuteAsync(Update, new
                    {
                        nome = item.Nome,
                        ano = item.Ano,
                        anoLetivo = item.AnoLetivo,
                        modalidadeCodigo = item.ModalidadeCodigo,
                        semestre = item.Semestre,
                        qtDuracaoAula = item.QuantidadeDuracaoAula,
                        tipoTurno = item.TipoTurno,
                        dataAtualizacao = item.DataAtualizacao,
                        id = item.Id,
                        ensinoEspecial = item.EnsinoEspecial,
                        etapaEja = item.EtapaEJA,
                        dataInicio = item.DataInicio,
                        serieEnsino = item.SerieEnsino,
                        dataFim = item.DataFim
                    });

                    resultado.Add(item);
                }

                resultado.AddRange(armazenados.Where(x => !resultado.Select(y => y.CodigoTurma).Contains(x.CodigoTurma)));
            }

            return resultado;
        }

        public async Task<IEnumerable<Turma>> ObterTurmasInfantilNaoDeProgramaPorAnoLetivoAsync(int anoLetivo)
        {
            var modalidade = Modalidade.Infantil;
            var turmas = new List<Turma>();
            var query = @"select
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
	                            and ano ~ E'^[0-9\.]+$'";

            await contexto.Conexao.QueryAsync<Turma, Ue, Dre, Turma>(query, (turma, ue, dre) =>
             {
                 ue.AdicionarDre(dre);
                 turma.AdicionarUe(ue);

                 var turmaExistente = turmas.FirstOrDefault(c => c.Id == turma.Id);
                 if (turmaExistente == null)
                     turmas.Add(turma);

                 return turma;
             }, new { anoLetivo, modalidade });

            return turmas;
        }

        private async Task AtualizarRemoverTurmasExtintasAsync(IEnumerable<Turma> entidades, int anoLetivo)
        {
            var codigosTurmas = entidades
                .Where(e => !e.Extinta)
                .OrderBy(e => e.CodigoTurma)
                .Select(e => $"'{e.CodigoTurma}'")?.ToArray();

            var listaTurmas = string.Join(",", codigosTurmas);
            var transacao = contexto.Conexao.BeginTransaction();

            try
            {
                var codigosTurmasParaHistorico = await ObterCodigosTurmasParaQueryAtualizarTurmasComoHistoricas(anoLetivo, true, listaTurmas, transacao);
                var sqlQueryAtualizarTurmasComoHistoricas = QueryDefinirTurmaHistorica
                                        .Replace("#codigosTurmasParaHistorico", MapearParaCodigosQuerySql(codigosTurmasParaHistorico));                

                await contexto.Conexao.ExecuteAsync(sqlQueryAtualizarTurmasComoHistoricas, transacao);

                var codigosTurmasARemover = await ObterCodigosTurmasParaQueryAtualizarTurmasComoHistoricas(anoLetivo, false, listaTurmas, transacao);
                var sqlExcluirTurmas = Delete.Replace("#queryIdsConselhoClasseTurmasForaListaCodigos", QueryIdsConselhoClasseTurmasForaListaCodigos)
                                         .Replace("#queryFechamentoAlunoTurmasForaListaCodigos", QueryFechamentoAlunoTurmasForaListaCodigos)
                                         .Replace("#queryIdsFechamentoTurmaTurmasForaListaCodigos", QueryIdsFechamentoTurmaTurmasForaListaCodigos)
                                         .Replace("#queryIdsFechamentoTurmaDisciplinaTurmasForaListaCodigos", QueryIdsFechamentoTurmaDisciplinaTurmasForaListaCodigos)
                                         .Replace("#queryIdsTurmasForaListaCodigos", QueryIdsTurmasForaListaCodigos)
                                         .Replace("#queryIdsAulasTurmasForaListaCodigos", QueryAulasTurmasForaListaCodigos)
                                         .Replace("#codigosTurmasARemover", MapearParaCodigosQuerySql(codigosTurmasARemover));
                await contexto.Conexao
                    .ExecuteAsync(sqlExcluirTurmas, transacao);

                transacao.Commit();
            }
            catch (Exception ex)
            {
                var erro = new Exception("Erro ao atualizar ou excluir turmas extintas", ex);
                SentrySdk.CaptureException(erro);
                transacao.Rollback();
            }
        }

        public async Task<IEnumerable<string>> ObterCodigosTurmasParaQueryAtualizarTurmasComoHistoricas(int anoLetivo, bool definirTurmasComoHistorica, string listaTurmas, IDbTransaction transacao)
        {
            var sqlQuery = GerarQueryCodigosTurmasForaLista(anoLetivo, true).Replace("#idsTurmas", listaTurmas);
            return await contexto.Conexao.QueryAsync<string>(sqlQuery, transacao);
        }
        private string MapearParaCodigosQuerySql(IEnumerable<string> codigos)
        {
            string[] arrCodigos = codigos.Select(x => $"'{x}'").ToArray();
            return string.Join(",", arrCodigos);
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

        private string GerarQueryCodigosTurmasForaLista(int anoLetivo, bool definirTurmasComoHistorica) =>
            $@"select distinct t.turma_id
	                from turma t
		                inner join tipo_calendario tc
			                on t.ano_letivo = tc.ano_letivo and
			                   t.modalidade_codigo = t.modalidade_codigo 
		                inner join periodo_escolar pe
			                on tc.id = pe.tipo_calendario_id 			
		                inner join (select id, data_inicio, modalidade_codigo
					                    from turma
					                where ano_letivo = {anoLetivo} and
						                  turma_id not in (#idsTurmas)) t2
			                on t.id = t2.id and
			                   t.modalidade_codigo = t2.modalidade_codigo
                where t.ano_letivo = {anoLetivo} and                      
	                  pe.bimestre = 1 and                      
	                  t.dt_fim_eol is not null and 
                      t.dt_fim_eol {(definirTurmasComoHistorica ? ">=" : "<")} pe.periodo_inicio"; //Turmas extintas após o 1º bimestre do ano letivo considerado serão marcadas como histórica

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
            } , new { ueId, modalidades, ano });
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
    }
}
