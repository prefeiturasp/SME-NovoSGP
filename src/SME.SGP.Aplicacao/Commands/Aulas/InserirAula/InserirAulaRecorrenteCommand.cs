using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao.Commands.Aulas.InserirAula
{
    public class InserirAulaRecorrenteCommand : IRequest<bool>
    {
        public InserirAulaRecorrenteCommand(Usuario usuario,
                                       DateTime dataAula,
                                       int quantidade,
                                       string codigoTurma,
                                       long componenteCurricularId,
                                       string nomeComponenteCurricular,
                                       long tipoCalendarioId,
                                       TipoAula tipoAula,
                                       string codigoUe,
                                       bool ehRegencia,
                                       RecorrenciaAula recorrenciaAula)
        {
            Usuario = usuario;
            DataAula = dataAula;
            Quantidade = quantidade;
            CodigoTurma = codigoTurma;
            ComponenteCurricularId = componenteCurricularId;
            NomeComponenteCurricular = nomeComponenteCurricular;
            TipoCalendarioId = tipoCalendarioId;
            TipoAula = tipoAula;
            CodigoUe = codigoUe;
            EhRegencia = ehRegencia;
            RecorrenciaAula = recorrenciaAula;
        }

        public Usuario Usuario { get; set; }
        public DateTime DataAula { get; set; }
        public int Quantidade { get; set; }
        public string CodigoTurma { get; set; }
        public long ComponenteCurricularId { get; set; }
        public string NomeComponenteCurricular { get; set; }
        public long TipoCalendarioId { get; set; }
        public TipoAula TipoAula { get; set; }
        public string CodigoUe { get; set; }
        public bool EhRegencia { get; set; }
        public RecorrenciaAula RecorrenciaAula  { get; set; }
    }
}
