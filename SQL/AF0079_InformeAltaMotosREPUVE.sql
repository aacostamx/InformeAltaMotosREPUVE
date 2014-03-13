USE ControlTiendas
GO
DROP PROCEDURE [dbo].[AF0079_InformeAltaMotosREPUVE]
GO

USE ControlTiendas
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AF0079_InformeAltaMotosREPUVE]
(@fInicio smalldatetime, @fFinal smalldatetime)
AS
-- =============================================
-- Autor: Antonio Acosta Murillo
-- Fecha: 21 Novimienbre 2013
-- Descripción General: Genera la información de las motocicletas vendidas para darlas de alta en el REPUVE
-- =============================================
BEGIN
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'AltaMotosREPUVE') DROP TABLE AltaMotosREPUVE
--Query de Maria José AF0076_Informe REPUVE
SELECT RTRIM(des_serie) Serie, 
RTRIM(b.nom_nombre)+' '+RTRIM(b.nom_apellidopaterno)+' '+RTRIM(b.nom_apellidomaterno) Nombre,fec_fechanacimiento,'CALLE'+' '+RTRIM(nom_calle)+' '+'#'+RTRIM(num_casaexterior)+' '+'COL.'+' '+RTRIM(nom_colonia)+' '+'CP'+' '+rtrim(num_codigopostal) Direccion,b.num_factura,b.fec_fechamovto
INTO AltaMotosREPUVE
FROM his_articulosmotocicletas a
LEFT JOIN his_clientemotocicletas b 
	ON (a.num_tienda = b.num_tienda and a.num_factura = b.num_factura)
LEFT JOIN his_datosentrega c 
	ON (a.num_tienda = c.num_tienda and a.num_factura = c.num_factura)
WHERE num_estatus = 2 and fec_fechaentrega BETWEEN @fInicio AND @fFinal
ORDER BY a.fec_fechamovto
END
GO

