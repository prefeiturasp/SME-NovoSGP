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
        private readonly IConsultasGrade consultasGrade;
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IServicoDiaLetivo servicoDiaLetivo;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoLog servicoLog;
        private readonly IServicoNotificacao servicoNotificacao;

        public ServicoAula(IRepositorioAula repositorioAula,
                           IServicoEOL servicoEOL,
                           IRepositorioTipoCalendario repositorioTipoCalendario,
                           IServicoDiaLetivo servicoDiaLetivo,
                           IConsultasGrade consultasGrade,
                           IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                           IServicoLog servicoLog,
                           IRepositorioAbrangencia repositorioAbrangencia,
                           IServicoNotificacao servicoNotificacao)
        {
            this.repositorioAula = repositorioAula ?? throw new System.ArgumentNullException(nameof(repositorioAula));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
            this.servicoDiaLetivo = servicoDiaLetivo ?? throw new System.ArgumentNullException(nameof(servicoDiaLetivo));
            this.consultasGrade = consultasGrade ?? throw new System.ArgumentNullException(nameof(consultasGrade));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
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

            if (!disciplinasProfessor.Any(c => c.CodigoComponenteCurricular.ToString() == aula.DisciplinaId) || !usuarioPodeCriarAulaNaTurmaUeEModalidade)
            {
                throw new NegocioException("Você não pode criar aulas para essa UE/Turma/Disciplina.");
            }

            if (!servicoDiaLetivo.ValidarSeEhDiaLetivo(aula.DataAula, aula.TipoCalendarioId, null, aula.UeId))
            {
                throw new NegocioException("Não é possível cadastrar essa aula pois a data informada está fora do período letivo.");
            }

            var gradeAulas = await consultasGrade.ObterGradeAulasTurma(aula.TurmaId, int.Parse(aula.DisciplinaId));
            if ((gradeAulas != null) && (gradeAulas.QuantidadeAulasRestante < aula.Quantidade))
                throw new NegocioException("Quantidade de aulas superior ao limíte de aulas da grade.");

            repositorioAula.Salvar(aula);

            if (aula.RecorrenciaAula != RecorrenciaAula.AulaUnica)
            {
                //TODO: ASSINCRONO
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
    }
}