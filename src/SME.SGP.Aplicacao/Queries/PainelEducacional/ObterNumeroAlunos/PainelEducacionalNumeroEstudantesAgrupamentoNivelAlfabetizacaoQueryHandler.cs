using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNumeroAlunos
{
    public class PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQueryHandler : IRequestHandler<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery, IEnumerable<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto>>
    {
        private readonly IRepositorioConsolidacaoAlfabetizacaoNivelEscrita repositorioConsolidacaoAlfabetizacaoNivelEscrita;

        public PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQueryHandler(IRepositorioConsolidacaoAlfabetizacaoNivelEscrita repositorioConsolidacaoAlfabetizacaoNivelEscrita)
        {
            this.repositorioConsolidacaoAlfabetizacaoNivelEscrita = repositorioConsolidacaoAlfabetizacaoNivelEscrita;
        }

        public async Task<IEnumerable<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto>> Handle(PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorioConsolidacaoAlfabetizacaoNivelEscrita.ObterNumeroAlunos(request.AnoLetivo, request.Periodo, request.CodigoDre, request.CodigoUe);

            return MapearParaDto(registros, request);
        }

        private IEnumerable<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto> MapearParaDto(IEnumerable<ContagemNivelEscritaDto> registros, PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery request)
        {
            var numeroAlunos = new List<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto>();
            foreach (var item in registros)
            {
                var nivelAlfabetizacao = item.NivelEscrita.Trim().ToUpper() switch
                {
                    "PS" => NivelAlfabetizacao.PreSilabico,
                    "SSV" => NivelAlfabetizacao.SilabicoSemValor,
                    "SCV" => NivelAlfabetizacao.SilabicoComValor,
                    "SA" => NivelAlfabetizacao.SilabicoAlfabetico,
                    "A" => NivelAlfabetizacao.Alfabetico
                };
                var nivelAlfabetizacaoDescricao = nivelAlfabetizacao.Description();
                numeroAlunos.Add(new PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto()
                {
                    NivelAlfabetizacao = nivelAlfabetizacao,
                    NivelAlfabetizacaoDescricao = nivelAlfabetizacaoDescricao,
                    Dre = request.CodigoDre,
                    Ue = request.CodigoUe,
                    Ano = request.AnoLetivo,
                    TotalAlunos = item.Quantidade,
                    Periodo = request.Periodo
                });
            }

            return numeroAlunos;
        }
    }
}
