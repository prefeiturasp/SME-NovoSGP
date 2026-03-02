using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.InformacoesEducacionais;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterInformacoesEducacionais
{
    public class PainelEducacionalRegistroInformacoesEducacionaisQueryHandler : IRequestHandler<PainelEducacionalRegistroInformacoesEducacionaisQuery, InformacoesEducacionaisRetornoDto>
    {
        private readonly IRepositorioPainelEducacionalInformacoesEducacionaisConsulta repositorio;

        public PainelEducacionalRegistroInformacoesEducacionaisQueryHandler(IRepositorioPainelEducacionalInformacoesEducacionaisConsulta repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<InformacoesEducacionaisRetornoDto> Handle(PainelEducacionalRegistroInformacoesEducacionaisQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorio.ObterInformacoesEducacionais(request.Filtro);

            return new InformacoesEducacionaisRetornoDto 
            {
                Ues = registros.Items,
                TotalPaginas = registros.TotalPaginas,
                TotalRegistros = registros.TotalRegistros,
            };
        }

        public class PainelEducacionalRegistroInformacoesEducacionaisQueryValidator : AbstractValidator<PainelEducacionalRegistroInformacoesEducacionaisQuery>
        {
            public PainelEducacionalRegistroInformacoesEducacionaisQueryValidator()
            {
                RuleFor(x => x.Filtro.AnoLetivo)
                    .NotEmpty().WithMessage("Informe o ano letivo");

                RuleFor(x => x.Filtro.CodigoDre)
                    .NotEmpty().WithMessage("Informe o código da Dre");
            }
        }
    }
}
