using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoUe
{
    public class PainelEducacionalAprovacaoUeQueryHandler
        : ConsultasBase, IRequestHandler<PainelEducacionalAprovacaoUeQuery, PainelEducacionalAprovacaoUeRetorno>
    {
        private readonly IRepositorioPainelEducacionalAprovacaoUe repositorioPainelEducacionalAprovacaoUe;

        public PainelEducacionalAprovacaoUeQueryHandler(
            IContextoAplicacao contextoAplicacao,
            IRepositorioPainelEducacionalAprovacaoUe repositorioPainelEducacionalAprovacaoUe) : base(contextoAplicacao)
        {
            this.repositorioPainelEducacionalAprovacaoUe = repositorioPainelEducacionalAprovacaoUe
                ?? throw new ArgumentNullException(nameof(repositorioPainelEducacionalAprovacaoUe));
        }

        public async Task<PainelEducacionalAprovacaoUeRetorno> Handle(
            PainelEducacionalAprovacaoUeQuery request,
            CancellationToken cancellationToken)
        {
            var resultado = await repositorioPainelEducacionalAprovacaoUe.ObterAprovacao(request.Filtro);
            var modalidades = resultado.Items
                .GroupBy(r => r.Modalidade)
                .Select(g => new PainelEducacionalAprovacaoUeDto
                {
                    Modalidade = g.Key,
                    Turmas = g.Select(t => new PainelEducacionalAprovacaoUeTurmaDto
                    {
                        Turma = t.Turma,
                        TotalPromocoes = t.TotalPromocoes,
                        TotalRetencoesAusencias = t.TotalRetencoesAusencias,
                        TotalRetencoesNotas = t.TotalRetencoesNotas
                    })
                    .OrderBy(t => t.Turma)
                    .ToList()
                })
                .OrderBy(m => m.Modalidade)
                .ToList();

            return new PainelEducacionalAprovacaoUeRetorno
            {
                Modalidades = modalidades,
                TotalPaginas = resultado.TotalPaginas,
                TotalRegistros = resultado.TotalRegistros
            };
        }

        public class PainelEducacionalAprovacaoUeQueryValidator : AbstractValidator<PainelEducacionalAprovacaoUeQuery>
        {
            public PainelEducacionalAprovacaoUeQueryValidator()
            {
                RuleFor(x => x.Filtro.AnoLetivo)
                    .NotEmpty().WithMessage("Informe o ano letivo");

                RuleFor(x => x.Filtro.CodigoUe)
                    .NotEmpty().WithMessage("Informe o código da Ue");

                RuleFor(x => x.Filtro.ModalidadeId)
                    .NotEmpty().WithMessage("Informe o código modalidade");
            }
        }
    }
}
