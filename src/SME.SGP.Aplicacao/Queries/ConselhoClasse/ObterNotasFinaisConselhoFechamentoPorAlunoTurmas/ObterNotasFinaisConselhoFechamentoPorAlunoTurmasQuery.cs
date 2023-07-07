using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFinaisConselhoFechamentoPorAlunoTurmasQuery : IRequest<IEnumerable<NotaConceitoFechamentoConselhoFinalDto>>
    {
        public ObterNotasFinaisConselhoFechamentoPorAlunoTurmasQuery(string[] turmasCodigos, string alunoCodigo, bool validaNota = false, bool validaConceito = false)
        {
            TurmasCodigos = turmasCodigos;
            AlunoCodigo = alunoCodigo;
            ValidaNota = validaNota;
            ValidaConceito = validaConceito;
        }

        public string[] TurmasCodigos { get; set; }
        public string AlunoCodigo { get; set; }
        public bool ValidaNota { get; set; }
        public bool ValidaConceito { get; set; }
    }
}
