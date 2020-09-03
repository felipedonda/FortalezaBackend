using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace FortalezaServer.Models
{
    public partial class fortalezaitdbContext : DbContext
    {
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public fortalezaitdbContext()
        {
        }

        public fortalezaitdbContext(DbContextOptions<fortalezaitdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Adicional> Adicional { get; set; }
        public virtual DbSet<AdicionalHasEstoque> AdicionalHasEstoque { get; set; }
        public virtual DbSet<AdicionalItemVenda> AdicionalItemVenda { get; set; }
        public virtual DbSet<Bandeira> Bandeira { get; set; }
        public virtual DbSet<Caixa> Caixa { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<ClienteHasEndereco> ClienteHasEndereco { get; set; }
        public virtual DbSet<Comanda> Comanda { get; set; }
        public virtual DbSet<ConfiguracoesGerais> ConfiguracoesGerais { get; set; }
        public virtual DbSet<Delivery> Delivery { get; set; }
        public virtual DbSet<Endereco> Endereco { get; set; }
        public virtual DbSet<Estoque> Estoque { get; set; }
        public virtual DbSet<EstoqueHasFornecedor> EstoqueHasFornecedor { get; set; }
        public virtual DbSet<EstoqueHasVenda> EstoqueHasVenda { get; set; }
        public virtual DbSet<Fiscal> Fiscal { get; set; }
        public virtual DbSet<FormaPagamento> FormaPagamento { get; set; }
        public virtual DbSet<Fornecedor> Fornecedor { get; set; }
        public virtual DbSet<Grupo> Grupo { get; set; }
        public virtual DbSet<InformacoesEmpresa> InformacoesEmpresa { get; set; }
        public virtual DbSet<InformacoesEmpresaHasEndereco> InformacoesEmpresaHasEndereco { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemHasEstoque> ItemHasEstoque { get; set; }
        public virtual DbSet<ItemHasGrupo> ItemHasGrupo { get; set; }
        public virtual DbSet<ItemVenda> ItemVenda { get; set; }
        public virtual DbSet<Movimento> Movimento { get; set; }
        public virtual DbSet<MovimentoHasBandeira> MovimentoHasBandeira { get; set; }
        public virtual DbSet<Pacote> Pacote { get; set; }
        public virtual DbSet<Pagamento> Pagamento { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioHasEndereco> UsuarioHasEndereco { get; set; }
        public virtual DbSet<Venda> Venda { get; set; }
        public virtual DbSet<VendaHasComanda> VendaHasComanda { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=172.17.0.2;port=3306;user=fortalezait_dbuser;password=fortalezait@;database=fortalezaitdb");
                optionsBuilder.UseLoggerFactory(MyLoggerFactory);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adicional>(entity =>
            {
                entity.HasKey(e => new { e.Idadicional, e.ItemIditem })
                    .HasName("PRIMARY");

                entity.ToTable("adicional");

                entity.HasIndex(e => e.ItemIditem)
                    .HasName("fk_adicional_item1_idx");

                entity.Property(e => e.Idadicional)
                    .HasColumnName("idadicional")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ItemIditem).HasColumnName("item_iditem");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("descricao")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Incluso).HasColumnName("incluso");

                entity.Property(e => e.Unidade)
                    .IsRequired()
                    .HasColumnName("unidade")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Valor)
                    .HasColumnName("valor")
                    .HasColumnType("decimal(11,2)");

                entity.HasOne(d => d.ItemIditemNavigation)
                    .WithMany(p => p.Adicional)
                    .HasForeignKey(d => d.ItemIditem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_adicional_item1");
            });

            modelBuilder.Entity<AdicionalHasEstoque>(entity =>
            {
                entity.HasKey(e => new { e.AdicionalIdadicional, e.AdicionalItemIditem, e.EstoqueIdestoque })
                    .HasName("PRIMARY");

                entity.ToTable("adicional_has_estoque");

                entity.HasIndex(e => e.EstoqueIdestoque)
                    .HasName("fk_adicional_has_estoque_estoque1_idx");

                entity.HasIndex(e => new { e.AdicionalIdadicional, e.AdicionalItemIditem })
                    .HasName("fk_adicional_has_estoque_adicional1_idx");

                entity.Property(e => e.AdicionalIdadicional).HasColumnName("adicional_idadicional");

                entity.Property(e => e.AdicionalItemIditem).HasColumnName("adicional_item_iditem");

                entity.Property(e => e.EstoqueIdestoque).HasColumnName("estoque_idestoque");

                entity.HasOne(d => d.EstoqueIdestoqueNavigation)
                    .WithMany(p => p.AdicionalHasEstoque)
                    .HasForeignKey(d => d.EstoqueIdestoque)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_adicional_has_estoque_estoque1");

                entity.HasOne(d => d.AdicionalI)
                    .WithMany(p => p.AdicionalHasEstoque)
                    .HasForeignKey(d => new { d.AdicionalIdadicional, d.AdicionalItemIditem })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_adicional_has_estoque_adicional1");
            });

            modelBuilder.Entity<AdicionalItemVenda>(entity =>
            {
                entity.HasKey(e => new { e.IdadicionalVenda, e.AdicionalIdadicional1, e.AdicionalItemIditem, e.ItemVendaIditemVenda, e.ItemVendaItemIditem, e.ItemVendaVendaIdvenda })
                    .HasName("PRIMARY");

                entity.ToTable("adicional_item_venda");

                entity.HasIndex(e => new { e.AdicionalIdadicional1, e.AdicionalItemIditem })
                    .HasName("fk_adicional_item_venda_adicional1_idx");

                entity.HasIndex(e => new { e.ItemVendaIditemVenda, e.ItemVendaItemIditem, e.ItemVendaVendaIdvenda })
                    .HasName("fk_adicional_item_venda_item_venda1_idx");

                entity.Property(e => e.IdadicionalVenda)
                    .HasColumnName("idadicional_venda")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AdicionalIdadicional1).HasColumnName("adicional_idadicional1");

                entity.Property(e => e.AdicionalItemIditem).HasColumnName("adicional_item_iditem");

                entity.Property(e => e.ItemVendaIditemVenda).HasColumnName("item_venda_iditem_venda");

                entity.Property(e => e.ItemVendaItemIditem).HasColumnName("item_venda_item_iditem");

                entity.Property(e => e.ItemVendaVendaIdvenda).HasColumnName("item_venda_venda_idvenda");

                entity.Property(e => e.AdicionalIdadicional).HasColumnName("adicional_idadicional");

                entity.Property(e => e.Custo)
                    .HasColumnName("custo")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.Quantidade)
                    .HasColumnName("quantidade")
                    .HasColumnType("decimal(12,3)");

                entity.Property(e => e.ValorVenda)
                    .HasColumnName("valor_venda")
                    .HasColumnType("decimal(11,2)");

                entity.HasOne(d => d.AdicionalI)
                    .WithMany(p => p.AdicionalItemVenda)
                    .HasForeignKey(d => new { d.AdicionalIdadicional1, d.AdicionalItemIditem })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_adicional_item_venda_adicional1");

                entity.HasOne(d => d.ItemVenda)
                    .WithMany(p => p.AdicionalItemVenda)
                    .HasForeignKey(d => new { d.ItemVendaIditemVenda, d.ItemVendaItemIditem, d.ItemVendaVendaIdvenda })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_adicional_item_venda_item_venda1");
            });

            modelBuilder.Entity<Bandeira>(entity =>
            {
                entity.HasKey(e => e.Idbandeira)
                    .HasName("PRIMARY");

                entity.ToTable("bandeira");

                entity.Property(e => e.Idbandeira).HasColumnName("idbandeira");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Ordem).HasColumnName("ordem");

                entity.Property(e => e.PrazoCredito).HasColumnName("prazo_credito");

                entity.Property(e => e.PrazoDebito).HasColumnName("prazo_debito");

                entity.Property(e => e.TaxaCredito)
                    .HasColumnName("taxa_credito")
                    .HasColumnType("decimal(5,2)");

                entity.Property(e => e.TaxaDebito)
                    .HasColumnName("taxa_debito")
                    .HasColumnType("decimal(5,2)");
            });

            modelBuilder.Entity<Caixa>(entity =>
            {
                entity.HasKey(e => e.Idcaixa)
                    .HasName("PRIMARY");

                entity.ToTable("caixa");

                entity.HasIndex(e => e.Idresponsavel)
                    .HasName("fk_caixa_usuario1_idx");

                entity.Property(e => e.Idcaixa).HasColumnName("idcaixa");

                entity.Property(e => e.Aberto).HasColumnName("aberto");

                entity.Property(e => e.HoraAbertura).HasColumnName("hora_abertura");

                entity.Property(e => e.HoraFechamento).HasColumnName("hora_fechamento");

                entity.Property(e => e.Idresponsavel).HasColumnName("idresponsavel");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SaldoAbertura)
                    .HasColumnName("saldo_abertura")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.SaldoFechamento)
                    .HasColumnName("saldo_fechamento")
                    .HasColumnType("decimal(11,2)");

                entity.HasOne(d => d.IdresponsavelNavigation)
                    .WithMany(p => p.Caixa)
                    .HasForeignKey(d => d.Idresponsavel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_caixa_usuario1");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Idcliente)
                    .HasName("PRIMARY");

                entity.ToTable("cliente");

                entity.Property(e => e.Idcliente).HasColumnName("idcliente");

                entity.Property(e => e.Celular)
                    .HasColumnName("celular")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Cpf).HasColumnName("cpf");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Telefone)
                    .HasColumnName("telefone")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ClienteHasEndereco>(entity =>
            {
                entity.HasKey(e => new { e.ClienteIdcliente, e.EnderecoIdendereco })
                    .HasName("PRIMARY");

                entity.ToTable("cliente_has_endereco");

                entity.HasIndex(e => e.ClienteIdcliente)
                    .HasName("fk_cliente_has_endereco_cliente1_idx");

                entity.HasIndex(e => e.EnderecoIdendereco)
                    .HasName("fk_cliente_has_endereco_endereco1_idx");

                entity.Property(e => e.ClienteIdcliente).HasColumnName("cliente_idcliente");

                entity.Property(e => e.EnderecoIdendereco).HasColumnName("endereco_idendereco");

                entity.HasOne(d => d.ClienteIdclienteNavigation)
                    .WithMany(p => p.ClienteHasEndereco)
                    .HasForeignKey(d => d.ClienteIdcliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cliente_has_endereco_cliente1");

                entity.HasOne(d => d.EnderecoIdenderecoNavigation)
                    .WithMany(p => p.ClienteHasEndereco)
                    .HasForeignKey(d => d.EnderecoIdendereco)
                    .HasConstraintName("fk_cliente_has_endereco_endereco1");
            });

            modelBuilder.Entity<Comanda>(entity =>
            {
                entity.HasKey(e => e.Idcomanda)
                    .HasName("PRIMARY");

                entity.ToTable("comanda");

                entity.Property(e => e.Idcomanda).HasColumnName("idcomanda");

                entity.Property(e => e.Codigo).HasColumnName("codigo");
            });

            modelBuilder.Entity<ConfiguracoesGerais>(entity =>
            {
                entity.HasKey(e => e.IdconfiguracoesGerais)
                    .HasName("PRIMARY");

                entity.ToTable("configuracoes_gerais");

                entity.Property(e => e.IdconfiguracoesGerais).HasColumnName("idconfiguracoes_gerais");

                entity.Property(e => e.CobrarMaiorvalorVariacoes).HasColumnName("cobrar_maiorvalor_variacoes");
            });

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.HasKey(e => e.Iddelivery)
                    .HasName("PRIMARY");

                entity.ToTable("delivery");

                entity.Property(e => e.Iddelivery).HasColumnName("iddelivery");

                entity.Property(e => e.Endereco)
                    .IsRequired()
                    .HasColumnName("endereco")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Entregue).HasColumnName("entregue");

                entity.Property(e => e.HoraSaiuEntrega).HasColumnName("hora_saiu_entrega");

                entity.Property(e => e.NomeCliente)
                    .IsRequired()
                    .HasColumnName("nome_cliente")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SaiuEntrega).HasColumnName("saiu_entrega");
            });

            modelBuilder.Entity<Endereco>(entity =>
            {
                entity.HasKey(e => e.Idendereco)
                    .HasName("PRIMARY");

                entity.ToTable("endereco");

                entity.Property(e => e.Idendereco).HasColumnName("idendereco");

                entity.Property(e => e.Bairro)
                    .HasColumnName("bairro")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Cep)
                    .HasColumnName("cep")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Complemento)
                    .HasColumnName("complemento")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Descricao)
                    .HasColumnName("descricao")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Logradouro)
                    .IsRequired()
                    .HasColumnName("logradouro")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Municipio)
                    .IsRequired()
                    .HasColumnName("municipio")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Numero)
                    .IsRequired()
                    .HasColumnName("numero")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Tipo)
                    .HasColumnName("tipo")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Uf)
                    .HasColumnName("UF")
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Estoque>(entity =>
            {
                entity.HasKey(e => e.Idestoque)
                    .HasName("PRIMARY");

                entity.ToTable("estoque");

                entity.Property(e => e.Idestoque).HasColumnName("idestoque");

                entity.Property(e => e.Custo)
                    .HasColumnName("custo")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.Disponivel).HasColumnName("disponivel");

                entity.Property(e => e.HoraEntrada).HasColumnName("hora_entrada");

                entity.Property(e => e.NotaFiscal)
                    .HasColumnName("nota_fiscal")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Observacao)
                    .HasColumnName("observacao")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OrigemVenda).HasColumnName("origem_venda");

                entity.Property(e => e.Quantidade)
                    .HasColumnName("quantidade")
                    .HasColumnType("decimal(12,3)");

                entity.Property(e => e.QuantidadeDisponivel)
                    .HasColumnName("quantidade_disponivel")
                    .HasColumnType("decimal(12,3)");

                entity.Property(e => e.Saida).HasColumnName("saida");
            });

            modelBuilder.Entity<EstoqueHasFornecedor>(entity =>
            {
                entity.HasKey(e => new { e.EstoqueIdestoque, e.FornecedorIdfornecedor })
                    .HasName("PRIMARY");

                entity.ToTable("estoque_has_fornecedor");

                entity.HasIndex(e => e.EstoqueIdestoque)
                    .HasName("fk_estoque_has_fornecedor_estoque1_idx");

                entity.HasIndex(e => e.FornecedorIdfornecedor)
                    .HasName("fk_estoque_has_fornecedor_fornecedor1_idx");

                entity.Property(e => e.EstoqueIdestoque).HasColumnName("estoque_idestoque");

                entity.Property(e => e.FornecedorIdfornecedor).HasColumnName("fornecedor_idfornecedor");

                entity.HasOne(d => d.EstoqueIdestoqueNavigation)
                    .WithMany(p => p.EstoqueHasFornecedor)
                    .HasForeignKey(d => d.EstoqueIdestoque)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_estoque_has_fornecedor_estoque1");

                entity.HasOne(d => d.FornecedorIdfornecedorNavigation)
                    .WithMany(p => p.EstoqueHasFornecedor)
                    .HasForeignKey(d => d.FornecedorIdfornecedor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_estoque_has_fornecedor_fornecedor1");
            });

            modelBuilder.Entity<EstoqueHasVenda>(entity =>
            {
                entity.HasKey(e => new { e.EstoqueIdestoque, e.VendaIdvenda })
                    .HasName("PRIMARY");

                entity.ToTable("estoque_has_venda");

                entity.HasIndex(e => e.EstoqueIdestoque)
                    .HasName("fk_estoque_has_venda_estoque1_idx");

                entity.HasIndex(e => e.VendaIdvenda)
                    .HasName("fk_estoque_has_venda_venda1_idx");

                entity.Property(e => e.EstoqueIdestoque).HasColumnName("estoque_idestoque");

                entity.Property(e => e.VendaIdvenda).HasColumnName("venda_idvenda");

                entity.HasOne(d => d.EstoqueIdestoqueNavigation)
                    .WithMany(p => p.EstoqueHasVenda)
                    .HasForeignKey(d => d.EstoqueIdestoque)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_estoque_has_venda_estoque1");

                entity.HasOne(d => d.VendaIdvendaNavigation)
                    .WithMany(p => p.EstoqueHasVenda)
                    .HasForeignKey(d => d.VendaIdvenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_estoque_has_venda_venda1");
            });

            modelBuilder.Entity<Fiscal>(entity =>
            {
                entity.HasKey(e => e.ItemIditem)
                    .HasName("PRIMARY");

                entity.ToTable("fiscal");

                entity.Property(e => e.ItemIditem).HasColumnName("item_iditem");

                entity.Property(e => e.Cest)
                    .HasColumnName("CEST")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Cfop)
                    .HasColumnName("CFOP")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Csosn)
                    .HasColumnName("CSOSN")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ImpostoEstadual)
                    .HasColumnName("imposto_estadual")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ImpostoFederal)
                    .HasColumnName("imposto_federal")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ImpostoMunicipal)
                    .HasColumnName("imposto_municipal")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Ncm)
                    .HasColumnName("NCM")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Origem)
                    .HasColumnName("origem")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.ItemIditemNavigation)
                    .WithOne(p => p.Fiscal)
                    .HasForeignKey<Fiscal>(d => d.ItemIditem)
                    .HasConstraintName("fk_fiscal_item1");
            });

            modelBuilder.Entity<FormaPagamento>(entity =>
            {
                entity.HasKey(e => e.IdformaPagamento)
                    .HasName("PRIMARY");

                entity.ToTable("forma_pagamento");

                entity.Property(e => e.IdformaPagamento).HasColumnName("idforma_pagamento");

                entity.Property(e => e.Bandeira).HasColumnName("bandeira");

                entity.Property(e => e.DebitarCliente).HasColumnName("debitar_cliente");

                entity.Property(e => e.Debito).HasColumnName("debito");

                entity.Property(e => e.GerarContasReceber).HasColumnName("gerar_contas_receber");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Ordem).HasColumnName("ordem");
            });

            modelBuilder.Entity<Fornecedor>(entity =>
            {
                entity.HasKey(e => e.Idfornecedor)
                    .HasName("PRIMARY");

                entity.ToTable("fornecedor");

                entity.Property(e => e.Idfornecedor).HasColumnName("idfornecedor");
            });

            modelBuilder.Entity<Grupo>(entity =>
            {
                entity.HasKey(e => e.Idgrupo)
                    .HasName("PRIMARY");

                entity.ToTable("grupo");

                entity.Property(e => e.Idgrupo).HasColumnName("idgrupo");

                entity.Property(e => e.Descricao)
                    .HasColumnName("descricao")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Visivel).HasColumnName("visivel");
            });

            modelBuilder.Entity<InformacoesEmpresa>(entity =>
            {
                entity.HasKey(e => e.IdinformacoesEmpresa)
                    .HasName("PRIMARY");

                entity.ToTable("informacoes_empresa");

                entity.Property(e => e.IdinformacoesEmpresa).HasColumnName("idinformacoes_empresa");

                entity.Property(e => e.Cnpj).HasColumnName("cnpj");

                entity.Property(e => e.NomeFantasia)
                    .IsRequired()
                    .HasColumnName("nome_fantasia")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RazaoSocial)
                    .HasColumnName("razao_social")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InformacoesEmpresaHasEndereco>(entity =>
            {
                entity.HasKey(e => new { e.InformacoesEmpresaIdinformacoesEmpresa, e.EnderecoIdendereco })
                    .HasName("PRIMARY");

                entity.ToTable("informacoes_empresa_has_endereco");

                entity.HasIndex(e => e.EnderecoIdendereco)
                    .HasName("fk_informacoes_empresa_has_endereco_endereco1_idx");

                entity.HasIndex(e => e.InformacoesEmpresaIdinformacoesEmpresa)
                    .HasName("fk_informacoes_empresa_has_endereco_informacoes_empresa1_idx");

                entity.Property(e => e.InformacoesEmpresaIdinformacoesEmpresa).HasColumnName("informacoes_empresa_idinformacoes_empresa");

                entity.Property(e => e.EnderecoIdendereco).HasColumnName("endereco_idendereco");

                entity.HasOne(d => d.EnderecoIdenderecoNavigation)
                    .WithMany(p => p.InformacoesEmpresaHasEndereco)
                    .HasForeignKey(d => d.EnderecoIdendereco)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_informacoes_empresa_has_endereco_endereco1");

                entity.HasOne(d => d.InformacoesEmpresaIdinformacoesEmpresaNavigation)
                    .WithMany(p => p.InformacoesEmpresaHasEndereco)
                    .HasForeignKey(d => d.InformacoesEmpresaIdinformacoesEmpresa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_informacoes_empresa_has_endereco_informacoes_empresa1");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Iditem)
                    .HasName("PRIMARY");

                entity.ToTable("item");

                entity.Property(e => e.Iditem).HasColumnName("iditem");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("descricao")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Disponivel).HasColumnName("disponivel");

                entity.Property(e => e.Estoque).HasColumnName("estoque");

                entity.Property(e => e.EstoqueMinimo)
                    .HasColumnName("estoque_minimo")
                    .HasColumnType("decimal(12,3)");

                entity.Property(e => e.Imagem)
                    .HasColumnName("imagem")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PermiteCombo).HasColumnName("permite_combo");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnName("tipo")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Unidade)
                    .IsRequired()
                    .HasColumnName("unidade")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.UnidadeInteira).HasColumnName("unidade_inteira");

                entity.Property(e => e.Valor)
                    .HasColumnName("valor")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.Visivel).HasColumnName("visivel");
            });

            modelBuilder.Entity<ItemHasEstoque>(entity =>
            {
                entity.HasKey(e => new { e.ItemIditem, e.EstoqueIdestoque })
                    .HasName("PRIMARY");

                entity.ToTable("item_has_estoque");

                entity.HasIndex(e => e.EstoqueIdestoque)
                    .HasName("fk_item_has_estoque_estoque1_idx");

                entity.HasIndex(e => e.ItemIditem)
                    .HasName("fk_item_has_estoque_item1_idx");

                entity.Property(e => e.ItemIditem).HasColumnName("item_iditem");

                entity.Property(e => e.EstoqueIdestoque).HasColumnName("estoque_idestoque");

                entity.HasOne(d => d.EstoqueNavigation)
                    .WithMany(p => p.ItemHasEstoque)
                    .HasForeignKey(d => d.EstoqueIdestoque)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_item_has_estoque_estoque1");

                entity.HasOne(d => d.ItemNavigation)
                    .WithMany(p => p.ItemHasEstoque)
                    .HasForeignKey(d => d.ItemIditem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_item_has_estoque_item1");
            });

            modelBuilder.Entity<ItemHasGrupo>(entity =>
            {
                entity.HasKey(e => new { e.ItemIditem, e.GrupoIdgrupo })
                    .HasName("PRIMARY");

                entity.ToTable("item_has_grupo");

                entity.HasIndex(e => e.GrupoIdgrupo)
                    .HasName("fk_item_has_grupo_grupo1_idx");

                entity.HasIndex(e => e.ItemIditem)
                    .HasName("fk_item_has_grupo_item1_idx");

                entity.Property(e => e.ItemIditem).HasColumnName("item_iditem");

                entity.Property(e => e.GrupoIdgrupo).HasColumnName("grupo_idgrupo");

                entity.HasOne(d => d.GrupoNavigation)
                    .WithMany(p => p.ItemHasGrupo)
                    .HasForeignKey(d => d.GrupoIdgrupo)
                    .HasConstraintName("fk_item_has_grupo_grupo1");

                entity.HasOne(d => d.ItemNavigation)
                    .WithMany(p => p.ItemHasGrupo)
                    .HasForeignKey(d => d.ItemIditem)
                    .HasConstraintName("fk_item_has_grupo_item1");
            });

            modelBuilder.Entity<ItemVenda>(entity =>
            {
                entity.HasKey(e => new { e.IditemVenda, e.ItemIditem, e.VendaIdvenda })
                    .HasName("PRIMARY");

                entity.ToTable("item_venda");

                entity.HasIndex(e => e.ItemIditem)
                    .HasName("fk_item_venda_item1_idx");

                entity.HasIndex(e => e.VendaIdvenda)
                    .HasName("fk_item_venda_venda1_idx");

                entity.Property(e => e.IditemVenda)
                    .HasColumnName("iditem_venda")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ItemIditem).HasColumnName("item_iditem");

                entity.Property(e => e.VendaIdvenda).HasColumnName("venda_idvenda");

                entity.Property(e => e.Cancelado).HasColumnName("cancelado");

                entity.Property(e => e.Custo)
                    .HasColumnName("custo")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.Motivo)
                    .HasColumnName("motivo")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Quantidade)
                    .HasColumnName("quantidade")
                    .HasColumnType("decimal(12,3)");

                entity.Property(e => e.Valor)
                    .HasColumnName("valor")
                    .HasColumnType("decimal(11,2)");

                entity.HasOne(d => d.ItemNavigation)
                    .WithMany(p => p.ItemVenda)
                    .HasForeignKey(d => d.ItemIditem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_item_venda_item1");

                entity.HasOne(d => d.VendaNavigation)
                    .WithMany(p => p.ItemVenda)
                    .HasForeignKey(d => d.VendaIdvenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_item_venda_venda1");
            });

            modelBuilder.Entity<Movimento>(entity =>
            {
                entity.HasKey(e => new { e.Idmovimento, e.FormaPagamentoIdformaPagamento, e.CaixaIdcaixa })
                    .HasName("PRIMARY");

                entity.ToTable("movimento");

                entity.HasIndex(e => e.CaixaIdcaixa)
                    .HasName("fk_movimento_caixa1_idx");

                entity.HasIndex(e => e.FormaPagamentoIdformaPagamento)
                    .HasName("fk_movimento_forma_pagamento1_idx");

                entity.Property(e => e.Idmovimento)
                    .HasColumnName("idmovimento")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FormaPagamentoIdformaPagamento).HasColumnName("forma_pagamento_idforma_pagamento");

                entity.Property(e => e.CaixaIdcaixa).HasColumnName("caixa_idcaixa");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("descricao")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.HoraEntrada).HasColumnName("hora_entrada");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnName("tipo")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Valor)
                    .HasColumnName("valor")
                    .HasColumnType("decimal(11,2)");

                entity.HasOne(d => d.CaixaIdcaixaNavigation)
                    .WithMany(p => p.Movimento)
                    .HasForeignKey(d => d.CaixaIdcaixa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_movimento_caixa1");

                entity.HasOne(d => d.FormaPagamentoNavigation)
                    .WithMany(p => p.Movimento)
                    .HasForeignKey(d => d.FormaPagamentoIdformaPagamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_movimento_forma_pagamento1");
            });

            modelBuilder.Entity<MovimentoHasBandeira>(entity =>
            {
                entity.HasKey(e => new { e.MovimentoIdmovimento, e.MovimentoFormaPagamentoIdformaPagamento, e.MovimentoCaixaIdcaixa, e.BandeiraIdbandeira })
                    .HasName("PRIMARY");

                entity.ToTable("movimento_has_bandeira");

                entity.HasIndex(e => e.BandeiraIdbandeira)
                    .HasName("fk_movimento_has_bandeira_bandeira1_idx");

                entity.HasIndex(e => new { e.MovimentoIdmovimento, e.MovimentoFormaPagamentoIdformaPagamento, e.MovimentoCaixaIdcaixa })
                    .HasName("fk_movimento_has_bandeira_movimento1_idx");

                entity.Property(e => e.MovimentoIdmovimento).HasColumnName("movimento_idmovimento");

                entity.Property(e => e.MovimentoFormaPagamentoIdformaPagamento).HasColumnName("movimento_forma_pagamento_idforma_pagamento");

                entity.Property(e => e.MovimentoCaixaIdcaixa).HasColumnName("movimento_caixa_idcaixa");

                entity.Property(e => e.BandeiraIdbandeira).HasColumnName("bandeira_idbandeira");

                entity.HasOne(d => d.BandeiraIdbandeiraNavigation)
                    .WithMany(p => p.MovimentoHasBandeira)
                    .HasForeignKey(d => d.BandeiraIdbandeira)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_movimento_has_bandeira_bandeira1");

                entity.HasOne(d => d.Movimento)
                    .WithMany(p => p.MovimentoHasBandeira)
                    .HasForeignKey(d => new { d.MovimentoIdmovimento, d.MovimentoFormaPagamentoIdformaPagamento, d.MovimentoCaixaIdcaixa })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_movimento_has_bandeira_movimento1");
            });

            modelBuilder.Entity<Pacote>(entity =>
            {
                entity.HasKey(e => e.ItemIditem)
                    .HasName("PRIMARY");

                entity.ToTable("pacote");

                entity.HasIndex(e => e.IditemProduto)
                    .HasName("fk_pacote_item2_idx");

                entity.Property(e => e.ItemIditem).HasColumnName("item_iditem");

                entity.Property(e => e.IditemProduto).HasColumnName("iditem_produto");

                entity.Property(e => e.Padrao).HasColumnName("padrao");

                entity.Property(e => e.Quantidade)
                    .HasColumnName("quantidade")
                    .HasColumnType("decimal(11,3)");

                entity.HasOne(d => d.ProdutoNavigation)
                    .WithMany(p => p.PacoteParentNavigation)
                    .HasForeignKey(d => d.IditemProduto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_pacote_item2");

                entity.HasOne(d => d.ItemParentNavigation)
                    .WithOne(p => p.PacoteNavigation)
                    .HasForeignKey<Pacote>(d => d.ItemIditem)
                    .HasConstraintName("fk_pacote_item1");
            });

            modelBuilder.Entity<Pagamento>(entity =>
            {
                entity.HasKey(e => new { e.VendaIdvenda, e.MovimentoIdmovimento, e.MovimentoFormaPagamentoIdformaPagamento, e.MovimentoCaixaIdcaixa })
                    .HasName("PRIMARY");

                entity.ToTable("pagamento");

                entity.HasIndex(e => e.VendaIdvenda)
                    .HasName("fk_venda_has_movimento_venda1_idx");

                entity.HasIndex(e => new { e.MovimentoIdmovimento, e.MovimentoFormaPagamentoIdformaPagamento, e.MovimentoCaixaIdcaixa })
                    .HasName("fk_venda_has_movimento_movimento1_idx");

                entity.Property(e => e.VendaIdvenda).HasColumnName("venda_idvenda");

                entity.Property(e => e.MovimentoIdmovimento).HasColumnName("movimento_idmovimento");

                entity.Property(e => e.MovimentoFormaPagamentoIdformaPagamento).HasColumnName("movimento_forma_pagamento_idforma_pagamento");

                entity.Property(e => e.MovimentoCaixaIdcaixa).HasColumnName("movimento_caixa_idcaixa");

                entity.HasOne(d => d.VendaIdvendaNavigation)
                    .WithMany(p => p.Pagamento)
                    .HasForeignKey(d => d.VendaIdvenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_venda_has_movimento_venda1");

                entity.HasOne(d => d.Movimento)
                    .WithMany(p => p.Pagamento)
                    .HasForeignKey(d => new { d.MovimentoIdmovimento, d.MovimentoFormaPagamentoIdformaPagamento, d.MovimentoCaixaIdcaixa })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_venda_has_movimento_movimento1");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Idusuario)
                    .HasName("PRIMARY");

                entity.ToTable("usuario");

                entity.Property(e => e.Idusuario).HasColumnName("idusuario");

                entity.Property(e => e.Cpf).HasColumnName("cpf");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Senha)
                    .IsRequired()
                    .HasColumnName("senha")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Telefone1)
                    .HasColumnName("telefone1")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Telefone2)
                    .HasColumnName("telefone2")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UsuarioHasEndereco>(entity =>
            {
                entity.HasKey(e => new { e.UsuarioIdusuario, e.EnderecoIdendereco })
                    .HasName("PRIMARY");

                entity.ToTable("usuario_has_endereco");

                entity.HasIndex(e => e.EnderecoIdendereco)
                    .HasName("fk_usuario_has_endereco_endereco1_idx");

                entity.HasIndex(e => e.UsuarioIdusuario)
                    .HasName("fk_usuario_has_endereco_usuario1_idx");

                entity.Property(e => e.UsuarioIdusuario).HasColumnName("usuario_idusuario");

                entity.Property(e => e.EnderecoIdendereco).HasColumnName("endereco_idendereco");

                entity.HasOne(d => d.EnderecoIdenderecoNavigation)
                    .WithMany(p => p.UsuarioHasEndereco)
                    .HasForeignKey(d => d.EnderecoIdendereco)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_usuario_has_endereco_endereco1");

                entity.HasOne(d => d.UsuarioIdusuarioNavigation)
                    .WithMany(p => p.UsuarioHasEndereco)
                    .HasForeignKey(d => d.UsuarioIdusuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_usuario_has_endereco_usuario1");
            });

            modelBuilder.Entity<Venda>(entity =>
            {
                entity.HasKey(e => e.Idvenda)
                    .HasName("PRIMARY");

                entity.ToTable("venda");

                entity.HasIndex(e => e.Idresponsavel)
                    .HasName("fk_venda_usuario1_idx");

                entity.Property(e => e.Idvenda).HasColumnName("idvenda");

                entity.Property(e => e.Aberta).HasColumnName("aberta");

                entity.Property(e => e.Alteracao)
                    .HasColumnName("alteracao")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.CustoTotal)
                    .HasColumnName("custo_total")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.HoraEntrada).HasColumnName("hora_entrada");

                entity.Property(e => e.Idresponsavel).HasColumnName("idresponsavel");

                entity.Property(e => e.Observacao)
                    .HasColumnName("observacao")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Paga).HasColumnName("paga");

                entity.Property(e => e.ValorTotal)
                    .HasColumnName("valor_total")
                    .HasColumnType("decimal(11,2)");

                entity.HasOne(d => d.IdresponsavelNavigation)
                    .WithMany(p => p.Venda)
                    .HasForeignKey(d => d.Idresponsavel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_venda_usuario1");
            });

            modelBuilder.Entity<VendaHasComanda>(entity =>
            {
                entity.HasKey(e => new { e.VendaIdvenda, e.ComandaIdcomanda })
                    .HasName("PRIMARY");

                entity.ToTable("venda_has_comanda");

                entity.HasIndex(e => e.ComandaIdcomanda)
                    .HasName("fk_venda_has_comanda_comanda1_idx");

                entity.HasIndex(e => e.VendaIdvenda)
                    .HasName("fk_venda_has_comanda_venda1_idx");

                entity.Property(e => e.VendaIdvenda).HasColumnName("venda_idvenda");

                entity.Property(e => e.ComandaIdcomanda).HasColumnName("comanda_idcomanda");

                entity.HasOne(d => d.ComandaIdcomandaNavigation)
                    .WithMany(p => p.VendaHasComanda)
                    .HasForeignKey(d => d.ComandaIdcomanda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_venda_has_comanda_comanda1");

                entity.HasOne(d => d.VendaIdvendaNavigation)
                    .WithMany(p => p.VendaHasComanda)
                    .HasForeignKey(d => d.VendaIdvenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_venda_has_comanda_venda1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
