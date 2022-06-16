/*--------------------------------------------------
description: 將訊息使用者代號置換為使用者名稱
author: Robin
date: 2022/06/15
testing Code:
--------------------------------------------------
DECLARE @body nvarchar(max) = N'[[user:1534799417]] 請問這功能目前有在使用嗎?'

select dbo.ReplaceUserIdToUserName(@body)
--------------------------------------------------*/
CREATE FUNCTION [dbo].[ReplaceUserIdToUserName]
(
	@body nvarchar(max)
)
RETURNS nvarchar(max)
AS 
BEGIN

IF charindex('[[user:', @body) = 0
BEGIN
	RETURN @body;
END

select @body = replace(@body, U.oldText, U.newText)
			+ iif(charindex(U.oldText, @body)>0, '<cc>cc:'+U.newText+'</cc>', '')
from(
	select oldText='[[user:'+u1.id+']]', newText=u1.[user_name]
	from dbo.Users u1
) U

return @body;

END