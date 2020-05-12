using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class PodeEditarRelatorioQuery : IRequest<bool>
    {
        public PodeEditarRelatorioQuery(Turma turma, int bimestreAtual)
        {
            if (turma == null)
                throw new NegocioException("É necessário informar a turma");
            if (bimestreAtual == 0)
                throw new NegocioException("É necessário informar o bimestre atual");

            Turma = turma;
        }

        public Turma Turma { get; set; }
        public int BimestreAtual { get; set; }
    }
}
