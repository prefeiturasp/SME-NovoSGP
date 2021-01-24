using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class DiarioBordoObservacaoNotificacaoUsuarioDto
    {
        protected DiarioBordoObservacaoNotificacaoUsuarioDto() { }

        public DiarioBordoObservacaoNotificacaoUsuarioDto(long idObservacao, long idNotificacao, long idUsuario)
        {
            this.IdObservacao = idObservacao;
            this.IdNotificacao = idNotificacao;
            this.IdUsuario = idUsuario;
        }

        public long Id { get; set; }
        public long IdObservacao { get; set; }
        public long IdNotificacao { get; set; }
        public long IdUsuario { get; set; }
    }
}
