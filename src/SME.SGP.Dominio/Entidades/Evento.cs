using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class Evento : EntidadeBase, ICloneable
    {
        public Evento()
        {
            Excluido = false;
            Status = EntidadeStatus.Aprovado;
        }

        public DateTime DataFim { get; set; }
        public DateTime DataInicio { get; set; }
        public string Descricao { get; set; }
        public string DreId { get; set; }
        public long? EventoPaiId { get; set; }
        public bool Excluido { get; set; }
        public FeriadoCalendario FeriadoCalendario { get; set; }
        public long? FeriadoId { get; set; }
        public EventoLetivo Letivo { get; set; }
        public bool Migrado { get; set; }
        public string Nome { get; set; }
        public EntidadeStatus Status { get; set; }
        public TipoCalendario TipoCalendario { get; set; }
        public long TipoCalendarioId { get; set; }
        public EventoTipo TipoEvento { get; set; }
        public long TipoEventoId { get; set; }
        public TipoPerfil? TipoPerfilCadastro { get; set; }
        public string UeId { get; set; }
        public WorkflowAprovacao WorkflowAprovacao { get; set; }
        public long? WorkflowAprovacaoId { get; set; }

        public void AdicionarTipoCalendario(TipoCalendario tipoCalendario)
        {
            if (tipoCalendario == null)
                throw new NegocioException("É necessário informar um tipo de Calendário.");

            TipoCalendarioId = tipoCalendario.Id;
            TipoCalendario = tipoCalendario;
        }

        public void AdicionarTipoEvento(EventoTipo tipoEvento)
        {
            TipoEvento = tipoEvento;
        }

        public void AprovarWorkflow()
        {
            if (Status != EntidadeStatus.AguardandoAprovacao)
                throw new NegocioException("Este Evento não pode ser aprovado.");

            Status = EntidadeStatus.Aprovado;
        }

        public object Clone()
        {
            return new Evento
            {
                AlteradoEm = AlteradoEm,
                AlteradoPor = AlteradoPor,
                AlteradoRF = AlteradoRF,
                CriadoEm = CriadoEm,
                CriadoPor = CriadoPor,
                CriadoRF = CriadoRF,
                DataFim = DataFim,
                DataInicio = DataInicio,
                Descricao = Descricao,
                DreId = DreId,
                Excluido = Excluido,
                FeriadoCalendario = FeriadoCalendario,
                FeriadoId = FeriadoId,
                Letivo = Letivo,
                Nome = Nome,
                TipoCalendario = TipoCalendario,
                TipoCalendarioId = TipoCalendarioId,
                TipoEvento = TipoEvento,
                TipoEventoId = TipoEventoId,
                UeId = UeId
            };
        }

        public bool DeveSerEmDiaLetivo()
        {
            TipoEventoObrigatorio();
            if (TipoEvento.Letivo == EventoLetivo.Sim && Letivo != EventoLetivo.Sim)
            {
                throw new NegocioException("O evento deve ser do tipo 'Letivo'.");
            }
            else
            {
                if (TipoEvento.Letivo == EventoLetivo.Nao && Letivo != EventoLetivo.Nao)
                {
                    throw new NegocioException("O evento não deve ser do tipo 'Letivo'.");
                }
            }
            return TipoEvento.Letivo == EventoLetivo.Sim;
        }

        public bool EhTipoEventoFechamento()
        {
            return TipoEvento.Codigo == (long)Dominio.TipoEvento.FechamentoBimestre;
        }

        public void EnviarParaWorkflowDeAprovacao(long idWorkflow)
        {
            WorkflowAprovacaoId = idWorkflow;
            Status = EntidadeStatus.AguardandoAprovacao;
        }

        public void EstaNoAnoLetivoDoCalendario()
        {
            if (TipoCalendario == null)
            {
                throw new NegocioException("O tipo de calendário não foi preenchido.");
            }
            if (TipoCalendario.AnoLetivo != DataInicio.Year)
            {
                throw new NegocioException("A data do evento deve ser no mesmo ano do calendário selecionado.");
            }
        }

        public void EstaNoPeriodoLetivo(IEnumerable<PeriodoEscolar> periodos)
        {
            if (periodos == null || !periodos.Any())
            {
                throw new NegocioException("Não é permitido cadastrar esse evento pois não existe período escolar cadastrado para o calendário informado.");
            }

            //if (!periodos.Any(c => c.PeriodoInicio.Date <= DataInicio.Date && c.PeriodoFim.Date >= DataFim.Date))
            //{
            //    throw new NegocioException("Não é permitido cadastrar um evento nesta data pois essa data não está dentro do 'Período Letivo'.");
            //}
        }

        public bool EstaNoRangeDeDatas(IEnumerable<(DateTime, DateTime)> datas)
        {
            return datas.Any(a => (DataInicio.Date <= a.Item1.Date && DataFim >= a.Item2.Date)
            || (DataInicio.Date <= a.Item2.Date && DataFim >= a.Item2.Date)
            || (DataInicio.Date >= a.Item1.Date && DataFim <= a.Item2.Date));
        }

        public void Excluir()
        {
            if (Excluido)
                throw new NegocioException("Este evento já está excluido.");

            Excluido = true;
        }

        public IEnumerable<Evento> ObterRecorrencia(PadraoRecorrencia padraoRecorrencia, PadraoRecorrenciaMensal? padraoRecorrenciaMensal, DateTime dataInicio, DateTime dataFinal, IEnumerable<DayOfWeek> diasDaSemana, int repeteACada, int? diaDeOcorrencia)
        {
            if (Id == 0)
                throw new NegocioException("Só é possível aplicar recorrência em eventos já registrados.");

            if (dataFinal == DateTime.MinValue)
                throw new NegocioException("A data final é obrigatória para calcular a recorrência de eventos.");
            if (padraoRecorrencia == PadraoRecorrencia.Semanal)
                return ObterRecorrenciaSemanal(dataInicio, dataFinal, diasDaSemana, repeteACada);
            else
            {
                if (!padraoRecorrenciaMensal.HasValue)
                    throw new NegocioException("Para esse padrão de recorrência o tipo de recorrência mensal deve ser informado.");

                if (padraoRecorrenciaMensal == PadraoRecorrenciaMensal.NoDia && !diaDeOcorrencia.HasValue)
                    throw new NegocioException("O dia de ocorrência é obrigatório para calcular esse tipo recorrência de eventos.");

                return ObterRecorrenciaMensal(padraoRecorrenciaMensal.Value, dataInicio, dataFinal, diaDeOcorrencia, repeteACada, diasDaSemana);
            }
        }

        public bool PermiteConcomitancia()
        {
            TipoEventoObrigatorio();
            return TipoEvento.Concomitancia;
        }

        public bool? PodeAlterar()
            => !TipoEvento.SomenteLeitura && Status != EntidadeStatus.AguardandoAprovacao;

        public void PodeCriarEventoLiberacaoExcepcional(Usuario usuario, bool dataConfirmada, IEnumerable<PeriodoEscolar> periodos)
        {
            if (this.TipoEvento.Codigo == (long)Dominio.TipoEvento.LiberacaoExcepcional)
            {
                if (!usuario.PossuiPerfilSme())
                    throw new NegocioException("Somente usuário com perfil SME pode cadastrar esse tipo de evento.");

                if (string.IsNullOrEmpty(this.DreId))
                    throw new NegocioException("Para este tipo de evento, deve ser informado uma Dre.");

                if (string.IsNullOrEmpty(this.UeId))
                    throw new NegocioException("Para este tipo de evento, deve ser informado uma Ue.");

                if (!periodos.Any(a => (a.PeriodoInicio <= this.DataInicio && a.PeriodoFim >= this.DataFim)) && !dataConfirmada)
                    throw new NegocioException("Esta data é fora do período escolar, tem certeza que deseja manter esta data? (Sim/Não).", 602);
            }
        }

        public void PodeCriarEventoOrganizacaoEscolarComPerfilSme(Usuario usuario)
        {
            if (this.TipoEvento.Codigo == (long)Dominio.TipoEvento.OrganizacaoEscolar)
            {
                if (usuario.ObterTipoPerfilAtual() != TipoPerfil.SME)
                    throw new NegocioException("Somente usuário com perfil SME pode cadastrar esse tipo de evento.");
            }
        }

        public void PodeSerEnviadoParaAprovacao()
        {
            if (Status != EntidadeStatus.AguardandoAprovacao)
                throw new NegocioException("Este envento não está Aguardando Aprovação.");
        }

        public void ReprovarWorkflow()
        {
            if (Status != EntidadeStatus.AguardandoAprovacao)
                throw new NegocioException("Este Evento não pode ser recusado.");

            Status = EntidadeStatus.Recusado;
        }

        public void ValidaPeriodoEvento()
        {
            TipoEventoObrigatorio();

            if (TipoEvento.TipoData == EventoTipoData.InicioFim && DataFim == DateTime.MinValue)
            {
                throw new NegocioException("Neste tipo de evento a data final do evento deve ser informada.");
            }
            else if (TipoEvento.TipoData == EventoTipoData.Unico)
            {
                DataFim = DataInicio;
            }
        }

        public void VerificaSeDataMenorQueHoje()
        {
            if (DataInicio.Date < DateTime.Today)
                throw new NegocioException("A data do evento não pode ser menor que a atual.");
        }

        public void VerificaSeEventoAconteceJuntoComOrganizacaoEscolar(IEnumerable<Evento> eventos, Usuario usuario)
        {
            if (eventos.Any() && usuario.PossuiPerfilDreOuUe())
            {
                if (TipoEvento.TipoData == EventoTipoData.InicioFim)
                {
                    if (eventos.Any(a => (a.DataInicio.Date >= this.DataInicio.Date && this.DataInicio.Date <= a.DataFim.Date) ||
                                          (a.DataInicio.Date >= this.DataFim.Date && this.DataFim.Date <= a.DataFim.Date)))
                    {
                        throw new NegocioException($"Não é possível adicionar um evento nesta data pois ele se encontra no período do evento {eventos.FirstOrDefault().Nome} ");
                    }
                }
                else
                {
                    if (eventos.Any(a => (a.DataInicio >= this.DataInicio && a.DataInicio <= this.DataFim)))
                    {
                        throw new NegocioException($"Não é possível adicionar um evento nesta data pois ele se encontra no período do evento {eventos.FirstOrDefault().Nome} ");
                    }
                }
            }
        }

        private static DateTime ObterPrimeiroDiaDoMes(DateTime dataAtual)
        {
            return new DateTime(dataAtual.Year, dataAtual.Month, 1);
        }

        private static DateTime ObterUltimoDiaDaSemanaDoMes(DateTime dataAtual, DayOfWeek diaDaSemana)
        {
            var primeiroDiaDoMes = ObterPrimeiroDiaDoMes(dataAtual);
            var ultimoDiaDoMes = primeiroDiaDoMes.AddMonths(1).AddDays(-1);
            if (ultimoDiaDoMes.DayOfWeek == diaDaSemana)
                return ultimoDiaDoMes;
            else
            {
                for (DateTime data = ultimoDiaDoMes; data >= primeiroDiaDoMes; data = data.AddDays(-1))
                {
                    if (data.DayOfWeek == diaDaSemana)
                    {
                        dataAtual = data;
                        break;
                    }
                }
                return dataAtual;
            }
        }

        private void AdicionaEventosPorDiasDaSemana(IEnumerable<DayOfWeek> diasDaSemana, List<Evento> eventos, DateTime dataInicial, DateTime dataFinal)
        {
            var dataLimite = dataInicial.ObterSabado();
            if (dataFinal < dataLimite)
                dataLimite = dataFinal;
            for (DateTime data = dataInicial; data <= dataLimite; data = data.AddDays(1))
            {
                if (diasDaSemana.Contains(data.DayOfWeek))
                {
                    var evento = (Evento)Clone();
                    evento.DataInicio = data;
                    evento.DataFim = data;
                    evento.EventoPaiId = Id;
                    evento.Id = 0;
                    eventos.Add(evento);
                }
            }
        }

        private DateTime ObterDiaDaSemanaDoMes(DateTime dataAtual, PadraoRecorrenciaMensal padraoRecorrenciaMensal, DayOfWeek diaDaSemana)
        {
            DateTime primeiroDiaDoMes = ObterPrimeiroDiaDoMes(dataAtual);
            var quantidadeDeDiasDaSemana = 0;

            for (DateTime data = primeiroDiaDoMes; quantidadeDeDiasDaSemana < (int)padraoRecorrenciaMensal; data = data.AddDays(1))
            {
                if (data.DayOfWeek == diaDaSemana)
                {
                    quantidadeDeDiasDaSemana++;
                    dataAtual = data;
                }
            }
            return dataAtual;
        }

        private DateTime ObterProximaDataRecorrenciaMensal(DateTime dataAtual, PadraoRecorrenciaMensal padraoRecorrenciaMensal, IEnumerable<DayOfWeek> diasDaSemana, int? diaOcorrencia)
        {
            if (padraoRecorrenciaMensal != PadraoRecorrenciaMensal.NoDia)
            {
                if (diasDaSemana == null)
                {
                    throw new NegocioException("Os dias da semana são obrigatórios para esse tipo de recorrência.");
                }
                if (padraoRecorrenciaMensal == PadraoRecorrenciaMensal.Ultima)
                {
                    return ObterUltimoDiaDaSemanaDoMes(dataAtual, diasDaSemana.FirstOrDefault());
                }
                dataAtual = ObterDiaDaSemanaDoMes
                    (dataAtual, padraoRecorrenciaMensal, diasDaSemana.FirstOrDefault());
            }
            else
            {
                return ObterRecorrenciaParaDiaFixo(dataAtual, diaOcorrencia.Value);
            }
            return dataAtual;
        }

        private IEnumerable<Evento> ObterRecorrenciaMensal(PadraoRecorrenciaMensal padraoRecorrenciaMensal, DateTime dataInicio, DateTime dataFinal, int? diaOcorrencia, int repeteACada, IEnumerable<DayOfWeek> diasDaSemana)
        {
            var eventos = new List<Evento>();
            var dataAtual = dataInicio;
            while (dataAtual <= dataFinal)
            {
                dataAtual = ObterProximaDataRecorrenciaMensal(dataAtual, padraoRecorrenciaMensal, diasDaSemana, diaOcorrencia);
                if (dataAtual >= dataInicio)
                {
                    var evento = (Evento)Clone();
                    evento.DataInicio = dataAtual;
                    evento.DataFim = dataAtual;
                    evento.EventoPaiId = Id;
                    evento.Id = 0;
                    eventos.Add(evento);
                }
                dataAtual = dataAtual.AddMonths(repeteACada);
            }

            return eventos;
        }

        private DateTime ObterRecorrenciaParaDiaFixo(DateTime mesAtual, int diaOcorrencia)
        {
            return new DateTime(mesAtual.Year, mesAtual.Month, diaOcorrencia);
        }

        private IEnumerable<Evento> ObterRecorrenciaSemanal(DateTime dataInicio, DateTime dataFinal, IEnumerable<DayOfWeek> diasDaSemana, int repeteACada)
        {
            var dataInicial = dataInicio.ObterDomingo();
            if (dataInicial.DayOfWeek < dataInicio.DayOfWeek)
                dataInicial = dataInicio;

            var eventos = new List<Evento>();
            while (dataInicial <= dataFinal)
            {
                AdicionaEventosPorDiasDaSemana(diasDaSemana, eventos, dataInicial, dataFinal);
                dataInicial = dataInicial.AddDays(7 * repeteACada);
                dataInicial = dataInicial.ObterDomingo();
            }
            return eventos;
        }

        private void TipoEventoObrigatorio()
        {
            if (TipoEvento == null)
            {
                throw new NegocioException("O tipo de evento não foi encontrado.");
            }
        }
    }
}