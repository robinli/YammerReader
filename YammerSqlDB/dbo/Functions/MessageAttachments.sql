/*--------------------------------------------------
description: 查詢單一訊息裡的附加檔案
author: Robin
date: 2022/06/10
testing Code:
--------------------------------------------------
DECLARE @message_id varchar(16);

SET @message_id = '1741159949402112';

SELECT * FROM dbo.MessageAttachments(@message_id);
--------------------------------------------------*/
CREATE FUNCTION [dbo].[MessageAttachments]
(
	@message_id varchar(16)
)
RETURNS @AttachmentTable TABLE(
	row_no int
	, file_name nvarchar(255)
	, file_path nvarchar(255)
	, file_type varchar(8)
	, file_url nvarchar(255))
AS
BEGIN

DECLARE @attachments nvarchar(max)
	, @Delimiter varchar(4)=','
	, @uploadedfile_keyword varchar(13) ='uploadedfile:';

select @attachments = M.attachments
from dbo.Messages M
where M.id = @message_id;

insert into @AttachmentTable(row_no, file_name, file_path, file_type, file_url)
select P3.row_no
	, file_name = F.name
	, file_path = F.path
	, file_type = substring(path, charindex('.', path)+1, len(path)-charindex('.', path))
	, file_url = 'Yammer/GetPicture/dir/'+U.user_code+'/id/'+F.file_id+'/'+F.path
from(
	select P2.row_no, P2.file_id
	from(
		select P1.row_no 
			, file_id = IIF(CHARINDEX(@uploadedfile_keyword, P1.attachment)=1, substring(P1.attachment, 14, len(P1.attachment)-13), null)
		from(
			select row_no=s1.ID, attachment = s1.[DATA]
			from dbo.Split(@attachments, @Delimiter) s1
		) P1
	) P2
	where P2.file_id is not null
) P3
inner join dbo.Files F
	on F.file_id = P3.file_id
inner join dbo.Users U
	on U.id = F.uploader_id;

RETURN;
END