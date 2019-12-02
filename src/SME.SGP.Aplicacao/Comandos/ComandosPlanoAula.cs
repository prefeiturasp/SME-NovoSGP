using System;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ComandosPlanoAula : IComandosPlanoAula
    {
        private readonly IRepositorioPlanoAula repositorio;
        private readonly IRepositorioObjetivoAprendizagemAula repositorioObjetivosAula;
        private readonly IRepositorioAula repositorioAula;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IConsultasObjetivoAprendizagem consultasObjetivosPlanoAnual;
        private readonly IConsultasPlanoAnual consultasPlanoAnual;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;

        public ComandosPlanoAula(IRepositorioPlanoAula repositorioPlanoAula, 
                        IRepositorioObjetivoAprendizagemAula repositorioObjetivosAula,
                        IRepositorioAula repositorioAula,
                        IConsultasAbrangencia consultasAbrangencia,
                        IConsultasObjetivoAprendizagem consultasObjetivosPlanoAnual,
                        IConsultasPlanoAnual consultasPlanoAnual,
                        IServicoUsuario servicoUsuario,
                        IUnitOfWork unitOfWork)
        {
            this.repositorio = repositorioPlanoAula;
            this.repositorioObjetivosAula = repositorioObjetivosAula;
            this.repositorioAula = repositorioAula;
            this.consultasAbrangencia = consultasAbrangencia;
            this.consultasObjetivosPlanoAnual = consultasObjetivosPlanoAnual;
            this.consultasPlanoAnual = consultasPlanoAnual;
            this.unitOfWork = unitOfWork;
            this.servicoUsuario = servicoUsuario;
        }

        public async Task ExcluirPlanoDaAula(long aulaId)
        {
            await repositorio.ExcluirPlanoDaAula(aulaId);
        }

        public async Task Salvar(PlanoAulaDto planoAulaDto)
        {
            var aula = repositorioAula.ObterPorId(planoAulaDto.AulaId);
            var abrangenciaTurma = await consultasAbrangencia.ObterAbrangenciaTurma(aula.TurmaId);

            if (abrangenciaTurma == null)
                throw new NegocioException("Usuario sem acesso a turma da respectiva aula");

            PlanoAula planoAula = await repositorio.ObterPlanoAulaPorAula(planoAulaDto.AulaId);
            planoAula = MapearParaDominio(planoAulaDto, planoAula);

            if (planoAulaDto.ObjetivosAprendizagemJurema == null || !planoAulaDto.ObjetivosAprendizagemJurema.Any())
            {
                var permitePlanoSemObjetivos = false;

                // Os seguintes componentes curriculares (disciplinas) não tem seleção de objetivos de aprendizagem
                // Libras, Sala de Leitura
                permitePlanoSemObjetivos = new string[] { "218", "1061" }.Contains(aula.DisciplinaId);

                // EJA e Médio não obrigam seleção
                if (!permitePlanoSemObjetivos)
                {
                    permitePlanoSemObjetivos = new[] { Modalidade.EJA, Modalidade.Medio }.Contains(abrangenciaTurma.Modalidade);
                }

                // Para professores substitutos (CJ) a seleção dos objetivos deve ser opcional
                if (!permitePlanoSemObjetivos)
                {
                    var usuario = await servicoUsuario.ObterUsuarioLogado();
                    permitePlanoSemObjetivos = usuario.EhProfessorCj();
                }

                if (!permitePlanoSemObjetivos)
                    throw new NegocioException("A seleção de objetivos de aprendizagem é obrigatória para criação do plano de aula");
            }

            var bimestre = (aula.DataAula.Month + 2) / 3;
            var planoAnualId = await consultasPlanoAnual.ObterIdPlanoAnualPorAnoEscolaBimestreETurma(
                        aula.DataAula.Year, aula.UeId, long.Parse(aula.TurmaId), bimestre, long.Parse(aula.DisciplinaId));

            if (planoAnualId <= 0)
                throw new NegocioException("Não foi possível concluir o cadasatro, pois não existe plano anual cadastrado");

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                repositorio.Salvar(planoAula);
                // Salvar Objetivos
                await repositorioObjetivosAula.LimparObjetivosAula(planoAula.Id);
                if (planoAulaDto.ObjetivosAprendizagemJurema != null)
                    foreach(var objetivoJuremaId in planoAulaDto.ObjetivosAprendizagemJurema)
                    {
                        var objetivoPlanoAnualId = await consultasObjetivosPlanoAnual
                                .ObterIdPorObjetivoAprendizagemJurema(planoAnualId, objetivoJuremaId);

                        repositorioObjetivosAula.Salvar(new ObjetivoAprendizagemAula(planoAula.Id, objetivoPlanoAnualId));
                    }

                unitOfWork.PersistirTransacao();
            }        
        }

        private PlanoAula MapearParaDominio(PlanoAulaDto planoDto, PlanoAula planoAula = null)
        {
            if (planoAula == null)
                planoAula = new PlanoAula();

            planoAula.AulaId = planoDto.AulaId;
            planoAula.Descricao = planoDto.Descricao;
            planoAula.DesenvolvimentoAula = planoDto.DesenvolvimentoAula;
            planoAula.RecuperacaoAula = planoDto.RecuperacaoAula;
            planoAula.LicaoCasa = planoDto.LicaoCasa;

            return planoAula;
        }
    }
}
