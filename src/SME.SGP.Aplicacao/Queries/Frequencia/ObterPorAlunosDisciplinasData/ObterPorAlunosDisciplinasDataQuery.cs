using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPorAlunosDisciplinasDataQuery : IRequest<IEnumerable<FrequenciaAluno>>
    {
        public ObterPorAlunosDisciplinasDataQuery(string[] codigosAlunos,
            string[] disciplinasIds,
            DateTime dataAtual)
        {
            CodigosAlunos = codigosAlunos;
            DisciplinasIds = disciplinasIds;
            DataAtual = dataAtual;
        }

        public ObterPorAlunosDisciplinasDataQuery(string[] codigosAlunos,
            string[] disciplinasIds,
            DateTime dataAtual,
            string turmaCodigo) : this(codigosAlunos, disciplinasIds, dataAtual)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string[] CodigosAlunos { get; set; }
        public string[] DisciplinasIds { get; set; }
        public DateTime DataAtual { get; set; }
        public string TurmaCodigo { get; set; }
    }
}
