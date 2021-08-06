using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class InserirAtribuicaoCJCommand : IRequest
    {
        public InserirAtribuicaoCJCommand(AtribuicaoCJ atribuicaoCJ, IEnumerable<ProfessorTitularDisciplinaEol> professoresTitulares, IEnumerable<AtribuicaoCJ> atribuicoesAtuais, Usuario usuario, bool ehHistorico)
        {
            AtribuicaoCJ = atribuicaoCJ;
            ProfessoresTitulares = professoresTitulares;
            AtribuicoesAtuais = atribuicoesAtuais;
            Usuario = usuario;
            EhHistorico = ehHistorico;
        }

        public AtribuicaoCJ AtribuicaoCJ { get; set; }
        public IEnumerable<ProfessorTitularDisciplinaEol> ProfessoresTitulares { get; set; }
        public IEnumerable<AtribuicaoCJ> AtribuicoesAtuais { get; set; }
        public Usuario Usuario { get; set; }
        public bool EhHistorico { get; }
    }
}
