
/*--------------------------------------------------
description: 查詢 Messages
author: Robin
date: 2022/06/16
testing Code:
--------------------------------------------------
select * 
from dbo.viewMessages M
where M.thread_id='1041165328293888' 
order by thread_line_no asc
--------------------------------------------------*/
CREATE VIEW [dbo].[viewMessages]
AS
select M.id, M.replied_to_id, M.parent_id, M.thread_id, M.thread_line_no, M.group_id, M.group_name
	, M.sender_id, M.sender_name
	, body = dbo.ReplaceUserIdToUserName(M.body)
	, M.attachments, M.created_at
	, thread_count = IIF(M.thread_line_no=0, (select max(thread_line_no) from dbo.Messages R where R.thread_id = M.thread_id), 0)
	, thread_last_at = IIF(M.thread_line_no=0, (select max(created_at) from dbo.Messages R where R.thread_id = M.thread_id), M.created_at)
from dbo.Messages M
where M.body<>'[redacted]' 
and M.message_type='normal'