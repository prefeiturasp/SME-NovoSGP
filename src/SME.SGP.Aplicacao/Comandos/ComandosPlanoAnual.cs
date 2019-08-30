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
        private readonly IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem;
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;
        private readonly IRepositorioObjetivoAprendizagemPlano repositorioObjetivoAprendizagemPlano;
        private readonly IRepositorioPlanoAnual repositorioPlanoAnual;
        private readonly IUnitOfWork unitOfWork;

        public ComandosPlanoAnual(IRepositorioPlanoAnual repositorioPlanoAnual,
                                  IRepositorioObjetivoAprendizagemPlano repositorioObjetivoAprendizagemPlano,
                                  IRepositorioComponenteCurricular repositorioComponenteCurricular,
                                  IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem,
                                  IUnitOfWork unitOfWork)
        {
            this.repositorioPlanoAnual = repositorioPlanoAnual ?? throw new ArgumentNullException(nameof(repositorioPlanoAnual));
            this.repositorioObjetivoAprendizagemPlano = repositorioObjetivoAprendizagemPlano ?? throw new ArgumentNullException(nameof(repositorioObjetivoAprendizagemPlano));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem ?? throw new ArgumentNullException(nameof(consultasObjetivoAprendizagem));
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

        private static void ValidarObjetivoPertenceAoComponenteCurricular(IEnumerable<ObjetivoAprendizagemDto> objetivosAprendizagem, ObjetivoAprendizagemSimplificadoDto objetivo, ComponenteCurricular componenteEol)
        {
            var objetivoAprendizagem = objetivosAprendizagem.FirstOrDefault(c => c.Id == objetivo.Id);
            if (objetivoAprendizagem.IdComponenteCurricular != componenteEol.CodigoJurema)
            {
                throw new NegocioException($"O objetivo de aprendizagem: '{objetivoAprendizagem.Codigo}' não pertence ao componente curricular: {componenteEol.DescricaoEOL}");
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
            if (planoAnualDto.ObjetivosAprendizagem != null && planoAnualDto.ObjetivosAprendizagem.Any())
            {
                var idsObjetivos = objetivosAprendizagemPlanoAnual?.Select(c => c.ObjetivoAprendizagemJuremaId);
                IEnumerable<ComponenteCurricular> componentesCurriculares = ObterComponentesCurriculares();
                IEnumerable<ObjetivoAprendizagemDto> objetivosAprendizagem = ObterObjetivosDeAprendizagem();

                foreach (var objetivo in planoAnualDto.ObjetivosAprendizagem)
                {
                    if (idsObjetivos != null && !idsObjetivos.Contains(objetivo.Id))
                    {
                        SalvarObjetivoAprendizagem(planoAnualDto, componentesCurriculares, objetivosAprendizagem, objetivo);
                    }
                }
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

        private IEnumerable<ComponenteCurricular> ObterComponentesCurriculares()
        {
            var componentesCurriculares = repositorioComponenteCurricular.Listar();
            if (componentesCurriculares == null)
            {
                throw new NegocioException("Não foi possível recuperar a lista de componentes curriculares.");
            }

            return componentesCurriculares;
        }

        private IEnumerable<ObjetivoAprendizagemDto> ObterObjetivosDeAprendizagem()
        {
            var objetivosAprendizagem = consultasObjetivoAprendizagem.Listar().Result;
            if (objetivosAprendizagem == null)
            {
                throw new NegocioException("Não foi possível recuperar a lista de objetivos de aprendizagem.");
            }

            return objetivosAprendizagem;
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
                var idsObjetivos = planoAnualDto.ObjetivosAprendizagem.Select(x => x.Id);
                var objetivosRemover = objetivosAprendizagemPlanoAnual.Where(c => !idsObjetivos.Contains(c.ObjetivoAprendizagemJuremaId));

                foreach (var objetivo in objetivosRemover)
                {
                    repositorioObjetivoAprendizagemPlano.Remover(objetivo.Id);
                }
            }
        }

        private void SalvarObjetivoAprendizagem(PlanoAnualDto planoAnualDto, IEnumerable<ComponenteCurricular> componentesCurriculares, IEnumerable<ObjetivoAprendizagemDto> objetivosAprendizagem, ObjetivoAprendizagemSimplificadoDto objetivo)
        {
            var componenteEol = componentesCurriculares.FirstOrDefault(c => c.CodigoJurema == objetivo.IdComponenteCurricular);

            ValidarObjetivoPertenceAoComponenteCurricular(objetivosAprendizagem, objetivo, componenteEol);

            repositorioObjetivoAprendizagemPlano.Salvar(new ObjetivoAprendizagemPlano()
            {
                ObjetivoAprendizagemJuremaId = objetivo.Id,
                ComponenteCurricularId = componenteEol.Id,
                PlanoId = planoAnualDto.Id
            });
        }
    }
}