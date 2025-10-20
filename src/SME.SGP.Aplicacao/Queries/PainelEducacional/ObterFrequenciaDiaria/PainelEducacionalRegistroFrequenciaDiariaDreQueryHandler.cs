using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaDiaria;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaDiaria
{
    public class PainelEducacionalRegistroFrequenciaDiariaDreQueryHandler : IRequestHandler<PainelEducacionalRegistroFrequenciaDiariaDreQuery, FrequenciaDiariaDreDto>
    {
        private readonly IRepositorioPainelEducacionalRegistroFrequenciaDiariaConsulta repositorio;

        public PainelEducacionalRegistroFrequenciaDiariaDreQueryHandler(IRepositorioPainelEducacionalRegistroFrequenciaDiariaConsulta repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<FrequenciaDiariaDreDto> Handle(PainelEducacionalRegistroFrequenciaDiariaDreQuery request, CancellationToken cancellationToken)
        {
            var resultado = await repositorio.ObterFrequenciaDiariaPorDre(request.Filtro);
            return new FrequenciaDiariaDreDto
            {
                Ues = resultado.Items.Select(r =>
                {
                    return new RegistroFrequenciaDiariaUeDto
                    {
                        Data = r.Data,
                        Ue = r.Ue,
                        QuantidadeAlunos = r.QuantidadeAlunos,
                        PercentualFrequencia = r.PercentualFrequencia,
                        NivelFrequencia = r.NivelFrequencia
                    };
                })
                .OrderBy(r => r.Ue)
                .ToList(),

                TotalPaginas = resultado.TotalPaginas,
                TotalRegistros = resultado.TotalRegistros
            };
        }

        public class PainelEducacionalRegistroFrequenciaDiariaDreQueryValidator : AbstractValidator<PainelEducacionalRegistroFrequenciaDiariaDreQuery>
        {
            public PainelEducacionalRegistroFrequenciaDiariaDreQueryValidator()
            {
                RuleFor(x => x.Filtro.AnoLetivo)
                    .NotEmpty().WithMessage("Informe o ano letivo");

                RuleFor(x => x.Filtro.CodigoDre)
                    .NotEmpty().WithMessage("Informe o código da Dre");

                RuleFor(x => x.Filtro.DataFrequencia)
                    .Must(data => string.IsNullOrEmpty(data) || DateTime.TryParse(data, out _))
                    .WithMessage("Informe uma data de frequência válida");
            }
        }
    }
}
