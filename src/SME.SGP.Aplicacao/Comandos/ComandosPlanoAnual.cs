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

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                foreach (var bimestrePlanoAnual in planoAnualDto.Bimestres)
                {
                    var planoAnualOrigem = ObterPlanoAnualSimplificado(planoAnualDto, bimestrePlanoAnual.Bimestre.Value);

                    if (planoAnualOrigem == null)
                        throw new NegocioException("Plano anual de origem não encontrado");

                    await ValidaTurmasProfessor(migrarPlanoAnualDto, planoAnualDto);

                    foreach (var turmaId in migrarPlanoAnualDto.IdsTurmasDestino)
                    {
                        var planoCopia = new PlanoAnualDto(
                            planoAnualDto.AnoLetivo,
                            planoAnualDto.Bimestres,
                            planoAnualDto.EscolaId,
                            planoAnualDto.Id,
                            planoAnualDto.TurmaId);

                        planoCopia.TurmaId = turmaId;

                        var planoAnual = ObterPlanoAnualSimplificado(planoCopia, bimestrePlanoAnual.Bimestre.Value);

                        if (planoAnual == null)
                            planoAnual = MapearParaDominio(planoCopia, planoAnual, bimestrePlanoAnual);

                        planoAnual.Descricao = planoAnualOrigem.Descricao;
                        Salvar(planoCopia, planoAnual, bimestrePlanoAnual);
                    }
                }

                unitOfWork.PersistirTransacao();
            }
        }

        public void Salvar(PlanoAnualDto planoAnualDto)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                foreach (var bimestrePlanoAnual in planoAnualDto.Bimestres)
                {
                    PlanoAnual planoAnual = ObterPlanoAnualSimplificado(planoAnualDto, bimestrePlanoAnual.Bimestre.Value);
                    planoAnual = MapearParaDominio(planoAnualDto, planoAnual, bimestrePlanoAnual);
                    Salvar(planoAnualDto, planoAnual, bimestrePlanoAnual);
                }
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

        private void AjustarObjetivosAprendizagem(PlanoAnualDto planoAnualDto, BimestrePlanoAnualDto bimestrePlanoAnualDto)
        {
            var objetivosAprendizagemPlanoAnual = repositorioObjetivoAprendizagemPlano.ObterObjetivosAprendizagemPorIdPlano(planoAnualDto.Id);

            if (objetivosAprendizagemPlanoAnual != null)
            {
                RemoverObjetivos(objetivosAprendizagemPlanoAnual, bimestrePlanoAnualDto);
                InserirObjetivos(planoAnualDto, objetivosAprendizagemPlanoAnual, bimestrePlanoAnualDto);
            }
        }

        private void InserirObjetivos(PlanoAnualDto planoAnualDto, IEnumerable<ObjetivoAprendizagemPlano> objetivosAprendizagemPlanoAnual, BimestrePlanoAnualDto bimestrePlanoAnualDto)
        {
            if (bimestrePlanoAnualDto.ObjetivosAprendizagem != null && bimestrePlanoAnualDto.ObjetivosAprendizagem.Any())
            {
                var idsObjetivos = objetivosAprendizagemPlanoAnual?.Select(c => c.ObjetivoAprendizagemJuremaId);
                IEnumerable<ComponenteCurricular> componentesCurriculares = ObterComponentesCurriculares();
                IEnumerable<ObjetivoAprendizagemDto> objetivosAprendizagem = ObterObjetivosDeAprendizagem();

                foreach (var objetivo in bimestrePlanoAnualDto.ObjetivosAprendizagem)
                {
                    if (idsObjetivos != null && !idsObjetivos.Contains(objetivo.Id))
                    {
                        SalvarObjetivoAprendizagem(planoAnualDto, componentesCurriculares, objetivosAprendizagem, objetivo);
                    }
                }
            }
        }

        private PlanoAnual MapearParaDominio(PlanoAnualDto planoAnualDto, PlanoAnual planoAnual, BimestrePlanoAnualDto bimestrePlanoAnual)
        {
            if (planoAnual == null)
            {
                planoAnual = new PlanoAnual();
            }
            planoAnual.Ano = planoAnualDto.AnoLetivo.Value;
            planoAnual.Bimestre = bimestrePlanoAnual.Bimestre.Value;
            planoAnual.Descricao = bimestrePlanoAnual.Descricao;
            planoAnual.EscolaId = planoAnualDto.EscolaId;
            planoAnual.TurmaId = planoAnualDto.TurmaId.Value;
            planoAnual.ComponenteCurricularEolId = planoAnualDto.ComponenteCurricularEolId;
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

        private PlanoAnual ObterPlanoAnualSimplificado(PlanoAnualDto planoAnualDto, int bimestre)
        {
            return repositorioPlanoAnual.ObterPlanoAnualSimplificadoPorAnoEscolaBimestreETurma(planoAnualDto.AnoLetivo.Value,
                                                                                                      planoAnualDto.EscolaId,
                                                                                                      planoAnualDto.TurmaId.Value,
                                                                                                      bimestre);
        }

        private void RemoverObjetivos(IEnumerable<ObjetivoAprendizagemPlano> objetivosAprendizagemPlanoAnual, BimestrePlanoAnualDto bimestrePlanoAnualDto)
        {
            if (objetivosAprendizagemPlanoAnual != null)
            {
                var idsObjetivos = bimestrePlanoAnualDto.ObjetivosAprendizagem.Select(x => x.Id);
                var objetivosRemover = objetivosAprendizagemPlanoAnual.Where(c => !idsObjetivos.Contains(c.ObjetivoAprendizagemJuremaId));

                foreach (var objetivo in objetivosRemover)
                {
                    repositorioObjetivoAprendizagemPlano.Remover(objetivo.Id);
                }
            }
        }

        private void Salvar(PlanoAnualDto planoAnualDto, PlanoAnual planoAnual, BimestrePlanoAnualDto bimestrePlanoAnualDto)
        {
            planoAnualDto.Id = repositorioPlanoAnual.Salvar(planoAnual);
            if (!planoAnual.Migrado)
                AjustarObjetivosAprendizagem(planoAnualDto, bimestrePlanoAnualDto);
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