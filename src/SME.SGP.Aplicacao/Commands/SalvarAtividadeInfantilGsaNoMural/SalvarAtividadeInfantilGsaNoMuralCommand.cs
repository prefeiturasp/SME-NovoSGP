using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class SalvarAtividadeInfantilGsaNoMuralCommand : IRequest
    {
        public SalvarAtividadeInfantilGsaNoMuralCommand(long aulaId, string usuarioRf, string titulo, string mensagem, long avisoClassroomId, DateTime dataCriacao, DateTime? dataAlteracao, string email)
        {
            AulaId = aulaId;
            UsuarioRf = usuarioRf;
            Titulo = titulo;
            Mensagem = mensagem;
            AvisoClassroomId = avisoClassroomId;
            DataCriacao = dataCriacao;
            DataAlteracao = dataAlteracao;
            Email = email;
        }

        public long AulaId { get; }
        public string UsuarioRf { get; }
        public string Titulo { get; }
        public string Mensagem { get; }
        public long AvisoClassroomId { get; }
        public DateTime DataCriacao { get; }
        public DateTime? DataAlteracao { get; }
        public string Email { get; }
    }
}
