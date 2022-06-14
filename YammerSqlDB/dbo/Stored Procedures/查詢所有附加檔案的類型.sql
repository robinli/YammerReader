/*---------------------------------------------------- 
description:  查詢所有附加檔案的類型
author: Robin
date: 2022/06/10
testing code: 
-----------------------------------------------------
EXEC [dbo].[查詢所有附加檔案的類型]
-----------------------------------------------------*/
CREATE PROCEDURE [dbo].[查詢所有附加檔案的類型] 

AS
BEGIN

select distinct substring(path, charindex('.', path)+1, len(path)-charindex('.', path))
from dbo.Files

END