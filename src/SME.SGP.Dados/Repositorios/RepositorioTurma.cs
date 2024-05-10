using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTurma : IRepositorioTurma
    {
        private readonly ISgpContext contexto;
        private readonly IServicoTelemetria servicoTelemetria;

        public RepositorioTurma(ISgpContext contexto, IServicoTelemetria servicoTelemetria)
        {
            this.contexto = contexto;
            this.servicoTelemetria = servicoTelemetria;
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

        public async Task<IEnumerable<string>> ObterCodigosTurmasParaQueryAtualizarTurmasComoHistoricas(int anoLetivo, bool definirTurmasComoHistorica, string listaTurmas, IDbTransaction transacao)
        {
            var sqlQuery = GerarQueryCodigosTurmasForaLista(anoLetivo, true).Replace("#idsTurmas", listaTurmas);
            return await contexto.Conexao.QueryAsync<string>(sqlQuery, transacao);
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
                
                if (codigosTurmasParaHistorico.Any())
                {
                    var sqlQueryAtualizarTurmasComoHistoricas = QueryDefinirTurmaHistorica
                        .Replace("#turmaId", MapearParaCodigosQuerySql(codigosTurmasParaHistorico));

                    await contexto.Conexao.ExecuteAsync(sqlQueryAtualizarTurmasComoHistoricas, transacao);
                }

                var codigosTurmasARemover = await ObterCodigosTurmasParaQueryAtualizarTurmasComoHistoricas(anoLetivo, false, listaTurmas, transacao);
                
                if (codigosTurmasARemover.Any())
                {
                    var sqlExcluirTurmas = Delete.Replace("#queryIdsConselhoClasseTurmasForaListaCodigos", QueryIdsConselhoClasseTurmasForaListaCodigos)
                         .Replace("#queryFechamentoAlunoTurmasForaListaCodigos", QueryFechamentoAlunoTurmasForaListaCodigos)
                         .Replace("#queryIdsFechamentoTurmaTurmasForaListaCodigos", QueryIdsFechamentoTurmaTurmasForaListaCodigos)
                         .Replace("#queryIdsFechamentoTurmaDisciplinaTurmasForaListaCodigos", QueryIdsFechamentoTurmaDisciplinaTurmasForaListaCodigos)
                         .Replace("#queryIdsTurmasForaListaCodigos", QueryIdsTurmasForaListaCodigos)
                         .Replace("#queryIdsAulasTurmasForaListaCodigos", QueryAulasTurmasForaListaCodigos)
                         .Replace("#turmaId", MapearParaCodigosQuerySql(codigosTurmasARemover));
                    await contexto.Conexao
                        .ExecuteAsync(sqlExcluirTurmas, transacao);
                }

                transacao.Commit();
            }
            catch (Exception ex)
            {
                var erro = new Exception("Erro ao atualizar ou excluir turmas extintas", ex);
                transacao.Rollback();
            }
        }
        private string MapearParaCodigosQuerySql(IEnumerable<string> codigos)
        {
            string[] arrCodigos = codigos.Select(x => $"'{x}'").ToArray();
            return string.Join(",", arrCodigos);
        }
                
        public async Task<bool> AtualizarTurmaParaHistorica(string turmaId, int? semestre = null)
        {
            var query = $@"update public.turma 
                             set historica = true,
                                 data_atualizacao = @dataAtualizacao
                                {(semestre.HasValue ? ", semestre = @semestre " : string.Empty)}
                           where turma_id = @turmaId";

            var retorno = await contexto.Conexao.ExecuteAsync(query, new { turmaId, dataAtualizacao = DateTime.Now, semestre });

            return retorno != 0;

        }

        public async Task<bool> AtualizarTurmaModalidadeEParaHistorica(string turmaId, Modalidade modalidadeEol, int? semestre = null)
        {
            var query = $@"update public.turma 
                             set historica = true,
                                 modalidade_codigo = @modalidadeEol,
                                 data_atualizacao = @dataAtualizacao
                                 {(semestre.HasValue ? ", semestre = @semestre " : string.Empty)}
                           where turma_id = @turmaId";

            var retorno = await contexto.Conexao.ExecuteAsync(query, new { turmaId, modalidadeEol , dataAtualizacao = DateTime.Now, semestre });

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

        public async Task ExcluirTurmaExtintaAsync(long turmaId)
        {
            var sqlExcluirTurma = @"delete from compensacao_ausencia where turma_id = @turmaId;
                                    delete from pendencia_usuario where pendencia_id in (select id from public.pendencia where turma_id = @turmaId);
                                    delete from pendencia where turma_id = @turmaId;
                                    delete from notificacao_plano_aee where plano_aee_id in (select id from public.plano_aee where turma_id = @turmaId);
                                    delete from plano_aee where turma_id = @turmaId;
                                    delete from consolidado_conselho_classe_aluno_turma where turma_id = @turmaId;
                                    delete from consolidacao_acompanhamento_aprendizagem_aluno where turma_id = @turmaId;
                                    delete from consolidacao_diarios_bordo where turma_id = @turmaId;
                                    delete from consolidacao_registros_pedagogicos where turma_id = @turmaId;
                                    delete from consolidacao_frequencia_turma where turma_id = @turmaId;
                                    delete from consolidado_fechamento_componente_turma where turma_id = @turmaId;
                                    delete from frequencia_turma_evasao_aluno where frequencia_turma_evasao_id in (select id from frequencia_turma_evasao where turma_id = @turmaId);
                                    delete from frequencia_turma_evasao where turma_id = @turmaId;
                                    delete from turma where id = @turmaId;";
            
            var parametros = new { turmaId };
            await servicoTelemetria.RegistrarAsync(async () =>
                        await SqlMapper.ExecuteScalarAsync(contexto.Conexao, sqlExcluirTurma, parametros), "query", "Excluir Turma Extinta", 
                                                            sqlExcluirTurma, parametros.ToString());
            
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
                historica = deveMarcarHistorica,
                turma.NomeFiltro
            };

            var retorno = await contexto.Conexao.ExecuteAsync(query, parametros);
            return retorno != 0;
        }

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

        private const string QueryIdsTurmasForaListaCodigos = "select id from public.turma where turma_id in (#turmaId)";

        private const string QueryIdsFechamentoTurmaDisciplinaTurmasForaListaCodigos = @"select id
                                                                                         from public.fechamento_turma_disciplina
                                                                                         where fechamento_turma_id in (select id
                                                                                                                       from public.fechamento_turma
                                                                                                                       where turma_id in (#turmaId))";

        private const string QueryIdsFechamentoTurmaTurmasForaListaCodigos = @"select id
                                                                               from public.fechamento_turma
                                                                               where turma_id in (#turmaId)";

        private const string QueryIdsConselhoClasseTurmasForaListaCodigos = @"select id
                                                                              from public.conselho_classe
                                                                              where fechamento_turma_id in (#queryIdsFechamentoTurmaTurmasForaListaCodigos)";

        private const string QueryFechamentoAlunoTurmasForaListaCodigos = @"select id
                                                                            from public.fechamento_aluno
                                                                            where fechamento_turma_disciplina_id in (#queryIdsFechamentoTurmaDisciplinaTurmasForaListaCodigos)";

        private const string QueryAulasTurmasForaListaCodigos = @"select id from public.aula where turma_id in (#turmaId)";

        
        private const string QueryDefinirTurmaHistorica = "update public.turma set historica = true where turma_id in (#turmaId);";
    }
}
