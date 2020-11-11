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

insert into xue values('张三',12,'男','广西');
insert into xue values('赵四',56,'女','湖南');
insert into xue values('王五',28,'男','东北');
insert into xue values('西西',98,'女','浙江');

select*from xue
