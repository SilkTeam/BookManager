create database Stusent
go


create table xue
(  
id int primary key identity(1,1) not null ,
name varchar(32),
age int,
sex varchar(20),
addres varchar(256)

);

insert into xue values('����',12,'��','����');
insert into xue values('����',56,'Ů','����');
insert into xue values('����',28,'��','����');
insert into xue values('����',98,'Ů','�㽭');

select*from xue
