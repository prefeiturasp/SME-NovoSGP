using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class InserirOuAtualizarCacheFechamentoNotaConceitoCommand : IRequest<bool>
    {
        public InserirOuAtualizarCacheFechamentoNotaConceitoCommand(long componenteCurricularId,
            string turmaCodigo, IEnumerable<FechamentoNotaConceitoDto> fechamentosNotasConceitos,
            bool emAprovacao, int? bimestre)
        {
            ComponenteCurricularId = componenteCurricularId;
            TurmaCodigo = turmaCodigo;
            FechamentosNotasConceitos = fechamentosNotasConceitos;
            EmAprovacao = emAprovacao;
            Bimestre = bimestre;
        }

        public long ComponenteCurricularId { get; }
        public string TurmaCodigo { get; }
        public IEnumerable<FechamentoNotaConceitoDto> FechamentosNotasConceitos { get; }
        public bool EmAprovacao { get; }
        public int? Bimestre { get; }
    }
}