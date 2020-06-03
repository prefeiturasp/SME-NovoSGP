using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao.Commands.Aulas.InserirAula
{
    public class InserirAulaUnicaCommand : IRequest<RetornoBaseDto>
    {
        public InserirAulaUnicaCommand(Usuario usuario, DateTime dataAula, int quantidade, Turma turma, long componenteCurricularId, string nomeComponenteCurricular, TipoCalendario tipoCalendario, TipoAula tipoAula)
        {
            //TODO VALIDAR CAMPOS
            Usuario = usuario;
            DataAula = dataAula;
            Quantidade = quantidade;
            Turma = turma;
            ComponenteCurricularId = componenteCurricularId;
            NomeComponenteCurricular = nomeComponenteCurricular;
            TipoCalendario = tipoCalendario;
            TipoAula = tipoAula;
        }

        public DateTime DataAula { get; private set; }
        public Turma Turma { get; private set; }
        public long ComponenteCurricularId { get; private set; }
        public string UeId { get; private set; }
        public Usuario Usuario { get; private set; }
        public TipoCalendario TipoCalendario { get; private set; }
        public string NomeComponenteCurricular { get; private set; }
        public int Quantidade { get; private set; }
        public TipoAula TipoAula { get; private set; }
    }
}
