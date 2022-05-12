using System;
using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresTitularesPorUeQuery : IRequest<IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        public ObterProfessoresTitularesPorUeQuery(string ueCodigo, DateTime dataReferencia)
        {
            UeCodigo = ueCodigo;
            DataReferencia = dataReferencia;
        }

        public string UeCodigo { get; }
        public DateTime DataReferencia { get; }
    }
}