using System;

namespace SME.SGP.Notificacoes.Hub.Interface
{
    public class MensagemCriacaoNotificacaoDto : MensagemNotificacaoDto
    {
        public MensagemCriacaoNotificacaoDto() { }
        public MensagemCriacaoNotificacaoDto(long id, long codigo, string titulo, DateTime data, string usuarioRf)
        {
            Id = id;
            Codigo = codigo;
            Titulo = titulo;
            Data = data;
            UsuarioRf = usuarioRf;
        }

        public string Titulo { get; set; }
        public DateTime Data { get; set; }
        public long Id { get; set; }
    }
}
