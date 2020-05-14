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
    public class ComandosPlanoAula : IComandosPlanoAula
    {
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem;
        private readonly IConsultasPlanoAnual consultasPlanoAnual;
        private readonly IConsultasProfessor consultasProfessor;
        private readonly IRepositorioPlanoAula repositorio;
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioObjetivoAprendizagemPlano repositorioObjetivoAprendizagemPlano;
        private readonly IRepositorioObjetivoAprendizagemAula repositorioObjetivosAula;
        private readonly IServicoEOL servicoEol;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;

        public ComandosPlanoAula(IRepositorioPlanoAula repositorioPlanoAula,
                        IRepositorioObjetivoAprendizagemAula repositorioObjetivosAula,
                        IRepositorioObjetivoAprendizagemPlano repositorioObjetivoAprendizagemPlano,
                        IRepositorioAula repositorioAula,
                        IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
                        IConsultasAbrangencia consultasAbrangencia,
                        IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem,
                        IConsultasPlanoAnual consultasPlanoAnual,
                        IConsultasProfessor consultasProfessor,
                        IServicoUsuario servicoUsuario,
                        IUnitOfWork unitOfWork,
                        IServicoEOL servicoEol)
        {
            this.repositorio = repositorioPlanoAula;
            this.repositorioObjetivosAula = repositorioObjetivosAula;
            this.repositorioObjetivoAprendizagemPlano = repositorioObjetivoAprendizagemPlano;
            this.repositorioAula = repositorioAula;
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ;
            this.consultasAbrangencia = consultasAbrangencia;
            this.consultasProfessor = consultasProfessor;
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem;
            this.consultasPlanoAnual = consultasPlanoAnual;
            this.unitOfWork = unitOfWork;
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
            this.servicoUsuario = servicoUsuario;
        }

        public async Task ExcluirPlanoDaAula(long aulaId)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var aula = repositorioAula.ObterPorId(aulaId);

            await VerificaSeProfessorPodePersistirTurmaDisciplina(usuario.CodigoRf, aula.TurmaId, aula.DisciplinaId, aula.DataAula, usuario);

            await repositorio.ExcluirPlanoDaAula(aulaId);
        }

        public async Task Migrar(MigrarPlanoAulaDto migrarPlanoAulaDto)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var planoAulaDto = repositorio.ObterPorId(migrarPlanoAulaDto.PlanoAulaId);
            var aula = repositorioAula.ObterPorId(planoAulaDto.AulaId);

            await ValidarMigracao(migrarPlanoAulaDto, usuario.CodigoRf, usuario.EhProfessorCj(), aula.UeId);

            var objetivosPlanoAulaDto = await repositorioObjetivosAula.ObterObjetivosPlanoAula(migrarPlanoAulaDto.PlanoAulaId);

            unitOfWork.IniciarTransacao();

            foreach (var planoTurma in migrarPlanoAulaDto.IdsPlanoTurmasDestino)
            {
                AulaConsultaDto aulaConsultaDto = await
                     repositorioAula.ObterAulaDataTurmaDisciplina(
                         planoTurma.Data,
                         planoTurma.TurmaId,
                         migrarPlanoAulaDto.DisciplinaId
                     );

                if (aulaConsultaDto == null)
                    throw new NegocioException($"Não há aula cadastrada para a turma {planoTurma.TurmaId} para a data {planoTurma.Data.ToString("dd/MM/yyyy")} nesta disciplina!");

                var planoCopia = new PlanoAulaDto()
                {
                    Id = planoTurma.Sobreescrever ? migrarPlanoAulaDto.PlanoAulaId : 0,
                    AulaId = aulaConsultaDto.Id,
                    Descricao = planoAulaDto.Descricao,
                    DesenvolvimentoAula = planoAulaDto.DesenvolvimentoAula,
                    LicaoCasa = migrarPlanoAulaDto.MigrarLicaoCasa ? planoAulaDto.LicaoCasa : string.Empty,
                    ObjetivosAprendizagemJurema = !usuario.EhProfessorCj() ||
                                                   migrarPlanoAulaDto.MigrarObjetivos ?
                                                   objetivosPlanoAulaDto.Select(o => o.ObjetivoAprendizagemPlano.ObjetivoAprendizagemJuremaId).ToList() : null,
                    RecuperacaoAula = migrarPlanoAulaDto.MigrarRecuperacaoAula ?
                                        planoAulaDto.RecuperacaoAula : string.Empty
                };

                await Salvar(planoCopia, false);
            }

            unitOfWork.PersistirTransacao();
        }

        public async Task Salvar(PlanoAulaDto planoAulaDto, bool controlarTransacao = true)
        {
            var aula = repositorioAula.ObterPorId(planoAulaDto.AulaId);

            var abrangenciaTurma = await consultasAbrangencia.ObterAbrangenciaTurma(aula.TurmaId);
            if (abrangenciaTurma == null)
                throw new NegocioException("Usuario sem acesso a turma da respectiva aula");

            var usuario = await servicoUsuario.ObterUsuarioLogado();

            await VerificaSeProfessorPodePersistirTurmaDisciplina(usuario.CodigoRf, aula.TurmaId, aula.DisciplinaId, aula.DataAula, usuario);

            PlanoAula planoAula = await repositorio.ObterPlanoAulaPorAula(planoAulaDto.AulaId);
            planoAula = MapearParaDominio(planoAulaDto, planoAula);

            if (planoAulaDto.ObjetivosAprendizagemJurema == null || !planoAulaDto.ObjetivosAprendizagemJurema.Any() && !planoAula.Migrado)
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
                    permitePlanoSemObjetivos = usuario.EhProfessorCj();
                }

                // Caso a disciplina não possui vinculo com Jurema, os objetivos não devem ser exigidos
                if (!permitePlanoSemObjetivos)
                {
                    permitePlanoSemObjetivos = !(consultasObjetivoAprendizagem.DisciplinaPossuiObjetivosDeAprendizagem(Convert.ToInt64(aula.DisciplinaId)));
                }

                if (!permitePlanoSemObjetivos)
                    throw new NegocioException("A seleção de objetivos de aprendizagem é obrigatória para criação do plano de aula");
            }

            var bimestre = (aula.DataAula.Month + 2) / 3;
            var planoAnualId = await consultasPlanoAnual.ObterIdPlanoAnualPorAnoEscolaBimestreETurma(
                        aula.DataAula.Year, aula.UeId, long.Parse(aula.TurmaId), bimestre, long.Parse(aula.DisciplinaId));

            if (planoAnualId <= 0 && !usuario.PerfilAtual.Equals(Perfis.PERFIL_CJ))
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

        private async Task ValidarMigracao(MigrarPlanoAulaDto migrarPlanoAulaDto, string codigoRf, bool ehProfessorCj, string ueId)
        {
            var turmasAtribuidasAoProfessor = consultasProfessor.Listar(codigoRf);
            var idsTurmasSelecionadas = migrarPlanoAulaDto.IdsPlanoTurmasDestino.Select(x => x.TurmaId).ToList();

            await ValidaTurmasProfessor(ehProfessorCj, ueId,
                                  migrarPlanoAulaDto.DisciplinaId,
                                  codigoRf,
                                  turmasAtribuidasAoProfessor,
                                  idsTurmasSelecionadas);

            ValidaTurmasAno(ehProfessorCj, migrarPlanoAulaDto.MigrarObjetivos,
                            turmasAtribuidasAoProfessor, idsTurmasSelecionadas);
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

        private async Task ValidaTurmasProfessor(bool ehProfessorCJ, string ueId, string disciplinaId, string codigoRf,
                                                   IEnumerable<ProfessorTurmaDto> turmasAtribuidasAoProfessor,
                                           IEnumerable<string> idsTurmasSelecionadas)
        {
            var idsTurmasProfessor = turmasAtribuidasAoProfessor?.Select(c => c.CodTurma).ToList();

            IEnumerable<AtribuicaoCJ> lstTurmasCJ = await
                         repositorioAtribuicaoCJ.ObterPorFiltros(null, null, ueId, Convert.ToInt64(disciplinaId), codigoRf, null, null);

            if (
                    (
                        ehProfessorCJ &&
                        (
                            lstTurmasCJ == null ||
                            idsTurmasSelecionadas.Any(c => !lstTurmasCJ.Select(tcj => tcj.TurmaId).Contains(c))
                        )
                    ) ||
                    (
                        idsTurmasProfessor == null ||
                        idsTurmasSelecionadas.Any(c => !idsTurmasProfessor.Contains(Convert.ToInt32(c)))
                    )

               )
                throw new NegocioException("Somente é possível migrar o plano de aula para turmas atribuidas ao professor");
        }

        private async Task VerificaSeProfessorPodePersistirTurmaDisciplina(string codigoRf, string turmaId, string disciplinaId, DateTime dataAula, Usuario usuario = null)
        {
            if (usuario == null)
                usuario = await servicoUsuario.ObterUsuarioLogado();

            if (!usuario.EhProfessorCj() && !await servicoUsuario.PodePersistirTurmaDisciplina(codigoRf, turmaId, disciplinaId, dataAula))
                throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, disciplina e data.");
        }
    }
}