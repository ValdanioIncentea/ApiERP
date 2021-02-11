using ErpBS100;
using PortalApi.App_Start;
using PortalApi.Models;
using PortalApi.Repository;
using PortalApi.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PortalApi.Controllers
{

    public class FornecedorController : ApiController
    {

        EntidadeRepository entidadeRepository = new EntidadeRepository();

        [Route("api/Fornecedor/BuscarEntidadesIntegradas")]
        [HttpGet]
        public List<DadosFornecedoViewModel> BuscarEntidadesIntegradas()
        {

            return entidadeRepository.BuscarEntidadesIntegradas();

        }

        [Route("api/Fornecedor/EliminarReferenciasDeEntidadesIntegradas")]
        [HttpPost]
        public void EliminarReferenciasDeEntidadesIntegradas(List<DadosFornecedoViewModel> Dados)
        {

            entidadeRepository.EliminarReferenciasDeEntidadesIntegradas(Dados);

        }

    }

}
