/*---------------------------------------------------- 
description:  查詢範例
author: Robin
date: 2022/06/08
testing code: 
-----------------------------------------------------
EXEC [dbo].[查詢範例]
-----------------------------------------------------*/
CREATE PROCEDURE [dbo].[查詢範例] 

AS
BEGIN

/*列出所有 group */
select id, group_name, order_num
from dbo.Groups
order by order_num


/*列出某一群組 最新回覆前 10 筆資料 */
select top 10 * 
from dbo.viewMessages M
where M.group_id='5259393'
and M.parent_id=''
order by thread_last_at desc


/*列出某一討論串的所有回覆資料 */
select * 
from dbo.viewMessages M
where M.thread_id='1426527352569856'
and M.parent_id<>''
order by thread_last_at asc

END