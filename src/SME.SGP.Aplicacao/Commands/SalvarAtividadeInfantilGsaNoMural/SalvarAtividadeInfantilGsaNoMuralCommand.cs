using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class SalvarAtividadeInfantilGsaNoMuralCommand : IRequest
    {
        public SalvarAtividadeInfantilGsaNoMuralCommand(long aulaId, string usuarioRf, string titulo, string descricao, DateTime dataCriacao, DateTime? dataAlteracao, long atividadeClassroomId, string email)
        {
            AulaId = aulaId; 
            UsuarioRf = usuarioRf;
            Titulo = titulo;
            Mensagem = descricao;
            DataCriacao = dataCriacao;
            DataAlteracao = dataAlteracao;
            AvisoClassroomId = atividadeClassroomId;
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
