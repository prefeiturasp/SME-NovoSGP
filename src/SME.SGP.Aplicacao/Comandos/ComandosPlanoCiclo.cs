using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ComandosPlanoCiclo : IComandosPlanoCiclo
    {
        private readonly IRepositorioMatrizSaberPlano repositorioMatrizSaberPlano;
        private readonly IRepositorioObjetivoDesenvolvimentoPlano repositorioObjetivoDesenvolvimentoPlano;
        private readonly IRepositorioPlanoCiclo repositorioPlanoCiclo;
        private readonly IUnitOfWork unitOfWork;

        public ComandosPlanoCiclo(IRepositorioPlanoCiclo repositorioPlanoCiclo,
                                  IRepositorioMatrizSaberPlano repositorioMatrizSaberPlano,
                                  IRepositorioObjetivoDesenvolvimentoPlano repositorioObjetivoDesenvolvimentoPlano,
                                  IUnitOfWork unitOfWork)
        {
            this.repositorioPlanoCiclo = repositorioPlanoCiclo ?? throw new ArgumentNullException(nameof(repositorioPlanoCiclo));
            this.repositorioMatrizSaberPlano = repositorioMatrizSaberPlano ?? throw new ArgumentNullException(nameof(repositorioMatrizSaberPlano));
            this.repositorioObjetivoDesenvolvimentoPlano = repositorioObjetivoDesenvolvimentoPlano ?? throw new ArgumentNullException(nameof(repositorioObjetivoDesenvolvimentoPlano));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void Salvar(PlanoCicloDto planoCicloDto)
        {
            var planoCiclo = MapearParaDominio(planoCicloDto);
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                repositorioPlanoCiclo.Salvar(planoCiclo);
                AjustarMatrizes(planoCiclo, planoCicloDto);
                AjustarObjetivos(planoCiclo, planoCicloDto);
                unitOfWork.PersistirTransacao();
            }
        }

        private void AjustarMatrizes(PlanoCiclo planoCiclo, PlanoCicloDto planoCicloDto)
        {
            var matrizesPlanoCiclo = repositorioMatrizSaberPlano.ObterMatrizesPorIdPlano(planoCiclo.Id);

            var idsMatrizes = matrizesPlanoCiclo == null ? new List<long>() : matrizesPlanoCiclo.Select(c => c.MatrizSaberId)?.ToList();
            RemoverMatrizes(planoCicloDto, matrizesPlanoCiclo);
            InserirMatrizes(planoCiclo, planoCicloDto, idsMatrizes);
        }

        private void AjustarObjetivos(PlanoCiclo planoCiclo, PlanoCicloDto planoCicloDto)
        {
            var objetivosPlanoCiclo = repositorioObjetivoDesenvolvimentoPlano.ObterObjetivosDesenvolvimentoPorIdPlano(planoCiclo.Id);
            var idsObjetivos = objetivosPlanoCiclo == null ? new List<long>() : objetivosPlanoCiclo.Select(c => c.ObjetivoDesenvolvimentoId)?.ToList();

            InserirObjetivos(planoCicloDto, idsObjetivos);
            RemoverObjetivos(planoCicloDto, objetivosPlanoCiclo);
        }

        private void InserirMatrizes(PlanoCiclo planoCiclo, PlanoCicloDto planoCicloDto, List<long> idsMatrizes)
        {
            var matrizesIncluir = planoCicloDto.IdsMatrizesSaber.Except(idsMatrizes);

            foreach (var idMatrizIncluir in matrizesIncluir)
            {
                repositorioMatrizSaberPlano.Salvar(new MatrizSaberPlano()
                {
                    MatrizSaberId = idMatrizIncluir,
                    PlanoId = planoCiclo.Id
                });
            }
        }

        private void InserirObjetivos(PlanoCicloDto planoCicloDto, List<long> idsObjetivos)
        {
            var objetivosIncluir = planoCicloDto.IdsObjetivosDesenvolvimento.Except(idsObjetivos);

            foreach (var idObjetivo in objetivosIncluir)
            {
                repositorioObjetivoDesenvolvimentoPlano.Salvar(new ObjetivoDesenvolvimentoPlano()
                {
                    ObjetivoDesenvolvimentoId = idObjetivo,
                    PlanoId = planoCicloDto.Id
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
            planoCiclo.Descricao = planoCicloDto.Descricao;
            return planoCiclo;
        }

        private void RemoverMatrizes(PlanoCicloDto planoCicloDto, IEnumerable<MatrizSaberPlano> matrizesPlanoCiclo)
        {
            var matrizesRemover = matrizesPlanoCiclo == null ? new List<MatrizSaberPlano>() : matrizesPlanoCiclo.Where(c => !planoCicloDto.IdsMatrizesSaber.Contains(c.MatrizSaberId));

            foreach (var matriz in matrizesRemover)
            {
                repositorioMatrizSaberPlano.Remover(matriz.Id);
            }
        }

        private void RemoverObjetivos(PlanoCicloDto planoCicloDto, IEnumerable<ObjetivoDesenvolvimentoPlano> objetivosPlanoCiclo)
        {
            var objetivosRemover = objetivosPlanoCiclo == null ? new List<ObjetivoDesenvolvimentoPlano>() : objetivosPlanoCiclo.Where(c => !planoCicloDto.IdsObjetivosDesenvolvimento.Contains(c.ObjetivoDesenvolvimentoId));

            foreach (var objetivo in objetivosRemover)
            {
                repositorioObjetivoDesenvolvimentoPlano.Remover(objetivo.Id);
            }
        }
    }
}