using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
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
        private readonly IConsultasPlanoAnual consultasPlanoAnual;
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;
        private readonly IRepositorioObjetivoAprendizagemPlano repositorioObjetivoAprendizagemPlano;
        private readonly IRepositorioPlanoAnual repositorioPlanoAnual;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;

        public ComandosPlanoAnual(IRepositorioPlanoAnual repositorioPlanoAnual,
                                  IRepositorioObjetivoAprendizagemPlano repositorioObjetivoAprendizagemPlano,
                                  IRepositorioComponenteCurricular repositorioComponenteCurricular,
                                  IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem,
                                  IConsultasProfessor consultasProfessor,
                                  IConsultasPlanoAnual consultasPlanoAnual,
                                  IUnitOfWork unitOfWork,
                                  IServicoUsuario servicoUsuario,
                                  IServicoEOL servicoEOL)
        {
            this.repositorioPlanoAnual = repositorioPlanoAnual ?? throw new ArgumentNullException(nameof(repositorioPlanoAnual));
            this.repositorioObjetivoAprendizagemPlano = repositorioObjetivoAprendizagemPlano ?? throw new ArgumentNullException(nameof(repositorioObjetivoAprendizagemPlano));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem ?? throw new ArgumentNullException(nameof(consultasObjetivoAprendizagem));
            this.consultasProfessor = consultasProfessor ?? throw new ArgumentNullException(nameof(consultasProfessor));
            this.consultasPlanoAnual = consultasPlanoAnual ?? throw new ArgumentNullException(nameof(consultasProfessor));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task Migrar(MigrarPlanoAnualDto migrarPlanoAnualDto)
        {
            var planoAnualDto = migrarPlanoAnualDto.PlanoAnual;
            var planoCopia = new PlanoAnualDto(
                planoAnualDto.AnoLetivo,
                planoAnualDto.Bimestres,
                planoAnualDto.EscolaId,
                planoAnualDto.Id,
                planoAnualDto.TurmaId,
                planoAnualDto.ComponenteCurricularEolId);

            unitOfWork.IniciarTransacao();

            foreach (var bimestrePlanoAnual in migrarPlanoAnualDto.BimestresDestino.OrderBy(c => c))
            {
                var planoAnualOrigem = ObterPlanoAnualSimplificado(planoAnualDto, bimestrePlanoAnual);

                if (planoAnualOrigem == null)
                    throw new NegocioException("Plano anual de origem não encontrado");

                var bimestreAtual = planoAnualDto.Bimestres.FirstOrDefault(c => c.Bimestre == bimestrePlanoAnual);
                foreach (var turmaId in migrarPlanoAnualDto.IdsTurmasDestino)
                {
                    planoCopia.TurmaId = turmaId;

                    var planoAnual = ObterPlanoAnualSimplificado(planoCopia, bimestrePlanoAnual);

                    if (planoAnual == null)
                        planoAnual = MapearParaDominio(planoCopia, planoAnual, bimestrePlanoAnual, bimestreAtual.Descricao);

                    planoAnual.Descricao = planoAnualOrigem.Descricao;
                    Salvar(planoCopia, planoAnual, bimestreAtual);
                }
            }
            unitOfWork.PersistirTransacao();
        }

        public async Task<IEnumerable<PlanoAnualCompletoDto>> Salvar(PlanoAnualDto planoAnualDto)
        {

            var usuarioAtual = servicoUsuario.ObterUsuarioLogado().Result;
            if (string.IsNullOrWhiteSpace(usuarioAtual.CodigoRf))
            {
                throw new NegocioException("Não foi possível obter o RF do usuário.");
            }

            unitOfWork.IniciarTransacao();
            foreach (var bimestrePlanoAnual in planoAnualDto.Bimestres)
            {
                PlanoAnual planoAnual = ObterPlanoAnualSimplificado(planoAnualDto, bimestrePlanoAnual.Bimestre.Value);
                if (planoAnual != null)
                {
                    var podePersistir = await servicoUsuario.PodePersistirTurmaDisciplina(usuarioAtual.CodigoRf, planoAnualDto.TurmaId.ToString(), planoAnualDto.ComponenteCurricularEolId.ToString(), DateTime.Now);
                    if (usuarioAtual.PerfilAtual == Perfis.PERFIL_PROFESSOR && !podePersistir)
                        throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, disciplina e data.");
                }
                planoAnual = MapearParaDominio(planoAnualDto, planoAnual, bimestrePlanoAnual.Bimestre.Value, bimestrePlanoAnual.Descricao);
                Salvar(planoAnualDto, planoAnual, bimestrePlanoAnual);
            }
            unitOfWork.PersistirTransacao();

            var resposta = await consultasPlanoAnual.ObterPorUETurmaAnoEComponenteCurricular(planoAnualDto.EscolaId, planoAnualDto.TurmaId.ToString(), planoAnualDto.AnoLetivo.Value, planoAnualDto.ComponenteCurricularEolId);
            return resposta; 
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

        private PlanoAnual MapearParaDominio(PlanoAnualDto planoAnualDto, PlanoAnual planoAnual, int bimestre, string descricao)
        {
            if (planoAnual == null)
            {
                planoAnual = new PlanoAnual();
            }
            planoAnual.Ano = planoAnualDto.AnoLetivo.Value;
            planoAnual.Bimestre = bimestre;
            planoAnual.Descricao = descricao;
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
                                                                                                      bimestre,
                                                                                                      planoAnualDto.ComponenteCurricularEolId);
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

        private async Task ValidaTurmasProfessor(MigrarPlanoAnualDto migrarPlanoAnualDto, PlanoAnualDto planoAnualDto, string codigoRF)
        {
            var turmasAtribuidasAoProfessor = await consultasProfessor.ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(codigoRF, planoAnualDto.EscolaId, planoAnualDto.AnoLetivo.Value);
            var idsTurmasProfessor = turmasAtribuidasAoProfessor?.Select(c => c.CodigoTurma).ToList();

            if (idsTurmasProfessor == null || migrarPlanoAnualDto.IdsTurmasDestino.Any(c => !idsTurmasProfessor.Contains(c)))
            {
                throw new NegocioException("Somente é possível migrar o plano anual para turmas atribuidas ao professor");
            }
        }
    }
}