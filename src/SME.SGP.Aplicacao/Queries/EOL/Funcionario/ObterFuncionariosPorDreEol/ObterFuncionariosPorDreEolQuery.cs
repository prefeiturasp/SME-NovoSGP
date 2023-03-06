using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorDreEolQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public ObterFuncionariosPorDreEolQuery(Guid perfil, string dreCodigo, string ueCodigo,
            string rfCodigo, string nomeServidor)
        {
            Perfil = perfil;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            RfCodigo = rfCodigo;
            NomeServidor = nomeServidor;
        }

        public Guid Perfil { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public string RfCodigo { get; set; }
        public string NomeServidor { get; set; }
    }
}
