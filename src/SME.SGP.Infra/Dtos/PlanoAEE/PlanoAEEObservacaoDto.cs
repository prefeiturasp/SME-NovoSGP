using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PlanoAEEObservacaoDto {
        public PlanoAEEObservacaoDto(long id, string observacao)
        {
            Id = id;
            Observacao = observacao;
            Usuarios = new List<UsuarioNomeDto>();
        }

        public long Id { get; set; }
        public string Observacao { get; set; }
        public List<UsuarioNomeDto> Usuarios { get; set; }
        public AuditoriaDto Auditoria { get; set; }

        public void AdicionaUsuario(UsuarioNomeDto usuario)
        {
            if (usuario != null)
                Usuarios.Add(usuario);
        }
    }


}
