using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosPlanoAnual : IComandosPlanoAnual
    {
        private readonly IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem;
        private readonly IConsultasProfessor consultasProfessor;
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;
        private readonly IRepositorioObjetivoAprendizagemPlano repositorioObjetivoAprendizagemPlano;
        private readonly IRepositorioPlanoAnual repositorioPlanoAnual;
        private readonly IUnitOfWork unitOfWork;

        public ComandosPlanoAnual(IRepositorioPlanoAnual repositorioPlanoAnual,
                                  IRepositorioObjetivoAprendizagemPlano repositorioObjetivoAprendizagemPlano,
                                  IRepositorioComponenteCurricular repositorioComponenteCurricular,
                                  IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem,
                                  IConsultasProfessor consultasProfessor,
                                  IUnitOfWork unitOfWork)
        {
            this.repositorioPlanoAnual = repositorioPlanoAnual ?? throw new ArgumentNullException(nameof(repositorioPlanoAnual));
            this.repositorioObjetivoAprendizagemPlano = repositorioObjetivoAprendizagemPlano ?? throw new ArgumentNullException(nameof(repositorioObjetivoAprendizagemPlano));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem ?? throw new ArgumentNullException(nameof(consultasObjetivoAprendizagem));
            this.consultasProfessor = consultasProfessor ?? throw new ArgumentNullException(nameof(consultasProfessor));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task Migrar(MigrarPlanoAnualDto migrarPlanoAnualDto)
        {
            var planoAnualDto = migrarPlanoAnualDto.PlanoAnual;
            var planoAnualOrigem = ObterPlanoAnualSimplificado(planoAnualDto);
            if (planoAnualOrigem == null)
            {
                throw new NegocioException("Plano anual de origemnão encontrado");
            }

            await ValidaTurmasProfessor(migrarPlanoAnualDto, planoAnualDto);

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                foreach (var turmaId in migrarPlanoAnualDto.IdsTurmasDestino)
                {
                    planoAnualDto.TurmaId = turmaId;
                    var planoAnual = ObterPlanoAnualSimplificado(planoAnualDto);
                    if (planoAnual == null)
                    {
                        planoAnual = MapearParaDominio(planoAnualDto, planoAnual);
                    }
                    planoAnual.Descricao = planoAnualOrigem.Descricao;
                    Salvar(planoAnualDto, planoAnual);
                }
                unitOfWork.PersistirTransacao();
            }
        }

        public void Salvar(PlanoAnualDto planoAnualDto)
        {
            PlanoAnual planoAnual = ObterPlanoAnualSimplificado(planoAnualDto);
            planoAnual = MapearParaDominio(planoAnualDto, planoAnual);
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                Salvar(planoAnualDto, planoAnual);
                unitOfWork.PersistirTransacao();
            }
        }

        private static void ValidarObjetivoPertenceAoComponenteCurricular(IEnumerable<ObjetivoAprendizagemDto> objetivosAprendizagem,
                                                                          ObjetivoAprendizagemSimplificadoDto objetivo,
                                                                          ComponenteCurricular componenteEol)
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

        private PlanoAnual MapearParaDominio(PlanoAnualDto planoAnualDto, PlanoAnual planoAnual)
        {
            if (planoAnual == null)
            {
                planoAnual = new PlanoAnual();
            }
            planoAnual.Ano = planoAnualDto.AnoLetivo.Value;
            planoAnual.Bimestre = planoAnualDto.Bimestre.Value;
            planoAnual.Descricao = planoAnualDto.Descricao;
            planoAnual.EscolaId = planoAnualDto.EscolaId;
            planoAnual.TurmaId = planoAnualDto.TurmaId.Value;
            return planoAnual;
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

        private PlanoAnual ObterPlanoAnualSimplificado(PlanoAnualDto planoAnualDto)
        {
            return repositorioPlanoAnual.ObterPlanoAnualSimplificadoPorAnoEscolaBimestreETurma(planoAnualDto.AnoLetivo.Value,
                                                                                                      planoAnualDto.EscolaId,
                                                                                                      planoAnualDto.TurmaId.Value,
                                                                                                      planoAnualDto.Bimestre.Value);
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

        private void Salvar(PlanoAnualDto planoAnualDto, PlanoAnual planoAnual)
        {
            planoAnualDto.Id = repositorioPlanoAnual.Salvar(planoAnual);
            AjustarObjetivosAprendizagem(planoAnualDto);
        }

        private void SalvarObjetivoAprendizagem(PlanoAnualDto planoAnualDto,
                                                IEnumerable<ComponenteCurricular> componentesCurriculares,
                                                IEnumerable<ObjetivoAprendizagemDto> objetivosAprendizagem,
                                                ObjetivoAprendizagemSimplificadoDto objetivo)
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

        private async Task ValidaTurmasProfessor(MigrarPlanoAnualDto migrarPlanoAnualDto, PlanoAnualDto planoAnualDto)
        {
            var turmasAtribuidasAoProfessor = await consultasProfessor.ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(migrarPlanoAnualDto.RFProfessor, planoAnualDto.EscolaId, planoAnualDto.AnoLetivo.Value);
            var idsTurmasProfessor = turmasAtribuidasAoProfessor?.Select(c => c.CodigoTurma).ToList();

            if (idsTurmasProfessor == null || migrarPlanoAnualDto.IdsTurmasDestino.Any(c => !idsTurmasProfessor.Contains(c)))
            {
                throw new NegocioException("Somente é possível migrar o plano anual para turmas atribuidas ao professor");
            }
        }
    }
}