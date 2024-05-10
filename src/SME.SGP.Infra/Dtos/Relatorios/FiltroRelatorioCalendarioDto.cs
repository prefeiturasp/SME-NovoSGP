using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioCalendarioDto
    {
        //informações do front
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public long TipoCalendarioId { get; set; }
        public bool EhSME { get; set; }

        //Informações do usuário logado
        public string UsuarioRF { get; internal set; }
        public Guid UsuarioPerfil { get; internal set; }
        public bool ConsideraPendenteAprovacao { get; internal set; }
        public bool PodeVisualizarEventosOcorrenciaDre { get; internal set; }

        public void SetarDadosUsuario(Usuario usuario)
        {
            UsuarioRF = usuario.CodigoRf;
            UsuarioPerfil = usuario.PerfilAtual;
            ConsideraPendenteAprovacao = usuario.TemPerfilSupervisorOuDiretor() || usuario.PodeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme(); 
            PodeVisualizarEventosOcorrenciaDre = usuario.PodeVisualizarEventosOcorrenciaDre();
        }

    }
}
