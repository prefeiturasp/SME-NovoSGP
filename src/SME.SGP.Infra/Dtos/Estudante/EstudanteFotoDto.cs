using Microsoft.AspNetCore.Http;
using System;

namespace SME.SGP.Infra
{
    public class EstudanteFotoDto
    {
        public string AlunoCodigo { get; set; }
        public IFormFile File { get; set; }
    }
}