using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

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