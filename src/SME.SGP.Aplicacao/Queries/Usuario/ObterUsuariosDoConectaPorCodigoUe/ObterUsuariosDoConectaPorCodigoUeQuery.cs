using MediatR;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.Usuario.ObterUsuariosDoConectaPorCodigoUe
{
    public class ObterUsuariosDoConectaPorCodigoUeQuery : IRequest<IEnumerable<DadosLoginUsuarioConectaDto>>
    {
        public string CodigoUe { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }

        public ObterUsuariosDoConectaPorCodigoUeQuery(string codigoUe, string login = null, string nome = null)
        {
            CodigoUe = codigoUe ?? throw new ArgumentNullException(nameof(codigoUe));
            Login = login;
            Nome = nome;
        }
    }
}
