using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosAtivosPorUeENomeQuery : IRequest<IEnumerable<AlunoParaAutoCompleteAtivoDto>>
    {
        public ObterAlunosAtivosPorUeENomeQuery(string ueCodigo, DateTime dataReferencia, string alunoNome, long alunoCodigo)
        {
            UeCodigo = ueCodigo;
            DataReferencia = dataReferencia;
            AlunoNome = alunoNome;
            AlunoCodigo = alunoCodigo;
        }

        public string UeCodigo { get; set; }        
        public DateTime DataReferencia { get; set; }
        public string AlunoNome { get; set; }
        public long AlunoCodigo { get; set; }
    }
}
