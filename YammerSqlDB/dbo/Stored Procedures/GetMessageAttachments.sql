/*---------------------------------------------------- 
description:  查詢指定一筆或多筆訊息裡的附加檔案
author: Robin
date: 2022/06/10
testing code: 
-----------------------------------------------------
DECLARE @message_id varchar(1024);

SET @message_id = '1739726408843264,1741159949402112';

EXEC [dbo].GetMessageAttachments @message_id;
-----------------------------------------------------*/
CREATE PROCEDURE [dbo].[GetMessageAttachments]
	@message_id varchar(1024) /*多筆以 , 分隔*/
AS
BEGIN
DECLARE @Delimiter varchar(4)=','

select message_id=M.[DATA]
	, A.*
from dbo.Split(@message_id, @Delimiter) M
cross apply(
	select * from dbo.MessageAttachments(M.[DATA])
) A


END