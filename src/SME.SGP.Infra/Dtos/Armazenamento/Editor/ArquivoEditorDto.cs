using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ArquivoEditorDto
    {
        public ArquivoEditorDto() { }

        public string[] Files { get; set; }
        public string BaseUrl { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public string Path { get; set; }
        public string ContentType { get; set; }
    }
}
