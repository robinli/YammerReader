﻿CREATE TABLE [dbo].[Messages] (
    [id]                      NVARCHAR (16)  NOT NULL,
    [replied_to_id]           NVARCHAR (255) NULL,
    [parent_id]               NVARCHAR (255) NULL,
    [thread_id]               NVARCHAR (255) NULL,
    [conversation_id]         NVARCHAR (255) NULL,
    [group_id]                NVARCHAR (255) NULL,
    [group_name]              NVARCHAR (255) NULL,
    [participants]            NVARCHAR (255) NULL,
    [in_private_group]        BIT            NOT NULL,
    [in_private_conversation] BIT            NOT NULL,
    [sender_id]               NVARCHAR (255) NULL,
    [sender_type]             NVARCHAR (255) NULL,
    [sender_name]             NVARCHAR (255) NULL,
    [sender_email]            NVARCHAR (255) NULL,
    [body]                    NVARCHAR (MAX) NULL,
    [delegate_id]             NVARCHAR (255) NULL,
    [api_url]                 NVARCHAR (255) NULL,
    [attachments]             NVARCHAR (MAX) NULL,
    [deleted_by_id]           NVARCHAR (255) NULL,
    [deleted_by_type]         NVARCHAR (255) NULL,
    [created_at]              DATETIME       NULL,
    [deleted_at]              NVARCHAR (255) NULL,
    [title]                   NVARCHAR (255) NULL,
    [html_body]               NVARCHAR (MAX) NULL,
    [message_type]            NVARCHAR (255) NULL,
    [gdpr_delete_url]         NVARCHAR (255) NULL,
    [thread_line_no]          INT            NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED ([id] ASC)
);






GO
CREATE NONCLUSTERED INDEX [idx_Messages_thread_id]
    ON [dbo].[Messages]([thread_id] ASC);

