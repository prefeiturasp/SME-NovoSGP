using System;
using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAusenciaParaCompensacaoQuery : IRequest<IEnumerable<RegistroFaltasNaoCompensadaDto>>
    {
        public ObterAusenciaParaCompensacaoQuery(long compensacaoId, string turmaCodigo, string disciplinaId, int bimestre, IEnumerable<AlunoQuantidadeCompensacaoDto> alunosQuantidadeCompensacoes)
        {
            CompensacaoId = compensacaoId;
            TurmaCodigo = turmaCodigo;
            DisciplinaId = disciplinaId;
            AlunosQuantidadeCompensacoes = alunosQuantidadeCompensacoes;
            Bimestre = bimestre;
        }

        public long CompensacaoId { get; }
        public string TurmaCodigo { get; }
        public string DisciplinaId { get; }
        public int Bimestre { get; }
        public IEnumerable<AlunoQuantidadeCompensacaoDto> AlunosQuantidadeCompensacoes  { get; }
    }
}