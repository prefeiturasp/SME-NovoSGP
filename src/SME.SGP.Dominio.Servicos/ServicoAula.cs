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

        public ServicoAula(IRepositorioAula repositorioAula,
                           IServicoEOL servicoEOL,
                           IRepositorioTipoCalendario repositorioTipoCalendario,
                           IServicoDiaLetivo servicoDiaLetivo,
                           IConsultasGrade consultasGrade,
                           IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                           IServicoLog servicoLog,
                           IRepositorioAbrangencia repositorioAbrangencia,
                           IServicoUsuario servicoUsuario)
        {
            this.repositorioAula = repositorioAula ?? throw new System.ArgumentNullException(nameof(repositorioAula));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
            this.servicoDiaLetivo = servicoDiaLetivo ?? throw new System.ArgumentNullException(nameof(servicoDiaLetivo));
            this.consultasGrade = consultasGrade ?? throw new System.ArgumentNullException(nameof(consultasGrade));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
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
    }
}