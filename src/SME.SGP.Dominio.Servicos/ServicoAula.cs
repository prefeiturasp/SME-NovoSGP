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
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
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
                           IConsultasPeriodoEscolar consultasPeriodoEscolar,
                           IServicoLog servicoLog,
                           IRepositorioAbrangencia repositorioAbrangencia,
                           IServicoNotificacao servicoNotificacao)
        {
            this.repositorioAula = repositorioAula ?? throw new System.ArgumentNullException(nameof(repositorioAula));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
            this.servicoDiaLetivo = servicoDiaLetivo ?? throw new System.ArgumentNullException(nameof(servicoDiaLetivo));
            this.consultasGrade = consultasGrade ?? throw new System.ArgumentNullException(nameof(consultasGrade));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
        }

        public async Task<string> Salvar(Aula aula, Usuario usuario, RecorrenciaAula recorrencia)
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

            var ehInclusao = aula.Id == 0;

            // Busca quantidade de aulas semanais da grade de aula
            var semana = (aula.DataAula.DayOfYear / 7) + 1;
            var gradeAulas = await consultasGrade.ObterGradeAulasTurma(aula.TurmaId, int.Parse(aula.DisciplinaId), semana.ToString());
            var quantidadeAulasRestantes = gradeAulas.QuantidadeAulasRestante;

            if (!ehInclusao)
            {
                // Na alteração tem que considerar que uma aula possa estar mudando de dia na mesma semana, então não soma as aulas do proprio registro
                var aulasSemana = await repositorioAula.ObterAulas(aula.TipoCalendarioId, aula.TurmaId, aula.UeId, null, semana);
                var quantidadeAulasSemana = aulasSemana.Where(a => a.Id != aula.Id).Sum(a => a.Quantidade);

                quantidadeAulasRestantes = gradeAulas.QuantidadeAulasGrade - quantidadeAulasSemana;
            }
            if ((gradeAulas != null) && (quantidadeAulasRestantes < aula.Quantidade))
                throw new NegocioException("Quantidade de aulas superior ao limíte de aulas da grade.");

            repositorioAula.Salvar(aula);

            // Verifica recorrencia da gravação
            if (recorrencia != RecorrenciaAula.AulaUnica)
            {
                await GravarRecorrencia(ehInclusao, aula, usuario, recorrencia);

                var mensagem = ehInclusao ? "cadastrada" : "alterada";
                return $"Aula {mensagem} com sucesso. Serão {mensagem}s aulas recorrentes, em breve você receberá uma notificação com o resultado do processamento.";
            }
            return "Aula cadastrada com sucesso.";
        }

        private async Task GravarRecorrencia(bool inclusao, Aula aula, Usuario usuario, RecorrenciaAula recorrencia)
        {
            var fimRecorrencia = consultasPeriodoEscolar.ObterFimPeriodoRecorrencia(aula.TipoCalendarioId, aula.DataAula.Date, recorrencia);
            //TODO: ASSINCRONO
            if (inclusao)
                GerarRecorrencia(aula, usuario, fimRecorrencia);
            else
                await AlterarRecorrencia(aula, usuario, fimRecorrencia);
        }

        private async Task AlterarRecorrencia(Aula aula, Usuario usuario, DateTime fimRecorrencia)
        {
            var dataRecorrencia = aula.DataAula.AddDays(7);
            var aulasRecorrencia = await repositorioAula.ObterAulasRecorrencia(aula.AulaPaiId ?? aula.Id, aula.Id, fimRecorrencia);
            List<(DateTime data, string erro)> aulasQueDeramErro = new List<(DateTime, string)>();

            foreach (var aulaRecorrente in aulasRecorrencia)
            {
                aulaRecorrente.DataAula = dataRecorrencia;
                aulaRecorrente.Quantidade = aula.Quantidade;

                try
                {
                    await Salvar(aulaRecorrente, usuario, aulaRecorrente.RecorrenciaAula);
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

                dataRecorrencia = dataRecorrencia.AddDays(7);
            }

            await NotificarUsuario(usuario, aula, $"Foram alteradas {aulasRecorrencia.Count() - aulasQueDeramErro.Count} aulas", aulasQueDeramErro);
        }

        private async Task GerarAulaDeRecorrenciaParaDias(Aula aula, List<DateTime> diasParaIncluirRecorrencia, Usuario usuario)
        {
            List<(DateTime data, string erro)> aulasQueDeramErro = new List<(DateTime, string)>();

            foreach (var dia in diasParaIncluirRecorrencia)
            {
                var aulaParaAdicionar = (Aula)aula.Clone();
                aulaParaAdicionar.DataAula = dia;
                aulaParaAdicionar.AdicionarAulaPai(aula);
                aulaParaAdicionar.RecorrenciaAula = RecorrenciaAula.AulaUnica;

                try
                {
                    await Salvar(aulaParaAdicionar, usuario, aulaParaAdicionar.RecorrenciaAula);
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

            await NotificarUsuario(usuario, aula, $"Foram criadas {diasParaIncluirRecorrencia.Count - aulasQueDeramErro.Count} aulas", aulasQueDeramErro);
        }

        private async Task NotificarUsuario(Usuario usuario, Aula aula, string mensagem, List<(DateTime data, string erro)> aulasQueDeramErro)
        {
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
            StringBuilder mensagemUsuario = new StringBuilder();

            mensagemUsuario.Append($"{mensagem} da disciplina {disciplina.Nome} para a turma {turmaAbrangencia.NomeTurma} da {ue.Nome} ({dre.Nome}).");

            if (aulasQueDeramErro.Any())
            {
                mensagemUsuario.Append("Não foi possível criar aulas nas seguintes datas:");
                foreach (var aulaComErro in aulasQueDeramErro)
                {
                    mensagemUsuario.AppendFormat("<br /> {0} - {1}", $"{aulaComErro.data.Day}/{aulaComErro.data.Month}/{aulaComErro.data.Year}", aulaComErro.erro);
                }
            }

            servicoNotificacao.Salvar(new Notificacao()
            {
                Ano = aula.CriadoEm.Year,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = dre.Codigo,
                Mensagem = mensagemUsuario.ToString(),
                UsuarioId = usuario.Id,
                Tipo = NotificacaoTipo.Calendario,
                Titulo = tituloMensagem,
                TurmaId = aula.TurmaId,
                UeId = ue.Codigo
            });
        }

        private void GerarRecorrencia(Aula aula, Usuario usuario, DateTime fimRecorrencia)
        {
            var inicioRecorrencia = aula.DataAula.AddDays(7);

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
    }
}