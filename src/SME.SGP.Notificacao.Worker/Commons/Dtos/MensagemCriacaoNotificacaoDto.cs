using System;

namespace SME.SGP.Notificacao.Worker
{
    public class MensagemCriacaoNotificacaoDto : MensagemNotificacaoDto
    {
        public MensagemCriacaoNotificacaoDto(long codigo, string titulo, DateTime data, string usuarioRf) : base(codigo, usuarioRf)
        {
            Titulo = titulo;
            Data = data;
        }

        public string Titulo { get; }
        public DateTime Data { get; }
    }
}
