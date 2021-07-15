using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class SalvarNotificacaoAulaPrevistaCommand : IRequest<bool>
    {
        public SalvarNotificacaoAulaPrevistaCommand(string titulo, string mensagem, string professorRF, string dreCodigo, string ueCodigo, string turmaCodigo, long usuarioId, int bimestre, string componenteCurricularId)
        {
            Titulo = titulo;
            Mensagem = mensagem;
            ProfessorRF = professorRF;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            TurmaCodigo = turmaCodigo;
            UsuarioId = usuarioId;
            Bimestre = bimestre;
            ComponenteCurricularId = componenteCurricularId;
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
