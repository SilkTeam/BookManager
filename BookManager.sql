Create Database BookManager
Go

Use BookManager
Go

--登录信息表
Create Table Sigin
(
	ID int primary key identity(1,1),
	Username varchar(20) not null,
	[Password] varchar(32) not null,-- 读者随机 管理员需要md5加密 测试忽略
	[Identity] int not null,-- 0读者 1图书管理员 2超级管理员
)
Go

--用户信息表
Create Table [User]
(
	ID int primary key identity(1,1),
	[Name] varchar(20) not null,-- 姓名
	Sex int not null,
	Age int,
	[Uid] int foreign key references Sigin(ID),
	Phone varchar(11),
	EntryTime datetime,
)
Go

--分类信息表
Create Table Category
(
	ID int primary key identity(1,1),
	[Name] varchar(20) not null,
)
Go

--图书信息表
Create Table Book
(
	ID int primary key identity(1,1),
	[Name] varchar(50) not null,
	Img varchar(255),
	[Description] text,
	Category int foreign key references Category(ID),
	Number int not null,
	Author varchar(32),
	EntryTime datetime,
)
Go

--借阅信息表
Create Table Borrow
(
	ID int primary key identity(1,1),
	BookID int foreign key references Book(ID),
	[Use] bit not null,-- true为借 false为还
	CardID int foreign key references [User](ID),
	GetTime datetime,
	LoseTime datetime,
)
Go

--日志信息表
Create Table [Log]
(
	ID int primary key identity(1,1),
	[Uid] int foreign key references [User](ID),
	Bid int foreign key references Book(ID),
	Info text not null,
	EntryTime datetime,
)
Go

-- 插入数据
Insert Into Sigin Values('admin','123456','2')-- 超级管理员
Insert Into Sigin Values('root','123456','1')-- 图书管理员
Insert Into [User] Values('admin','0','18','1','17677007700','2020/02/02')-- 超级管理员
Insert Into [User] Values('root','0','18','2','18677337733','2020/02/02')-- 图书管理员
Insert Into Category Values('测试分类')--分类
Insert Into Book Values('书名','','介绍','1','5','作者','2020/11/11')-- 图书
Go