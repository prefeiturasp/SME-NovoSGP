using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using SME.SGP.Aplicacao.Integracoes.Respostas;

namespace SME.SGP.Aplicacao
{
   public class ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilPlanejamentoQuery : IRequest<IEnumerable<DisciplinaResposta>>
    {
        public ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilPlanejamentoQuery(string codigoTurma, string login, Guid perfil)
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
