using SME.SGP.Dominio;
using SME.SGP.Dto;
using System;
using System.Linq;

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
            AjustarMatrizes(planoCiclo, planoCicloDto);
        }

        private void AjustarMatrizes(PlanoCiclo planoCiclo, PlanoCicloDto planoCicloDto)
        {
            var matrizesIncluir = planoCicloDto.IdsMatrizesSaber.Except(planoCiclo.Matrizes.Select(c => c.Id));
            var matrizesRemover = planoCiclo.Matrizes.Select(c => c.Id).Except(planoCicloDto.IdsMatrizesSaber);

            foreach (var idMatrizRemover in matrizesRemover)
            {
                repositorioMatrizSaberPlano.Remover(idMatrizRemover);
            }

            foreach (var idMatrizIncluir in matrizesIncluir)
            {
                repositorioMatrizSaberPlano.Salvar(new MatrizSaberPlano()
                {
                    MatrizSaberId = idMatrizIncluir,
                    PlanoId = planoCiclo.Id
                });
            }
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



            //var matrizSaberPlano = repositorioMatrizSaberPlano.ObterMatrizesSaberDoPlano(planoCicloDto.IdsMatrizesSaber, planoCiclo.Id);
            ////foreach (var matrizId in planoCicloDto.IdsMatrizesSaber)
            ////{
            ////}

            return planoCiclo;
        }
    }
}
