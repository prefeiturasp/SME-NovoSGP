using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class VerificarRemoverProcessoEmExecucaoCommand : IRequest<bool>
    {        
        public VerificarRemoverProcessoEmExecucaoCommand(string turmaId, string disciplinaId, int bimestre, TipoProcesso tipoProcesso)
        {
            TurmaId = turmaId;
            DisciplinaId = disciplinaId;
            Bimestre = bimestre;
            TipoProcesso = tipoProcesso;
        }

        public string TurmaId { get; set; }
        public string DisciplinaId { get; set; }
        public int Bimestre { get; set; }
        public TipoProcesso TipoProcesso { get; set; }
    }
}
