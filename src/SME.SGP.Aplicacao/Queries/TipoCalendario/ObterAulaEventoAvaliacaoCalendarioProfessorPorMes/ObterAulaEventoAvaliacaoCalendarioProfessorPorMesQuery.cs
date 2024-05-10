using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaEventoAvaliacaoCalendarioProfessorPorMesQuery : IRequest<IEnumerable<EventoAulaDiaDto>>
    {
        public string TurmaCodigo { get; set; }
        public int AnoLetivo { get; internal set; }
        public int Mes { get; internal set; }
        public IEnumerable<Evento> EventosDaUeSME { get; set; }
        public IEnumerable<Aula> Aulas { get; set; }
        public IEnumerable<AtividadeAvaliativa> Avaliacoes { get; set; }
        public string UsuarioCodigoRf { get; internal set; }
    }
}
