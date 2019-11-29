using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoAula : IServicoAula
    {
        private readonly IComandosWorkflowAprovacao comandosWorkflowAprovacao;
        private readonly IConfiguration configuration;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IConsultasGrade consultasGrade;
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IServicoDiaLetivo servicoDiaLetivo;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoLog servicoLog;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;

        public ServicoAula(IRepositorioAula repositorioAula,
                           IServicoEOL servicoEOL,
                           IRepositorioTipoCalendario repositorioTipoCalendario,
                           IServicoDiaLetivo servicoDiaLetivo,
                           IConsultasGrade consultasGrade,
                           IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                           IServicoLog servicoLog,
                           IRepositorioAbrangencia repositorioAbrangencia,
                           IServicoNotificacao servicoNotificacao,
                           IConsultasAbrangencia consultasAbrangencia,
                           IServicoUsuario servicoUsuario,
                           IComandosWorkflowAprovacao comandosWorkflowAprovacao,
                           IConfiguration configuration)
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
            this.configuration = configuration;
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
        }

        public async Task<string> Salvar(Aula aula, Usuario usuario)
        {
            var tipoCalendario = repositorioTipoCalendario.ObterPorId(aula.TipoCalendarioId);

            if (tipoCalendario == null)
                throw new NegocioException("O tipo de calendário não foi encontrado.");

            var disciplinasProfessor = await servicoEOL.ObterDisciplinasPorCodigoTurmaLoginEPerfil(aula.TurmaId, usuario.Login, usuario.ObterPerfilPrioritario());

            var usuarioPodeCriarAulaNaTurmaUeEModalidade = repositorioAula.UsuarioPodeCriarAulaNaUeTurmaEModalidade(aula, tipoCalendario.Modalidade);

            if (disciplinasProfessor == null || !disciplinasProfessor.Any(c => c.CodigoComponenteCurricular.ToString() == aula.DisciplinaId) || !usuarioPodeCriarAulaNaTurmaUeEModalidade)
            {
                throw new NegocioException("Você não pode criar aulas para essa UE/Turma/Disciplina.");
            }

            if (!servicoDiaLetivo.ValidarSeEhDiaLetivo(aula.DataAula, aula.TipoCalendarioId, null, aula.UeId))
            {
                throw new NegocioException("Não é possível cadastrar essa aula pois a data informada está fora do período letivo.");
            }

            if (aula.RecorrenciaAula == RecorrenciaAula.AulaUnica && aula.TipoAula == TipoAula.Reposicao)
            {
                var aulas = await repositorioAula.ObterAulas(aula.TipoCalendarioId, aula.TurmaId, aula.UeId, usuario.CodigoRf);
                var quantidadeDeAulasSomadas = aulas.ToList().FindAll(x => x.DataAula.Date == aula.DataAula.Date).Sum(x => x.Quantidade) + aula.Quantidade;

                var abrangencia = await consultasAbrangencia.ObterAbrangenciaTurma(aula.TurmaId);
                if (abrangencia == null)
                    throw new NegocioException("Abrangência da turma não localizada.");

                if (ReposicaoDeAulaPrecisaDeAprovacao(quantidadeDeAulasSomadas, abrangencia))
                {
                    var nomeDisciplina = await RetornaNomeDaDisciplina(aula, usuario);

                    repositorioAula.Salvar(aula);
                    PersistirWorkflowReposicaoAula(aula, abrangencia.NomeDre, abrangencia.NomeUe, nomeDisciplina,
                                                 abrangencia.NomeTurma, abrangencia.CodigoDre);
                    return "Aula cadastrada com sucesso e enviada para aprovação.";
                }
            }
            else
            {
                var semana = (aula.DataAula.DayOfYear / 7) + 1;
                var gradeAulas = await consultasGrade.ObterGradeAulasTurma(aula.TurmaId, int.Parse(aula.DisciplinaId), semana.ToString());
                if ((gradeAulas != null) && (gradeAulas.QuantidadeAulasRestante < aula.Quantidade))
                    throw new NegocioException("Quantidade de aulas superior ao limíte de aulas da grade.");
            }

            repositorioAula.Salvar(aula);

            if (aula.RecorrenciaAula != RecorrenciaAula.AulaUnica)
            {
                //TODO: ASSINCRONO
                GerarRecorrencia(aula, usuario);
                return "Aula cadastrada com sucesso. Serão cadastradas aulas recorrentes, em breve você receberá uma notificação com o resultado do processamento.";
            }
            return "Aula cadastrada com sucesso.";
        }

        private static bool ReposicaoDeAulaPrecisaDeAprovacao(int quantidadeAulasExistentesNoDia, Dto.AbrangenciaFiltroRetorno abrangencia)
        {
            return ((abrangencia.Modalidade == Modalidade.Fundamental && abrangencia.Ano >= 1 && abrangencia.Ano <= 5) ||  //Valida se é Fund 1
                               (Modalidade.EJA == abrangencia.Modalidade && (abrangencia.Ano == 1 || abrangencia.Ano == 2)) // Valida se é Eja Alfabetizacao ou  Basica
                               && quantidadeAulasExistentesNoDia > 1) || ((abrangencia.Modalidade == Modalidade.Fundamental && abrangencia.Ano >= 6 && abrangencia.Ano <= 9) || //valida se é fund 2
                                     (Modalidade.EJA == abrangencia.Modalidade && abrangencia.Ano == 3 || abrangencia.Ano == 4) ||  // Valida se é Eja Complementar ou Final
                                     (abrangencia.Modalidade == Modalidade.Medio) && quantidadeAulasExistentesNoDia > 2);
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
            var perfilAtual = usuario.PerfilAtual;

            var turmaAbrangencia = await repositorioAbrangencia.ObterAbrangenciaTurma(aula.TurmaId, usuario.Login, perfilAtual);

            if (turmaAbrangencia is null)
                throw new NegocioException($"Não foi possível localizar a turma de Id {aula.TurmaId} na abrangência ");

            var disciplinasEol = await servicoEOL.ObterDisciplinasPorCodigoTurmaLoginEPerfil(aula.TurmaId, usuario.Login, perfilAtual);

            if (disciplinasEol is null && !disciplinasEol.Any())
                throw new NegocioException($"Não foi possível localizar as disciplinas da turma {aula.TurmaId}");

            var disciplina = disciplinasEol.FirstOrDefault(a => a.CodigoComponenteCurricular == int.Parse(aula.DisciplinaId));

            if (disciplina == null)
                throw new NegocioException($"Não foi possível localizar a disciplina de Id {aula.DisciplinaId}.");

            var ue = await repositorioAbrangencia.ObterUe(aula.UeId, usuario.Login, perfilAtual);
            if (ue == null)
                throw new NegocioException($"Não foi possível localizar a Ue de Id {aula.UeId}.");

            var dre = await repositorioAbrangencia.ObterDre(string.Empty, aula.UeId, usuario.Login, perfilAtual);
            if (dre == null)
                throw new NegocioException($"Não foi possível localizar a Dre da Ue de Id {aula.UeId}.");

            var tituloMensagem = $"Criação de Aulas de {disciplina.Nome} na turma {turmaAbrangencia.NomeTurma}";
            StringBuilder mensagem = new StringBuilder();

            mensagem.Append($"Foram criadas {diasParaIncluirRecorrencia.Count - aulasQueDeramErro.Count} aulas da disciplina {disciplina.Nome} para a turma {turmaAbrangencia.NomeTurma} da {ue.Nome} ({dre.Nome}).");

            if (aulasQueDeramErro.Any())
            {
                mensagem.Append("Não foi possível criar aulas nas seguintes datas:");
                foreach (var aulaComErro in aulasQueDeramErro)
                {
                    mensagem.AppendFormat("<br /> {0} - {1}", $"{aulaComErro.Item1.Day}/{aulaComErro.Item1.Month}/{aulaComErro.Item1.Year}", aulaComErro.Item2);
                }
            }

            servicoNotificacao.Salvar(new Notificacao()
            {
                Ano = aula.CriadoEm.Year,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = dre.Codigo,
                Mensagem = mensagem.ToString(),
                UsuarioId = usuario.Id,
                Tipo = NotificacaoTipo.Calendario,
                Titulo = tituloMensagem,
                TurmaId = aula.TurmaId,
                UeId = ue.Codigo
            });
        }

        private void GerarRecorrencia(Aula aula, Usuario usuario)
        {
            var periodos = repositorioPeriodoEscolar.ObterPorTipoCalendario(aula.TipoCalendarioId);
            if (periodos == null || !periodos.Any())
                throw new NegocioException("Não foi possível obter os períodos deste tipo de calendário.");

            var inicioRecorrencia = aula.DataAula.AddDays(7);
            var fimRecorrencia = inicioRecorrencia;
            if (aula.RecorrenciaAula == RecorrenciaAula.RepetirBimestreAtual)
            {
                // Busca ultimo dia do periodo atual
                fimRecorrencia = periodos.Where(a => a.PeriodoFim >= aula.DataAula.Date)
                    .OrderBy(a => a.PeriodoInicio)
                    .FirstOrDefault().PeriodoFim;
            }
            else
            if (aula.RecorrenciaAula == RecorrenciaAula.RepetirTodosBimestres)
            {
                // Busca ultimo dia do ultimo periodo
                fimRecorrencia = periodos.Max(a => a.PeriodoFim);
            }

            GerarRecorrenciaParaPeriodos(aula, inicioRecorrencia, fimRecorrencia, usuario);
        }

        private async void GerarRecorrenciaParaPeriodos(Aula aula, DateTime inicioRecorrencia, DateTime fimRecorrencia, Usuario usuario)
        {
            List<DateTime> diasParaIncluirRecorrencia = new List<DateTime>();

            diasParaIncluirRecorrencia.AddRange(ObterDiaEntreDatas(inicioRecorrencia, fimRecorrencia));

            await GerarAulaDeRecorrenciaParaDias(aula, diasParaIncluirRecorrencia, usuario);
        }

        private IEnumerable<DateTime> ObterDiaEntreDatas(DateTime inicio, DateTime fim)
        {
            for (DateTime i = inicio; i < fim; i = i.AddDays(7))
            {
                yield return i;
            }
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

        private async Task<string> RetornaNomeDaDisciplina(Aula aula, Usuario usuario)
        {
            var disciplinasEol = await servicoEOL.ObterDisciplinasPorCodigoTurmaLoginEPerfil(aula.TurmaId, usuario.Login, usuario.PerfilAtual);

            if (disciplinasEol is null && !disciplinasEol.Any())
                throw new NegocioException($"Não foi possível localizar as disciplinas da turma {aula.TurmaId}");

            var disciplina = disciplinasEol.FirstOrDefault(a => a.CodigoComponenteCurricular == int.Parse(aula.DisciplinaId));

            if (disciplina == null)
                throw new NegocioException($"Não foi possível localizar a disciplina de Id {aula.DisciplinaId}.");

            return disciplina.Nome;
        }
    }
}