using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace FortalezaServer.Models
{
    public partial class fortalezaitdbContext : DbContext
    {
        public fortalezaitdbContext()
        {
        }

        public fortalezaitdbContext(DbContextOptions<fortalezaitdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Adicional> Adicional { get; set; }
        public virtual DbSet<AdicionalItemVenda> AdicionalItemVenda { get; set; }
        public virtual DbSet<Bandeira> Bandeira { get; set; }
        public virtual DbSet<Caixa> Caixa { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<ClienteHasMovimento> ClienteHasMovimento { get; set; }
        public virtual DbSet<Comanda> Comanda { get; set; }
        public virtual DbSet<ConfiguracoesGerais> ConfiguracoesGerais { get; set; }
        public virtual DbSet<Endereco> Endereco { get; set; }
        public virtual DbSet<Entregador> Entregador { get; set; }
        public virtual DbSet<Estoque> Estoque { get; set; }
        public virtual DbSet<EstoqueHasFornecedor> EstoqueHasFornecedor { get; set; }
        public virtual DbSet<EstoqueHasVenda> EstoqueHasVenda { get; set; }
        public virtual DbSet<Fiscal> Fiscal { get; set; }
        public virtual DbSet<FormaPagamento> FormaPagamento { get; set; }
        public virtual DbSet<Fornecedor> Fornecedor { get; set; }
        public virtual DbSet<Grupo> Grupo { get; set; }
        public virtual DbSet<InformacoesEmpresa> InformacoesEmpresa { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemHasAdicional> ItemHasAdicional { get; set; }
        public virtual DbSet<ItemHasEstoque> ItemHasEstoque { get; set; }
        public virtual DbSet<ItemHasGrupo> ItemHasGrupo { get; set; }
        public virtual DbSet<ItemVenda> ItemVenda { get; set; }
        public virtual DbSet<Metodo> Metodo { get; set; }
        public virtual DbSet<Movimento> Movimento { get; set; }
        public virtual DbSet<Pacote> Pacote { get; set; }
        public virtual DbSet<Pagamento> Pagamento { get; set; }
        public virtual DbSet<Pedido> Pedido { get; set; }
        public virtual DbSet<Preferencias> Preferencias { get; set; }
        public virtual DbSet<TaxasEntrega> TaxasEntrega { get; set; }
        public virtual DbSet<TipoEntregador> TipoEntregador { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Venda> Venda { get; set; }
        public virtual DbSet<VendaHasComanda> VendaHasComanda { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("server=172.17.0.3;port=3306;user=fortalezait_dbuser;password=fortalezait@;database=fortalezaitdb");
                optionsBuilder.UseLoggerFactory(MyLoggerFactory);
            }
        }

        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adicional>(entity =>
            {
                entity.HasKey(e => e.Idadicional)
                    .HasName("PRIMARY");

                entity.ToTable("adicional");

                entity.Property(e => e.Idadicional).HasColumnName("idadicional");

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
            });

            modelBuilder.Entity<AdicionalItemVenda>(entity =>
            {
                entity.HasKey(e => e.IdadicionalVenda)
                    .HasName("PRIMARY");

                entity.ToTable("adicional_item_venda");

                entity.HasIndex(e => e.Idadicional)
                    .HasName("fk_adicional_item_venda_adicional1_idx");

                entity.HasIndex(e => e.IditemVenda)
                    .HasName("fk_adicional_item_venda_item_venda1_idx");

                entity.Property(e => e.IdadicionalVenda).HasColumnName("idadicional_venda");

                entity.Property(e => e.AdicionalIdadicional).HasColumnName("adicional_idadicional");

                entity.Property(e => e.Custo)
                    .HasColumnName("custo")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.Idadicional).HasColumnName("idadicional");

                entity.Property(e => e.IditemVenda).HasColumnName("iditem_venda");

                entity.Property(e => e.Quantidade)
                    .HasColumnName("quantidade")
                    .HasColumnType("decimal(12,3)");

                entity.Property(e => e.ValorVenda)
                    .HasColumnName("valor_venda")
                    .HasColumnType("decimal(11,2)");

                entity.HasOne(d => d.IdadicionalNavigation)
                    .WithMany(p => p.AdicionalItemVenda)
                    .HasForeignKey(d => d.Idadicional)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_adicional_item_venda_adicional1");

                entity.HasOne(d => d.IditemVendaNavigation)
                    .WithMany(p => p.AdicionalItemVenda)
                    .HasForeignKey(d => d.IditemVenda)
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

                entity.HasIndex(e => e.Idendereco)
                    .HasName("fk_cliente_endereco1_idx");

                entity.Property(e => e.Idcliente).HasColumnName("idcliente");

                entity.Property(e => e.Celular)
                    .HasColumnName("celular")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Cpf)
                    .HasColumnName("cpf")
                    .HasMaxLength(14)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Idendereco).HasColumnName("idendereco");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Telefone)
                    .HasColumnName("telefone")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdenderecoNavigation)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.Idendereco)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_cliente_endereco1");
            });

            modelBuilder.Entity<ClienteHasMovimento>(entity =>
            {
                entity.HasKey(e => new { e.ClienteIdcliente, e.MovimentoIdmovimento })
                    .HasName("PRIMARY");

                entity.ToTable("cliente_has_movimento");

                entity.HasIndex(e => e.ClienteIdcliente)
                    .HasName("fk_cliente_has_movimento_cliente1_idx");

                entity.HasIndex(e => e.MovimentoIdmovimento)
                    .HasName("fk_cliente_has_movimento_movimento1_idx");

                entity.Property(e => e.ClienteIdcliente).HasColumnName("cliente_idcliente");

                entity.Property(e => e.MovimentoIdmovimento).HasColumnName("movimento_idmovimento");

                entity.HasOne(d => d.ClienteIdclienteNavigation)
                    .WithMany(p => p.ClienteHasMovimento)
                    .HasForeignKey(d => d.ClienteIdcliente)
                    .HasConstraintName("fk_cliente_has_movimento_cliente1");

                entity.HasOne(d => d.MovimentoIdmovimentoNavigation)
                    .WithMany(p => p.ClienteHasMovimento)
                    .HasForeignKey(d => d.MovimentoIdmovimento)
                    .HasConstraintName("fk_cliente_has_movimento_movimento1");
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
                    .HasColumnName("logradouro")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Municipio)
                    .HasColumnName("municipio")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Numero)
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

            modelBuilder.Entity<Entregador>(entity =>
            {
                entity.HasKey(e => e.Identregador)
                    .HasName("PRIMARY");

                entity.ToTable("entregador");

                entity.Property(e => e.Identregador).HasColumnName("identregador");

                entity.Property(e => e.Ativo).HasColumnName("ativo");

                entity.Property(e => e.Disponivel).HasColumnName("disponivel");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(255)
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
                entity.HasKey(e => new { e.Idestoque, e.Idfornecedor })
                    .HasName("PRIMARY");

                entity.ToTable("estoque_has_fornecedor");

                entity.HasIndex(e => e.Idestoque)
                    .HasName("fk_estoque_has_fornecedor_estoque1_idx");

                entity.HasIndex(e => e.Idfornecedor)
                    .HasName("fk_estoque_has_fornecedor_fornecedor1_idx");

                entity.Property(e => e.Idestoque).HasColumnName("idestoque");

                entity.Property(e => e.Idfornecedor).HasColumnName("idfornecedor");

                entity.HasOne(d => d.IdestoqueNavigation)
                    .WithMany(p => p.EstoqueHasFornecedor)
                    .HasForeignKey(d => d.Idestoque)
                    .HasConstraintName("fk_estoque_has_fornecedor_estoque1");

                entity.HasOne(d => d.IdfornecedorNavigation)
                    .WithMany(p => p.EstoqueHasFornecedor)
                    .HasForeignKey(d => d.Idfornecedor)
                    .HasConstraintName("fk_estoque_has_fornecedor_fornecedor1");
            });

            modelBuilder.Entity<EstoqueHasVenda>(entity =>
            {
                entity.HasKey(e => new { e.Idestoque, e.Idvenda })
                    .HasName("PRIMARY");

                entity.ToTable("estoque_has_venda");

                entity.HasIndex(e => e.Idestoque)
                    .HasName("fk_estoque_has_venda_estoque1_idx");

                entity.HasIndex(e => e.Idvenda)
                    .HasName("fk_estoque_has_venda_venda1_idx");

                entity.Property(e => e.Idestoque).HasColumnName("idestoque");

                entity.Property(e => e.Idvenda).HasColumnName("idvenda");

                entity.HasOne(d => d.IdestoqueNavigation)
                    .WithMany(p => p.EstoqueHasVenda)
                    .HasForeignKey(d => d.Idestoque)
                    .HasConstraintName("fk_estoque_has_venda_estoque1");

                entity.HasOne(d => d.IdvendaNavigation)
                    .WithMany(p => p.EstoqueHasVenda)
                    .HasForeignKey(d => d.Idvenda)
                    .HasConstraintName("fk_estoque_has_venda_venda1");
            });

            modelBuilder.Entity<Fiscal>(entity =>
            {
                entity.HasKey(e => e.Iditem)
                    .HasName("PRIMARY");

                entity.ToTable("fiscal");

                entity.Property(e => e.Iditem).HasColumnName("iditem");

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

                entity.HasOne(d => d.IditemNavigation)
                    .WithOne(p => p.Fiscal)
                    .HasForeignKey<Fiscal>(d => d.Iditem)
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

                entity.HasIndex(e => e.Idendereco)
                    .HasName("fk_informacoes_empresa_endereco1_idx");

                entity.Property(e => e.IdinformacoesEmpresa).HasColumnName("idinformacoes_empresa");

                entity.Property(e => e.Cnae).HasColumnName("cnae");

                entity.Property(e => e.Cnpj).HasColumnName("cnpj");

                entity.Property(e => e.Idendereco).HasColumnName("idendereco");

                entity.Property(e => e.Ie).HasColumnName("ie");

                entity.Property(e => e.NomeFantasia)
                    .IsRequired()
                    .HasColumnName("nome_fantasia")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RazaoSocial)
                    .HasColumnName("razao_social")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RegimeTributario).HasColumnName("regime_tributario");

                entity.HasOne(d => d.IdenderecoNavigation)
                    .WithMany(p => p.InformacoesEmpresa)
                    .HasForeignKey(d => d.Idendereco)
                    .HasConstraintName("fk_informacoes_empresa_endereco1");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Iditem)
                    .HasName("PRIMARY");

                entity.ToTable("item");

                entity.Property(e => e.Iditem).HasColumnName("iditem");

                entity.Property(e => e.CodigoBarras).HasColumnName("codigo_barras");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("descricao")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Disponivel)
                    .HasColumnName("disponivel")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Estoque).HasColumnName("estoque");

                entity.Property(e => e.EstoqueMinimo)
                    .HasColumnName("estoque_minimo")
                    .HasColumnType("decimal(12,3)");

                entity.Property(e => e.Imagem)
                    .HasColumnName("imagem")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PermiteCombo)
                    .HasColumnName("permite_combo")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.QuantidadeVarejo)
                    .HasColumnName("quantidade_varejo")
                    .HasColumnType("decimal(12,3)");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnName("tipo")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Unidade)
                    .IsRequired()
                    .HasColumnName("unidade")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.UnidadeInteira)
                    .HasColumnName("unidade_inteira")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Valor)
                    .HasColumnName("valor")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.ValorVarejo)
                    .HasColumnName("valor_varejo")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.Varejo).HasColumnName("varejo");

                entity.Property(e => e.Visivel)
                    .HasColumnName("visivel")
                    .HasDefaultValueSql("'1'");
            });

            modelBuilder.Entity<ItemHasAdicional>(entity =>
            {
                entity.HasKey(e => new { e.Iditem, e.Idadicional })
                    .HasName("PRIMARY");

                entity.ToTable("item_has_adicional");

                entity.HasIndex(e => e.Idadicional)
                    .HasName("fk_item_has_adicional_adicional1_idx");

                entity.HasIndex(e => e.Iditem)
                    .HasName("fk_item_has_adicional_item1_idx");

                entity.Property(e => e.Iditem).HasColumnName("iditem");

                entity.Property(e => e.Idadicional).HasColumnName("idadicional");

                entity.HasOne(d => d.IdadicionalNavigation)
                    .WithMany(p => p.ItemHasAdicional)
                    .HasForeignKey(d => d.Idadicional)
                    .HasConstraintName("fk_item_has_adicional_adicional1");

                entity.HasOne(d => d.IditemNavigation)
                    .WithMany(p => p.ItemHasAdicional)
                    .HasForeignKey(d => d.Iditem)
                    .HasConstraintName("fk_item_has_adicional_item1");
            });

            modelBuilder.Entity<ItemHasEstoque>(entity =>
            {
                entity.HasKey(e => new { e.Iditem, e.Idestoque })
                    .HasName("PRIMARY");

                entity.ToTable("item_has_estoque");

                entity.HasIndex(e => e.Idestoque)
                    .HasName("fk_item_has_estoque_estoque1_idx");

                entity.HasIndex(e => e.Iditem)
                    .HasName("fk_item_has_estoque_item1_idx");

                entity.Property(e => e.Iditem).HasColumnName("iditem");

                entity.Property(e => e.Idestoque).HasColumnName("idestoque");

                entity.HasOne(d => d.IdestoqueNavigation)
                    .WithMany(p => p.ItemHasEstoque)
                    .HasForeignKey(d => d.Idestoque)
                    .HasConstraintName("fk_item_has_estoque_estoque1");

                entity.HasOne(d => d.IditemNavigation)
                    .WithMany(p => p.ItemHasEstoque)
                    .HasForeignKey(d => d.Iditem)
                    .HasConstraintName("fk_item_has_estoque_item1");
            });

            modelBuilder.Entity<ItemHasGrupo>(entity =>
            {
                entity.HasKey(e => new { e.Iditem, e.Idgrupo })
                    .HasName("PRIMARY");

                entity.ToTable("item_has_grupo");

                entity.HasIndex(e => e.Idgrupo)
                    .HasName("fk_item_has_grupo_grupo1_idx");

                entity.HasIndex(e => e.Iditem)
                    .HasName("fk_item_has_grupo_item1_idx");

                entity.Property(e => e.Iditem).HasColumnName("iditem");

                entity.Property(e => e.Idgrupo).HasColumnName("idgrupo");

                entity.HasOne(d => d.IdgrupoNavigation)
                    .WithMany(p => p.ItemHasGrupo)
                    .HasForeignKey(d => d.Idgrupo)
                    .HasConstraintName("fk_item_has_grupo_grupo1");

                entity.HasOne(d => d.IditemNavigation)
                    .WithMany(p => p.ItemHasGrupo)
                    .HasForeignKey(d => d.Iditem)
                    .HasConstraintName("fk_item_has_grupo_item1");
            });

            modelBuilder.Entity<ItemVenda>(entity =>
            {
                entity.HasKey(e => e.IditemVenda)
                    .HasName("PRIMARY");

                entity.ToTable("item_venda");

                entity.HasIndex(e => e.Iditem)
                    .HasName("fk_item_venda_item1_idx");

                entity.HasIndex(e => e.Idvenda)
                    .HasName("fk_item_venda_venda1_idx");

                entity.Property(e => e.IditemVenda).HasColumnName("iditem_venda");

                entity.Property(e => e.Custo)
                    .HasColumnName("custo")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.Iditem).HasColumnName("iditem");

                entity.Property(e => e.Idvenda).HasColumnName("idvenda");

                entity.Property(e => e.Indice).HasColumnName("indice");

                entity.Property(e => e.Quantidade)
                    .HasColumnName("quantidade")
                    .HasColumnType("decimal(12,3)");

                entity.Property(e => e.Valor)
                    .HasColumnName("valor")
                    .HasColumnType("decimal(11,2)");

                entity.HasOne(d => d.IditemNavigation)
                    .WithMany(p => p.ItemVenda)
                    .HasForeignKey(d => d.Iditem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_item_venda_item1");

                entity.HasOne(d => d.IdvendaNavigation)
                    .WithMany(p => p.ItemVenda)
                    .HasForeignKey(d => d.Idvenda)
                    .HasConstraintName("fk_item_venda_venda1");
            });

            modelBuilder.Entity<Metodo>(entity =>
            {
                entity.HasKey(e => e.Idmetodo)
                    .HasName("PRIMARY");

                entity.ToTable("metodo");

                entity.Property(e => e.Idmetodo).HasColumnName("idmetodo");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PossuiEntregador).HasColumnName("possui_entregador");
            });

            modelBuilder.Entity<Movimento>(entity =>
            {
                entity.HasKey(e => e.Idmovimento)
                    .HasName("PRIMARY");

                entity.ToTable("movimento");

                entity.HasIndex(e => e.Idbandeira)
                    .HasName("fk_movimento_bandeira1_idx");

                entity.HasIndex(e => e.Idcaixa)
                    .HasName("fk_movimento_caixa1_idx");

                entity.HasIndex(e => e.IdformaPagamento)
                    .HasName("fk_movimento_forma_pagamento1_idx");

                entity.Property(e => e.Idmovimento).HasColumnName("idmovimento");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("descricao")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.HoraEntrada).HasColumnName("hora_entrada");

                entity.Property(e => e.Idbandeira).HasColumnName("idbandeira");

                entity.Property(e => e.Idcaixa).HasColumnName("idcaixa");

                entity.Property(e => e.IdformaPagamento).HasColumnName("idforma_pagamento");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnName("tipo")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Valor)
                    .HasColumnName("valor")
                    .HasColumnType("decimal(11,2)");

                entity.HasOne(d => d.IdbandeiraNavigation)
                    .WithMany(p => p.Movimento)
                    .HasForeignKey(d => d.Idbandeira)
                    .HasConstraintName("fk_movimento_bandeira1");

                entity.HasOne(d => d.IdcaixaNavigation)
                    .WithMany(p => p.Movimento)
                    .HasForeignKey(d => d.Idcaixa)
                    .HasConstraintName("fk_movimento_caixa1");

                entity.HasOne(d => d.IdformaPagamentoNavigation)
                    .WithMany(p => p.Movimento)
                    .HasForeignKey(d => d.IdformaPagamento)
                    .HasConstraintName("fk_movimento_forma_pagamento1");
            });

            modelBuilder.Entity<Pacote>(entity =>
            {
                entity.HasKey(e => e.Iditem)
                    .HasName("PRIMARY");

                entity.ToTable("pacote");

                entity.HasIndex(e => e.IditemProduto)
                    .HasName("fk_pacote_item2_idx");

                entity.Property(e => e.Iditem).HasColumnName("iditem");

                entity.Property(e => e.IditemProduto).HasColumnName("iditem_produto");

                entity.Property(e => e.Padrao).HasColumnName("padrao");

                entity.Property(e => e.Quantidade)
                    .HasColumnName("quantidade")
                    .HasColumnType("decimal(11,3)");

                entity.HasOne(d => d.IditemNavigation)
                    .WithOne(p => p.PacoteIditemNavigation)
                    .HasForeignKey<Pacote>(d => d.Iditem)
                    .HasConstraintName("fk_pacote_item1");

                entity.HasOne(d => d.IditemProdutoNavigation)
                    .WithMany(p => p.PacoteIditemProdutoNavigation)
                    .HasForeignKey(d => d.IditemProduto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_pacote_item2");
            });

            modelBuilder.Entity<Pagamento>(entity =>
            {
                entity.HasKey(e => new { e.Idvenda, e.Idmovimento })
                    .HasName("PRIMARY");

                entity.ToTable("pagamento");

                entity.HasIndex(e => e.Idmovimento)
                    .HasName("fk_venda_has_movimento_movimento1_idx");

                entity.HasIndex(e => e.Idvenda)
                    .HasName("fk_venda_has_movimento_venda1_idx");

                entity.Property(e => e.Idvenda).HasColumnName("idvenda");

                entity.Property(e => e.Idmovimento).HasColumnName("idmovimento");

                entity.HasOne(d => d.IdmovimentoNavigation)
                    .WithMany(p => p.Pagamento)
                    .HasForeignKey(d => d.Idmovimento)
                    .HasConstraintName("fk_venda_has_movimento_movimento1");

                entity.HasOne(d => d.IdvendaNavigation)
                    .WithMany(p => p.Pagamento)
                    .HasForeignKey(d => d.Idvenda)
                    .HasConstraintName("fk_venda_has_movimento_venda1");
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(e => e.Idvenda)
                    .HasName("PRIMARY");

                entity.ToTable("pedido");

                entity.HasIndex(e => e.Identregador)
                    .HasName("fk_pedido_entregador1_idx");

                entity.HasIndex(e => e.Idmetodo)
                    .HasName("fk_pedido_metodo1_idx");

                entity.HasIndex(e => e.IdtipoEntregador)
                    .HasName("fk_pedido_tipo_entregador1_idx");

                entity.Property(e => e.Idvenda).HasColumnName("idvenda");

                entity.Property(e => e.DataEntregue).HasColumnName("data_entregue");

                entity.Property(e => e.DataPrazo).HasColumnName("data_prazo");

                entity.Property(e => e.DataSaida).HasColumnName("data_saida");

                entity.Property(e => e.DateRetorno).HasColumnName("date_retorno");

                entity.Property(e => e.Delivery).HasColumnName("delivery");

                entity.Property(e => e.Entregue).HasColumnName("entregue");

                entity.Property(e => e.Identregador).HasColumnName("identregador");

                entity.Property(e => e.Idmetodo).HasColumnName("idmetodo");

                entity.Property(e => e.IdtipoEntregador).HasColumnName("idtipo_entregador");

                entity.Property(e => e.NumeroPedido).HasColumnName("numero_pedido");

                entity.Property(e => e.Observacao)
                    .HasColumnName("observacao")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TempoCozinha).HasColumnName("tempo_cozinha");

                entity.Property(e => e.TempoEntrega).HasColumnName("tempo_entrega");

                entity.HasOne(d => d.IdentregadorNavigation)
                    .WithMany(p => p.Pedido)
                    .HasForeignKey(d => d.Identregador)
                    .HasConstraintName("fk_pedido_entregador1");

                entity.HasOne(d => d.IdmetodoNavigation)
                    .WithMany(p => p.Pedido)
                    .HasForeignKey(d => d.Idmetodo)
                    .HasConstraintName("fk_pedido_metodo1");

                entity.HasOne(d => d.IdtipoEntregadorNavigation)
                    .WithMany(p => p.Pedido)
                    .HasForeignKey(d => d.IdtipoEntregador)
                    .HasConstraintName("fk_pedido_tipo_entregador1");

                entity.HasOne(d => d.IdvendaNavigation)
                    .WithOne(p => p.Pedido)
                    .HasForeignKey<Pedido>(d => d.Idvenda)
                    .HasConstraintName("fk_table1_venda1");
            });

            modelBuilder.Entity<Preferencias>(entity =>
            {
                entity.HasKey(e => e.Idpreferencias)
                    .HasName("PRIMARY");

                entity.ToTable("preferencias");

                entity.Property(e => e.Idpreferencias).HasColumnName("idpreferencias");

                entity.Property(e => e.CodigoTaxaEntrega).HasColumnName("codigo_taxa_entrega");

                entity.Property(e => e.ModoTaxaEntrega).HasColumnName("modo_taxa_entrega");

                entity.Property(e => e.NomeTaxaEntrega)
                    .IsRequired()
                    .HasColumnName("nome_taxa_entrega")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TaxaEntregaPadrao)
                    .HasColumnName("taxa_entrega_padrao")
                    .HasColumnType("decimal(11,3)");
            });

            modelBuilder.Entity<TaxasEntrega>(entity =>
            {
                entity.HasKey(e => e.IdtaxasEntrega)
                    .HasName("PRIMARY");

                entity.ToTable("taxas_entrega");

                entity.Property(e => e.IdtaxasEntrega).HasColumnName("idtaxas_entrega");

                entity.Property(e => e.Bairro)
                    .HasColumnName("bairro")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Raio)
                    .HasColumnName("raio")
                    .HasColumnType("decimal(5,2)");

                entity.Property(e => e.Taxa)
                    .HasColumnName("taxa")
                    .HasColumnType("decimal(11,3)");

                entity.Property(e => e.Tipo).HasColumnName("tipo");
            });

            modelBuilder.Entity<TipoEntregador>(entity =>
            {
                entity.HasKey(e => e.IdtipoEntregador)
                    .HasName("PRIMARY");

                entity.ToTable("tipo_entregador");

                entity.Property(e => e.IdtipoEntregador).HasColumnName("idtipo_entregador");

                entity.Property(e => e.Nome)
                    .HasColumnName("nome")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TempoMedio).HasColumnName("tempo_medio");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Idusuario)
                    .HasName("PRIMARY");

                entity.ToTable("usuario");

                entity.HasIndex(e => e.Idendereco)
                    .HasName("fk_usuario_endereco1_idx");

                entity.Property(e => e.Idusuario).HasColumnName("idusuario");

                entity.Property(e => e.Cpf).HasColumnName("cpf");

                entity.Property(e => e.Idendereco).HasColumnName("idendereco");

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

                entity.HasOne(d => d.IdenderecoNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.Idendereco)
                    .HasConstraintName("fk_usuario_endereco1");
            });

            modelBuilder.Entity<Venda>(entity =>
            {
                entity.HasKey(e => e.Idvenda)
                    .HasName("PRIMARY");

                entity.ToTable("venda");

                entity.HasIndex(e => e.Idcliente)
                    .HasName("fk_venda_cliente1_idx");

                entity.HasIndex(e => e.Idresponsavel)
                    .HasName("fk_venda_usuario1_idx");

                entity.Property(e => e.Idvenda).HasColumnName("idvenda");

                entity.Property(e => e.Aberta).HasColumnName("aberta");

                entity.Property(e => e.Acrescimo)
                    .HasColumnName("acrescimo")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.CustoTotal)
                    .HasColumnName("custo_total")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.Desconto)
                    .HasColumnName("desconto")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.Descricao)
                    .HasColumnName("descricao")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.HoraEntrada).HasColumnName("hora_entrada");

                entity.Property(e => e.HoraFechamento).HasColumnName("hora_fechamento");

                entity.Property(e => e.Idcliente).HasColumnName("idcliente");

                entity.Property(e => e.Idresponsavel).HasColumnName("idresponsavel");

                entity.Property(e => e.NumeroVenda).HasColumnName("numero_venda");

                entity.Property(e => e.Paga).HasColumnName("paga");

                entity.Property(e => e.Tipo).HasColumnName("tipo");

                entity.Property(e => e.ValorPago)
                    .HasColumnName("valor_pago")
                    .HasColumnType("decimal(11,2)");

                entity.HasOne(d => d.IdclienteNavigation)
                    .WithMany(p => p.Venda)
                    .HasForeignKey(d => d.Idcliente)
                    .HasConstraintName("fk_venda_cliente1");

                entity.HasOne(d => d.IdresponsavelNavigation)
                    .WithMany(p => p.Venda)
                    .HasForeignKey(d => d.Idresponsavel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_venda_usuario1");
            });

            modelBuilder.Entity<VendaHasComanda>(entity =>
            {
                entity.HasKey(e => new { e.Idvenda, e.Idcomanda })
                    .HasName("PRIMARY");

                entity.ToTable("venda_has_comanda");

                entity.HasIndex(e => e.Idcomanda)
                    .HasName("fk_venda_has_comanda_comanda1_idx");

                entity.HasIndex(e => e.Idvenda)
                    .HasName("fk_venda_has_comanda_venda1_idx");

                entity.Property(e => e.Idvenda).HasColumnName("idvenda");

                entity.Property(e => e.Idcomanda).HasColumnName("idcomanda");

                entity.HasOne(d => d.IdcomandaNavigation)
                    .WithMany(p => p.VendaHasComanda)
                    .HasForeignKey(d => d.Idcomanda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_venda_has_comanda_comanda1");

                entity.HasOne(d => d.IdvendaNavigation)
                    .WithMany(p => p.VendaHasComanda)
                    .HasForeignKey(d => d.Idvenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_venda_has_comanda_venda1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
