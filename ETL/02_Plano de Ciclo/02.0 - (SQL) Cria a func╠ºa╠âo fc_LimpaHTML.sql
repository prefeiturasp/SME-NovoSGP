USE [GestaoPedagogica]
GO

/****** Object:  UserDefinedFunction [dbo].[fc_LimpaHTML]    Script Date: 18/12/2019 14:39:21 ******/
DROP FUNCTION [dbo].[fc_LimpaHTML]
GO

CREATE FUNCTION [dbo].[fc_LimpaHTML]
	(
		@HTMLText VARCHAR(MAX)
	)
	RETURNS VARCHAR(MAX)
AS

BEGIN
	DECLARE @Start  INT
	DECLARE @End    INT
	DECLARE @Length INT


	-- Retirando os atributos na tag de parágrafo <P>
	SET @Start = CHARINDEX('<p ', @HTMLText)
	SET @End = CHARINDEX('>', @HTMLText,  @Start)
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) 
	BEGIN
	  SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '<p>')
	  SET @Start = CHARINDEX('<p ', @HTMLText)
	  SET @End = CHARINDEX('>', @HTMLText, CHARINDEX('<p ', @HTMLText))
	  SET @Length = (@End - @Start) + 1
	END


	-- Substituindo comandos HTML (tags) para quebra de linha e lista
	SET @HTMLText = REPLACE(@HTMLText, '<br/>',CHAR(13) + CHAR(10))
	SET @HTMLText = REPLACE(@HTMLText, '<br />',CHAR(13) + CHAR(10))
	SET @HTMLText = REPLACE(@HTMLText, '<p> </p>',CHAR(13) + CHAR(10))
	SET @HTMLText = REPLACE(@HTMLText, '<p> .</p>',CHAR(13) + CHAR(10))
	SET @HTMLText = REPLACE(@HTMLText, '<p>.</p>',CHAR(13) + CHAR(10))
	SET @HTMLText = REPLACE(@HTMLText, CHAR(9),' ')
	SET @HTMLText = REPLACE(@HTMLText, '<li>','* ')
	SET @HTMLText = REPLACE(@HTMLText, '</li>',CHAR(13) + CHAR(10))
	SET @HTMLText = LTRIM(@HTMLText)
	SET @HTMLText = RTRIM(@HTMLText)


    -- Renomeando comandos HTML (tags) para manter as tags <P>, <B>, <I>, <U>
	SET @HTMLText = REPLACE(@HTMLText, '<p>','[[[p]]]')
	SET @HTMLText = REPLACE(@HTMLText, '<b>','[[[b]]]')
	SET @HTMLText = REPLACE(@HTMLText, '<i>','[[[i]]]')
	SET @HTMLText = REPLACE(@HTMLText, '<u>','[[[u]]]')
	SET @HTMLText = REPLACE(@HTMLText, '<em>','[[[em]]]')
	SET @HTMLText = REPLACE(@HTMLText, '<strong>','[[[strong]]]')
	SET @HTMLText = REPLACE(@HTMLText, '</p>','[[[/p]]]')
	SET @HTMLText = REPLACE(@HTMLText, '</b>','[[[/b]]]')
	SET @HTMLText = REPLACE(@HTMLText, '</i>','[[[/i]]]')
	SET @HTMLText = REPLACE(@HTMLText, '</u>','[[[/u]]]')
	SET @HTMLText = REPLACE(@HTMLText, '</em>','[[[/em]]]')
	SET @HTMLText = REPLACE(@HTMLText, '</strong>','[[[/strong]]]')
	SET @HTMLText = LTRIM(@HTMLText)
	SET @HTMLText = RTRIM(@HTMLText)


	-- Tirando sujeira do início da variável
	IF SUBSTRING(@HTMLText,1,2) = '=-'
	BEGIN
	   SET @HTMLText = SUBSTRING(@HTMLText,3,DATALENGTH(@HTMLText))
	END
	IF SUBSTRING(@HTMLText,1,1) = '='
	BEGIN
	   SET @HTMLText = SUBSTRING(@HTMLText,2,DATALENGTH(@HTMLText))
	END
	IF SUBSTRING(@HTMLText,1,1) = '-'
	BEGIN
	   SET @HTMLText = SUBSTRING(@HTMLText,2,DATALENGTH(@HTMLText))
	END
	SET @HTMLText = LTRIM(@HTMLText)
	SET @HTMLText = RTRIM(@HTMLText)


	-- Removendo todos os outros comandos HTML (tags)
	SET @Start = CHARINDEX('<', @HTMLText)
	SET @End = CHARINDEX('>', @HTMLText, CHARINDEX('<', @HTMLText))
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) 
	BEGIN
	  SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '')
	  SET @Start = CHARINDEX('<', @HTMLText)
	  SET @End = CHARINDEX('>', @HTMLText, CHARINDEX('<', @HTMLText))
	  SET @Length = (@End - @Start) + 1
	END
	SET @HTMLText = LTRIM(@HTMLText)
	SET @HTMLText = RTRIM(@HTMLText)


	-- Voltando as tags <P>, <B>, <I>, <U>
	SET @HTMLText = REPLACE(@HTMLText, '[[[p]]]','<p>')
	SET @HTMLText = REPLACE(@HTMLText, '[[[b]]]','<b>')
	SET @HTMLText = REPLACE(@HTMLText, '[[[i]]]','<i>')
	SET @HTMLText = REPLACE(@HTMLText, '[[[u]]]','<u>')
	SET @HTMLText = REPLACE(@HTMLText, '[[[em]]]','<em>')
	SET @HTMLText = REPLACE(@HTMLText, '[[[strong]]]','<strong>')
	SET @HTMLText = REPLACE(@HTMLText, '[[[/p]]]','</p>')
	SET @HTMLText = REPLACE(@HTMLText, '[[[/b]]]','</b>')
	SET @HTMLText = REPLACE(@HTMLText, '[[[/i]]]','</i>')
	SET @HTMLText = REPLACE(@HTMLText, '[[[/u]]]','</u>')
	SET @HTMLText = REPLACE(@HTMLText, '[[[/strong]]]','</strong>')
	SET @HTMLText = REPLACE(@HTMLText, '[[[/em]]]','</em>')
	SET @HTMLText = LTRIM(@HTMLText)
	SET @HTMLText = RTRIM(@HTMLText)


	-- Substituindo caracteres especiais
	SET @HTMLText = REPLACE(@HTMLText, '&ndash;' COLLATE Latin1_General_CS_AS, '*'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&mdash;' COLLATE Latin1_General_CS_AS, '*'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&bull;' COLLATE Latin1_General_CS_AS, '*'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&amp;' COLLATE Latin1_General_CS_AS, '&'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, 'amp;' COLLATE Latin1_General_CS_AS, ''  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&euro;' COLLATE Latin1_General_CS_AS, '€'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&lt;' COLLATE Latin1_General_CS_AS, '<'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&lsaquo;' COLLATE Latin1_General_CS_AS, '<'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&gt;' COLLATE Latin1_General_CS_AS, '>'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&rsaquo;' COLLATE Latin1_General_CS_AS, '>'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&oelig;' COLLATE Latin1_General_CS_AS, 'oe'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&nbsp;' COLLATE Latin1_General_CS_AS, ' '  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&copy;' COLLATE Latin1_General_CS_AS, '©'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&laquo;' COLLATE Latin1_General_CS_AS, '«'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&reg;' COLLATE Latin1_General_CS_AS, '®'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&plusmn;' COLLATE Latin1_General_CS_AS, '±'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&sup2;' COLLATE Latin1_General_CS_AS, '²'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&sup3;' COLLATE Latin1_General_CS_AS, '³'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&micro;' COLLATE Latin1_General_CS_AS, 'µ'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&middot;' COLLATE Latin1_General_CS_AS, '*'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&ordm;' COLLATE Latin1_General_CS_AS, 'º'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&raquo;' COLLATE Latin1_General_CS_AS, '»'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&frac14;' COLLATE Latin1_General_CS_AS, '¼'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&frac12;' COLLATE Latin1_General_CS_AS, '½'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&frac34;' COLLATE Latin1_General_CS_AS, '¾'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Aelig' COLLATE Latin1_General_CS_AS, 'Æ'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&aelig;' COLLATE Latin1_General_CS_AS, 'æ'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&divide;' COLLATE Latin1_General_CS_AS, '÷'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&ordf;' COLLATE Latin1_General_CS_AS, 'ª'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&deg;' COLLATE Latin1_General_CS_AS, '°'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Ccedil;' COLLATE Latin1_General_CS_AS, 'Ç'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&ccedil;' COLLATE Latin1_General_CS_AS, 'ç'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Aacute;' COLLATE Latin1_General_CS_AS, 'Á'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&aacute;' COLLATE Latin1_General_CS_AS, 'á'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Agrave;' COLLATE Latin1_General_CS_AS, 'À'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&agrave;' COLLATE Latin1_General_CS_AS, 'à'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Acirc;' COLLATE Latin1_General_CS_AS, 'Â'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&acirc;' COLLATE Latin1_General_CS_AS, 'â'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Auml;' COLLATE Latin1_General_CS_AS, 'Ä'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&auml;' COLLATE Latin1_General_CS_AS, 'ä'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Atilde;' COLLATE Latin1_General_CS_AS, 'Ã'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&atilde;' COLLATE Latin1_General_CS_AS, 'ã'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Eacute;' COLLATE Latin1_General_CS_AS, 'É'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&eacute;' COLLATE Latin1_General_CS_AS, 'é'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Egrave;' COLLATE Latin1_General_CS_AS, 'È'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&egrave;' COLLATE Latin1_General_CS_AS, 'è'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Ecirc;' COLLATE Latin1_General_CS_AS, 'Ê'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&ecirc;' COLLATE Latin1_General_CS_AS, 'ê'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Euml;' COLLATE Latin1_General_CS_AS, 'Ë'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&euml;' COLLATE Latin1_General_CS_AS, 'ë'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Iacute;' COLLATE Latin1_General_CS_AS, 'Í'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&iacute;' COLLATE Latin1_General_CS_AS, 'í'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Igrave;' COLLATE Latin1_General_CS_AS, 'Ì'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&igrave;' COLLATE Latin1_General_CS_AS, 'ì'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Icirc;' COLLATE Latin1_General_CS_AS, 'Î'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&icirc;' COLLATE Latin1_General_CS_AS, 'î'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Iuml;' COLLATE Latin1_General_CS_AS, 'Ï'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&iuml;' COLLATE Latin1_General_CS_AS, 'ï'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Oacute;' COLLATE Latin1_General_CS_AS, 'Ó'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&oacute;' COLLATE Latin1_General_CS_AS, 'ó'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Ograve;' COLLATE Latin1_General_CS_AS, 'Ò'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&ograve;' COLLATE Latin1_General_CS_AS, 'ò'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Ocirc;' COLLATE Latin1_General_CS_AS, 'Ô'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&ocirc;' COLLATE Latin1_General_CS_AS, 'ô'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Ouml;' COLLATE Latin1_General_CS_AS, 'Ö'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&ouml;' COLLATE Latin1_General_CS_AS, 'ö'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Otilde;' COLLATE Latin1_General_CS_AS, 'Õ'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&otilde;' COLLATE Latin1_General_CS_AS, 'õ'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Uacute;' COLLATE Latin1_General_CS_AS, 'Ú'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&uacute;' COLLATE Latin1_General_CS_AS, 'ú'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Ugrave;' COLLATE Latin1_General_CS_AS, 'Ù'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&ugrave;' COLLATE Latin1_General_CS_AS, 'ù'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Ucirc;' COLLATE Latin1_General_CS_AS, 'Û'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&ucirc;' COLLATE Latin1_General_CS_AS, 'û'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&Uuml;' COLLATE Latin1_General_CS_AS, 'Ü'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&uuml;' COLLATE Latin1_General_CS_AS, 'ü'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&oslash;' COLLATE Latin1_General_CS_AS, 'ø'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&amp;' COLLATE Latin1_General_CS_AS, '&'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&rsquo;' COLLATE Latin1_General_CS_AS, ''''  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&quot;' COLLATE Latin1_General_CS_AS, ''''  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&ldquo;' COLLATE Latin1_General_CS_AS, ''''  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&rdquo;' COLLATE Latin1_General_CS_AS, ''''  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '&bdquo;' COLLATE Latin1_General_CS_AS, ''''  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '"' COLLATE Latin1_General_CS_AS, ''''  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '•' COLLATE Latin1_General_CS_AS, '*'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '·' COLLATE Latin1_General_CS_AS, '*'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '–' COLLATE Latin1_General_CS_AS, '*'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, '=-' COLLATE Latin1_General_CS_AS, '*'  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, ';;;;' COLLATE Latin1_General_CS_AS, ''  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = REPLACE(@HTMLText, ';' COLLATE Latin1_General_CS_AS, ','  COLLATE Latin1_General_CS_AS)
	SET @HTMLText = LTRIM(@HTMLText)
	SET @HTMLText = RTRIM(@HTMLText)


	IF DATALENGTH(@HTMLText) <= 1
	BEGIN
	   SET @HTMLText = NULL
	END


	RETURN @HTMLText

END
GO


