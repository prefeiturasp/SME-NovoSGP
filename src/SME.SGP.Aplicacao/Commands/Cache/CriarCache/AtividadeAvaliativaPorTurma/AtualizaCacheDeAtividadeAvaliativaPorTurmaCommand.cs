using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.Cache.CriarCache.AtividadeAvaliativaPorTurma
{
    public class AtualizaCacheDeAtividadeAvaliativaPorTurmaCommand : IRequest<IEnumerable<NotaConceito>>
    {
        public AtualizaCacheDeAtividadeAvaliativaPorTurmaCommand(string codigoTurma,
                                                      List<NotaConceito> entidadesSalvar,
                                                      List<NotaConceito> entidadesAlterar,
                                                      List<NotaConceito> entidadesExcluir)
        {
            EntidadesSalvar = entidadesSalvar;
            EntidadesAlterar = entidadesAlterar;
            EntidadesExcluir = entidadesExcluir;
            CodigoTurma = codigoTurma;
        }

        public string CodigoTurma { get; }

        public List<NotaConceito> EntidadesSalvar { get; set; }
        public List<NotaConceito> EntidadesAlterar { get; set; }
        public List<NotaConceito> EntidadesExcluir { get; set; }
    }
}
