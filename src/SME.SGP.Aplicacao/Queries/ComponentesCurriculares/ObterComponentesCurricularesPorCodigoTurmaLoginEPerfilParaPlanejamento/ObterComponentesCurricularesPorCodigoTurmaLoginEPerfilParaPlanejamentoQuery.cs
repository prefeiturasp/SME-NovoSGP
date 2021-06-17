using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
   public class ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery : IRequest<IEnumerable<ComponenteCurricularEol>>
    {
        public ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery(string codigoTurma, string login, Guid perfil)
        {
            CodigoTurma = codigoTurma;
            Login = login;
            Perfil = perfil;
        }

        public string CodigoTurma { get; set; }

        public string Login { get; set; }

        public Guid Perfil { get; set; }
    }
}
