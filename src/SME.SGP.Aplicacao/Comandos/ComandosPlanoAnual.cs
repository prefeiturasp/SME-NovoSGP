using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ComandosPlanoAnual : IComandosPlanoAnual
    {
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;
        private readonly IRepositorioDisciplinaPlano repositorioDisciplinaPlano;
        private readonly IRepositorioObjetivoAprendizagemPlano repositorioObjetivoAprendizagemPlano;
        private readonly IRepositorioPlanoAnual repositorioPlanoAnual;
        private readonly IUnitOfWork unitOfWork;

        public ComandosPlanoAnual(IRepositorioPlanoAnual repositorioPlanoAnual,
                                  IRepositorioObjetivoAprendizagemPlano repositorioObjetivoAprendizagemPlano,
                                  IRepositorioDisciplinaPlano repositorioDisciplinaPlano,
                                  IRepositorioComponenteCurricular repositorioComponenteCurricular,
                                  IUnitOfWork unitOfWork)
        {
            this.repositorioPlanoAnual = repositorioPlanoAnual ?? throw new ArgumentNullException(nameof(repositorioPlanoAnual));
            this.repositorioObjetivoAprendizagemPlano = repositorioObjetivoAprendizagemPlano ?? throw new ArgumentNullException(nameof(repositorioObjetivoAprendizagemPlano));
            this.repositorioDisciplinaPlano = repositorioDisciplinaPlano ?? throw new ArgumentNullException(nameof(repositorioDisciplinaPlano));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void Salvar(PlanoAnualDto planoAnualDto)
        {
            PlanoAnual planoAnual = ObterPlanoAnual(planoAnualDto);
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                planoAnualDto.Id = repositorioPlanoAnual.Salvar(planoAnual);
                AjustarObjetivosAprendizagem(planoAnualDto);
                unitOfWork.PersistirTransacao();
            }
        }

        private void AjustarObjetivosAprendizagem(PlanoAnualDto planoAnualDto)
        {
            var objetivosAprendizagemPlanoAnual = repositorioObjetivoAprendizagemPlano.ObterObjetivosAprendizagemPorIdPlano(planoAnualDto.Id);

            if (objetivosAprendizagemPlanoAnual != null)
            {
                RemoverObjetivos(planoAnualDto, objetivosAprendizagemPlanoAnual);
                InserirObjetivos(planoAnualDto, objetivosAprendizagemPlanoAnual);
            }
        }

        private void InserirObjetivos(PlanoAnualDto planoAnualDto, IEnumerable<ObjetivoAprendizagemPlano> objetivosAprendizagemPlanoAnual)
        {
            var objetivosIncluir = planoAnualDto.IdsObjetivosAprendizagem.Except(objetivosAprendizagemPlanoAnual.Select(c => c.CodigoComponenteEOL));
            var componentesCurriculares = repositorioComponenteCurricular.Listar();

            foreach (var codigoComponenteEol in objetivosIncluir)
            {
                var componenteEol = componentesCurriculares.FirstOrDefault(c => c.CodigoEOL == codigoComponenteEol);
                repositorioObjetivoAprendizagemPlano.Salvar(new ObjetivoAprendizagemPlano()
                {
                    CodigoComponenteEOL = codigoComponenteEol,
                    CodigoComponenteJurema = componenteEol.CodigoJurema,
                    PlanoId = planoAnualDto.Id
                });
            }
        }

        private void MapearParaDominio(PlanoAnualDto planoAnualDto, PlanoAnual planoAnual)
        {
            planoAnual.Ano = planoAnualDto.Ano.Value;
            planoAnual.Bimestre = planoAnualDto.Bimestre.Value;
            planoAnual.Descricao = planoAnualDto.Descricao;
            planoAnual.EscolaId = planoAnualDto.EscolaId.Value;
            planoAnual.TurmaId = planoAnualDto.TurmaId.Value;
        }

        private PlanoAnual ObterPlanoAnual(PlanoAnualDto planoAnualDto)
        {
            var planoAnual = new PlanoAnual();
            if (planoAnualDto.Id > 0)
            {
                planoAnual = repositorioPlanoAnual.ObterPorId(planoAnualDto.Id);
                if (planoAnual == null)
                {
                    throw new NegocioException("Plano anual não encontrado.");
                }
            }
            else
            {
                var planoExistente = repositorioPlanoAnual.ValidarPlanoExistentePorAnoEscolaTurmaEBimestre(planoAnualDto.Ano.Value, planoAnualDto.EscolaId.Value, planoAnualDto.TurmaId.Value, planoAnualDto.Bimestre.Value);
                if (planoExistente)
                {
                    throw new NegocioException("Já existe um plano anual com o ano, escola, turma e bimestre informados.");
                }
            }
            MapearParaDominio(planoAnualDto, planoAnual);
            return planoAnual;
        }

        private void RemoverObjetivos(PlanoAnualDto planoAnualDto, IEnumerable<ObjetivoAprendizagemPlano> objetivosAprendizagemPlanoAnual)
        {
            if (objetivosAprendizagemPlanoAnual != null)
            {
                var objetivosRemover = objetivosAprendizagemPlanoAnual.Where(c => !planoAnualDto.IdsObjetivosAprendizagem.Contains(c.CodigoComponenteEOL));

                foreach (var objetivo in objetivosRemover)
                {
                    repositorioObjetivoAprendizagemPlano.Remover(objetivo.Id);
                }
            }
        }
    }
}