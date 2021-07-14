using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class AtribuirPerfilCommand : IRequest
    {
        public AtribuirPerfilCommand(string codigoRf, Guid perfil)
        {
            CodigoRf = codigoRf;
            Perfil = perfil;
        }

        public string CodigoRf { get; set; }
        public Guid Perfil { get; set; }
    }
}
