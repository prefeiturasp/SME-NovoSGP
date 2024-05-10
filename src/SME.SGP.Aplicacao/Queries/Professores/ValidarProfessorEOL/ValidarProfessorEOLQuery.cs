using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ValidarProfessorEOLQuery : IRequest<bool>
    {
        public ValidarProfessorEOLQuery(string professorRf)
        {
            ProfessorRf = professorRf;
        }

        public string ProfessorRf { get; set; }
    }
}
