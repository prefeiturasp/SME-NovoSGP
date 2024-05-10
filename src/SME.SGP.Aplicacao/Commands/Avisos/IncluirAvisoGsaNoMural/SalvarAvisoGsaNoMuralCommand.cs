﻿using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class SalvarAvisoGsaNoMuralCommand : IRequest
    {
        public SalvarAvisoGsaNoMuralCommand(long aulaId, string usuarioRf, string mensagem, long avisoClassroomId, DateTime dataCriacao, DateTime? dataAlteracao, string email)
        {
            AulaId = aulaId;
            UsuarioRf = usuarioRf;
            Mensagem = mensagem;
            AvisoClassroomId = avisoClassroomId;
            DataCriacao = dataCriacao;
            DataAlteracao = dataAlteracao;
            Email = email;
        }

        public long AulaId { get; }
        public string UsuarioRf { get; }
        public string Mensagem { get; }
        public long AvisoClassroomId { get; }
        public DateTime DataCriacao { get; }
        public DateTime? DataAlteracao { get; }
        public string Email { get; }
    }
}
