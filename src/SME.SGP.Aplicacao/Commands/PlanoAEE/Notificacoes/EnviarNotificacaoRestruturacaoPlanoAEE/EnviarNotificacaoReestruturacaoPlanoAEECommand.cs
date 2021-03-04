using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class EnviarNotificacaoReestruturacaoPlanoAEECommand : IRequest<bool>
    {
        public EnviarNotificacaoReestruturacaoPlanoAEECommand(long reestruturacaoId, Usuario usuario)
        {
            ReestruturacaoId = reestruturacaoId;
            Usuario = usuario;
        }

        public long ReestruturacaoId { get; }
        public Usuario Usuario { get; }
    }
}
