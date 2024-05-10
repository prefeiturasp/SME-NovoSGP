﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using SME.SGP.Infra.Utilitarios;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosPlanoCiclo : IComandosPlanoCiclo
    {
        private readonly IRepositorioMatrizSaberPlano repositorioMatrizSaberPlano;
        private readonly IRepositorioObjetivoDesenvolvimentoPlano repositorioObjetivoDesenvolvimentoPlano;
        private readonly IRepositorioPlanoCiclo repositorioPlanoCiclo;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        private readonly IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions;

        public ComandosPlanoCiclo(IRepositorioPlanoCiclo repositorioPlanoCiclo,
                                  IRepositorioMatrizSaberPlano repositorioMatrizSaberPlano,
                                  IRepositorioObjetivoDesenvolvimentoPlano repositorioObjetivoDesenvolvimentoPlano,
                                  IUnitOfWork unitOfWork, IMediator mediator,
                                  IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions)
        {
            this.repositorioPlanoCiclo = repositorioPlanoCiclo ?? throw new ArgumentNullException(nameof(repositorioPlanoCiclo));
            this.repositorioMatrizSaberPlano = repositorioMatrizSaberPlano ?? throw new ArgumentNullException(nameof(repositorioMatrizSaberPlano));
            this.repositorioObjetivoDesenvolvimentoPlano = repositorioObjetivoDesenvolvimentoPlano ?? throw new ArgumentNullException(nameof(repositorioObjetivoDesenvolvimentoPlano));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
        }

        public async Task Salvar(PlanoCicloDto planoCicloDto)
        {
            string descricaoAtual;
            var planoCiclo = MapearParaDominio(planoCicloDto, out descricaoAtual);
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    planoCicloDto.Id = repositorioPlanoCiclo.Salvar(planoCiclo);
                    AjustarMatrizes(planoCiclo, planoCicloDto);
                    AjustarObjetivos(planoCiclo, planoCicloDto);
                    unitOfWork.PersistirTransacao();
                } catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
                await MoverRemoverExcluidos(planoCicloDto,descricaoAtual);
            }
        }

        private void AjustarMatrizes(PlanoCiclo planoCiclo, PlanoCicloDto planoCicloDto)
        {
            var matrizesPlanoCiclo = repositorioMatrizSaberPlano.ObterMatrizesPorIdPlano(planoCiclo.Id);

            var idsMatrizes = matrizesPlanoCiclo?.Select(c => c.MatrizSaberId)?.ToList();
            RemoverMatrizes(planoCicloDto, matrizesPlanoCiclo);
            InserirMatrizes(planoCiclo, planoCicloDto, idsMatrizes);
        }

        private void AjustarObjetivos(PlanoCiclo planoCiclo, PlanoCicloDto planoCicloDto)
        {
            var objetivosPlanoCiclo = repositorioObjetivoDesenvolvimentoPlano.ObterObjetivosDesenvolvimentoPorIdPlano(planoCiclo.Id);
            var idsObjetivos = objetivosPlanoCiclo?.Select(c => c.ObjetivoDesenvolvimentoId)?.ToList();

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
                repositorioObjetivoDesenvolvimentoPlano.Salvar(new RecuperacaoParalelaObjetivoDesenvolvimentoPlano()
                {
                    ObjetivoDesenvolvimentoId = idObjetivo,
                    PlanoId = planoCicloDto.Id
                });
            }
        }

        private PlanoCiclo MapearParaDominio(PlanoCicloDto planoCicloDto,out string descricaoAtual)
        {
            descricaoAtual = string.Empty;
            if (planoCicloDto.EhNulo())
            {
                throw new ArgumentNullException(nameof(planoCicloDto));
            }
            if (planoCicloDto.Id == 0 && repositorioPlanoCiclo.ObterPlanoCicloPorAnoCicloEEscola(planoCicloDto.Ano, planoCicloDto.CicloId, planoCicloDto.EscolaId))
            {
                throw new NegocioException("Já existe um plano ciclo referente a este Ano/Ciclo/Escola.");
            }

            var planoCiclo = repositorioPlanoCiclo.ObterPorId(planoCicloDto.Id);
            if (planoCiclo.EhNulo())
            {
                planoCiclo = new PlanoCiclo();
            }
            else
            {
                descricaoAtual = planoCiclo.Descricao;
            }
            if (!planoCiclo.Migrado)
            {
                if (planoCicloDto.IdsMatrizesSaber.EhNulo() || !planoCicloDto.IdsMatrizesSaber.Any())
                {
                    throw new NegocioException("A matriz de saberes deve conter ao menos 1 elemento.");
                }
                if (planoCicloDto.IdsObjetivosDesenvolvimento.EhNulo() || !planoCicloDto.IdsObjetivosDesenvolvimento.Any())
                {
                    throw new NegocioException("Os objetivos de desenvolvimento sustentável devem conter ao menos 1 elemento.");
                }
            }
            planoCiclo.Descricao = planoCicloDto.Descricao.Replace(configuracaoArmazenamentoOptions.Value.BucketTemp, configuracaoArmazenamentoOptions.Value.BucketArquivos);
            planoCiclo.CicloId = planoCicloDto.CicloId;
            planoCiclo.Ano = planoCicloDto.Ano;
            planoCiclo.EscolaId = planoCicloDto.EscolaId;
            return planoCiclo;
        }
        private async Task MoverRemoverExcluidos(PlanoCicloDto novo, string atual)
        {
            if (!string.IsNullOrEmpty(novo.Descricao))
            {
                var moverArquivo = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.PlanoCiclo, atual, novo.Descricao));
                novo.Descricao = moverArquivo;
            }
            if (!string.IsNullOrEmpty(atual))
            {
                var deletarArquivosNaoUtilziados = await mediator.Send(new RemoverArquivosExcluidosCommand(atual, novo.Descricao, TipoArquivo.PlanoCiclo.Name()));
            }
        }
        private void RemoverMatrizes(PlanoCicloDto planoCicloDto, IEnumerable<MatrizSaberPlano> matrizesPlanoCiclo)
        {
            if (matrizesPlanoCiclo.NaoEhNulo())
            {
                var matrizesRemover = matrizesPlanoCiclo.Where(c => !planoCicloDto.IdsMatrizesSaber.Contains(c.MatrizSaberId));
                foreach (var matriz in matrizesRemover)
                {
                    repositorioMatrizSaberPlano.Remover(matriz.Id);
                }
            }
        }

        private void RemoverObjetivos(PlanoCicloDto planoCicloDto, IEnumerable<RecuperacaoParalelaObjetivoDesenvolvimentoPlano> objetivosPlanoCiclo)
        {
            if (objetivosPlanoCiclo.NaoEhNulo())
            {
                var objetivosRemover = objetivosPlanoCiclo.Where(c => !planoCicloDto.IdsObjetivosDesenvolvimento.Contains(c.ObjetivoDesenvolvimentoId));

                foreach (var objetivo in objetivosRemover)
                {
                    repositorioObjetivoDesenvolvimentoPlano.Remover(objetivo.Id);
                }
            }
        }
    }
}