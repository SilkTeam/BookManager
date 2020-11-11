Create Database BookManager
Go

Use BookManager
Go

Create Table Sigin
(
	ID int primary key identity(1,1),
	Username varchar(20) not null,
	[Password] varchar(32) not null,-- �������
	[Identity] int not null,-- 0���� 1ͼ�����Ա 2��������Ա
)
Go

Create Table [User]
(
	ID int primary key identity(1,1),
	CardID varchar(32) not null,-- ���߿��� md5�û���
	[Name] varchar(20) not null,-- ����
	Sex int not null,
	Age int,
	[Uid] int foreign key references Sigin(ID),
	EntryTime datetime,
)
Go

Create Table Category
(
	ID int primary key identity(1,1),
	[Name] varchar(20) not null,
)
Go

Create Table Book
(
	ID int primary key identity(1,1),
	BookID varchar(32) not null,
	[Name] varchar(50) not null,
	[Description] text,
	Category int foreign key references Category(ID),
	Number int not null,
	Author varchar(32),
	EntryTime datetime,
)
Go

Create Table Borrow
(
	ID int primary key identity(1,1),
	BookID int foreign key references Book(ID),
	CardID int foreign key references [User](ID),
	EntryTime datetime,
)
Go

Create Table [Log]
(
	ID int primary key identity(1,1),
	Info text not null,
	EntryTime datetime,
)
Go