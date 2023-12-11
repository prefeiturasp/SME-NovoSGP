using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarNotificacaoAulaPrevistaCommand : IRequest<bool>
    {
        public SalvarNotificacaoAulaPrevistaCommand(RegistroAulaPrevistaDivergenteDto AulaPrevistaDto, string titulo, string mensagem, long usuarioId)
        {
            Titulo = titulo;
            Mensagem = mensagem;
            ProfessorRF = AulaPrevistaDto.ProfessorRf;
            DreCodigo = AulaPrevistaDto.CodigoDre;
            UeCodigo = AulaPrevistaDto.CodigoUe;
            TurmaCodigo = AulaPrevistaDto.CodigoTurma;
            UsuarioId = usuarioId;
            Bimestre = AulaPrevistaDto.Bimestre;
            ComponenteCurricularId = AulaPrevistaDto.DisciplinaId;
        }

        public string Titulo { get; }
        public string Mensagem { get; }
        public string ProfessorRF { get; }
        public string DreCodigo { get; }
        public string UeCodigo { get; }
        public string TurmaCodigo { get; }
        public long UsuarioId { get; }
        public int Bimestre { get; }
        public string ComponenteCurricularId { get; }
    }
}
