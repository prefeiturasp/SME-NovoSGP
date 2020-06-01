using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao.Commands.Aula.InserirAula
{
    public class InserirAulaUnicaCommand : IRequest<RetornoBaseDto>
    {
        public InserirAulaUnicaCommand(Usuario usuario, DateTime dataAula, Turma turma, string componenteCurricularId, string codigoRfProfessor, TipoCalendario tipoCalendario)
        {
            //TODO VALIDAR CAMPOS

            DataAula = dataAula;
            Turma = turma;
            ComponenteCurricularId = componenteCurricularId;
            CodigoRfProfessor = codigoRfProfessor;
            TipoCalendario = tipoCalendario;
        }

        public DateTime DataAula { get; private set; }

        public Turma Turma { get; private set; }

        public string ComponenteCurricularId { get; set; }

        public string CodigoRfProfessor { get; set; }

        public string UeId { get; set; }

        public TipoCalendario TipoCalendario { get; set; }

    }
}
