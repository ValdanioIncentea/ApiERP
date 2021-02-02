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
        private readonly EntidadeRepository entidadeRepository = new EntidadeRepository();

        [Route("api/Fornecedor/Integrar")]
        [HttpGet]
        public void IntegrarFornecedor()
        {
           //entidadeRepository.IntegrarFornecedor();
        }        
        
    }
}
