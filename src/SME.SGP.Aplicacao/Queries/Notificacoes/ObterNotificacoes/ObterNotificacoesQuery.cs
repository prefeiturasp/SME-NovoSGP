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

        public ObterNotificacoesQuery(string dreId, string ueId, int status, string turmaId, string usuarioRf, int tipo, int categoria, string titulo, long codigo, int anoLetivo, Paginacao paginacao)
        {
            DreId = dreId;
            UeId = ueId;
            Status = status;
            TurmaId = turmaId;
            UsuarioRf = usuarioRf;
            Tipo = tipo;
            Categoria = categoria;
            Titulo = titulo;
            Codigo = codigo;
            AnoLetivo = anoLetivo;
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