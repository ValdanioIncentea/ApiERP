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

    public class DadosMestreController : ApiController
    {
        private readonly DadosMestreRepository dadosMestreRepository = new DadosMestreRepository();

        
        [Route("api/DadosMestre/BuscarSeguimentosTerceiros")]
        [HttpGet]
        public List<DadosFornecedoViewModel> BuscarSeguimentosTerceiros()
        {
            return dadosMestreRepository.BuscarSeguimentosTerceiros();
        }
        
        [Route("api/DadosMestre/BuscarMoedas")]
        [HttpGet]
        public List<MoedaViewModel> BuscarMoedas()
        {
            return dadosMestreRepository.BuscarMoedas();
        }
        
        [Route("api/DadosMestre/BuscarCambioHistorico")]
        [HttpGet]
        public List<MoedasHistoricoViewModel> BuscarCambioHistorico()
        {
            return dadosMestreRepository.BuscarCambioHistorico();
        }
        
        [Route("api/DadosMestre/BuscarDocumentosBancos")]
        [HttpGet]
        public List<DadosFornecedoViewModel> BuscarDocumentosBancos()
        {
            return dadosMestreRepository.BuscarDocumentosBancos();
        } 
        
        [Route("api/DadosMestre/BuscarCondicaoDePagamentos")]
        [HttpGet]
        public List<DadosFornecedoViewModel> BuscarCondicaoDePagamentos()
        {
            return dadosMestreRepository.BuscarCondicaoDePagamentos();
        }
        
        [Route("api/DadosMestre/Armazens")]
        [HttpGet]
        public List<ArmazenViewModel> BuscarArmazens()
        {
            return dadosMestreRepository.BuscarArmazens();
        }
        
        [Route("api/DadosMestre/Documentos")]
        [HttpGet]
        public List<DocumentoViewModel> BuscarDocumentos()
        {
            return dadosMestreRepository.BuscarDocumentos();
        }  
               
        [Route("api/DadosMestre/MovimentosDosArtigos")]
        [HttpGet]
        public List<MovimentosViewModel> BuscarMovimentosDosArtigos()
        {
            return dadosMestreRepository.BuscarUltimoMovimentoPorArtigo();
        }  
        
        [Route("api/DadosMestre/Localizacoes")]
        [HttpGet]
        public List<LocalizacaoViewModel> BuscarLocalizacoes()
        {
            return dadosMestreRepository.BuscarLocalizacoes();
        } 
         
        [Route("api/DadosMestre/UnidadesConversao")]
        [HttpGet]
        public List<UnidadeConversaoViewModel> BuscarUnidadesConversao()
        {
            return dadosMestreRepository.BuscarUnidadesConversao();
        } 
        
        [Route("api/DadosMestre/Lotes")]
        [HttpGet]
        public List<LoteViewModel> BuscarLotes()
        {
            return dadosMestreRepository.BuscarLotes();
        } 
        
        [Route("api/DadosMestre/CentrosDeCusto")]
        [HttpGet]
        public List<CentrosDeCustosViewModel> BuscarCentrosDeCustos()
        {
            return dadosMestreRepository.BuscarCentrosDeCustos();
        }
        
        [Route("api/DadosMestre/Artigo")]
        [HttpGet]
        public List<ArtigoViewModel> BuscarTodosArtigos()
        {
            return dadosMestreRepository.BuscarArtigos();
        }

        [Route("api/DadosMestre/Moedas")]
        [HttpGet]
        public List<ArtigoMoedaViewModel> RetornarMoedasDosArtigos()
        {
            return dadosMestreRepository.BuscarArtigoMoeda();
        }

        [Route("api/DadosMestre/Familias")]
        [HttpGet]
        public List<FamiliaViewModel> RetornarFamilias()
        {
            return dadosMestreRepository.BuscarFamilias();
        }

        [Route("api/DadosMestre/Entidades")]
        [HttpGet]
        public List<EntidadesViewModel> RetornarEntidades()
        {
            return dadosMestreRepository.BusarEntidades();
        }

        [Route("api/DadosMestre/Departamentos")]
        [HttpGet]
        public List<DepartamentoViewModel> RetornarDepartamentos()
        {
            return dadosMestreRepository.BuscarDepartamentos();
        }

        [Route("api/DadosMestre/Obras")]
        [HttpGet]
        public List<ObrasViewModel> RetornarObras()
        {
            return dadosMestreRepository.BuscarObras();
        }

        [Route("api/DadosMestre/ReferenciasIntegradas")]
        [HttpGet]
        public List<DocumentosIntegradoViewModel> RetornarReferenciasIntegradas()
        {
            return dadosMestreRepository.BuscarReferenciasDeDocumentosIntegrados();
        }

        [Route("api/DadosMestre/AlterarEstado")]
        [HttpPost]
        public void AlterarEstadoDoDocumento(DocumentoAprovadosViewModel documentoAprovados)
        {
              dadosMestreRepository.AlterarEstadoDocumentoInterno(documentoAprovados);
        } 
        
        [Route("api/DadosMestre/EliminarReferencias")]
        [HttpPost]
        public void EliminarReferencias(DocumentoAprovadosViewModel ReferenciasDeDocumentos)
        {
             dadosMestreRepository.EliminarReferenciasDeDocumentosIntegrados(ReferenciasDeDocumentos);
        }

    }
}
