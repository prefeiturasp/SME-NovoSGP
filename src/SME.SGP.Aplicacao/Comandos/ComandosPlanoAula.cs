using System;
using System.Collections.Generic;
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
        private readonly IRepositorioObjetivoAprendizagemPlano repositorioObjetivoAprendizagemPlano;
        private readonly IRepositorioAula repositorioAula;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem;
        private readonly IConsultasPlanoAnual consultasPlanoAnual;
        private readonly IConsultasProfessor consultasProfessor;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;

        public ComandosPlanoAula(IRepositorioPlanoAula repositorioPlanoAula,
                        IRepositorioObjetivoAprendizagemAula repositorioObjetivosAula,
                        IRepositorioObjetivoAprendizagemPlano repositorioObjetivoAprendizagemPlano,
                        IRepositorioAula repositorioAula,
                        IConsultasAbrangencia consultasAbrangencia,
                        IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem,
                        IConsultasPlanoAnual consultasPlanoAnual,
                        IConsultasProfessor consultasProfessor,
                        IServicoUsuario servicoUsuario,
                        IUnitOfWork unitOfWork)
        {
            this.repositorio = repositorioPlanoAula;
            this.repositorioObjetivosAula = repositorioObjetivosAula;
            this.repositorioObjetivoAprendizagemPlano = repositorioObjetivoAprendizagemPlano;
            this.repositorioAula = repositorioAula;
            this.consultasAbrangencia = consultasAbrangencia;
            this.consultasProfessor = consultasProfessor;
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem;
            this.consultasPlanoAnual = consultasPlanoAnual;
            this.unitOfWork = unitOfWork;
            this.servicoUsuario = servicoUsuario;
        }

        public async Task Migrar(MigrarPlanoAulaDto migrarPlanoAulaDto)
        {
            ValidarMigracao(migrarPlanoAulaDto);

            var planoAulaDto = repositorio.ObterPorId(migrarPlanoAulaDto.PlanoAulaId);
            var objetivosPlanoAulaDto = await repositorioObjetivosAula.ObterObjetivosPlanoAula(migrarPlanoAulaDto.PlanoAulaId);

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                foreach (var planoTurma in migrarPlanoAulaDto.IdsPlanoTurmasDestino)
                {
                    AulaConsultaDto aulaConsultaDto = await
                         repositorioAula.ObterAulaDataTurmaDisciplina(
                             planoTurma.Data,
                             planoTurma.TurmaId,
                             migrarPlanoAulaDto.DisciplinaId
                         );

                    if (aulaConsultaDto == null)
                        throw new NegocioException($"Não há aula cadastrada para a turma {planoTurma.TurmaId} para a data {planoTurma.Data.ToShortDateString()} nesta disciplina!");

                    var planoCopia = new PlanoAulaDto()
                    {
                        Id = planoTurma.Sobreescrever ? migrarPlanoAulaDto.PlanoAulaId : 0,
                        AulaId = aulaConsultaDto.Id,
                        Descricao = planoAulaDto.Descricao,
                        DesenvolvimentoAula = planoAulaDto.DesenvolvimentoAula,
                        LicaoCasa = migrarPlanoAulaDto.MigrarLicaoCasa ? planoAulaDto.LicaoCasa : string.Empty,
                        ObjetivosAprendizagemJurema = !migrarPlanoAulaDto.EhProfessorCJ ||
                                                       migrarPlanoAulaDto.MigrarObjetivos ?
                                                       objetivosPlanoAulaDto.Select(o => o.ObjetivoAprendizagemPlano.ObjetivoAprendizagemJuremaId).ToList() : null,
                        RecuperacaoAula = migrarPlanoAulaDto.MigrarRecuperacaoAula ?
                                            planoAulaDto.RecuperacaoAula : string.Empty
                    };

                    await Salvar(planoCopia, false);
                }

                unitOfWork.PersistirTransacao();
            }
        }

        public async Task ExcluirPlanoDaAula(long aulaId)
        {
            await repositorio.ExcluirPlanoDaAula(aulaId);
        }

        public async Task Salvar(PlanoAulaDto planoAulaDto, bool controlarTransacao = true)
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
                throw new NegocioException("Não foi possível concluir o cadastro, pois não existe plano anual cadastrado");

            if (controlarTransacao)
            {
                using (var transacao = unitOfWork.IniciarTransacao())
                {
                    await SalvarPlanoAula(planoAula, planoAulaDto, planoAnualId);

                    unitOfWork.PersistirTransacao();
                }
            }
            else
            {
                await SalvarPlanoAula(planoAula, planoAulaDto, planoAnualId);
            }
        }

        private async Task SalvarPlanoAula(PlanoAula planoAula, PlanoAulaDto planoAulaDto, long planoAnualId)
        {
            repositorio.Salvar(planoAula);
            // Salvar Objetivos
            await repositorioObjetivosAula.LimparObjetivosAula(planoAula.Id);
            if (planoAulaDto.ObjetivosAprendizagemJurema != null)
                foreach (var objetivoJuremaId in planoAulaDto.ObjetivosAprendizagemJurema)
                {
                    var objetivoPlanoAnualId = await consultasObjetivoAprendizagem
                        .ObterIdPorObjetivoAprendizagemJurema(planoAnualId, objetivoJuremaId);

                    if (objetivoPlanoAnualId <= 0)
                    {
                        objetivoPlanoAnualId = await SalvarObjetivoPlanoAnual(objetivoJuremaId, planoAnualId);
                    }

                    repositorioObjetivosAula.Salvar(new ObjetivoAprendizagemAula(planoAula.Id, objetivoPlanoAnualId));
                }
        }

        private async Task<long> SalvarObjetivoPlanoAnual(long objetivoJuremaId, long planoAnualId)
        {
            var objAprendizagem = await consultasObjetivoAprendizagem.
                                                            ObterAprendizagemSimplificadaPorId(objetivoJuremaId);

            return repositorioObjetivoAprendizagemPlano.Salvar(new ObjetivoAprendizagemPlano()
            {
                ObjetivoAprendizagemJuremaId = objetivoJuremaId,
                ComponenteCurricularId = objAprendizagem.IdComponenteCurricular,
                PlanoId = planoAnualId
            });
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

        private void ValidarMigracao(MigrarPlanoAulaDto migrarPlanoAulaDto)
        {
            var turmasAtribuidasAoProfessor = consultasProfessor.Listar(migrarPlanoAulaDto.RFProfessor);
            var idsTurmasSelecionadas = migrarPlanoAulaDto.IdsPlanoTurmasDestino.Select(x => x.TurmaId).ToList();

            ValidaTurmasProfessor(migrarPlanoAulaDto.EhProfessorCJ,
                                  turmasAtribuidasAoProfessor,
                                  idsTurmasSelecionadas);

            ValidaTurmasAno(migrarPlanoAulaDto.EhProfessorCJ, migrarPlanoAulaDto.MigrarObjetivos,
                            turmasAtribuidasAoProfessor, idsTurmasSelecionadas);
        }

        private void ValidaTurmasProfessor(bool ehProfessorCJ,
                                           IEnumerable<ProfessorTurmaDto> turmasAtribuidasAoProfessor,
                                           IEnumerable<string> idsTurmasSelecionadas)
        {
            var idsTurmasProfessor = turmasAtribuidasAoProfessor?.Select(c => c.CodTurma).ToList();

            if (ehProfessorCJ)
            {
                //regras prof cj
            }
            else if (idsTurmasProfessor == null || idsTurmasSelecionadas.Any(c => !idsTurmasProfessor.Contains(Convert.ToInt32(c))))
            {
                throw new NegocioException("Somente é possível migrar o plano de aula para turmas atribuidas ao professor");
            }
        }

        private void ValidaTurmasAno(bool ehProfessorCJ, bool migrarObjetivos,
                                     IEnumerable<ProfessorTurmaDto> turmasAtribuidasAoProfessor,
                                     IEnumerable<string> idsTurmasSelecionadas)
        {
            if (!ehProfessorCJ || migrarObjetivos)
            {
                var turmasAtribuidasSelecionadas = turmasAtribuidasAoProfessor.Where(t => idsTurmasSelecionadas.Contains(t.CodTurma.ToString()));
                var anoTurma = turmasAtribuidasSelecionadas.First().Ano;

                if (!turmasAtribuidasSelecionadas.All(x => x.Ano == anoTurma))
                {
                    throw new NegocioException("Somente é possível migrar o plano de aula para turmas dentro do mesmo ano");
                }
            }
        }
    }
}
