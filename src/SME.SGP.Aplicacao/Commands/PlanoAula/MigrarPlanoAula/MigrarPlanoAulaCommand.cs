using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class MigrarPlanoAulaCommand : IRequest<bool>
    {
        public MigrarPlanoAulaCommand(MigrarPlanoAulaDto planoAulaMigrar, Usuario usuario)
        {
            PlanoAulaMigrar = planoAulaMigrar;
            Usuario = usuario;
        }

        public Usuario Usuario { get; set; }
        public MigrarPlanoAulaDto PlanoAulaMigrar { get; set; }
    }
}
