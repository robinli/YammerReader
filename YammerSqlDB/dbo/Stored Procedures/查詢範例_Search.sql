/*---------------------------------------------------- 
description:  查詢範例
author: Robin
date: 2022/06/14
testing code: 
-----------------------------------------------------
EXEC [dbo].[查詢範例_Search]
-----------------------------------------------------*/
CREATE PROCEDURE [dbo].[查詢範例_Search] 

AS
BEGIN

/*關鍵字搜尋 - 第一階*/
WITH P AS(
select id, replied_to_id, parent_id, M.thread_id, thread_line_no
, group_id, group_name, sender_id, sender_name
, body, attachments, created_at, thread_count, thread_last_at
, P1.match_rows
from dbo.viewMessages M
inner join(
	select thread_id, match_rows=count(1)
	from dbo.Messages m0
	where m0.body like N'%報表%'
	group by thread_id
) P1
	on P1.thread_id=M.id
)
select P.*, ttlrows=convert(int, COUNT(1) OVER ())
from P
order by thread_last_at desc
OFFSET 0 ROWS
FETCH NEXT 10 ROWS ONLY;

/*關鍵字搜尋 - 回覆*/
select id, replied_to_id, parent_id, thread_id, thread_line_no
, group_id, group_name, sender_id, sender_name
, body, attachments, created_at
from dbo.Messages m0
where thread_id = '1752982353543168'
and m0.body like N'%報表%'
and parent_id<>''

END