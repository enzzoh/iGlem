USE [master]
GO
/****** Object:  Database [DATAtcc]    Script Date: 20/11/2024 22:37:05 ******/
CREATE DATABASE [DATAtcc]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DATAtcc', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\DATAtcc.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DATAtcc_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\DATAtcc_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [DATAtcc] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DATAtcc].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DATAtcc] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DATAtcc] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DATAtcc] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DATAtcc] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DATAtcc] SET ARITHABORT OFF 
GO
ALTER DATABASE [DATAtcc] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DATAtcc] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DATAtcc] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DATAtcc] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DATAtcc] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DATAtcc] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DATAtcc] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DATAtcc] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DATAtcc] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DATAtcc] SET  ENABLE_BROKER 
GO
ALTER DATABASE [DATAtcc] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DATAtcc] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DATAtcc] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DATAtcc] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DATAtcc] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DATAtcc] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DATAtcc] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DATAtcc] SET RECOVERY FULL 
GO
ALTER DATABASE [DATAtcc] SET  MULTI_USER 
GO
ALTER DATABASE [DATAtcc] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DATAtcc] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DATAtcc] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DATAtcc] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DATAtcc] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DATAtcc] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'DATAtcc', N'ON'
GO
ALTER DATABASE [DATAtcc] SET QUERY_STORE = ON
GO
ALTER DATABASE [DATAtcc] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [DATAtcc]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 20/11/2024 22:37:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[documento]    Script Date: 20/11/2024 22:37:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[documento](
	[documentoid] [int] IDENTITY(1,1) NOT NULL,
	[caminhodocumento] [varchar](max) NULL,
	[documentonome] [varchar](100) NULL,
	[FileData] [varbinary](max) NULL,
	[idusuario] [int] NULL,
	[conteudodocumento] [varchar](max) NULL,
	[datacriacao] [datetime] NULL,
	[IsFavorite] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[documentoid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[documento_palavra_chave]    Script Date: 20/11/2024 22:37:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[documento_palavra_chave](
	[id_documento] [int] NOT NULL,
	[id_palavra_chave] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_documento] ASC,
	[id_palavra_chave] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[localizacao]    Script Date: 20/11/2024 22:37:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[localizacao](
	[id_localizacao] [int] IDENTITY(1,1) NOT NULL,
	[paragrafo] [int] NULL,
	[pagina] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id_localizacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[palavra_chave]    Script Date: 20/11/2024 22:37:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[palavra_chave](
	[id_palavra_chave] [int] IDENTITY(1,1) NOT NULL,
	[palavra] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id_palavra_chave] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[palavra_chave_localizacao]    Script Date: 20/11/2024 22:37:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[palavra_chave_localizacao](
	[id_palavra_chave] [int] NOT NULL,
	[id_localizacao] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_palavra_chave] ASC,
	[id_localizacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[usuario]    Script Date: 20/11/2024 22:37:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[usuario](
	[idusuario] [int] IDENTITY(1,1) NOT NULL,
	[emailusuario] [varchar](50) NULL,
	[senhausuario] [varchar](40) NULL,
	[recmail] [varchar](50) NULL,
	[nomeusuario] [varchar](100) NULL,
	[telefoneusuario] [varchar](15) NULL,
	[IsAdmin] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[idusuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[documento] ADD  DEFAULT ((0)) FOR [IsFavorite]
GO
ALTER TABLE [dbo].[usuario] ADD  DEFAULT ((0)) FOR [IsAdmin]
GO
ALTER TABLE [dbo].[documento]  WITH CHECK ADD  CONSTRAINT [fk_usuario_documento] FOREIGN KEY([idusuario])
REFERENCES [dbo].[usuario] ([idusuario])
GO
ALTER TABLE [dbo].[documento] CHECK CONSTRAINT [fk_usuario_documento]
GO
ALTER TABLE [dbo].[documento_palavra_chave]  WITH CHECK ADD  CONSTRAINT [fk_documento] FOREIGN KEY([id_documento])
REFERENCES [dbo].[documento] ([documentoid])
GO
ALTER TABLE [dbo].[documento_palavra_chave] CHECK CONSTRAINT [fk_documento]
GO
ALTER TABLE [dbo].[documento_palavra_chave]  WITH CHECK ADD  CONSTRAINT [fk_palavra_chave] FOREIGN KEY([id_palavra_chave])
REFERENCES [dbo].[palavra_chave] ([id_palavra_chave])
GO
ALTER TABLE [dbo].[documento_palavra_chave] CHECK CONSTRAINT [fk_palavra_chave]
GO
ALTER TABLE [dbo].[palavra_chave_localizacao]  WITH CHECK ADD  CONSTRAINT [fk_localizacao] FOREIGN KEY([id_localizacao])
REFERENCES [dbo].[localizacao] ([id_localizacao])
GO
ALTER TABLE [dbo].[palavra_chave_localizacao] CHECK CONSTRAINT [fk_localizacao]
GO
ALTER TABLE [dbo].[palavra_chave_localizacao]  WITH CHECK ADD  CONSTRAINT [fk_palavra_chave_localizacao] FOREIGN KEY([id_palavra_chave])
REFERENCES [dbo].[palavra_chave] ([id_palavra_chave])
GO
ALTER TABLE [dbo].[palavra_chave_localizacao] CHECK CONSTRAINT [fk_palavra_chave_localizacao]
GO
USE [master]
GO
ALTER DATABASE [DATAtcc] SET  READ_WRITE 
GO
