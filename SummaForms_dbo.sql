create table __EFMigrationsHistory
(
    MigrationId    nvarchar(150) not null
        constraint PK___EFMigrationsHistory
            primary key,
    ProductVersion nvarchar(32)  not null
)
go

INSERT INTO SummaForms.dbo.__EFMigrationsHistory (MigrationId, ProductVersion) VALUES (N'20200508094932_InitialMigration', N'3.1.3');
INSERT INTO SummaForms.dbo.__EFMigrationsHistory (MigrationId, ProductVersion) VALUES (N'20200515094656_ChangeRepositoryModel', N'3.1.3');
INSERT INTO SummaForms.dbo.__EFMigrationsHistory (MigrationId, ProductVersion) VALUES (N'20200528204819_AddQuestionValueProperty', N'3.1.3');
INSERT INTO SummaForms.dbo.__EFMigrationsHistory (MigrationId, ProductVersion) VALUES (N'20200529091515_AddIndexProperty', N'3.1.3');

create table FormCategory
(
    Id   uniqueidentifier not null
        constraint PK_FormCategory
            primary key,
    Name nvarchar(max)
)
go

INSERT INTO SummaForms.dbo.FormCategory (Id, Name) VALUES (N'4A00AE11-313A-470B-9C1D-B5AC981C583B', N'Administratie & dienstverlening');
INSERT INTO SummaForms.dbo.FormCategory (Id, Name) VALUES (N'DD404B62-6C8C-4525-A378-CD136B5F88CA', N'Software & ICT');
INSERT INTO SummaForms.dbo.FormCategory (Id, Name) VALUES (N'1AE68A11-7595-465C-B21F-EDB29599A38B', N'Zorg & hulpverlening');

create table Forms
(
    Id          uniqueidentifier not null
        constraint PK_Forms
            primary key,
    AuthorId    uniqueidentifier not null,
    CategoryId  uniqueidentifier
        constraint FK_Forms_FormCategory_CategoryId
            references FormCategory,
    Title       nvarchar(max),
    Description nvarchar(max),
    TimeCreated datetime2        not null
)
go

create index IX_Forms_CategoryId
    on Forms (CategoryId)
go

INSERT INTO SummaForms.dbo.Forms (Id, AuthorId, CategoryId, Title, Description, TimeCreated) VALUES (N'A4F27885-C3D7-4418-9924-BA844B5A05F0', N'025CF29C-AA1D-4793-AC18-FF4B2334DA4F', N'DD404B62-6C8C-4525-A378-CD136B5F88CA', N'Development form', N'Hello world!', N'2020-06-02 21:17:00.0000000');

create table Question
(
    Id      uniqueidentifier not null
        constraint PK_Question
            primary key,
    Type    int              not null,
    FormId  uniqueidentifier
        constraint FK_Question_Forms_FormId
            references Forms,
    Value   nvarchar(max),
    [Index] int default 0    not null
)
go

create index IX_Question_FormId
    on Question (FormId)
go

INSERT INTO SummaForms.dbo.Question (Id, Type, FormId, Value, [Index]) VALUES (N'0A88C039-B4B4-4B74-AD05-48EB887577C9', 2, N'A4F27885-C3D7-4418-9924-BA844B5A05F0', N'How would you rate this form?', 2);
INSERT INTO SummaForms.dbo.Question (Id, Type, FormId, Value, [Index]) VALUES (N'844A3FFA-F3DD-47E5-91F5-8C48CE070F61', 3, N'A4F27885-C3D7-4418-9924-BA844B5A05F0', N'What is your favorite programming joke?', 3);
INSERT INTO SummaForms.dbo.Question (Id, Type, FormId, Value, [Index]) VALUES (N'65FF4F14-E08A-4FF8-A594-B2058109B325', 0, N'A4F27885-C3D7-4418-9924-BA844B5A05F0', N'Hello world isn''t a question, or is it?', 0);
INSERT INTO SummaForms.dbo.Question (Id, Type, FormId, Value, [Index]) VALUES (N'F7B47E46-D917-4BCC-A8B0-DCA01B041080', 1, N'A4F27885-C3D7-4418-9924-BA844B5A05F0', N'Who is guaranteed to like this application? ', 1);

create table QuestionOption
(
    Id         uniqueidentifier not null
        constraint PK_QuestionOption
            primary key,
    Type       int              not null,
    Value      nvarchar(max),
    QuestionId uniqueidentifier
        constraint FK_QuestionOption_Question_QuestionId
            references Question,
    [Index]    int default 0    not null
)
go

create index IX_QuestionOption_QuestionId
    on QuestionOption (QuestionId)
go

INSERT INTO SummaForms.dbo.QuestionOption (Id, Type, Value, QuestionId, [Index]) VALUES (N'69D934ED-5107-4F47-B71B-1DF7DC3C5C2B', 2, N'1,50', N'0A88C039-B4B4-4B74-AD05-48EB887577C9', 0);
INSERT INTO SummaForms.dbo.QuestionOption (Id, Type, Value, QuestionId, [Index]) VALUES (N'426234C8-DEBC-4790-A15A-5B9CD2CD6668', 1, N'Me', N'F7B47E46-D917-4BCC-A8B0-DCA01B041080', 1);
INSERT INTO SummaForms.dbo.QuestionOption (Id, Type, Value, QuestionId, [Index]) VALUES (N'52B84CEB-A271-4162-A0A1-9A9E9550799C', 0, N'No, it isn''t', N'65FF4F14-E08A-4FF8-A594-B2058109B325', 1);
INSERT INTO SummaForms.dbo.QuestionOption (Id, Type, Value, QuestionId, [Index]) VALUES (N'74A07312-92E4-4078-A160-9CFB41392478', 1, N'Everybody', N'F7B47E46-D917-4BCC-A8B0-DCA01B041080', 2);
INSERT INTO SummaForms.dbo.QuestionOption (Id, Type, Value, QuestionId, [Index]) VALUES (N'209DC003-73B9-4599-BF55-CD917BC67C86', 0, N'Yes, it is', N'65FF4F14-E08A-4FF8-A594-B2058109B325', 0);
INSERT INTO SummaForms.dbo.QuestionOption (Id, Type, Value, QuestionId, [Index]) VALUES (N'6D4B4B22-D05F-4DCD-B3BB-F7781519533F', 1, N'You', N'F7B47E46-D917-4BCC-A8B0-DCA01B041080', 0);

create table Repository
(
    Id          uniqueidentifier                                not null
        constraint PK_Repository
            primary key,
    FormId      uniqueidentifier
        constraint FK_Repository_Forms_FormId
            references Forms,
    PublishDate datetime2 default '0001-01-01T00:00:00.0000000' not null
)
go

create index IX_Repository_FormId
    on Repository (FormId)
go

INSERT INTO SummaForms.dbo.Repository (Id, FormId, PublishDate) VALUES (N'73B1DA83-8D6E-4FE5-8CE9-D69A78F1002E', N'A4F27885-C3D7-4418-9924-BA844B5A05F0', N'2020-02-06 21:25:00.0000000');