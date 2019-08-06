using SME.SGP.Dominio;
using SME.SGP.Dto;
using System;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ManterPlanoCiclo : IManterPlanoCiclo
    {
        private readonly IRepositorioPlanoCiclo repositorioPlanoCiclo;
        private readonly IRepositorioMatrizSaberPlano repositorioMatrizSaberPlano;

        public ManterPlanoCiclo(IRepositorioPlanoCiclo repositorioPlanoCiclo,
                               IRepositorioMatrizSaberPlano repositorioMatrizSaberPlano)
        {
            this.repositorioPlanoCiclo = repositorioPlanoCiclo ?? throw new ArgumentNullException(nameof(repositorioPlanoCiclo));
            this.repositorioMatrizSaberPlano = repositorioMatrizSaberPlano ?? throw new ArgumentNullException(nameof(repositorioMatrizSaberPlano));
        }
        public void Salvar(PlanoCicloDto planoCicloDto)
        {
            var planoCiclo = MapearParaDominio(planoCicloDto);
            repositorioPlanoCiclo.Salvar(planoCiclo);
        }

        private PlanoCiclo MapearParaDominio(PlanoCicloDto planoCicloDto)
        {
            if (planoCicloDto == null)
            {
                return null;
            }
            var planoCiclo = repositorioPlanoCiclo.ObterPorId(planoCicloDto.Id);
            if (planoCiclo == null)
            {
                planoCiclo = new PlanoCiclo();
            }

            var matrizSaberPlano = repositorioMatrizSaberPlano.ObterMatrizesSaberDoPlano(planoCicloDto.IdsMatrizesSaber, planoCiclo.Id);
            //foreach (var matrizId in planoCicloDto.IdsMatrizesSaber)
            //{
            //}

            return planoCiclo;
        }
    }
}
