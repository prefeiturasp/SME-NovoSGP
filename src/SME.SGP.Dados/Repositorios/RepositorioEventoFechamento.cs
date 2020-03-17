﻿using System;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEventoFechamento : RepositorioBase<EventoFechamento>, IRepositorioEventoFechamento
    {
        public RepositorioEventoFechamento(ISgpContext database) : base(database)
        {
        }

        public EventoFechamento ObterPorIdFechamento(long fechamentoId)
        {
            return database.Conexao.QueryFirstOrDefault<EventoFechamento>("select * from evento_fechamento where fechamento_id = @fechamentoId", new
            {
                fechamentoId,
            });
        }

        public async Task<bool> UeEmFechamento(DateTime dataReferencia, string dreCodigo, string ueCodigo, int bimestre, long tipoCalendarioId)
        {
            var query = @"select count(ef.id)
                         from evento_fechamento ef
                        inner join evento e on e.id = ef.evento_id
                        inner join periodo_fechamento_bimestre pfb on pfb.id = ef.fechamento_id
                        inner join periodo_escolar pe on pe.id = pfb.periodo_escolar_id
                        where not e.excluido 
                          and e.data_inicio <= @dataReferencia
                          and e.data_fim >= @dataReferencia
                          and e.dre_id = @dreCodigo
                          and e.ue_id = @ueCodigo
                          and pe.bimestre = @bimestre
                          and pe.tipo_calendario_id = @tipoCalendarioId";

            return await database.Conexao.QueryFirstAsync<int>(query, new
            {
                dataReferencia,
                dreCodigo,
                ueCodigo,
                bimestre,
                tipoCalendarioId
            }) > 0;
        }
    }
}