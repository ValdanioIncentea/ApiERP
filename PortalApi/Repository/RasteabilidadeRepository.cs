﻿using PortalApi.App_Start;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PortalApi.Repository
{
    public class RasteabilidadeRepository
    {

        HelperRepository _helperRepository = new HelperRepository();

        public string LinhaOrigemDoDocumentoInternoDeOrigem(string NumeroDeProcesso, string Artigo, string TipoDocumentos)
        {
            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    string queryCabe = $"SELECT l.Id FROM LinhasInternos l inner join CabecInternos c on l.IdCabecInternos = c.Id where c.TipoDoc IN {TipoDocumentos} AND l.Artigo = '{Artigo}' AND c.CDU_PROCESSO = '{NumeroDeProcesso}'";

                    DataTable IDdoDocumentos = new DataTable();

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    _helperRepository.CriarLog("Rastreabilidade", $" QUERY:  " + queryCabe, "Rastreabilidade");

                    reader.Fill(IDdoDocumentos);

                    if (IDdoDocumentos.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in IDdoDocumentos.Rows)
                        {
                            return Linha["Id"].ToString();
                        }

                    }

                    return null;

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: LinhaOrigemDoDocumentoInternoDeOrigem" + ex.Message.ToString(), "Erro");
                    throw;
                }

            }

        }

        public string BuscarOrigemDoDocumetoDeCompra(string NumeroDeProcesso, string TipoDeDocumento, string Entidade)
        {

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    string queryCabe = $"select Id from CabecCompras where Entidade = '{Entidade}' AND TipoDoc = '{TipoDeDocumento}' and CDU_Processo = '{NumeroDeProcesso}'";

                    DataTable IDdoDocumentos = new DataTable();

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(IDdoDocumentos);

                    if (IDdoDocumentos.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in IDdoDocumentos.Rows)
                        {
                            return Linha["Id"].ToString();
                        }

                    }

                    return null;

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: BuscarOrigemDoDocumetoDeCompra" + ex.Message.ToString(), "Erro");
                    throw;
                }
            }
        }

        public Dictionary<string, string> PesquisarDoDocumetoPorId(string Id)
        {

            var CabecalhoDoDocumento = new Dictionary<string, string>();

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    string queryCabe = $"select Serie,Filial,NumDoc from cabecCompras where Id = '{Id}'";

                    DataTable DocumentoDeCompra = new DataTable();

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(DocumentoDeCompra);

                    if (DocumentoDeCompra.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in DocumentoDeCompra.Rows)
                        {
                            CabecalhoDoDocumento.Add("Serie", Linha["Serie"].ToString());
                            CabecalhoDoDocumento.Add("Filial", Linha["Filial"].ToString());
                            CabecalhoDoDocumento.Add("NumDoc", Linha["NumDoc"].ToString());
                        }

                    }
                    else
                    {

                        queryCabe = $"select Serie,Filial,NumDoc from CabecInternos where Id = '{Id}'";

                        DataTable DocumentoInterno = new DataTable();

                        SqlDataAdapter readerDocInterno = new SqlDataAdapter(queryCabe, conexao);

                        readerDocInterno.Fill(DocumentoInterno);

                        if (DocumentoInterno.Rows.Count > 0)
                        {

                            foreach (DataRow Linha in DocumentoInterno.Rows)
                            {
                                CabecalhoDoDocumento.Add("Serie", Linha["Serie"].ToString());
                                CabecalhoDoDocumento.Add("Filial", Linha["Filial"].ToString());
                                CabecalhoDoDocumento.Add("NumDoc", Linha["NumDoc"].ToString());
                            }

                        }

                    }


                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: BuscarOrigemDoDocumetoDeCompra" + ex.Message.ToString(), "Erro");
                    throw;
                }
            }

            return CabecalhoDoDocumento;

        }

        public DataTable PesquisarLinhaDoDocumetoNoDocumentoDeOrgiem(string NumeroDeProcesso, string TipoDoc, string Artigo)
        {

            DataTable LinhaDoDocumentoDeCompra = new DataTable();

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    string queryCabe = $"select Serie,Filial,NumDoc,NumLinha from cabecCompras c inner join LinhasCompras l on l.IdCabecCompras = c.Id where c.TipoDoc = '{TipoDoc}' and l.CDU_PROCESSO = '{NumeroDeProcesso}' and l.Artigo = '{Artigo}'";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(LinhaDoDocumentoDeCompra);

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: PesquisarLinhaDoDocumetoNoDocumentoDeOrgiem" + ex.Message.ToString(), "Erro");
                    throw;
                }

            }

            return LinhaDoDocumentoDeCompra;

        }

        public DataTable BuscarLinhasDoDocumetoDeCompra(string IdDocumentoCompras)
        {

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    string queryCabe = $"select Artigo,NumLinha from LinhasCompras where IdCabecCompras = '{IdDocumentoCompras}'";

                    DataTable LinhasDoDocumento = new DataTable();

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(LinhasDoDocumento);

                    return LinhasDoDocumento;

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: BuscarOrigemDoDocumetoDeCompra" + ex.Message.ToString(), "Erro");
                    throw;
                }

            }
        }
    }
}