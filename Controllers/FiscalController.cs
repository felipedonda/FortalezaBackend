using FortalezaServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace FortalezaServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FiscalController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public FiscalController(fortalezaitdbContext context)
        {
            _context = context;
        }

        private XmlDocument GerarCfe(Venda venda, InformacoesEmpresa informacoesEmpresa)
        {
            XmlDocument Cfe = new XmlDocument();
            XmlElement root = Cfe.CreateElement("CFe");
            XmlElement infCFe = Cfe.CreateElement("infCFe");
            infCFe.SetAttribute("versaoDadosEnt", "");
            
                XmlElement ide = Cfe.CreateElement("ide");

                    XmlElement CNPJSoftwareHouse = Cfe.CreateElement("CNPJ");
                    CNPJSoftwareHouse.InnerText = "00000000000000"; //TODO
                    ide.AppendChild(CNPJSoftwareHouse);

                    XmlElement signAC = Cfe.CreateElement("signAC");
                    signAC.InnerText = "";
                    for(int i = 0; i < 344; i++ ) { signAC.InnerText += "0"; } //TODO
                    ide.AppendChild(signAC);
                
                    XmlElement numeroCaixa = Cfe.CreateElement("numeroCaixa");
                    numeroCaixa.InnerText = venda.IdcaixaNavigation.IdnomeCaixaNavigation.Nome.ToString("000");
                    ide.AppendChild(numeroCaixa);

                    infCFe.AppendChild(ide);

                XmlElement emit = Cfe.CreateElement("emit");

                    XmlElement CNPJ = Cfe.CreateElement("CNPJ");
                    CNPJ.InnerText = informacoesEmpresa.Cpf;
                    emit.AppendChild(CNPJ);

                    XmlElement IE = Cfe.CreateElement("IE");
                    IE.InnerText = informacoesEmpresa.InscricaoEstadual;
                    emit.AppendChild(IE);

                    /*  APLICÁVEL SOMENTE PARA NOTAS COM SERVIÇOS
                    XmlElement IM = Cfe.CreateElement("IM");
                    IM.InnerText = informacoesEmpresa.InscricaoMunicipal;
                    emit.AppendChild(IM);

                    XmlElement cRegTribISSQN = Cfe.CreateElement("cRegTribISSQN");
                    cRegTribISSQN.InnerText = informacoesEmpresa.RegimeTributario.ToString(); //TODO informacoesEmpresa.cRegTribISSQN
                    //1 - Microempresa Municipal; 2 - Estimativa; 3 - Sociedade de  Profissionais; 4 - Cooperativa; 5 - Microempresário Individual (MEI); 
                    emit.AppendChild(cRegTribISSQN);
                        */

                    XmlElement indRatISSQN = Cfe.CreateElement("indRatISSQN");
                    if(informacoesEmpresa.IndiceRateioIssqn == 1)
                    {
                        indRatISSQN.InnerText = "S";
                    }
                    else
                    {
                        indRatISSQN.InnerText = "N";
                    }
                    emit.AppendChild(indRatISSQN);

                    infCFe.AppendChild(emit);
                
                XmlElement dest = Cfe.CreateElement("dest");
                    if(venda.IdclienteNavigation != null)
                    {
                        if(venda.IdclienteNavigation.IsCpf)
                        {
                            XmlElement destCPF = Cfe.CreateElement("CPF");
                            destCPF.InnerText = venda.IdclienteNavigation.Cpf;
                            dest.AppendChild(destCPF);
                        }
                        else
                        {
                            XmlElement destCNPJ = Cfe.CreateElement("CNPJ");
                            destCNPJ.InnerText = venda.IdclienteNavigation.Cpf;
                            dest.AppendChild(destCNPJ);
                        }
                        XmlElement xNome = Cfe.CreateElement("xNome");
                        xNome.InnerText = venda.IdclienteNavigation.Nome;
                        dest.AppendChild(xNome);
                    }
                    infCFe.AppendChild(dest);
                foreach(var item in venda.ItemVenda)
                {
                    XmlElement det = Cfe.CreateElement("det");
                        det.SetAttribute("nItem",item.Indice.ToString());
                            XmlElement prod = Cfe.CreateElement("prod");
                                XmlElement cProd = Cfe.CreateElement("cProd");
                                cProd.InnerText = item.Iditem.ToString("0000");
                                prod.AppendChild(cProd);

                                if(!string.IsNullOrEmpty(item.IditemNavigation.CodigoBarras))
                                {
                                    XmlElement cEAN = Cfe.CreateElement("cEAN");
                                    cEAN.InnerText = item.IditemNavigation.CodigoBarras.ToString();
                                    prod.AppendChild(cEAN);
                                }

                                XmlElement xProd = Cfe.CreateElement("xProd");
                                xProd.InnerText = item.IditemNavigation.Descricao;
                                prod.AppendChild(xProd);
                                
                                if(item.IditemNavigation.Fiscal.Ncm != null)
                                {
                                    XmlElement NCM = Cfe.CreateElement("NCM");
                                    NCM.InnerText = (item.IditemNavigation.Fiscal.Ncm ?? default).ToString("00000000");
                                    prod.AppendChild(NCM);
                                }

                                if(item.IditemNavigation.Fiscal.Cest != null)
                                {
                                    XmlElement CEST = Cfe.CreateElement("CEST");
                                    CEST.InnerText = (item.IditemNavigation.Fiscal.Cest ?? default).ToString("00000000");
                                    prod.AppendChild(CEST);
                                }

                                XmlElement CFOP = Cfe.CreateElement("CFOP");
                                CFOP.InnerText = item.IditemNavigation.Fiscal.Cfop.ToString("0000");
                                prod.AppendChild(CFOP);

                                XmlElement uCom = Cfe.CreateElement("uCom");
                                uCom.InnerText = item.IditemNavigation.Unidade;
                                prod.AppendChild(uCom);

                                XmlElement qCom = Cfe.CreateElement("qCom");
                                qCom.InnerText = item.Quantidade.ToString("0.0000");
                                prod.AppendChild(qCom);

                                XmlElement vUnCom = Cfe.CreateElement("vUnCom");
                                vUnCom.InnerText = (item.Valor ?? default).ToString("0.00");
                                prod.AppendChild(vUnCom);

                                XmlElement indRegra = Cfe.CreateElement("indRegra");
                                indRegra.InnerText = "A";
                                prod.AppendChild(indRegra);
                                
                                if(item.Desconto > 0)
                                {
                                    XmlElement vDesc = Cfe.CreateElement("vDesc");
                                    vDesc.InnerText = (item.Desconto ?? default).ToString("0.00");
                                    prod.AppendChild(vDesc);
                                }

                                if (item.Acrescimo > 0)
                                {
                                    XmlElement vOutro = Cfe.CreateElement("vOutro");
                                    vOutro.InnerText = (item.Acrescimo ?? default).ToString("0.00");
                                    prod.AppendChild(vOutro);
                                }
                                
                                det.AppendChild(prod);

                            XmlElement imposto = Cfe.CreateElement("imposto");
                                
                                XmlElement ICMS = Cfe.CreateElement("ICMS");
                                    XmlElement Orig = Cfe.CreateElement("Orig");
                                    Orig.InnerText = item.IditemNavigation.Fiscal.Origem.ToString("0");

                                    XmlElement pICMS = Cfe.CreateElement("pICMS");
                                    pICMS.InnerText = item.IditemNavigation.Fiscal.AliquotaIcms.ToString("0.00");

                                    if(informacoesEmpresa.RegimeTributario == 0)
                                    {
                                        XmlElement CSOSN = Cfe.CreateElement("CSOSN");
                                        CSOSN.InnerText = (informacoesEmpresa.Csosn ?? default).ToString("000");

                                        if(informacoesEmpresa.Csosn == 900)
                                        {
                                            XmlElement ICMSSN900 = Cfe.CreateElement("ICMSSN900");
                                                ICMSSN900.AppendChild(Orig);
                                                ICMSSN900.AppendChild(CSOSN);


                                                ICMSSN900.AppendChild(pICMS);

                                                ICMS.AppendChild(ICMSSN900);
                                        }
                                        else
                                        {
                                            XmlElement ICMSSN102 = Cfe.CreateElement("ICMSSN102");
                                                ICMSSN102.AppendChild(Orig);
                                                ICMSSN102.AppendChild(CSOSN);

                                                ICMS.AppendChild(ICMSSN102);
                                        }
                                    }
                                    else
                                    {
                                        XmlElement CSTIcms = Cfe.CreateElement("CST");
                                        CSTIcms.InnerText = item.IditemNavigation.Fiscal.CstIcms.ToString("00");

                                        if(item.IditemNavigation.Fiscal.CstIcms >= 40 && item.IditemNavigation.Fiscal.CstIcms <= 60)
                                        {
                                            XmlElement ICMS40 = Cfe.CreateElement("ICMS40");
                                                ICMS40.AppendChild(Orig);
                                                ICMS40.AppendChild(CSTIcms);

                                                ICMS.AppendChild(ICMS40);
                                        }
                                        else
                                        {
                                            XmlElement ICMS00 = Cfe.CreateElement("ICMS00");
                                                ICMS00.AppendChild(Orig);
                                                ICMS00.AppendChild(CSTIcms);
                                                ICMS00.AppendChild(pICMS);

                                                ICMS.AppendChild(ICMS00);
                                        }
                                    }
                                    imposto.AppendChild(ICMS);

                                XmlElement PIS = Cfe.CreateElement("PIS");
                                    XmlElement CSTPis = Cfe.CreateElement("CST");
                                    CSTPis.InnerText = (informacoesEmpresa.CstPis ?? default).ToString("00");

                                    decimal aliqPis;
                                    if(informacoesEmpresa.RegimeTributario == 1)
                                    {
                                        aliqPis = 0.0065m;
                                    }
                                    else
                                    {
                                        aliqPis = 0.0165m;
                                    }

                                    XmlElement pPIS = Cfe.CreateElement("pPIS");
                                    pPIS.InnerText = aliqPis.ToString("0.0000");

                                    XmlElement vBC = Cfe.CreateElement("vBC");
                                    vBC.InnerText = item.ValorTotal.ToString("0.00");

                                    switch (informacoesEmpresa.CstPis)
                                    {
                                        case 1:
                                        case 2:
                                        case 5:
                                            XmlElement PISAliq = Cfe.CreateElement("PISAliq");
                                                PISAliq.AppendChild(CSTPis);
                                                PISAliq.AppendChild(vBC);
                                                PISAliq.AppendChild(pPIS);

                                                PIS.AppendChild(PISAliq);
                                            break;
                                        case 3:
                                            XmlElement PISQtde = Cfe.CreateElement("PISQtde");
                                                PISQtde.AppendChild(CSTPis);

                                                XmlElement qBCProd = Cfe.CreateElement("qBCProd");
                                                qBCProd.InnerText = item.Quantidade.ToString("00");
                                                PISQtde.AppendChild(qBCProd);

                                                XmlElement vAliqProd = Cfe.CreateElement("vAliqProd");
                                                vAliqProd.InnerText = ((aliqPis * item.Valor) ?? default).ToString("0.00");
                                                PISQtde.AppendChild(vAliqProd);

                                                PIS.AppendChild(PISQtde);
                                            break;

                                        case 49:
                                            XmlElement PISSN = Cfe.CreateElement("PISSN");
                                                PISSN.AppendChild(CSTPis);

                                                PIS.AppendChild(PISSN);
                                            break;

                                        case 99:
                                            XmlElement PISOutr = Cfe.CreateElement("PISOutr");
                                                PISOutr.AppendChild(CSTPis);
                                                PISOutr.AppendChild(vBC);
                                                PISOutr.AppendChild(pPIS);

                                                PIS.AppendChild(PISOutr);
                                            break;
                                        default:
                                            XmlElement PISNT = Cfe.CreateElement("PISNT");
                                                PISNT.AppendChild(CSTPis);

                                                PIS.AppendChild(PISNT);
                                            break;
                                    }

                                    imposto.AppendChild(PIS);

                                /*
                                XmlElement PISST = Cfe.CreateElement("PISST");
                                    PISST.AppendChild(vBC);
                                    PISST.AppendChild(pPIS);
                    
                                    imposto.AppendChild(PISST);
                                */

                                XmlElement COFINS = Cfe.CreateElement("COFINS");
                                    XmlElement CSTCofins = Cfe.CreateElement("CST");
                                    CSTCofins.InnerText = (informacoesEmpresa.CstPis ?? default).ToString("00");
                                    decimal aliqCofins;
                                    if(informacoesEmpresa.RegimeTributario == 1)
                                    {
                                        aliqCofins = 0.03m;
                                    }
                                    else
                                    {
                                        aliqCofins = 0.076m;
                                    }

                                    XmlElement pCOFINS = Cfe.CreateElement("pCOFINS");
                                    pCOFINS.InnerText = aliqCofins.ToString("0.0000");

                                    switch(informacoesEmpresa.CstPis)
                                    {
                                        case 1:
                                        case 2:
                                        case 5:
                                            XmlElement COFINSAliq = Cfe.CreateElement("COFINSAliq");
                                                COFINSAliq.AppendChild(CSTCofins);
                                                COFINSAliq.AppendChild(vBC);
                                                COFINSAliq.AppendChild(pCOFINS);
                                                
                                                COFINS.AppendChild(COFINSAliq);
                                            break;

                                        case 3:
                                            XmlElement COFINSQtde = Cfe.CreateElement("COFINSQtde");
                                                COFINSQtde.AppendChild(CSTCofins);

                                                XmlElement qBCProd = Cfe.CreateElement("qBCProd");
                                                qBCProd.InnerText = item.Quantidade.ToString("00");
                                                COFINSQtde.AppendChild(qBCProd);

                                                XmlElement vAliqProd = Cfe.CreateElement("vAliqProd"); 
                                                vAliqProd.InnerText = (aliqCofins * item.Valor ?? default).ToString("0.00");
                                                COFINSQtde.AppendChild(vAliqProd);

                                                COFINS.AppendChild(COFINSQtde);
                                            break;

                                        case 49:
                                            XmlElement COFINSSN = Cfe.CreateElement("COFINSSN");
                                                COFINSSN.AppendChild(CSTCofins);

                                                COFINS.AppendChild(COFINSSN);
                                            break;

                                        case 99:
                                            XmlElement COFINSOutr = Cfe.CreateElement("COFINSOutr");
                                                COFINSOutr.AppendChild(CSTCofins);
                                                COFINSOutr.AppendChild(vBC);
                                                COFINSOutr.AppendChild(pCOFINS);

                                                COFINS.AppendChild(COFINSOutr);
                                            break;
                                        default:
                                            XmlElement COFINSNT = Cfe.CreateElement("COFINSNT");
                                                COFINSNT.AppendChild(CSTCofins);

                                                COFINS.AppendChild(COFINSNT);
                                            break;
                                    }

                                    imposto.AppendChild(COFINS);
                                /*
                                XmlElement COFINSST = Cfe.CreateElement("COFINSST");
                                    vBC = Cfe.CreateElement("vBC");
                                    vBC.InnerText = item.ValorTotal.ToString("0.00");
                                    COFINSST.AppendChild(vBC);

                                    XmlElement pCOFINS = Cfe.CreateElement("pCOFINS"); //lucro presumido: COFINS 3%  //lucro real: COFINS 7,6%
                                    pCOFINS.InnerText = (item.IditemNavigation.Fiscal.AliquotaCofins ?? default).ToString("0.0000");
                                    COFINSST.AppendChild(pCOFINS);
                    
                                    imposto.AppendChild(COFINSST);
                                */

                                det.AppendChild(imposto);

                        infCFe.AppendChild(det);
                }
                

            XmlElement total = Cfe.CreateElement("total");
                    if(venda.Desconto > 0 || venda.Acrescimo > 0)
                    {
                        XmlElement DescAcrEntr = Cfe.CreateElement("DescAcrEntr");
                        if(venda.Desconto > 0)
                        {
                            XmlElement vDescSubtot = Cfe.CreateElement("vDescSubtot");
                            vDescSubtot.InnerText = venda.Desconto.ToString("0.00");
                            DescAcrEntr.AppendChild(vDescSubtot);
                        }
                        else
                        {
                            XmlElement vAcresSubtot = Cfe.CreateElement("vAcresSubtot");
                            vAcresSubtot.InnerText = venda.Acrescimo.ToString("0.00");
                            DescAcrEntr.AppendChild(vAcresSubtot);
                        }                            
                        total.AppendChild(DescAcrEntr);
                    }
                    
                infCFe.AppendChild(total);

            XmlElement pgto = Cfe.CreateElement("pgto");
                
;                foreach(var pagamento in venda.Pagamento)
                {
                    XmlElement MP = Cfe.CreateElement("MP");
                        XmlElement cMP = Cfe.CreateElement("cMP");
                        cMP.InnerText = (pagamento.IdmovimentoNavigation.IdformaPagamento ?? default).ToString("00");
                        MP.AppendChild(cMP);

                        XmlElement vMP = Cfe.CreateElement("vMP");
                        vMP.InnerText = pagamento.IdmovimentoNavigation.Valor.ToString("0.00");
                        MP.AppendChild(vMP);
                        
                        if(pagamento.Credenciadora != null)
                        {
                            XmlElement cAdmC = Cfe.CreateElement("cAdmC");
                            cAdmC.InnerText = (pagamento.Credenciadora ?? default).ToString("000");
                            MP.AppendChild(cAdmC);
                        }
                        pgto.AppendChild(MP);
                }

                infCFe.AppendChild(pgto);
            root.AppendChild(infCFe);
            Cfe.AppendChild(root);
            return Cfe;
        }

        // GET: api/Fiscal/cfe-test
        [HttpGet("cfe-test")]
        public ActionResult<XmlDocument> GetCfeTestAsync()
        {
            var venda = new Venda
            {
                IdcaixaNavigation = new Caixa { IdnomeCaixaNavigation = new NomeCaixa { Nome = 1 } },
                ItemVenda = new List<ItemVenda>
                {
                    new ItemVenda
                    {
                        Iditem = 1,
                        Indice = 1,
                        IditemNavigation = new Item
                        {
                            Descricao = "Produto teste 1",
                            Fiscal = new Fiscal
                            {
                                Cfop = 5001,
                                Origem = 0,
                                CstIcms = 0,
                                AliquotaIcms = 10
                            },
                            Unidade = "kg",
                        },
                        Quantidade = 1,
                        Valor = 10
                    }
                },
                Pagamento = new List<Pagamento>
                {
                    new Pagamento
                    {
                        IdmovimentoNavigation = new Movimento
                        {
                            IdformaPagamento = 1,
                            Valor = 10
                        }
                    }
                }
            };

            var informacoesEmpresa = new InformacoesEmpresa
            {
                Cpf = "08238299000129",
                InscricaoEstadual = "149392863111",
                IndiceRateioIssqn = 0,
                CstPis = 8
            };

            XmlDocument Cfe = GerarCfe(venda, informacoesEmpresa);
            return Cfe;
        }

        // GET: api/Fiscal/cfe-venda/5
        [HttpGet("cfe-venda/{id}")]
        public async Task<ActionResult<string>> GetCfeVendaAsync(int id)
        {
            var venda = await _context.Venda.Where(e => e.Idvenda == id)
                .Include(e => e.IdcaixaNavigation)
                    .ThenInclude(e => e.IdnomeCaixaNavigation)
                .Include(e => e.ItemVenda)
                    .ThenInclude(e => e.IditemNavigation)
                        .ThenInclude(e => e.Fiscal)
                .Include(e => e.Pagamento)
                    .ThenInclude(e => e.IdmovimentoNavigation)
                        .ThenInclude(e => e.IdformaPagamentoNavigation)
                .Include(e => e.IdclienteNavigation)
                    .ThenInclude(e => e.IdenderecoNavigation)
                .FirstOrDefaultAsync();

            if (venda != null)
            {
                //InformacoesEmpresa informacoesEmpresa = await _context.InformacoesEmpresa.Where(e => e.IdinformacoesEmpresa == 1 ).Include(e => e.IdenderecoNavigation).FirstOrDefaultAsync();
                InformacoesEmpresa informacoesEmpresa = await _context.InformacoesEmpresa
                    .Where(e => e.IdinformacoesEmpresa == 1)
                    .FirstOrDefaultAsync();
                XmlDocument Cfe = GerarCfe(venda, informacoesEmpresa);
                return Cfe.OuterXml;
            }
            else
            {
                return NotFound();
            }
            
        }

    }
}
