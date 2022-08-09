using System;
using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao;

public class ObterNotasConceitosFechamentoPorTurmaIdEBimestreQuery : IRequest<IEnumerable<NotaConceitoComponenteBimestreAlunoDto>>
{
    public ObterNotasConceitosFechamentoPorTurmaIdEBimestreQuery(long[] turmasIds, int bimestre,
        DateTime? dataMatricula = null, DateTime? dataSituacao = null)
    {
        TurmasIds = turmasIds;
        Bimestre = bimestre;
        DataMatricula = dataMatricula;
        DataSituacao = dataSituacao;
    }

    public long[] TurmasIds { get; }
    public int Bimestre { get; }
    public DateTime? DataMatricula { get; }
    public DateTime? DataSituacao { get; }    
}