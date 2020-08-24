using ErpBS100;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.App_Start
{
    public class MotorERP
    {

        public static ErpBS AbrirEmpresaERPV10()
        {
            ErpBS bso = new ErpBS();
            bso.AbreEmpresaTrabalho(StdBE100.StdBETipos.EnumTipoPlataforma.tpEmpresarial, "POWERBI", "incentea", "criar+valor", null, "DEFAULT");
            return bso;
        }
    }
}