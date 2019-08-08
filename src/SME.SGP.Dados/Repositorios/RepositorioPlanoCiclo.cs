using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Linq;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoCiclo : RepositorioBase<PlanoCiclo>, IRepositorioPlanoCiclo
    {
        public RepositorioPlanoCiclo(ISgpContext conexao) : base(conexao)
        {
        }

        //public PlanoCicloDto ObterPlanoCicloComMatrizesEObjetivos(long id)
        //{
        //    var planoCiclo = ObterPorId(id);
        //    if (planoCiclo == null)
        //    {
        //        throw new NegocioException("Plano de ciclo não encontrado.");
        //    }

        //    var matrizes = database.Conexao().Query<MatrizSaberPlano>("select * from matriz_saber_plano where plano_id = @Id", new { Id = id });
        //    var objetivos = database.Conexao().Query<ObjetivoDesenvolvimentoPlano>("select * from objetivo_desenvolvimento_plano where plano_id = @Id", new { Id = id });
        //    var planoDto = new PlanoCicloDto()
        //    {
        //        Id = planoCiclo.Id,
        //        Descricao = planoCiclo.Descricao,
        //        IdsMatrizesSaber = matrizes?.Select(c => c.MatrizSaberId).ToList(),
        //        IdsObjetivosDesenvolvimento = objetivos?.Select(c => c.ObjetivoDesenvolvimentoId).ToList(),
        //    };
        //    return planoDto;
        //}

        //TODO TRANSFORMAR EM PIVOT
        public PlanoCicloDto ObterPlanoCicloComMatrizesEObjetivos(long id)
        {
            StringBuilder query = new StringBuilder();
            query.Append("select \n");
            query.Append("	msp.matriz_id, \n");
            query.Append("	odp.objetivo_desenvolvimento_id\n");
            query.Append("from \n");
            query.Append("	plano_ciclo p \n");
            query.Append("inner join matriz_saber_plano msp on \n");
            query.Append("	p.id = msp.plano_id \n");
            query.Append("inner join objetivo_desenvolvimento_plano odp on \n");
            query.Append("	p.id = odp.plano_id \n");
            query.Append("where \n");
            query.Append("	p.id = @Id");

            var listaPlanos = database.Conexao().Query<PlanoCicloCompletoDto>(query.ToString(), new { Id = id });
            var planoCiclo = base.ObterPorId(id);
            var planoDto = new PlanoCicloDto()
            {
                Descricao = planoCiclo.Descricao,
                Id = id,
            };

            planoDto.IdsMatrizesSaber.AddRange(listaPlanos.Select(c => c.MatrizId));
            planoDto.IdsObjetivosDesenvolvimento.AddRange(listaPlanos.Select(c => c.ObjetivoDesenvolvimentoId));
            return planoDto;
        }
    }
}