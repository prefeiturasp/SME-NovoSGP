using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPeriodoFechamento : RepositorioBase<PeriodoFechamento>, IRepositorioPeriodoFechamento
    {
        public RepositorioPeriodoFechamento(ISgpContext conexao) : base(conexao)
        {
        }

        public void AlterarPeriodosComHierarquiaInferior(DateTime inicioDoFechamento, DateTime finalDoFechamento, long periodoEscolarId, long? dreId)
        {
            var query = new StringBuilder("update ");
            query.AppendLine("periodo_fechamento_bimestre");
            query.AppendLine("set");
            query.AppendLine("inicio_fechamento = @inicioDoFechamento, ");
            query.AppendLine("final_fechamento = @finalDoFechamento");
            query.AppendLine("where");
            query.AppendLine("(inicio_fechamento < @inicioDoFechamento or final_fechamento > @finalDoFechamento)");
            query.AppendLine("and periodo_escolar_id = @periodoEscolarId");
            if (dreId.HasValue)
            {
                query.AppendLine("and periodo_fechamento_id = Any(select id from periodo_fechamento where dre_id = @dreId)");
            }
            database.Conexao.Execute(query.ToString(), new
            {
                inicioDoFechamento,
                finalDoFechamento,
                periodoEscolarId,
                dreId
            });
        }

        public PeriodoFechamento ObterPorTipoCalendarioDreEUE(long tipoCalendarioId, long? dreId, long? ueId)
        {
            var query = new StringBuilder("select f.*,fb.*,p.*, t.*, d.*,u.*");
            query.AppendLine("from");
            query.AppendLine("periodo_fechamento f");
            query.AppendLine("inner join periodo_fechamento_bimestre fb on");
            query.AppendLine("f.id = fb.periodo_fechamento_id");
            query.AppendLine("inner join periodo_escolar p on");
            query.AppendLine("fb.periodo_escolar_id = p.id");
            query.AppendLine("inner join tipo_calendario t on");
            query.AppendLine("p.tipo_calendario_id = t.id");
            query.AppendLine("left join dre d on f.dre_id = d.id");
            query.AppendLine("left join ue u on f.ue_id = u.id");
            query.AppendLine("where 1=1");
            query.AppendLine("and p.tipo_calendario_id = @tipoCalendarioId");

            if (dreId.HasValue)
                query.AppendLine("and f.dre_id = @dreId");
            else query.AppendLine("and f.dre_id is null");

            if (ueId.HasValue)
                query.AppendLine("and f.ue_id = @ueId");
            else query.AppendLine("and f.ue_id is null");

            var lookup = new Dictionary<long, PeriodoFechamento>();

            var lista = database.Conexao.Query<PeriodoFechamento, PeriodoFechamentoBimestre, PeriodoEscolar, TipoCalendario, Dre, Ue, PeriodoFechamento>(query.ToString(), (fechamento, fechamentoBimestre, periodoEscolar, tipoCalendario, dre, ue) =>
               {
                   PeriodoFechamento periodoFechamento;
                   if (!lookup.TryGetValue(fechamento.Id, out periodoFechamento))
                   {
                       periodoFechamento = fechamento;
                       lookup.Add(fechamento.Id, periodoFechamento);
                   }

                   periodoEscolar.AdicionarTipoCalendario(tipoCalendario);
                   fechamentoBimestre.AdicionarPeriodoEscolar(periodoEscolar);
                   periodoFechamento.AdicionarFechamentoBimestre(fechamentoBimestre);
                   periodoFechamento.AdicionarDre(dre);
                   periodoFechamento.AdicionarUe(ue);
                   return periodoFechamento;
               }, new
               {
                   tipoCalendarioId,
                   dreId,
                   ueId
               });
            return lookup.Values.FirstOrDefault();
        }

        public void SalvarBimestres(IEnumerable<PeriodoFechamentoBimestre> fechamentosBimestre, long fechamentoId)
        {
            if (fechamentosBimestre == null || !fechamentosBimestre.Any())
            {
                throw new NegocioException("A lista de bimestres é obrigatória.");
            }

            foreach (var bimestre in fechamentosBimestre)
            {
                bimestre.FechamentoId = fechamentoId;
                if (bimestre.Id > 0)
                    database.Conexao.Update(bimestre);
                else bimestre.Id = (long)database.Conexao.Insert(bimestre);
            }
        }

        public bool ValidaRegistrosForaDoPeriodo(DateTime inicioDoFechamento, DateTime finalDoFechamento, long fechamentoId, long periodoEscolarId, long? dreId)
        {
            var query = new StringBuilder("select 1 from periodo_fechamento_bimestre fb ");
            query.AppendLine("where");
            query.AppendLine("(fb.inicio_fechamento::date < @inicioDoFechamento::date");
            query.AppendLine("or fb.final_fechamento::date > @finalDoFechamento::date)");
            query.AppendLine("and fb.periodo_escolar_id = @periodoEscolarId");
            query.AppendLine("and fb.periodo_fechamento_id <> @fechamentoId");
            if (dreId.HasValue)
            {
                query.AppendLine("and fb.periodo_fechamento_id = Any(select id from periodo_fechamento where dre_id = @dreId)");
            }

            return database.Conexao.QueryFirstOrDefault<bool>(query.ToString(), new
            {
                inicioDoFechamento,
                finalDoFechamento,
                periodoEscolarId,
                dreId,
                fechamentoId
            });
        }
    }
}