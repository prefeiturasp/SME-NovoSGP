using System;
using System.Collections.Generic;
using System.Text;

namespace SME.GoogleClassroom.Infra
{
    public abstract class PoliticaPolly
    {
        public static string SGP => "RetryPolicySGP";
        public static string PublicaFila => "RetryPolicyFilasRabbit";
    }
}
