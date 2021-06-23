using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEventoBimestre : RepositorioBase<EventoBimestre>, IRepositorioEventoBimestre

    {
        public RepositorioEventoBimestre(ISgpContext conexao) : base(conexao)
        {

        }


        public async Task<int[]> ObterEventoBimestres(long eventoId)
        {
            string query = "select bimestre from evento_bimestre where evento_id = @eventoid";
            var bimestres = await database.Conexao.QueryAsync<int>(query, new { eventoId });

            return bimestres.ToArray();

        }

        public Task ExcluiEventoBimestre(long eventoId)
        {
            string query = @"delete from evento_bimestre where evento_id =  @eventoId";

            database.Execute(query, new { eventoId });
            return Task.CompletedTask;
        }

        public async Task<int[]> ObterBimestresEventoPorTipoCalendarioDataReferencia(long tipoCalendarioId, DateTime dataReferencia)
        {
            string query = @"select bimestre from (
                                 select MAX(e.data_inicio), bimestre from evento_bimestre eb
                                 inner join evento e on eb.evento_id = e.id
                                 where not e.excluido  
                                       and e.tipo_calendario_id = @tipoCalendarioId
                                       and e.tipo_evento_id = @tipoEvento
                                       and e.data_inicio <= @data
                                 group by bimestre) b ";

            var bimestres = await database.Conexao.QueryAsync<int>(query, new { tipoCalendarioId, tipoEvento = (int)TipoEvento.LiberacaoBoletim, data = dataReferencia.Date });

            return bimestres?.ToArray();
        }

        public async Task<int[]> ObterBimestresPorTipoCalendarioDeOutrosEventos(long tipoCalendarioId, long eventoId)
        {
            string query = @"select bimestre
                                      from evento_bimestre  eb
                                 inner join evento e on eb.evento_id = e.id
                                 where not e.excluido  
                                       and eb.evento_id  <>  @eventoId
                                 and e.tipo_calendario_id = @tipoCalendarioId
                                       and e.tipo_evento_id = @tipoEvento";

            var bimestres = await database.Conexao.QueryAsync<int>(query, new { eventoId, tipoCalendarioId, tipoEvento = (int)TipoEvento.LiberacaoBoletim });

            return bimestres?.ToArray();
        }


        public async Task<int[]> ObterBimestresPorEventoId(long eventoId)
        {
            string query = @"select bimestre
                                      from evento_bimestre  eb
                                 inner join evento e on eb.evento_id = e.id
                                 where not e.excluido  
                                       and eb.evento_id  = @eventoId
                                       and e.tipo_evento_id = @tipoEvento";

            var bimestres = await database.Conexao.QueryAsync<int>(query, new { eventoId, tipoEvento = (int)TipoEvento.LiberacaoBoletim });

            return bimestres?.ToArray();
        }

        public async Task<int[]> ObterBimestresPorCalendarioIdDiferenteEventoId(long eventoId)
        {
            string query = @"select bimestre
                                      from evento_bimestre  eb
                                 inner join evento e on eb.evento_id = e.id
                                 where not e.excluido  
                                       and eb.evento_id  = @eventoId
                                       and e.tipo_evento_id = @tipoEvento";

            var bimestres = await database.Conexao.QueryAsync<int>(query, new { eventoId, tipoEvento = (int)TipoEvento.LiberacaoBoletim });

            return bimestres?.ToArray();
        }
    }
}
