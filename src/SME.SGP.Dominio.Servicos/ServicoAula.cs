using Microsoft.Extensions.Configuration;
using SME.Background.Core;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoAula : IServicoAula
    {
        private readonly IComandosNotificacaoAula comandosNotificacaoAula;
        private readonly IComandosPlanoAula comandosPlanoAula;
        private readonly IComandosWorkflowAprovacao comandosWorkflowAprovacao;
        private readonly IConfiguration configuration;
        private readonly IConsultasFrequencia consultasFrequencia;
        private readonly IConsultasGrade consultasGrade;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasPlanoAula consultasPlanoAula;
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoDiaLetivo servicoDiaLetivo;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoFrequencia servicoFrequencia;
        private readonly IServicoLog servicoLog;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IServicoWorkflowAprovacao servicoWorkflowAprovacao;
        private readonly IUnitOfWork unitOfWork;

        public ServicoAula(IRepositorioAula repositorioAula,
                           IServicoEOL servicoEOL,
                           IRepositorioTipoCalendario repositorioTipoCalendario,
                           IServicoDiaLetivo servicoDiaLetivo,
                           IConsultasGrade consultasGrade,
                           IConsultasPeriodoEscolar consultasPeriodoEscolar,
                           IConsultasFrequencia consultasFrequencia,
                           IConsultasPlanoAula consultasPlanoAula,
                           IServicoLog servicoLog,
                           IServicoNotificacao servicoNotificacao,
                           IComandosWorkflowAprovacao comandosWorkflowAprovacao,
                           IComandosPlanoAula comandosPlanoAula,
                           IComandosNotificacaoAula comandosNotificacaoAula,
                           IServicoFrequencia servicoFrequencia,
                           IConfiguration configuration,
                           IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
                           IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
                           IRepositorioTurma repositorioTurma,
                           IServicoWorkflowAprovacao servicoWorkflowAprovacao,
                           IServicoUsuario servicoUsuario,
                           IUnitOfWork unitOfWork)
        {
            this.repositorioAula = repositorioAula ?? throw new System.ArgumentNullException(nameof(repositorioAula));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
            this.servicoDiaLetivo = servicoDiaLetivo ?? throw new System.ArgumentNullException(nameof(servicoDiaLetivo));
            this.consultasGrade = consultasGrade ?? throw new System.ArgumentNullException(nameof(consultasGrade));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasFrequencia = consultasFrequencia ?? throw new ArgumentNullException(nameof(consultasFrequencia));
            this.consultasPlanoAula = consultasPlanoAula ?? throw new ArgumentNullException(nameof(consultasPlanoAula));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
            this.comandosWorkflowAprovacao = comandosWorkflowAprovacao ?? throw new ArgumentNullException(nameof(comandosWorkflowAprovacao));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.comandosPlanoAula = comandosPlanoAula ?? throw new ArgumentNullException(nameof(comandosPlanoAula));
            this.servicoFrequencia = servicoFrequencia ?? throw new ArgumentNullException(nameof(servicoFrequencia));
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.servicoWorkflowAprovacao = servicoWorkflowAprovacao ?? throw new ArgumentNullException(nameof(servicoWorkflowAprovacao));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.comandosNotificacaoAula = comandosNotificacaoAula ?? throw new ArgumentNullException(nameof(comandosNotificacaoAula));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        private enum Operacao
        {
            Inclusao,
            Alteracao,
            Exclusao
        }

        public async Task<string> Excluir(Aula aula, RecorrenciaAula recorrencia, Usuario usuario)
        {
            await ExcluirAula(aula, usuario);

            if (recorrencia == RecorrenciaAula.AulaUnica)
                return "Aula e suas dependencias excluídas com sucesso!";

            Cliente.Executar<IServicoAula>(s => s.ExcluirRecorrencia(aula, recorrencia, usuario));
            return "Aula excluida com sucesso. Serão excluidas aulas recorrentes, em breve você receberá uma notificação com o resultado do processamento.";
        }

        public async Task GravarRecorrencia(bool inclusao, Aula aula, Usuario usuario, RecorrenciaAula recorrencia)
        {
            var fimRecorrencia = consultasPeriodoEscolar.ObterFimPeriodoRecorrencia(aula.TipoCalendarioId, aula.DataAula.Date, recorrencia);

            if (inclusao)
                await GerarRecorrencia(aula, usuario, fimRecorrencia);
            else
                await AlterarRecorrencia(aula, usuario, fimRecorrencia);
        }

        public async Task<string> Salvar(Aula aula, Usuario usuario, RecorrenciaAula recorrencia, int quantidadeOriginal = 0, bool ehRecorrencia = false)
        {
            if (!ehRecorrencia)
            {
                var aulaExistente = await repositorioAula.ObterAulaDataTurmaDisciplinaProfessorRf(aula.DataAula, aula.TurmaId, aula.DisciplinaId, aula.ProfessorRf);
                if (aulaExistente != null && !aulaExistente.Id.Equals(aula.Id))
                    throw new NegocioException("Já existe uma aula criada neste dia para este componente curricular.");

                var tipoCalendario = repositorioTipoCalendario.ObterPorId(aula.TipoCalendarioId);

                if (tipoCalendario == null)
                    throw new NegocioException("O tipo de calendário não foi encontrado.");

                aula.AtualizaTipoCalendario(tipoCalendario);

                await VerificaSeProfessorPodePersistirTurmaDisciplina(usuario.CodigoRf, aula.TurmaId, aula.DisciplinaId, aula.DataAula, usuario);

                var disciplinasProfessor = usuario.EhProfessorCj() ? ObterDisciplinasProfessorCJ(aula, usuario) : await ObterDisciplinasProfessor(aula, usuario);

                if (disciplinasProfessor == null || !disciplinasProfessor.Any(c => c.ToString() == aula.DisciplinaId))
                    throw new NegocioException("Você não pode criar aulas para essa UE/Turma/Disciplina.");

                var turma = repositorioTurma.ObterTurmaComUeEDrePorId(aula.TurmaId);

                if (turma == null)
                    throw new NegocioException("Turma não localizada.");

                aula.AtualizaTurma(turma);
            }

            if (aula.Id > 0)
                aula.PodeSerAlterada(usuario);

            var temLiberacaoExcepcionalNessaData = servicoDiaLetivo.ValidaSeEhLiberacaoExcepcional(aula.DataAula, aula.TipoCalendarioId, aula.UeId);

            if (!temLiberacaoExcepcionalNessaData && !servicoDiaLetivo.ValidarSeEhDiaLetivo(aula.DataAula, aula.TipoCalendarioId, null, aula.UeId))
                throw new NegocioException("Não é possível cadastrar essa aula pois a data informada está fora do período letivo.");

            if (aula.RecorrenciaAula != RecorrenciaAula.AulaUnica && aula.TipoAula == TipoAula.Reposicao)
                throw new NegocioException("Uma aula do tipo Reposição não pode ser recorrente.");

            var ehInclusao = aula.Id == 0;

            if (aula.RecorrenciaAula == RecorrenciaAula.AulaUnica && aula.TipoAula == TipoAula.Reposicao)
            {
                var aulas = repositorioAula.ObterAulas(aula.TipoCalendarioId, aula.TurmaId, aula.UeId, usuario.CodigoRf).Result;
                var quantidadeDeAulasSomadas = aulas.ToList().FindAll(x => x.DataAula.Date == aula.DataAula.Date).Sum(x => x.Quantidade) + aula.Quantidade;

                if (ReposicaoDeAulaPrecisaDeAprovacao(quantidadeDeAulasSomadas, aula.Turma))
                {
                    var nomeDisciplina = aula.DisciplinaNome;

                    repositorioAula.Salvar(aula);
                    PersistirWorkflowReposicaoAula(aula, aula.Turma.Ue.Dre.Nome, aula.Turma.Ue.Nome, nomeDisciplina,
                                                 aula.Turma.Nome, aula.Turma.Ue.Dre.CodigoDre);
                    return "Aula cadastrada com sucesso e enviada para aprovação.";
                }
            }
            else
            {
                if (usuario.EhProfessorCj() && aula.Quantidade > 2)
                    throw new NegocioException("Quantidade de aulas por dia/disciplina excedido.");

                // Busca quantidade de aulas semanais da grade de aula
                int semana = UtilData.ObterSemanaDoAno(aula.DataAula);

                var gradeAulas = await consultasGrade.ObterGradeAulasTurmaProfessor(aula.TurmaId, Convert.ToInt64(aula.DisciplinaId), semana, aula.DataAula, usuario.CodigoRf);

                var quantidadeAulasRestantes = gradeAulas == null ? int.MaxValue : gradeAulas.QuantidadeAulasRestante;

                ObterDisciplinaDaAula(aula);

                if (!ehInclusao)
                {
                    if (aula.ComponenteCurricularEol.Regencia)
                    {
                        if (aula.Turma.ModalidadeCodigo == Modalidade.EJA)
                        {
                            var aulasNoDia = await repositorioAula.ObterAulas(aula.TurmaId, aula.UeId, usuario.CodigoRf, data: aula.DataAula, aula.DisciplinaId);
                            if (aula.Quantidade != 5)
                                throw new NegocioException("Para regência de EJA só é permitido a criação de 5 aulas por dia.");
                        }
                        else if (aula.Quantidade != 1)
                            throw new NegocioException("Para regência de classe só é permitido a criação de 1 (uma) aula por dia.");
                    }
                    else
                    {
                        // Na alteração tem que considerar que uma aula possa estar mudando de dia na mesma semana, então não soma as aulas do proprio registro
                        var aulasSemana = await repositorioAula.ObterAulas(aula.TipoCalendarioId, aula.TurmaId, aula.UeId, usuario.CodigoRf, mes: null, semanaAno: semana, disciplinaId: aula.DisciplinaId);
                        var quantidadeAulasSemana = aulasSemana.Where(a => a.Id != aula.Id).Sum(a => a.Quantidade);

                        quantidadeAulasRestantes = gradeAulas == null ? int.MaxValue : gradeAulas.QuantidadeAulasGrade - quantidadeAulasSemana;
                        if ((gradeAulas != null) && (quantidadeAulasRestantes < aula.Quantidade))
                            throw new NegocioException("Quantidade de aulas superior ao limíte de aulas da grade.");
                    }
                }
                else
                {
                    if (aula.ComponenteCurricularEol.Regencia)
                    {
                        var aulasNoDia = await repositorioAula.ObterAulas(aula.TurmaId, aula.UeId, usuario.CodigoRf, data: aula.DataAula, aula.DisciplinaId);
                        if (aulasNoDia != null && aulasNoDia.Any())
                        {
                            if (aula.Turma.ModalidadeCodigo == Modalidade.EJA)
                                throw new NegocioException("Para regência de EJA só é permitido a criação de 5 aulas por dia.");
                            else throw new NegocioException("Para regência de classe só é permitido a criação de 1 (uma) aula por dia.");
                        }
                    }
                    if ((gradeAulas != null) && (quantidadeAulasRestantes < aula.Quantidade))
                        throw new NegocioException("Quantidade de aulas superior ao limíte de aulas da grade.");
                }
            }

            repositorioAula.Salvar(aula);

            // Na alteração de quantidade de aulas deve 0r a frequencia se registrada
            if (!ehInclusao && quantidadeOriginal != 0 && quantidadeOriginal != aula.Quantidade)
                if (consultasFrequencia.FrequenciaAulaRegistrada(aula.Id).Result)
                    await servicoFrequencia.AtualizarQuantidadeFrequencia(aula.Id, quantidadeOriginal, aula.Quantidade);

            // Verifica recorrencia da gravação
            if (recorrencia != RecorrenciaAula.AulaUnica)
            {
                Cliente.Executar<IServicoAula>(s => s.GravarRecorrencia(ehInclusao, aula, usuario, recorrencia));

                var mensagem = ehInclusao ? "cadastrada" : "alterada";
                return $"Aula {mensagem} com sucesso. Serão {mensagem}s aulas recorrentes, em breve você receberá uma notificação com o resultado do processamento.";
            }

            return "Aula cadastrada com sucesso.";
        }

        private static bool ReposicaoDeAulaPrecisaDeAprovacao(int quantidadeAulasExistentesNoDia, Turma turma)
        {
            int.TryParse(turma.Ano, out int anoTurma);
            return ((turma.ModalidadeCodigo == Modalidade.Fundamental && anoTurma >= 1 && anoTurma <= 5) ||  //Valida se é Fund 1
                               (Modalidade.EJA == turma.ModalidadeCodigo && (anoTurma == 1 || anoTurma == 2)) // Valida se é Eja Alfabetizacao ou  Basica
                               && quantidadeAulasExistentesNoDia > 1) || ((turma.ModalidadeCodigo == Modalidade.Fundamental && anoTurma >= 6 && anoTurma <= 9) || //valida se é fund 2
                                     (Modalidade.EJA == turma.ModalidadeCodigo && anoTurma == 3 || anoTurma == 4) ||  // Valida se é Eja Complementar ou Final
                                     (turma.ModalidadeCodigo == Modalidade.Medio) && quantidadeAulasExistentesNoDia > 2);
        }

        private async Task AlterarRecorrencia(Aula aula, Usuario usuario, DateTime fimRecorrencia)
        {
            var dataRecorrencia = aula.DataAula.AddDays(7);
            var aulasRecorrencia = await repositorioAula.ObterAulasRecorrencia(aula.AulaPaiId ?? aula.Id, aula.Id, fimRecorrencia);
            List<(DateTime data, string erro)> aulasQueDeramErro = new List<(DateTime, string)>();
            List<(DateTime data, bool existeFrequencia, bool existePlanoAula)> aulasComFrenciaOuPlano = new List<(DateTime data, bool existeFrequencia, bool existePlanoAula)>();

            List<DateTime> diasParaAlterarRecorrencia = new List<DateTime>();
            ObterDiasDaRecorrencia(dataRecorrencia, fimRecorrencia, diasParaAlterarRecorrencia);
            var datasComRegistro = await repositorioAula.ObterDatasAulasExistentes(diasParaAlterarRecorrencia, aula.TurmaId, aula.DisciplinaId, usuario.CodigoRf);
            if (datasComRegistro.Count() > 0)
                aulasQueDeramErro.AddRange(
                        datasComRegistro.Select(d =>
                            (d, $"Já existe uma aula criada neste dia para este componente curricular.")
                        ));

            foreach (var aulaRecorrente in aulasRecorrencia)
            {
                var ehDataComRegistro = datasComRegistro.Select(d => d.Equals(dataRecorrencia)).FirstOrDefault();
                if (!ehDataComRegistro)
                {
                    var existeFrequencia = await consultasFrequencia.FrequenciaAulaRegistrada(aulaRecorrente.Id);
                    var existePlanoAula = await consultasPlanoAula.PlanoAulaRegistrado(aulaRecorrente.Id);

                    if (existeFrequencia || existePlanoAula)
                        aulasComFrenciaOuPlano.Add((aulaRecorrente.DataAula, existeFrequencia, existePlanoAula));

                    var quantidadeOriginal = aulaRecorrente.Quantidade;

                    aulaRecorrente.DataAula = dataRecorrencia;
                    aulaRecorrente.Quantidade = aula.Quantidade;

                    try
                    {
                        await Salvar(aulaRecorrente, usuario, aulaRecorrente.RecorrenciaAula, quantidadeOriginal, true);
                    }
                    catch (NegocioException nex)
                    {
                        aulasQueDeramErro.Add((dataRecorrencia, nex.Message));
                    }
                    catch (Exception ex)
                    {
                        servicoLog.Registrar(ex);
                        aulasQueDeramErro.Add((dataRecorrencia, $"Erro Interno: {ex.Message}"));
                    }
                }

                dataRecorrencia = dataRecorrencia.AddDays(7);
            }

            await NotificarUsuario(usuario, aula, Operacao.Alteracao, aulasRecorrencia.Count() - aulasQueDeramErro.Count, aulasQueDeramErro, aulasComFrenciaOuPlano);
        }

        private async Task ExcluirAula(Aula aula, Usuario usuario)
        {
            if (await repositorioAtividadeAvaliativa.VerificarSeExisteAvaliacao(aula.DataAula.Date, aula.UeId, aula.TurmaId, usuario.CodigoRf, aula.DisciplinaId))
                throw new NegocioException("Aula com avaliação vinculada. Para excluir esta aula primeiro deverá ser excluída a avaliação.");

            await VerificaSeProfessorPodePersistirTurmaDisciplina(usuario.CodigoRf, aula.TurmaId, aula.DisciplinaId, aula.DataAula, usuario);

            unitOfWork.IniciarTransacao();
            try
            {
                if (aula.WorkflowAprovacaoId.HasValue)
                    await servicoWorkflowAprovacao.ExcluirWorkflowNotificacoes(aula.WorkflowAprovacaoId.Value);

                await comandosNotificacaoAula.Excluir(aula.Id);
                await servicoFrequencia.ExcluirFrequenciaAula(aula.Id);
                await comandosPlanoAula.ExcluirPlanoDaAula(aula.Id);

                aula.Excluido = true;
                await repositorioAula.SalvarAsync(aula);

                unitOfWork.PersistirTransacao();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public async Task ExcluirRecorrencia(Aula aula, RecorrenciaAula recorrencia, Usuario usuario)
        {
            var fimRecorrencia = consultasPeriodoEscolar.ObterFimPeriodoRecorrencia(aula.TipoCalendarioId, aula.DataAula.Date, recorrencia);
            var aulasRecorrencia = await repositorioAula.ObterAulasRecorrencia(aula.AulaPaiId ?? aula.Id, aula.Id, fimRecorrencia);
            List<(DateTime data, string erro)> aulasQueDeramErro = new List<(DateTime, string)>();
            List<(DateTime data, bool existeFrequencia, bool existePlanoAula)> aulasComFrenciaOuPlano = new List<(DateTime data, bool existeFrequencia, bool existePlanoAula)>();

            foreach (var aulaRecorrente in aulasRecorrencia)
            {
                try
                {
                    var existeFrequencia = await consultasFrequencia.FrequenciaAulaRegistrada(aulaRecorrente.Id);
                    var existePlanoAula = await consultasPlanoAula.PlanoAulaRegistrado(aulaRecorrente.Id);

                    if (existeFrequencia || existePlanoAula)
                        aulasComFrenciaOuPlano.Add((aulaRecorrente.DataAula, existeFrequencia, existePlanoAula));

                    await ExcluirAula(aulaRecorrente, usuario);
                }
                catch (NegocioException nex)
                {
                    aulasQueDeramErro.Add((aulaRecorrente.DataAula, nex.Message));
                }
                catch (Exception ex)
                {
                    servicoLog.Registrar(ex);
                    aulasQueDeramErro.Add((aulaRecorrente.DataAula, $"Erro Interno: {ex.Message}"));
                }
            }

            await NotificarUsuario(usuario, aula, Operacao.Exclusao, aulasRecorrencia.Count() - aulasQueDeramErro.Count, aulasQueDeramErro, aulasComFrenciaOuPlano);
        }

        private async Task GerarAulaDeRecorrenciaParaDias(Aula aula, Usuario usuario, IEnumerable<PodePersistirNaDataRetornoEolDto> datasParaPersistencia, IEnumerable<DateTime> datasComRegistro)
        {
            List<(DateTime data, string erro)> aulasQueDeramErro = new List<(DateTime, string)>();
            List<DateTime> datasParaGeracao = datasParaPersistencia.Select(a => a.Data).ToList();

            if (datasComRegistro.Count() > 0)
                aulasQueDeramErro.AddRange(
                        datasComRegistro.Select(d =>
                            (d, $"Já existe uma aula criada neste dia para este componente curricular.")
                        ));

            foreach (var dia in datasParaPersistencia)
            {
                if (dia.PodePersistir)
                {
                    var aulaParaAdicionar = (Aula)aula.Clone();
                    aulaParaAdicionar.DataAula = dia.Data;
                    aulaParaAdicionar.AdicionarAulaPai(aula);
                    aulaParaAdicionar.RecorrenciaAula = RecorrenciaAula.AulaUnica;

                    try
                    {
                        await Salvar(aulaParaAdicionar, usuario, aulaParaAdicionar.RecorrenciaAula, 0, true);
                    }
                    catch (NegocioException nex)
                    {
                        aulasQueDeramErro.Add((dia.Data, nex.Message));
                    }
                    catch (Exception ex)
                    {
                        servicoLog.Registrar(ex);
                        aulasQueDeramErro.Add((dia.Data, "Erro Interno."));
                    }
                }
                else
                {
                    aulasQueDeramErro.Add((dia.Data, "Este professor não pode persistir nesta turma neste dia."));
                }
            }

            await NotificarUsuario(usuario, aula, Operacao.Inclusao, datasParaPersistencia.ToList().Count - aulasQueDeramErro.ToList().Count, aulasQueDeramErro);
        }

        private async Task GerarRecorrencia(Aula aula, Usuario usuario, DateTime fimRecorrencia)
        {
            var inicioRecorrencia = aula.DataAula.AddDays(7);

            await GerarRecorrenciaParaPeriodos(aula, inicioRecorrencia, fimRecorrencia, usuario);
        }

        private async Task GerarRecorrenciaParaPeriodos(Aula aula, DateTime inicioRecorrencia, DateTime fimRecorrencia, Usuario usuario)
        {
            List<DateTime> diasParaIncluirRecorrencia = new List<DateTime>();
            ObterDiasDaRecorrencia(inicioRecorrencia, fimRecorrencia, diasParaIncluirRecorrencia);

            var datasComRegistro = await repositorioAula.ObterDatasAulasExistentes(diasParaIncluirRecorrencia, aula.TurmaId, aula.DisciplinaId, usuario.CodigoRf);
            if (datasComRegistro.Count() > 0)
                diasParaIncluirRecorrencia.RemoveAll(d => datasComRegistro.Contains(d));

            List<PodePersistirNaDataRetornoEolDto> datasPersistencia = new List<PodePersistirNaDataRetornoEolDto>();

            if (diasParaIncluirRecorrencia.Any())
            {
                if (!usuario.EhProfessorCj())
                {
                    var datasAtribuicao = await servicoEOL.PodePersistirTurmaNasDatas(usuario.CodigoRf, aula.TurmaId, diasParaIncluirRecorrencia.Select(a => a.Date.ToString("s")).ToArray(), aula.ComponenteCurricularEol.Codigo);
                    if (datasAtribuicao == null || !datasAtribuicao.Any())
                        throw new NegocioException("Não foi possível validar datas para a atribuição do professor no EOL.");
                    else
                        datasPersistencia = datasAtribuicao.ToList();
                }
                else
                {
                    datasPersistencia.AddRange(
                        diasParaIncluirRecorrencia.Select(d =>
                        new PodePersistirNaDataRetornoEolDto()
                        {
                            Data = d,
                            PodePersistir = true
                        }));
                }
            }

            await GerarAulaDeRecorrenciaParaDias(aula, usuario, datasPersistencia, datasComRegistro);
        }

        private async Task NotificarUsuario(Usuario usuario, Aula aula, Operacao operacao, int quantidade, List<(DateTime data, string erro)> aulasQueDeramErro, List<(DateTime data, bool existeFrequencia, bool existePlanoAula)> aulasComFrenciaOuPlano = null)
        {
            var perfilAtual = usuario.PerfilAtual;
            if (perfilAtual == Guid.Empty)
                throw new NegocioException($"Não foi encontrado o perfil do usuário informado.");

            var operacaoStr = operacao == Operacao.Inclusao ? "Criação" : operacao == Operacao.Alteracao ? "Alteração" : "Exclusão";
            var tituloMensagem = $"{operacaoStr} de Aulas de {aula.DisciplinaNome} na turma {aula.Turma.Nome}";
            StringBuilder mensagemUsuario = new StringBuilder();

            operacaoStr = operacao == Operacao.Inclusao ? "criadas" : operacao == Operacao.Alteracao ? "alteradas" : "excluídas";
            mensagemUsuario.Append($"Foram {operacaoStr} {quantidade} aulas da disciplina {aula.DisciplinaNome} para a turma {aula.Turma.Nome} da {aula.Turma.Ue?.Nome} ({aula.Turma.Ue?.Dre?.Nome}).");

            if (aulasComFrenciaOuPlano != null && aulasComFrenciaOuPlano.Any())
            {
                mensagemUsuario.Append($"<br><br>Nas seguintes datas haviam registros de plano de aula e frequência:<br>");

                foreach (var aulaFrequenciaOuPlano in aulasComFrenciaOuPlano)
                {
                    var frequenciaPlano = aulaFrequenciaOuPlano.existeFrequencia ?
                                            $"Frequência{(aulaFrequenciaOuPlano.existePlanoAula ? " e Plano de Aula" : "")}"
                                            : "Plano de Aula";
                    mensagemUsuario.Append($"<br /> {aulaFrequenciaOuPlano.data.ToString("dd/MM/yyyy")} - {frequenciaPlano}");
                }
            }

            if (aulasQueDeramErro.Any())
            {
                operacaoStr = operacao == Operacao.Inclusao ? "criar" : operacao == Operacao.Alteracao ? "alterar" : "excluir";
                mensagemUsuario.Append($"<br><br>Não foi possível {operacaoStr} aulas nas seguintes datas:<br>");
                foreach (var aulaComErro in aulasQueDeramErro)
                {
                    mensagemUsuario.AppendFormat("<br /> {0} - {1}", $"{aulaComErro.data.Day}/{aulaComErro.data.Month}/{aulaComErro.data.Year}", aulaComErro.erro);
                }
            }

            var notificacao = new Notificacao()
            {
                Ano = aula.CriadoEm.Year,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = aula.Turma.Ue.Dre.CodigoDre,
                Mensagem = mensagemUsuario.ToString(),
                UsuarioId = usuario.Id,
                Tipo = NotificacaoTipo.Calendario,
                Titulo = tituloMensagem,
                TurmaId = aula.TurmaId,
                UeId = aula.Turma.Ue.CodigoUe,
            };

            unitOfWork.IniciarTransacao();
            try
            {
                // Salva Notificação
                servicoNotificacao.Salvar(notificacao);

                // Gera vinculo Notificacao x Aula
                await comandosNotificacaoAula.Inserir(notificacao.Id, aula.Id);

                unitOfWork.PersistirTransacao();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private IEnumerable<DateTime> ObterDiaEntreDatas(DateTime inicio, DateTime fim)
        {
            for (DateTime i = inicio; i <= fim; i = i.AddDays(7))
            {
                yield return i;
            }
        }

        private void ObterDiasDaRecorrencia(DateTime inicioRecorrencia, DateTime fimRecorrencia, List<DateTime> diasParaIncluirRecorrencia)
        {
            diasParaIncluirRecorrencia.AddRange(ObterDiaEntreDatas(inicioRecorrencia, fimRecorrencia));
            if (inicioRecorrencia.Date == fimRecorrencia.Date)
                diasParaIncluirRecorrencia.Add(inicioRecorrencia);
        }

        private void ObterDisciplinaDaAula(Aula aula)
        {
            if (aula.ComponenteCurricularEol == null || aula.ComponenteCurricularEol.Codigo == 0)
            {
                var disciplinas = servicoEOL.ObterDisciplinasPorIds(new[] { Convert.ToInt64(aula.DisciplinaId) });

                if (disciplinas == null || !disciplinas.Any())
                    throw new NegocioException("Disciplina não encontrada.");

                var disciplina = disciplinas.First();
                var componenteCurricularEol = TransformaComponenteCurricularDtoEmEntidade(disciplina);

                aula.AtualizaComponenteCurricularEol(componenteCurricularEol);
            }
        }

        private async Task<IEnumerable<long>> ObterDisciplinasProfessor(Aula aula, Usuario usuario)
        {
            IEnumerable<DisciplinaResposta> lstDisciplinasProf = await servicoEOL.ObterDisciplinasPorCodigoTurmaLoginEPerfil(aula.TurmaId, usuario.Login, usuario.PerfilAtual);

            return lstDisciplinasProf != null && lstDisciplinasProf.Any() ? lstDisciplinasProf.Select(d => Convert.ToInt64(d.CodigoComponenteCurricular)) : null;
        }

        private IEnumerable<long> ObterDisciplinasProfessorCJ(Aula aula, Usuario usuario)
        {
            IEnumerable<AtribuicaoCJ> lstDisciplinasProfCJ = repositorioAtribuicaoCJ.ObterPorFiltros(null, aula.TurmaId, aula.UeId, 0, usuario.CodigoRf, usuario.Nome, null).Result;

            return lstDisciplinasProfCJ != null && lstDisciplinasProfCJ.Any() ? lstDisciplinasProfCJ.Select(d => d.DisciplinaId) : null;
        }

        private void PersistirWorkflowReposicaoAula(Aula aula, string nomeDre, string nomeEscola, string nomeDisciplina,
                                                          string nomeTurma, string dreId)
        {
            var linkParaReposicaoAula = $"{configuration["UrlFrontEnd"]}calendario-escolar/calendario-professor/cadastro-aula/editar/:{aula.Id}/";

            var wfAprovacaoAula = new WorkflowAprovacaoDto()
            {
                Ano = aula.DataAula.Year,
                NotificacaoCategoria = NotificacaoCategoria.Workflow_Aprovacao,
                EntidadeParaAprovarId = aula.Id,
                Tipo = WorkflowAprovacaoTipo.ReposicaoAula,
                UeId = aula.UeId,
                DreId = dreId,
                NotificacaoTitulo = $"Criação de Aula de Reposição na turma {nomeTurma}",
                NotificacaoTipo = NotificacaoTipo.Calendario,
                NotificacaoMensagem = $"Foram criadas {aula.Quantidade} aula(s) de reposição de {nomeDisciplina} na turma {nomeTurma} da {nomeEscola} ({nomeDre}). Para que esta aula seja considerada válida você precisa aceitar esta notificação. Para visualizar a aula clique  <a href='{linkParaReposicaoAula}'>aqui</a>."
            };

            wfAprovacaoAula.Niveis.Add(new WorkflowAprovacaoNivelDto()
            {
                Cargo = Cargo.CP,
                Nivel = 1
            });
            wfAprovacaoAula.Niveis.Add(new WorkflowAprovacaoNivelDto()
            {
                Cargo = Cargo.Diretor,
                Nivel = 2
            });

            var idWorkflow = comandosWorkflowAprovacao.Salvar(wfAprovacaoAula);

            aula.EnviarParaWorkflowDeAprovacao(idWorkflow);

            repositorioAula.Salvar(aula);
        }

        private string RetornaNomeDaDisciplina(Aula aula)
        {
            var disciplinasEol = servicoEOL.ObterDisciplinasPorIds(new long[] { long.Parse(aula.DisciplinaId) });

            if (disciplinasEol is null && !disciplinasEol.Any())
                throw new NegocioException($"Não foi possível localizar as disciplinas da turma {aula.TurmaId}");

            var disciplina = disciplinasEol.FirstOrDefault();

            if (disciplina == null)
                throw new NegocioException($"Não foi possível localizar a disciplina de Id {aula.DisciplinaId}.");

            return disciplina.Nome;
        }

        private ComponenteCurricularEol TransformaComponenteCurricularDtoEmEntidade(DisciplinaDto disciplina)
        {
            return new ComponenteCurricularEol()
            {
                CdComponenteCurricularPai = disciplina.CdComponenteCurricularPai,
                Codigo = disciplina.CodigoComponenteCurricular,
                Compartilhada = disciplina.Compartilhada,
                LancaNota = disciplina.LancaNota,
                Nome = disciplina.Nome,
                PossuiObjetivos = disciplina.PossuiObjetivos,
                Regencia = disciplina.Regencia,
                RegistraFrequencia = disciplina.RegistraFrequencia,
                TerritorioSaber = disciplina.TerritorioSaber
            };
        }

        private async Task VerificaSeProfessorPodePersistirTurmaDisciplina(string codigoRf, string turmaId, string disciplinaId, DateTime dataAula, Usuario usuario = null)
        {
            if (!await servicoUsuario.PodePersistirTurmaDisciplina(codigoRf, turmaId, disciplinaId, dataAula, usuario))
                throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, disciplina e data.");
        }
    }
}