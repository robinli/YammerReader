/*--------------------------------------------------
description: 查詢 Messages
author: Robin
date: 2016/11/24
testing Code:
--------------------------------------------------
select top 10 * from dbo.viewMessages M
where M.group_id='5259393'
and M.parent_id=''
order by thread_last_at desc
--------------------------------------------------*/
CREATE VIEW [dbo].[viewMessages]
AS
select M.id, M.replied_to_id, M.parent_id, M.thread_id, M.group_id, M.group_name
	, M.sender_id, M.sender_name
	, M.body
	, M.attachments, M.created_at
	, thread_count = (select count(1)-1 from dbo.Messages R where R.thread_id = M.thread_id)
	, thread_last_at = (select max(created_at) from dbo.Messages R where R.thread_id = M.thread_id)
from dbo.Messages M