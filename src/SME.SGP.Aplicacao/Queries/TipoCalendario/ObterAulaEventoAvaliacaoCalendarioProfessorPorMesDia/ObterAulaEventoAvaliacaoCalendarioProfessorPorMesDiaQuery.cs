using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaEventoAvaliacaoCalendarioProfessorPorMesDiaQuery : IRequest<IEnumerable<EventoAulaDto>>
    {
        public IEnumerable<Evento> EventosDaUeSME { get; set; }
        public IEnumerable<Aula> Aulas { get; set; }
        public IEnumerable<AtividadeAvaliativa> Avaliacoes { get; set; }
        public string UsuarioCodigoRf { get; internal set; }        
        public IEnumerable<DisciplinaDto> ComponentesCurricularesParaVisualizacao { get; set; }
    }
}
