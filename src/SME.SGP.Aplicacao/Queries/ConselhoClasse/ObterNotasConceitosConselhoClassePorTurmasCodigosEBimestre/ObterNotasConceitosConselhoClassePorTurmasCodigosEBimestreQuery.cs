using System;
using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class
        ObterNotasConceitosConselhoClassePorTurmasCodigosEBimestreQuery : IRequest<
            IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        public ObterNotasConceitosConselhoClassePorTurmasCodigosEBimestreQuery(string[] turmasCodigos, int bimestre,
            DateTime? dataMatricula = null, DateTime? dataSituacao = null, long? tipoCalendario = null, string alunoCodigo = null)
        {
            TurmasCodigos = turmasCodigos;
            Bimestre = bimestre;
            DataMatricula = dataMatricula;
            DataSituacao = dataSituacao;
            TipoCalendario = tipoCalendario;
            AlunoCodigo = alunoCodigo;
        }

        public string[] TurmasCodigos { get; }
        public int Bimestre { get; }
        public DateTime? DataMatricula { get; }
        public DateTime? DataSituacao { get; }
        public long? TipoCalendario { get; set; }
        public string AlunoCodigo { get; set; }
    }
}