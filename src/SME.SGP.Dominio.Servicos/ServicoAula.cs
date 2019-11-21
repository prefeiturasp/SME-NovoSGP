using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoAula : IServicoAula
    {

        private readonly IConsultasGrade consultasGrade;
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IServicoDiaLetivo servicoDiaLetivo;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoLog servicoLog;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IComandosWorkflowAprovacao comandosWorkflowAprovacao;


        public ServicoAula(IRepositorioAula repositorioAula,
                           IServicoEOL servicoEOL,
                           IRepositorioTipoCalendario repositorioTipoCalendario,
                           IServicoDiaLetivo servicoDiaLetivo,
                           IConsultasGrade consultasGrade,
                           IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                           IServicoLog servicoLog,
                           IConsultasAbrangencia consultasAbrangencia,
                           IServicoUsuario servicoUsuario,
                           IComandosWorkflowAprovacao comandosWorkflowAprovacao)
        {
            this.repositorioAula = repositorioAula ?? throw new System.ArgumentNullException(nameof(repositorioAula));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
            this.servicoDiaLetivo = servicoDiaLetivo ?? throw new System.ArgumentNullException(nameof(servicoDiaLetivo));
            this.consultasGrade = consultasGrade ?? throw new System.ArgumentNullException(nameof(consultasGrade));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.comandosWorkflowAprovacao = comandosWorkflowAprovacao ?? throw new ArgumentNullException(nameof(comandosWorkflowAprovacao));
        }

        public async Task<string> Salvar(Aula aula, Usuario usuario)
        {
            var tipoCalendario = repositorioTipoCalendario.ObterPorId(aula.TipoCalendarioId);
            if (tipoCalendario == null)
                throw new NegocioException("O tipo de calendário não foi encontrado.");

            var disciplinasProfessor = await servicoEOL.ObterDisciplinasPorCodigoTurmaLoginEPerfil(aula.TurmaId, usuario.Login, usuario.ObterPerfilPrioritario());

            var usuarioPodeCriarAulaNaTurmaUeEModalidade = repositorioAula.UsuarioPodeCriarAulaNaUeTurmaEModalidade(aula, tipoCalendario.Modalidade);

            if (!disciplinasProfessor.Any(c => c.CodigoComponenteCurricular.ToString() == aula.DisciplinaId) || !usuarioPodeCriarAulaNaTurmaUeEModalidade)
            {
                throw new NegocioException("Você não pode criar aulas para essa UE/Turma/Disciplina.");
            }

            if (!servicoDiaLetivo.ValidarSeEhDiaLetivo(aula.DataAula, aula.TipoCalendarioId, null, aula.UeId))
            {
                throw new NegocioException("Não é possível cadastrar essa aula pois a data informada está fora do período letivo.");
            }

            if (aula.RecorrenciaAula != RecorrenciaAula.AulaUnica && aula.TipoAula == TipoAula.Reposicao)
            {
                throw new NegocioException("Uma aula do tipo Reposição não pode ser recorrente.");
            }

            if (aula.RecorrenciaAula == RecorrenciaAula.AulaUnica && aula.TipoAula == TipoAula.Reposicao)
            {
                var aulas = await repositorioAula.ObterAulasTurmaDisciplina(aula.TurmaId, aula.DisciplinaId);
                var quantidadeAulasExistentesNoDia = aulas.ToList().FindAll(x => x.DataAula.Date == aula.DataAula.Date).Sum(x => x.Quantidade) + aula.Quantidade;

                var abrangencia = await consultasAbrangencia.ObterAbrangenciaTurma(aula.TurmaId);
                if (abrangencia == null)
                    throw new NegocioException("Abrangência da turma não localizada.");

                if (((abrangencia.Modalidade == Modalidade.Fundamental && abrangencia.Ano >= 1 && abrangencia.Ano <= 5) ||  //Valida se é Fund 1 
                   (Modalidade.EJA == abrangencia.Modalidade && (abrangencia.Ano == 1 || abrangencia.Ano == 2)) // Valida se é Eja Alfabetizacao ou  Basica
                   && quantidadeAulasExistentesNoDia > 1) || ((abrangencia.Modalidade == Modalidade.Fundamental && abrangencia.Ano >= 6 && abrangencia.Ano <= 9) || //valida se é fund 2
                         (Modalidade.EJA == abrangencia.Modalidade && abrangencia.Ano == 3 || abrangencia.Ano == 4) ||  // Valida se é Eja Complementar ou Final 
                         (abrangencia.Modalidade == Modalidade.Medio) && quantidadeAulasExistentesNoDia > 2))
                {
                    repositorioAula.Salvar(aula);
                    PersistirWorkflowReposicaoAula(aula, string.Empty);
                }

            }

            else
            {
                var gradeAulas = await consultasGrade.ObterGradeAulasTurma(aula.TurmaId, int.Parse(aula.DisciplinaId));
                if ((gradeAulas != null) && (gradeAulas.QuantidadeAulasRestante < aula.Quantidade))
                    throw new NegocioException("Quantidade de aulas superior ao limíte de aulas da grade.");
            }

            repositorioAula.Salvar(aula);

            if (aula.RecorrenciaAula != RecorrenciaAula.AulaUnica)
            {
                //ASSINCRONO
                GerarRecorrencia(aula, usuario);
                return "Aula cadastrada com sucesso. Serão cadastradas aulas recorrentes, em breve você receberá uma notificação com o resultado do processamento.";
            }
            return "Aula cadastrada com sucesso.";
        }

        private async Task GerarAulaDeRecorrenciaParaDias(Aula aula, List<DateTime> diasParaIncluirRecorrencia, Usuario usuario)
        {
            List<(DateTime, string)> aulasQueDeramErro = new List<(DateTime, string)>();

            foreach (var dia in diasParaIncluirRecorrencia)
            {
                var aulaParaAdicionar = (Aula)aula.Clone();
                aulaParaAdicionar.DataAula = dia;
                aulaParaAdicionar.AdicionarAulaPai(aula);
                aulaParaAdicionar.RecorrenciaAula = RecorrenciaAula.AulaUnica;

                try
                {
                    await Salvar(aulaParaAdicionar, usuario);
                }
                catch (NegocioException nex)
                {
                    aulasQueDeramErro.Add((dia, nex.Message));
                }
                catch (Exception ex)
                {
                    servicoLog.Registrar(ex);
                    aulasQueDeramErro.Add((dia, "Erro Interno."));
                }
            }
            var perfilAtual = servicoUsuario.ObterPerfilAtual();
            //var nomeTurma = repositorioAbrangencia.ObterAbrangenciaTurma(aula.TurmaId, usuario.Login, perfilAtual);
            //var tituloMensagem = $"Criação de Aulas de {} na turma {aula.tur}";
            //Enviar Mensagem
        }

        private void GerarRecorrencia(Aula aula, Usuario usuario)
        {
            var periodos = repositorioPeriodoEscolar.ObterPorTipoCalendario(aula.TipoCalendarioId);
            if (periodos == null || !periodos.Any())
                throw new NegocioException("Não foi possível obter os períodos deste tipo de calendário.");

            var periodoAtual = periodos.Where(a => a.PeriodoFim >= aula.DataAula.Date)
                .OrderBy(a => a.PeriodoInicio)
                .FirstOrDefault();

            List<PeriodoEscolar> periodosParaGerarRecorrencia = new List<PeriodoEscolar>();

            if (aula.RecorrenciaAula == RecorrenciaAula.RepetirBimestreAtual)
            {
                periodosParaGerarRecorrencia.Add(periodoAtual);
            }
            else if (aula.RecorrenciaAula == RecorrenciaAula.RepetirTodosBimestres)
            {
                periodosParaGerarRecorrencia = periodos.Where(a => a.PeriodoInicio.Date >= aula.DataAula.Date)
                    .ToList();
            }

            GerarRecorrenciaParaPeriodos(aula, periodosParaGerarRecorrencia, usuario);
        }

        private async void GerarRecorrenciaParaPeriodos(Aula aula, IEnumerable<PeriodoEscolar> periodosParaGerarRecorrencia, Usuario usuario)
        {
            List<DateTime> diasParaIncluirRecorrencia = new List<DateTime>();

            foreach (var periodo in periodosParaGerarRecorrencia)
            {
                diasParaIncluirRecorrencia.AddRange(ObterDiaEntreDatas(periodo.PeriodoInicio, periodo.PeriodoFim)
                    .Where(d => d.DayOfWeek == aula.DataAula.DayOfWeek && d.Date > aula.DataAula.Date));
            }

            await GerarAulaDeRecorrenciaParaDias(aula, diasParaIncluirRecorrencia, usuario);
        }

        private IEnumerable<DateTime> ObterDiaEntreDatas(DateTime inicio, DateTime fim)
        {
            for (DateTime i = inicio; i < fim; i = i.AddDays(1))
            {
                yield return i;
            }
        }

        private async Task PersistirWorkflowReposicaoAula(Aula aula, string turma)
        {
            var loginAtual = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();
            var dre = "111";
            //Se esta aqui é porque existe escola 
            //  var escola = await repositorioAbrangencia.ObterUe(aula.UeId, loginAtual, perfilAtual);

            //if (escola == null)
            //  throw new NegocioException($"Não foi possível localizar a escola da criação do evento.");

            //       var linkParaEvento = $"{configuration["UrlFrontEnd"]}calendario-escolar/eventos/editar/:{evento.Id}/";
          //  var dre = await repositorioAbrangencia.ObterDre(loginAtual, perfilAtual);
            var wfAprovacaoAula = new WorkflowAprovacaoDto()
            {
                
                Ano = aula.DataAula.Year,
                NotificacaoCategoria = NotificacaoCategoria.Workflow_Aprovacao,
                EntidadeParaAprovarId = aula.Id,
                Tipo = WorkflowAprovacaoTipo.ReposicaoAula,
                UeId = aula.UeId,
                DreId = dre,
                NotificacaoTitulo = "Criação de Aula Teste Caique 21/11",
                NotificacaoTipo = NotificacaoTipo.Calendario,
                NotificacaoMensagem = $"Mensagem de Teste Reposicao aula caique 21/11 "

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
    }
}