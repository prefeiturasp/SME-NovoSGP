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
        private readonly IRepositorioDisciplinaPlano repositorioDisciplinaPlano;
        private readonly IRepositorioObjetivoAprendizagemPlano repositorioObjetivoAprendizagemPlano;
        private readonly IRepositorioPlanoAnual repositorioPlanoAnual;
        private readonly IUnitOfWork unitOfWork;

        public ComandosPlanoAnual(IRepositorioPlanoAnual repositorioPlanoAnual,
                                  IRepositorioObjetivoAprendizagemPlano repositorioObjetivoAprendizagemPlano,
                                  IRepositorioDisciplinaPlano repositorioDisciplinaPlano,
                                  IUnitOfWork unitOfWork)
        {
            this.repositorioPlanoAnual = repositorioPlanoAnual ?? throw new ArgumentNullException(nameof(repositorioPlanoAnual));
            this.repositorioObjetivoAprendizagemPlano = repositorioObjetivoAprendizagemPlano ?? throw new ArgumentNullException(nameof(repositorioObjetivoAprendizagemPlano));
            this.repositorioDisciplinaPlano = repositorioDisciplinaPlano ?? throw new ArgumentNullException(nameof(repositorioDisciplinaPlano));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void Salvar(PlanoAnualDto planoAnualDto)
        {
            PlanoAnual planoAnual = ObterPlanoAnual(planoAnualDto);
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                planoAnualDto.Id = repositorioPlanoAnual.Salvar(planoAnual);
                AjustarObjetivosAprendizagem(planoAnualDto);
                AjustarDisciplinas(planoAnualDto);
                unitOfWork.PersistirTransacao();
            }
        }

        private void AjustarDisciplinas(PlanoAnualDto planoAnualDto)
        {
            var disciplinasPlanoAnual = repositorioDisciplinaPlano.ObterDisciplinasPorIdPlano(planoAnualDto.Id);

            if (disciplinasPlanoAnual != null)
            {
                var idsDisciplinas = disciplinasPlanoAnual.Select(c => c.DisciplinaId);
                RemoverDisciplinas(planoAnualDto, disciplinasPlanoAnual);
                InserirDisciplinas(planoAnualDto, idsDisciplinas);
            }
        }

        private void AjustarObjetivosAprendizagem(PlanoAnualDto planoAnualDto)
        {
            var objetivosAprendizagemPlanoAnual = repositorioObjetivoAprendizagemPlano.ObterObjetivosAprendizagemPorIdPlano(planoAnualDto.Id);

            if (objetivosAprendizagemPlanoAnual != null)
            {
                var idsObjetivos = objetivosAprendizagemPlanoAnual.Select(c => c.ObjetivoAprendizagemId);
                RemoverObjetivos(planoAnualDto, objetivosAprendizagemPlanoAnual);
                InserirObjetivos(planoAnualDto, idsObjetivos);
            }
        }

        private void InserirDisciplinas(PlanoAnualDto planoAnualDto, IEnumerable<long> idsDisciplinas)
        {
            var disciplinasIncluir = planoAnualDto.IdsDisciplinas.Except(idsDisciplinas);

            foreach (var idDisciplina in disciplinasIncluir)
            {
                repositorioDisciplinaPlano.Salvar(new DisciplinaPlano()
                {
                    DisciplinaId = idDisciplina,
                    PlanoId = planoAnualDto.Id
                });
            }
        }

        private void InserirObjetivos(PlanoAnualDto planoAnualDto, IEnumerable<long> idsObjetivos)
        {
            var objetivosIncluir = planoAnualDto.IdsObjetivosAprendizagem.Except(idsObjetivos);

            foreach (var idObjetivo in objetivosIncluir)
            {
                repositorioObjetivoAprendizagemPlano.Salvar(new ObjetivoAprendizagemPlano()
                {
                    ObjetivoAprendizagemId = idObjetivo,
                    PlanoId = planoAnualDto.Id
                });
            }
        }

        private void MapearParaDominio(PlanoAnualDto planoAnualDto, PlanoAnual planoAnual)
        {
            planoAnual.Ano = planoAnualDto.Ano;
            planoAnual.Bimestre = planoAnualDto.Bimestre;
            planoAnual.Descricao = planoAnualDto.Descricao;
            planoAnual.EscolaId = planoAnualDto.EscolaId;
            planoAnual.TurmaId = planoAnualDto.TurmaId;
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
                var planoExistente = repositorioPlanoAnual.ValidarPlanoExistentePorAnoEscolaTurmaEBimestre(planoAnualDto.Ano, planoAnualDto.EscolaId, planoAnualDto.TurmaId, planoAnualDto.Bimestre);
                if (planoExistente)
                {
                    throw new NegocioException("Já existe um plano anual com o ano, escola, turma e bimestre informados.");
                }
            }
            MapearParaDominio(planoAnualDto, planoAnual);
            return planoAnual;
        }

        private void RemoverDisciplinas(PlanoAnualDto planoAnualDto, IEnumerable<DisciplinaPlano> disciplinasPlano)
        {
            if (disciplinasPlano != null)
            {
                var disciplinasRemover = disciplinasPlano.Where(c => !planoAnualDto.IdsDisciplinas.Contains(c.DisciplinaId));

                foreach (var disciplina in disciplinasRemover)
                {
                    repositorioDisciplinaPlano.Remover(disciplina.Id);
                }
            }
        }

        private void RemoverObjetivos(PlanoAnualDto planoAnualDto, IEnumerable<ObjetivoAprendizagemPlano> objetivosAprendizagemPlanoAnual)
        {
            if (objetivosAprendizagemPlanoAnual != null)
            {
                var objetivosRemover = objetivosAprendizagemPlanoAnual.Where(c => !planoAnualDto.IdsObjetivosAprendizagem.Contains(c.ObjetivoAprendizagemId));

                foreach (var objetivo in objetivosRemover)
                {
                    repositorioObjetivoAprendizagemPlano.Remover(objetivo.Id);
                }
            }
        }
    }
}