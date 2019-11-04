using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class Evento : EntidadeBase
    {
        public Evento()
        {
            Excluido = false;
        }

        public DateTime? DataFim { get; set; }
        public DateTime DataInicio { get; set; }
        public string Descricao { get; set; }
        public string DreId { get; set; }
        public bool Excluido { get; set; }
        public FeriadoCalendario FeriadoCalendario { get; set; }
        public long? FeriadoId { get; set; }
        public EventoLetivo Letivo { get; set; }
        public string Nome { get; set; }

        public TipoCalendario TipoCalendario { get; set; }

        public long TipoCalendarioId { get; set; }

        public EventoTipo TipoEvento { get; set; }

        public long TipoEventoId { get; set; }

        public string UeId { get; set; }

        public void AdicionarTipoEvento(EventoTipo tipoEvento)
        {
            TipoEvento = tipoEvento;
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
            if (!periodos.Any(c => c.PeriodoInicio >= DataInicio && c.PeriodoFim <= DataInicio))
            {
                throw new NegocioException("Não é permitido cadastrar um evento nesta data pois essa data não está dentro do 'Período Letivo'.");
            }
        }

        public void Excluir()
        {
            if (Excluido)
                throw new NegocioException("Este evento já está excluido.");

            Excluido = true;
        }

        public bool PermiteConcomitancia()
        {
            TipoEventoObrigatorio();
            return TipoEvento.Concomitancia;
        }

        public void PodeCriarEventoLiberacaoExcepcional(Evento evento, Usuario usuario, bool dataConfirmada, IEnumerable<PeriodoEscolar> periodos)
        {
            if (evento.TipoEvento.Codigo == (long)TipoEventoEnum.LiberacaoExcepcional)
            {
                if (!usuario.PossuiPerfilSme())
                    throw new NegocioException("Somente usuário com perfil SME pode cadastrar esse tipo de evento.");

                if (string.IsNullOrEmpty(evento.DreId))
                    throw new NegocioException("Para este tipo de evento, deve ser informado uma Dre.");

                if (string.IsNullOrEmpty(evento.UeId))
                    throw new NegocioException("Para este tipo de evento, deve ser informado uma Ue.");

                if (!periodos.Any(c => c.PeriodoInicio >= DataInicio && c.PeriodoFim <= DataInicio) && !dataConfirmada)
                    throw new NegocioException("Esta data é fora do período escolar, tem certeza que deseja manter esta data? (Sim/Não).", 602);
            }
        }

        public void PodeCriarEventoOrganizacaoEscolar(Usuario usuario)
        {
            if (this.TipoEvento.Codigo == (long)TipoEventoEnum.OrganizacaoEscolar)
            {
                if (!usuario.PossuiPerfilSme())
                    throw new NegocioException("Somente usuário com perfil SME pode cadastrar esse tipo de evento.");
            }
        }

        public void ValidaPeriodoEvento()
        {
            TipoEventoObrigatorio();
            if (TipoEvento.TipoData == EventoTipoData.InicioFim && !DataFim.HasValue)
            {
                throw new NegocioException("Neste tipo de evento a data final do evento deve ser informada.");
            }
            else
            {
                if (TipoEvento.TipoData == EventoTipoData.Unico && DataFim.HasValue)
                {
                    throw new NegocioException("Neste tipo de evento a data final do evento não deve ser informada.");
                }
            }
        }

        public void VerificaSeEventoAconteceJuntoComOrganizacaoEscolar(IEnumerable<Evento> eventos, Usuario usuario)
        {
            if (eventos.Any())
            {
                if (usuario.PossuiPerfilDreOuUe())
                {
                    if (TipoEvento.TipoData == EventoTipoData.InicioFim)
                    {
                        if (eventos.Any(a => (a.DataInicio.Date >= this.DataInicio.Date && this.DataInicio.Date <= a.DataFim.Value.Date) ||
                                              (a.DataInicio.Date >= this.DataFim.Value.Date && this.DataFim.Value.Date <= a.DataFim.Value.Date)))
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