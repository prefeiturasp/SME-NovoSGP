using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ConsultasNotificacao : IConsultasNotificacao
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public ConsultasNotificacao(IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioNotificacao));
        }

        public IEnumerable<NotificacaoBasicaDto> Listar(NotificacaoFiltroDto filtroNotificacaoDto)
        {
            var retorno = repositorioNotificacao.ObterPorDreOuEscolaOuStatusOuTurmoOuUsuarioOuTipo(filtroNotificacaoDto.DreId,
                filtroNotificacaoDto.EscolaId, (int)filtroNotificacaoDto.Status, filtroNotificacaoDto.TurmaId, filtroNotificacaoDto.UsuarioId, 
                (int)filtroNotificacaoDto.Tipo, (int)filtroNotificacaoDto.Categoria);

            return from r in retorno
                   select new NotificacaoBasicaDto()
                   {
                       NotificacaoId = r.Id,
                       Titulo = r.Titulo,
                       Data = r.CriadoEm.ToString(),
                       Status = r.Status.ToString(),
                       Tipo = r.Tipo.ToString()
                   };
        }
    }
}