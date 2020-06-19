using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorTurmaLoginEPerfilQuery: IRequest<IEnumerable<ComponenteCurricularEol>>
    {
        public ObterComponentesCurricularesPorTurmaLoginEPerfilQuery(string turmaCodigo, string usuarioRf, Guid perfil)
        {
            TurmaCodigo = turmaCodigo;
            UsuarioRf = usuarioRf;
            Perfil = perfil;
        }

        public string TurmaCodigo { get; set; }
        public string UsuarioRf { get; set; }
        public Guid Perfil { get; set; }
    }
}
