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
    public class PainelEducacionalRegistroFrequenciaDiariaUeQueryHandler : IRequestHandler<PainelEducacionalRegistroFrequenciaDiariaUeQuery, FrequenciaDiariaUeDto>
    {
        private readonly IRepositorioPainelEducacionalRegistroFrequenciaDiariaConsulta repositorio;

        public PainelEducacionalRegistroFrequenciaDiariaUeQueryHandler(IRepositorioPainelEducacionalRegistroFrequenciaDiariaConsulta repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<FrequenciaDiariaUeDto> Handle(PainelEducacionalRegistroFrequenciaDiariaUeQuery request, CancellationToken cancellationToken)
        {
            var resultado = await repositorio.ObterFrequenciaDiariaPorUe(request.Filtro);
            return new FrequenciaDiariaUeDto
            {
                Turmas = resultado.Items.Select(r =>
                {
                    return new RegistroFrequenciaDiariaTurmaDto
                    {
                        Data = r.Data,
                        Turma = r.Turma,
                        QuantidadeAlunos = r.QuantidadeAlunos,
                        PercentualFrequencia = r.PercentualFrequencia,
                        NivelFrequencia = r.NivelFrequencia
                    };
                })
                .OrderBy(r => r.Turma)
                .ToList(),

                TotalPaginas = resultado.TotalPaginas,
                TotalRegistros = resultado.TotalRegistros
            };
        }

        public class PainelEducacionalRegistroFrequenciaDiariaUeQueryValidator : AbstractValidator<PainelEducacionalRegistroFrequenciaDiariaUeQuery>
        {
            public PainelEducacionalRegistroFrequenciaDiariaUeQueryValidator()
            {
                RuleFor(x => x.Filtro.AnoLetivo)
                    .NotEmpty().WithMessage("Informe o ano letivo");

                RuleFor(x => x.Filtro.CodigoUe)
                    .NotEmpty().WithMessage("Informe o código da Ue");

                RuleFor(x => x.Filtro.DataFrequencia)
                    .Must(data => string.IsNullOrEmpty(data) || DateTime.TryParse(data, out _))
                    .WithMessage("Informe uma data de frequência válida");
            }
        }
    }
}
