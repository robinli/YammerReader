/*------------------------------------------------ 
description: 類似 C# 字串函式 Split 
author: Robin
date: 2022/02/22
reference: https://technet.microsoft.com/en-us/library/aa214485(v=sql.80).aspx
testing code: 
-------------------------------------------------- 
DECLARE @String NVARCHAR(max), @Delimiter NVARCHAR(4);

SET @String=N'uploadedfile:1336132263936, opengraphobject:[360824469504000 : https://corex.bud4.net/MIS/Msgtmp/Company#/ : title="Login" : description=""]';
SET @Delimiter = ','
SELECT * FROM dbo.Split(@String, @Delimiter);
--------------------------------------------------*/
CREATE FUNCTION [dbo].[Split]
(
    @String NVARCHAR(max)
	, @Delimiter NVARCHAR(4)
)
RETURNS TABLE
AS
RETURN
(
    WITH Split(stpos,endpos)
    AS(
		select t1.stpos, IIF(t1.endpos>0, t1.endpos, LEN(@String)+1) as endpos
		from(
			select stpos=convert(int, 0), endpos=CHARINDEX(@Delimiter,@String)
		) t1
		union all

		select convert(int, LEN(@Delimiter)+endpos), CHARINDEX(@Delimiter, @String, LEN(@Delimiter)+endpos+1)
		from Split
		where endpos > 0
    )
    select 'ID' = ROW_NUMBER() OVER (ORDER BY (SELECT 1)),
        'DATA' = RTRIM(LTRIM(SUBSTRING(@String,stpos,COALESCE(NULLIF(endpos,0),LEN(@String)+1)-stpos)))
    from Split
	where stpos < LEN(@String)
)