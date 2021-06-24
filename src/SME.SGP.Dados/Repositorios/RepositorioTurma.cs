using Dapper;
using Dommel;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
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
                        dt_fim_eol,
                        tipo_turma
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
                        dt_fim_eol = @dataFim,
                        tipo_turma = @tipoTurma
                    where
	                    id = @id;";

        private const string Delete = @"
                    delete from public.compensacao_ausencia_aluno
                    where compensacao_ausencia_id in (select id
                                                      from public.compensacao_ausencia
                                                      where turma_id = #turmaId);

                    delete from public.compensacao_ausencia
                    where turma_id = #turmaId;

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
                                                  where turma_id = #turmaId);

                    delete from public.fechamento_turma
                    where turma_id in (#queryIdsTurmasForaListaCodigos);
                  
                    delete from public.frequencia_aluno
                    where turma_id = #turmaId;

                    delete from public.diario_bordo
                    where aula_id in (#queryIdsAulasTurmasForaListaCodigos);         
                    
                    delete from public.notificacao_frequencia
                    where aula_id in (#queryIdsAulasTurmasForaListaCodigos);

                    delete from public.registro_frequencia
                    where aula_id in (#queryIdsAulasTurmasForaListaCodigos);

                    delete from public.aula
                    where turma_id = #turmaId;
                    
                    delete from public.turma
                    where turma_id = #turmaId;";

        private const string QueryIdsTurmasForaListaCodigos = "select id from public.turma where turma_id = #turmaId";

        private const string QueryIdsFechamentoTurmaDisciplinaTurmasForaListaCodigos = @"select id
                                                                                         from public.fechamento_turma_disciplina
                                                                                         where fechamento_turma_id in (select id
                                                                                                                       from public.fechamento_turma
                                                                                                                       where turma_id = #turmaId)";

        private const string QueryIdsFechamentoTurmaTurmasForaListaCodigos = @"select id
                                                                               from public.fechamento_turma
                                                                               where turma_id = #turmaId";

        private const string QueryIdsConselhoClasseTurmasForaListaCodigos = @"select id
                                                                              from public.conselho_classe
                                                                              where fechamento_turma_id in (#queryIdsFechamentoTurmaTurmasForaListaCodigos)";

        private const string QueryFechamentoAlunoTurmasForaListaCodigos = @"select id
                                                                            from public.fechamento_aluno
                                                                            where fechamento_turma_disciplina_id in (#queryIdsFechamentoTurmaDisciplinaTurmasForaListaCodigos)";

        private const string QueryAulasTurmasForaListaCodigos = @"select id from public.aula where turma_id = #turmaId";

        private const string QueryDefinirTurmaHistorica = "update public.turma set historica = true where turma_id = #turmaId;";

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
            var query = new StringBuilder(@"select t.turma_id from turma t
                                                inner join ue u on u.id  = t.ue_id 
                                            where  t.ano_letivo = @anoLetivo and t.modalidade_codigo = @modalidadeId ");

            if (!string.IsNullOrEmpty(ueCodigo))
                query.AppendLine("and u.ue_id  = @ueCodigo");

            if (anos != null && anos.Length > 0)
                query.AppendLine("and t.ano = any(@anos) ");

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

                var modificados = from c in iteracao
                                  join l in armazenados on c.CodigoTurma equals l.CodigoTurma
                                  where c.Nome != l.Nome ||
                                        c.Ano != l.Ano ||
                                        c.TipoTurma != l.TipoTurma ||
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
                                      TipoTurma = c.TipoTurma,
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
                                      Extinta = c.Extinta,
                                  };

                foreach (var item in modificados)
                {
                    await contexto.Conexao.ExecuteAsync(Update, new
                    {
                        nome = item.Nome,
                        ano = item.Ano,
                        tipoTurma = item.TipoTurma,
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
            var modalidade = Modalidade.EducacaoInfantil;
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

        public async Task<bool> AtualizarTurmaParaHistorica(string turmaId)
        {
            var query = @"update public.turma 
                             set historica = true,
                                 data_atualizacao = @dataAtualizacao
                           where turma_id = @turmaId";

            var retorno = await contexto.Conexao.ExecuteAsync(query, new { turmaId, dataAtualizacao = DateTime.Now });

            return retorno != 0;

        }

        public async Task<bool> SalvarAsync(TurmaParaSyncInstitucionalDto turma, long ueId)
        {
            var query = @"INSERT INTO public.turma
				                (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma, nome_filtro)
	                        values
	                            (@Codigo, @ueId, @NomeTurma, @Ano, @AnoLetivo, @CodigoModalidade, @Semestre, @DuracaoTurno, @TipoTurno, @DataAtualizacao, @historica, @DataFim, @EnsinoEspecial, @EtapaEJA, @DataInicioTurma, @SerieEnsino, @TipoTurma, @NomeFiltro);";

            var parametros = new
            {
                turma.Codigo,
                turma.UeCodigo,
                turma.NomeTurma,
                turma.Ano,
                turma.AnoLetivo,
                turma.CodigoModalidade,
                turma.Semestre,
                turma.DuracaoTurno,
                turma.TipoTurno,
                turma.DataAtualizacao,
                turma.DataFim,
                turma.EnsinoEspecial,
                turma.EtapaEJA,
                turma.DataInicioTurma,
                turma.SerieEnsino,
                turma.TipoTurma,
                ueId,
                historica = turma.Extinta,
                turma.NomeFiltro
            };
            var retorno = await contexto.Conexao.ExecuteAsync(query, parametros);

            return retorno != 0;

        }

        public async Task ExcluirTurmaExtintaAsync(string turmaCodigo, long turmaId)
        {

            var sqlExcluirTurmas = @"delete
                            from
	                            public.compensacao_ausencia_aluno
                            where
	                            compensacao_ausencia_id in (
	                            select
		                            id
	                            from
		                            public.compensacao_ausencia
	                            where
		                            turma_id = @turmaId);

                            delete
                            from
	                            public.compensacao_ausencia
                            where
	                            turma_id = @turmaId;

                            delete
                            from
	                            public.pendencia_fechamento
                            where
	                            fechamento_turma_disciplina_id in (
	                            select
		                            id
	                            from
		                            public.fechamento_turma_disciplina
	                            where
		                            fechamento_turma_id in (
		                            select
			                            id
		                            from
			                            public.fechamento_turma
		                            where
			                            turma_id = @turmaId));

                            delete
                            from
	                            public.wf_aprovacao_nota_fechamento
                            where
	                            fechamento_nota_id in (
	                            select
		                            id
	                            from
		                            public.fechamento_nota
	                            where
		                            fechamento_aluno_id in (
		                            select
			                            id
		                            from
			                            public.fechamento_aluno
		                            where
			                            fechamento_turma_disciplina_id in (
			                            select
				                            id
			                            from
				                            public.fechamento_turma_disciplina
			                            where
				                            fechamento_turma_id in (
				                            select
					                            id
				                            from
					                            public.fechamento_turma
				                            where
					                            turma_id = @turmaId))));

                            delete
                            from
	                            public.fechamento_nota
                            where
	                            fechamento_aluno_id in (
	                            select
		                            id
	                            from
		                            public.fechamento_aluno
	                            where
		                            fechamento_turma_disciplina_id in (
		                            select
			                            id
		                            from
			                            public.fechamento_turma_disciplina
		                            where
			                            fechamento_turma_id in (
			                            select
				                            id
			                            from
				                            public.fechamento_turma
			                            where
				                            turma_id = @turmaId)));

                            delete
                            from
	                            public.fechamento_aluno
                            where
	                            fechamento_turma_disciplina_id in (
	                            select
		                            id
	                            from
		                            public.fechamento_turma_disciplina
	                            where
		                            fechamento_turma_id in (
		                            select
			                            id
		                            from
			                            public.fechamento_turma
		                            where
			                            turma_id = @turmaId));

                            delete
                            from
	                            public.fechamento_turma_disciplina
                            where
	                            fechamento_turma_id in (
	                            select
		                            id
	                            from
		                            public.fechamento_turma
	                            where
		                            turma_id = @turmaId);

                            delete
                            from
	                            public.conselho_classe_nota
                            where
	                            conselho_classe_aluno_id in (
	                            select
		                            id
	                            from
		                            public.conselho_classe_aluno
	                            where
		                            conselho_classe_id in (
		                            select
			                            id
		                            from
			                            public.conselho_classe
		                            where
			                            fechamento_turma_id in (
			                            select
				                            id
			                            from
				                            public.fechamento_turma
			                            where
				                            turma_id = @turmaId)));

                            delete
                            from
	                            public.conselho_classe_aluno
                            where
	                            conselho_classe_id in (
	                            select
		                            id
	                            from
		                            public.conselho_classe
	                            where
		                            fechamento_turma_id in (
		                            select
			                            id
		                            from
			                            public.fechamento_turma
		                            where
			                            turma_id = @turmaId));

                            delete
                            from
	                            public.conselho_classe
                            where
	                            fechamento_turma_id in (
	                            select
		                            id
	                            from
		                            public.fechamento_turma
	                            where
		                            turma_id = @turmaId);

                            delete from public.fechamento_turma where turma_id = @turmaId;

                            delete from public.frequencia_aluno where turma_id = @turmaCodigo;

                            delete from public.diario_bordo where aula_id in (select id from public.aula where turma_id = @turmaCodigo);

                            delete from public.notificacao_frequencia where aula_id in (select id from public.aula where turma_id = @turmaCodigo);

                            delete from public.registro_frequencia where aula_id in ( select id from public.aula where turma_id = @turmaCodigo);

                            delete from public.aula where turma_id = @turmaCodigo;

                            delete from public.pendencia_registro_individual_aluno pria where pria.pendencia_registro_individual_id in (select id from public.pendencia_registro_individual pri where pri.turma_id = @turmaId);

                            delete from public.pendencia_registro_individual pri where pri.turma_id = @turmaId;
        
                            delete from public.consolidacao_frequencia_turma where turma_id = @turmaId;
    
                            delete from public.pendencia_professor where turma_id = @turmaId;

                            delete from public.turma where id = @turmaId;";


            var transacao = contexto.Conexao.BeginTransaction();

            try
            {
                await contexto.Conexao
                    .ExecuteAsync(sqlExcluirTurmas, new { turmaId, turmaCodigo }, transacao);

                transacao.Commit();
            }
            catch (Exception ex)
            {
                var erro = new Exception("Erro ao atualizar ou excluir turmas extintas", ex);
                SentrySdk.CaptureException(erro);
                transacao.Rollback();
            }
        }

        public async Task<bool> AtualizarTurmaSincronizacaoInstitucionalAsync(TurmaParaSyncInstitucionalDto turma, bool deveMarcarHistorica = false)
        {
            var query = @"update
	                            public.turma
                            set
	                            nome = @nomeTurma,
	                            ano = @ano,
	                            ano_letivo = @anoLetivo,
	                            modalidade_codigo = @codigoModalidade,
	                            semestre = @semestre,
	                            qt_duracao_aula = @duracaoTurno,
	                            tipo_turno = @tipoTurno,
	                            data_atualizacao = @dataAtualizacao,
                                ensino_especial = @ensinoEspecial,
                                etapa_eja = @etapaEja,
                                data_inicio = @dataInicioTurma,
                                serie_ensino = @serieEnsino,
                                dt_fim_eol = @dataFim,
                                tipo_turma = @tipoTurma,
                                historica = @historica,
                                nome_filtro = @nomeFiltro
                            where
	                            turma_id = @turmaId";

            var parametros = new
            {
                turma.NomeTurma,
                turma.Ano,
                turma.AnoLetivo,
                codigoModalidade = (int)turma.CodigoModalidade,
                turma.Semestre,
                turma.DuracaoTurno,
                turma.TipoTurno,
                turma.DataAtualizacao,
                turma.DataFim,
                turma.EnsinoEspecial,
                turma.EtapaEJA,
                turma.DataInicioTurma,
                turma.SerieEnsino,
                turma.TipoTurma,
                turmaId = turma.Codigo.ToString(),
                historica = deveMarcarHistorica ? true : false,
                turma.NomeFiltro
            };

            var retorno = await contexto.Conexao.ExecuteAsync(query, parametros);
            return retorno != 0;
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

                dataReferencia = new DateTime(anoLetivo, semestre == 1 ? 6 : 7, 1);
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

        public async Task<IEnumerable<TurmaConsolidacaoFechamentoGeralDto>> ObterTurmasConsolidacaoFechamentoGeralAsync()
        {
            var query = @"
                            select t.id as turmaId, t.modalidade_codigo as modalidade from turma t 
                            where t.tipo_turma  in  (1,2,7) 
                            and t.modalidade_codigo  in (3,5,6) 
                            and t.ano_letivo > 2020 ";

            return await contexto.Conexao.QueryAsync<TurmaConsolidacaoFechamentoGeralDto>(query);
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
    }
}
