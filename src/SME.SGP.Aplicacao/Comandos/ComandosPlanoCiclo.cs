using SME.SGP.Dominio;
using SME.SGP.Dto;
using System;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ComandosPlanoCiclo : IComandosPlanoCiclo
    {
        private readonly IRepositorioPlanoCiclo repositorioPlanoCiclo;
        private readonly IRepositorioMatrizSaberPlano repositorioMatrizSaberPlano;
        private readonly IUnitOfWork unitOfWork;

        public ComandosPlanoCiclo(IRepositorioPlanoCiclo repositorioPlanoCiclo,
                                  IRepositorioMatrizSaberPlano repositorioMatrizSaberPlano,
                                  IUnitOfWork unitOfWork)
        {
            this.repositorioPlanoCiclo = repositorioPlanoCiclo ?? throw new ArgumentNullException(nameof(repositorioPlanoCiclo));
            this.repositorioMatrizSaberPlano = repositorioMatrizSaberPlano ?? throw new ArgumentNullException(nameof(repositorioMatrizSaberPlano));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public void Salvar(PlanoCicloDto planoCicloDto)
        {
            var planoCiclo = MapearParaDominio(planoCicloDto);
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                repositorioPlanoCiclo.Salvar(planoCiclo);
                planoCiclo.Descricao = $"alterada{DateTime.Now.Millisecond}";
                repositorioPlanoCiclo.Salvar(planoCiclo);
                AjustarMatrizes(planoCiclo, planoCicloDto);
                unitOfWork.PersistirTransacao();
            }
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
            return planoCiclo;
        }
    }
}
