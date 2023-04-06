using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class InserirVariosCompensacaoAusenciaRegenciaCommand : IRequest<bool>
    {
        public InserirVariosCompensacaoAusenciaRegenciaCommand(IEnumerable<CompensacaoAusenciaDisciplinaRegencia> compensacaoAusenciaDisciplinaRegencias, Usuario usuarioLogado)
        {
            CompensacaoAusenciaDisciplinaRegencias = compensacaoAusenciaDisciplinaRegencias;
            UsuarioLogado = usuarioLogado;
        }

        public IEnumerable<CompensacaoAusenciaDisciplinaRegencia> CompensacaoAusenciaDisciplinaRegencias { get; }
        public Usuario UsuarioLogado { get; }

    }
}
