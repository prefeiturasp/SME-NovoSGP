using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class SalvarAtividadeInfantilCommand : IRequest
    {
        public SalvarAtividadeInfantilCommand(long aulaId, AtividadeGsaDto atividade)
        {
            AulaId = aulaId; 
            UsuarioRf = atividade.UsuarioRf;
            Titulo = atividade.Titulo;
            Mensagem = atividade.Descricao;
            DataCriacao = atividade.DataCriacao;
            DataAlteracao = atividade.DataAlteracao;
            AvisoClassroomId = atividade.AtividadeClassroomId;
            Email = atividade.Email;
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
