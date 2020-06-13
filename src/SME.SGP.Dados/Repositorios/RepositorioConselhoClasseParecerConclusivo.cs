﻿using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseParecerConclusivo : RepositorioBase<ConselhoClasseParecerConclusivo>, IRepositorioConselhoClasseParecerConclusivo
    {
        public RepositorioConselhoClasseParecerConclusivo(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterListaPorTurmaIdAsync(long turmaId, DateTime dataConsulta)
        {
            var where = "t.id = @parametro";

            return await ObterListaPorTurma(where, turmaId, dataConsulta);
        }

        public async Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterListaPorTurmaCodigoAsync(string turmaCodigo, DateTime dataConsulta)
        {
            var where = "t.turma_id = @parametro";

            return await ObterListaPorTurma(where, turmaCodigo, dataConsulta);
        }

        private async Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterListaPorTurma(string where, object parametro, DateTime dataConsulta)
        {
            var sql = string.Format(ObterSqlParecerConclusivoTurma(), where);

            var param = new { parametro, dataConsulta };

            return await database.Conexao.QueryAsync<ConselhoClasseParecerConclusivo>(sql, param);
        }

        private async Task<IEnumerable<ParecerConclusivoDto>> ObterListaResumidaPorTurma(string where, object parametro, DateTime dataConsulta)
        {
            var sql = string.Format(ObterSqlResumidoParecerConclusivoTurma(), where);

            var param = new { parametro, dataConsulta };

            return await database.Conexao.QueryAsync<ParecerConclusivoDto>(sql, param);
        }

        private string ObterSqlParecerConclusivoTurma()
        {
            return @"select ccp.* from conselho_classe_parecer ccp 
                        inner join conselho_classe_parecer_ano ccpa on ccp.id = ccpa.parecer_id 
                        inner join turma t on cast(ccpa.ano_turma as varchar) = t.ano and ccpa.modalidade = t.modalidade_codigo
                        where {0} and ccpa.inicio_vigencia <= @dataConsulta and (ccpa.fim_vigencia >= @dataConsulta or ccpa.fim_vigencia is null)";
        }

        private string ObterSqlResumidoParecerConclusivoTurma()
        {
            return @"select ccp.id, ccp.Nome from conselho_classe_parecer ccp 
                        inner join conselho_classe_parecer_ano ccpa on ccp.id = ccpa.parecer_id 
                        inner join turma t on cast(ccpa.ano_turma as varchar) = t.ano and ccpa.modalidade = t.modalidade_codigo
                        where {0} and ccpa.inicio_vigencia <= @dataConsulta and (ccpa.fim_vigencia >= @dataConsulta or ccpa.fim_vigencia is null)";
        }

        public async Task<IEnumerable<ParecerConclusivoDto>> ObterListaResumidaPorTurmaCodigoAsync(string turmaCodigo, DateTime dataConsulta)
        {
            var where = "t.turma_id = @parametro";

            return await ObterListaResumidaPorTurma(where, turmaCodigo, dataConsulta);
        }
    }
}
