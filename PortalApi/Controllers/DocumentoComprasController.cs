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
using System.Text;
using System.Web;
using System.Web.Http;
//using System.Text.Json;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;

namespace PortalApi.Controllers
{
    public class DocumentoComprasController : ApiController
    {

        private readonly DocumentosComprasRepository documentosComprasRepository = new DocumentosComprasRepository();
        private ErpBS BSO;
        public static HttpClient clienteHttp; 
        private readonly EntidadeRepository entidadeRepository = new EntidadeRepository();

        public DocumentoComprasController()
        {

            BSO = Singleton.AbrirEmpresaERPV10; 

        }

        [Route("api/Fornecedor/Integrar")]
        [HttpPost]
        public void IntegrarFornecedor(List<FornecedorViewModel> Fornecedores)
        {
            entidadeRepository.IntegrarFornecedor(BSO,Fornecedores);
        }

        [Route("api/erp/ExamOpenCompany")]
        [HttpGet]
        public string ExamOpenCompany()
        {

            try
            {
                //var test = Singleton.AbrirEmpresaERPV10;
                if(BSO != null)
                {
                    return "Empresa Aberta";
                }
                return "Empresa não aberta 1";
            }
            catch (Exception e)
            {
                return "Empresa não aberta 2: "+e.Message;
            }         

        }
        
        [Route("api/DocumentoInterno/Importar")]
        [HttpPost]
        public HttpResponseMessage IntegrarDocumentoInternosPeloPortal(List<DocumentoDeCompras> documentoDeCompras)
        {

            try
            {
                documentosComprasRepository.CriaCDUEmFaltaNasTabelas();
                documentosComprasRepository.CriaTDUparaIntegracaoEmFalta();
                return Request.CreateResponse(HttpStatusCode.OK, documentosComprasRepository.CriarDocumentoInterno(BSO, documentoDeCompras));
            }
            catch (Exception e)
            {

                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
            
        }

        [Route("api/DocumentoDeCompras/Importar")]
        [HttpPost]
        public HttpResponseMessage IntegrarDocumentoDeDocumentoPeloPortal(List<DocumentoDeCompras> documentoDeCompras)
        {
            try
            {
                documentosComprasRepository.CriaCDUEmFaltaNasTabelas();
                documentosComprasRepository.CriaTDUparaIntegracaoEmFalta();
                return Request.CreateResponse(HttpStatusCode.OK, documentosComprasRepository.CriarDocumentoDeCompra(BSO, documentoDeCompras));
            }
            catch (Exception e)
            {

                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
        }          
    }
}
