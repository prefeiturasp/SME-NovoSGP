using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizaCacheDeAtividadeAvaliativaPorTurmaCommand : IRequest<IEnumerable<NotaConceito>>
    {
        public AtualizaCacheDeAtividadeAvaliativaPorTurmaCommand(string codigoTurma,
                                                      IEnumerable<NotaConceito> entidadesSalvar,
                                                      IEnumerable<NotaConceito> entidadesAlterar,
                                                      IEnumerable<NotaConceito> entidadesExcluir)
        {
            EntidadesSalvar = entidadesSalvar;
            EntidadesAlterar = entidadesAlterar;
            EntidadesExcluir = entidadesExcluir;
            CodigoTurma = codigoTurma;
        }

        public string CodigoTurma { get; }

        public IEnumerable<NotaConceito> EntidadesSalvar { get; set; }
        public IEnumerable<NotaConceito> EntidadesAlterar { get; set; }
        public IEnumerable<NotaConceito> EntidadesExcluir { get; set; }
    }
}
