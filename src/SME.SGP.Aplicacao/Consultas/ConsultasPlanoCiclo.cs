using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ConsultasPlanoCiclo : IConsultasPlanoCiclo
    {
        private readonly IRepositorioMatrizSaberPlano repositorioMatrizSaberPlano;
        private readonly IRepositorioPlanoCiclo repositorioPlanoCiclo;

        public ConsultasPlanoCiclo(IRepositorioPlanoCiclo repositorioPlanoCiclo,
                                   IRepositorioMatrizSaberPlano repositorioMatrizSaberPlano)
        {
            this.repositorioPlanoCiclo = repositorioPlanoCiclo ?? throw new System.ArgumentNullException(nameof(repositorioPlanoCiclo));
            this.repositorioMatrizSaberPlano = repositorioMatrizSaberPlano ?? throw new System.ArgumentNullException(nameof(repositorioMatrizSaberPlano));
        }

        public PlanoCicloCompletoDto ObterPorId(long id)
        {
            return repositorioPlanoCiclo.ObterPlanoCicloComMatrizesEObjetivos(id);
        }

        private IEnumerable<PlanoCicloDto> MapearParaDto(IEnumerable<PlanoCiclo> planosCiclo)
        {
            return planosCiclo?.Select(plano => new PlanoCicloDto()
            {
                Descricao = plano.Descricao,
                Id = plano.Id
            });
        }
    }
}