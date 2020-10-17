using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FortalezaServer.Models;

namespace FortalezaServer
{
    public class DbInitialData
    {

        public static bool InitiateDb()
        {
            using var _context = new fortalezaitdbContext();
            return _context.Database.EnsureCreated();
        }
        public static void InsertInitialData()
        {
            using (var _context = new fortalezaitdbContext())
            {
                _context.FormaPagamento.AddRange(
                    new FormaPagamento
                    {
                        IdformaPagamento = 1,
                        Ordem = 1,
                        Nome = "Dinheiro",
                        DebitarCliente = 0,
                        Debito = 0,
                        Bandeira = 0,
                        GerarContasReceber = 0,
                    },
                    new FormaPagamento
                    {
                        IdformaPagamento = 2,
                        Ordem = 2,
                        Nome = "Cartão de Débito",
                        DebitarCliente = 0,
                        Debito = 1,
                        Bandeira = 1,
                        GerarContasReceber = 0,
                    },
                    new FormaPagamento
                    {
                        IdformaPagamento = 3,
                        Ordem = 3,
                        Nome = "Cartão de Crédito",
                        DebitarCliente = 0,
                        Debito = 0,
                        Bandeira = 1,
                        GerarContasReceber = 0,
                    },
                    new FormaPagamento
                    {
                        IdformaPagamento = 4,
                        Ordem = 4,
                        Nome = "Vale Alimentação",
                        DebitarCliente = 0,
                        Debito = 1,
                        Bandeira = 1,
                        GerarContasReceber = 0,
                    },
                    new FormaPagamento
                    {
                        IdformaPagamento = 5,
                        Ordem = 5,
                        Nome = "Vale Refeição",
                        DebitarCliente = 0,
                        Debito = 1,
                        Bandeira = 1,
                        GerarContasReceber = 0,
                    },
                    new FormaPagamento
                    {
                        IdformaPagamento = 6,
                        Ordem = 6,
                        Nome = "Vale Presente",
                        DebitarCliente = 0,
                        Debito = 0,
                        Bandeira = 0,
                        GerarContasReceber = 0,
                    },
                    new FormaPagamento
                    {
                        IdformaPagamento = 7,
                        Ordem = 7,
                        Nome = "Cheque",
                        DebitarCliente = 0,
                        Debito = 0,
                        Bandeira = 0,
                        GerarContasReceber = 0,
                    },
                    new FormaPagamento
                    {
                        IdformaPagamento = 8,
                        Ordem = 8,
                        Nome = "Crédito Cliente",
                        DebitarCliente = 1,
                        Debito = 0,
                        Bandeira = 0,
                        GerarContasReceber = 0,
                    }
                    );
                _context.Bandeira.AddRange(
                    new Bandeira
                    {
                        Idbandeira = 1,
                        Ordem = 1,
                        Nome = "Mastercard"
                    },
                    new Bandeira
                    {
                        Idbandeira = 2,
                        Ordem = 2,
                        Nome = "Visa"
                    },
                    new Bandeira
                    {
                        Idbandeira = 3,
                        Ordem = 3,
                        Nome = "Alelo"
                    },
                    new Bandeira
                    {
                        Idbandeira = 4,
                        Ordem = 4,
                        Nome = "Sodexo"
                    },
                    new Bandeira
                    {
                        Idbandeira = 5,
                        Ordem = 5,
                        Nome = "Ticket"
                    },
                    new Bandeira
                    {
                        Idbandeira = 6,
                        Ordem = 6,
                        Nome = "VR"
                    }
                    );
            }
        }
    }
}
