using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
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
                        QuantidadeEstudantes = r.QuantidadeEstudantes,
                        EstudantesPresentes = r.EstudantesPresentes,
                        PercentualFrequencia = r.PercentualFrequencia,
                        NivelFrequencia = ObterNomeNivelFrequencia(int.Parse(r.NivelFrequencia))
                    };
                })
                .OrderBy(r => r.Turma)
                .ToList(),

                TotalPaginas = resultado.TotalPaginas,
                TotalRegistros = resultado.TotalRegistros
            };
        }

        private string ObterNomeNivelFrequencia(int nivelFrequencia)
        {
            var nomeNivelFrequencia = "";
            if ((int)NivelFrequenciaEnum.Baixa == nivelFrequencia)
                return NivelFrequenciaEnum.Baixa.GetEnumDisplayName();

            if ((int)NivelFrequenciaEnum.Media == nivelFrequencia)
                return NivelFrequenciaEnum.Media.GetEnumDisplayName();

            if ((int)NivelFrequenciaEnum.Alta == nivelFrequencia)
                return NivelFrequenciaEnum.Alta.GetEnumDisplayName();

            return nomeNivelFrequencia;
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
