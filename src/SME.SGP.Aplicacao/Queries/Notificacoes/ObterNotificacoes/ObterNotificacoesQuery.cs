using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacoesQuery : IRequest<PaginacaoResultadoDto<Notificacao>>
    {
        public string DreId { get; set; }
        public string UeId { get; set; }
        public int Status { get; set; }
        public string TurmaId { get; set; }
        public string UsuarioRf { get; set; }
        public int Filtro { get; set; }
        public int Categoria { get; set; }
        public string Titulo { get; set; }
        public long Codigo { get; set; }
        public int AnoLetivo { get; set; }
        public Paginacao Paginacao { get; set; }
        public int Tipo { get; set; }

        public ObterNotificacoesQuery(NotificacaoFiltroDto filtro, Paginacao paginacao)
        {
            DreId = filtro.DreId;
            UeId = filtro.UeId;
            Status = (int)filtro.Status;
            TurmaId = filtro.TurmaId;
            UsuarioRf = filtro.UsuarioRf;
            Tipo = (int)filtro.Tipo;
            Categoria = (int)filtro.Categoria;
            Titulo = filtro.Titulo;
            Codigo = filtro.Codigo;
            AnoLetivo = filtro.AnoLetivo;
            Paginacao = paginacao;
        }
    }

    public class ObterNotificacoesQueryValidator : AbstractValidator<ObterNotificacoesQuery>
    {
        public ObterNotificacoesQueryValidator()
        {

        }
    }
}