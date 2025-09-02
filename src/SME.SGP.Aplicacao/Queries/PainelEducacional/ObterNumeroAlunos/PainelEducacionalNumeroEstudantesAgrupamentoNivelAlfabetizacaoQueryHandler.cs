using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNumeroAlunos
{
    public class PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQueryHandler : IRequestHandler<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery, IEnumerable<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto>>
    {
        private readonly IRepositorioPainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao repositorioPainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao;

        public PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQueryHandler(IRepositorioPainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao repositorioPainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao)
        {
            this.repositorioPainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao = repositorioPainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao;
        }

        public async Task<IEnumerable<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto>> Handle(PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorioPainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao.ObterNumeroAlunos(request.AnoLetivo, request.Periodo);

            return MapearParaDto(registros);
        }

        private IEnumerable<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto> MapearParaDto(IEnumerable<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao> registros)
        {
            var numeroAlunos = new List<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto>();
            foreach (var item in registros)
            {
                numeroAlunos.Add(new PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto()
                {
                    NivelAlfabetizacao = item.NivelAlfabetizacao,
                    NivelAlfabetizacaoDescricao = item.NivelAlfabetizacaoDescricao,
                    Ano = item.Ano,
                    TotalAlunos = item.TotalAlunos,
                    Periodo = item.Periodo
                });
            }

            return numeroAlunos;
        }
    }
}
