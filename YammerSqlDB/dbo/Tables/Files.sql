CREATE TABLE [dbo].[Files] (
    [id]                      NVARCHAR (16)  NOT NULL,
    [file_id]                 NVARCHAR (255) NULL,
    [name]                    NVARCHAR (255) NULL,
    [description]             NVARCHAR (255) NULL,
    [uploader_id]             NVARCHAR (255) NULL,
    [uploader_type]           NVARCHAR (255) NULL,
    [group_id]                NVARCHAR (255) NULL,
    [group_name]              NVARCHAR (255) NULL,
    [reverted_to_id]          NVARCHAR (255) NULL,
    [deleted_by_user_id]      NVARCHAR (255) NULL,
    [in_private_group]        BIT            NOT NULL,
    [in_private_conversation] BIT            NOT NULL,
    [file_api_url]            NVARCHAR (255) NULL,
    [download_url]            NVARCHAR (255) NULL,
    [path]                    NVARCHAR (255) NULL,
    [uploaded_at]             DATETIME       NULL,
    [deleted_at]              NVARCHAR (255) NULL,
    [original_network]        NVARCHAR (255) NULL,
    [storage_type]            NVARCHAR (255) NULL,
    CONSTRAINT [PK_Files] PRIMARY KEY CLUSTERED ([id] ASC)
);

