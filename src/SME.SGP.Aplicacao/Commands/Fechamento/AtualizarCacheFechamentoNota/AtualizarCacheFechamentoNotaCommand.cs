using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class AtualizarCacheFechamentoNotaCommand : IRequest<bool>
    {
        public AtualizarCacheFechamentoNotaCommand(
                            FechamentoNota fechamentoNota, 
                            string codigoAluno, 
                            string codigoTurma, 
                            bool emAprovacao = false,
                            ConselhoClasseAlunosNotaPorFechamentoIdDto conselhosClasseAlunos = null)
        {
            FechamentoNota = fechamentoNota;
            CodigoAluno = codigoAluno;
            CodigoTurma = codigoTurma;
            EmAprovacao = emAprovacao;
            ConselhosClasseAlunos = conselhosClasseAlunos;
        }

        public FechamentoNota FechamentoNota { get; set; }
        public string CodigoAluno { get; set; }
        public long FechamentoTurmaDisciplinaId { get; set; }
        public string CodigoTurma { get; set; }
        public bool EmAprovacao { get; set; }

        public ConselhoClasseAlunosNotaPorFechamentoIdDto ConselhosClasseAlunos;
    }
}
