/*---------------------------------------------------- 
description:  批次修改資料
author: Robin
date: 2022/06/16
testing code: 
-----------------------------------------------------
EXEC [dbo].[批次修改資料]
-----------------------------------------------------*/
CREATE PROCEDURE [dbo].[批次修改資料] 

AS
BEGIN

/*修改回覆流水號*/
update M
set M.thread_line_no=P1.thread_line_no
from dbo.Messages M
inner join(
	select id, thread_line_no = ROW_NUMBER() over(partition by thread_id order by created_at)-1
	from dbo.Messages M
	where  M.body<>'[redacted]' 
	and message_type='normal'
) P1
	on P1.id = M.id
where M.thread_line_no <> P1.thread_line_no

END