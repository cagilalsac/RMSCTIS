use RMSCTISDB

delete from userresources
delete from resources
delete from users
delete from roles

set identity_insert roles on
insert into roles (Id, Name) values (1, 'admin')
insert into roles (Id, Name) values (2, 'user')
set identity_insert roles off

set identity_insert users on
insert into users (Id, IsActive, Password, RoleId, Status, UserName) values (1, 1, 'cagil', 1, 1, 'cagil')
insert into users (Id, IsActive, Password, RoleId, Status, UserName) values (2, 1, 'leo', 2, 2, 'leo')
insert into users (Id, IsActive, Password, RoleId, Status, UserName) values (3, 1, 'luna', 2, 3, 'luna')
set identity_insert users off

set identity_insert resources on
insert into resources (Id, Content, Date, Score, Title) values (1, 'Primitive data types and variables.', '2023-01-09 12:30:00', 4.5, 'C# Types and Variables')
insert into resources (Id, Content, Date, Score, Title) values (2, 'Methods, if, switch and ternary conditionals.', '2023-03-27 16:15:20', 4, 'C# Methods and Conditionals')
insert into resources (Id, Content, Date, Score, Title) values (3, 'Loops and arrays as reference types.', '2023-05-15 19:45:40', 3.5, 'C# Loops and Collections')

insert into resources (Id, Content, Date, Score, Title) values (4, 'Select, alias, from, where, order by, group by.', '2022-02-14 04:55:10', 5, 'SQL Select')
insert into resources (Id, Content, Date, Score, Title) values (5, 'Inner join, left and right outer joins.', '2022-08-17 14:35:50', 4.5, 'SQL Joins')

insert into resources (Id, Content, Date, Score, Title) values (6, 'Controllers, actions and views.', '2023-09-10 16:17:35', 3, 'ASP.NET Core MVC 1')
insert into resources (Id, Content, Date, Score, Title) values (7, 'Layout, shared and partial views.', '2023-09-15 03:42:00', 2.5, 'ASP.NET Core MVC 2')
insert into resources (Id, Content, Date, Score, Title) values (8, 'Razor syntax with HTML and tag helpers.', '2023-09-20 07:13:10', 3, 'ASP.NET Core MVC 3')
insert into resources (Id, Content, Date, Score, Title) values (9, 'View models, view bag and view data.', '2023-09-25 14:55:25', 4.5, 'ASP.NET Core MVC 4')
set identity_insert resources off

insert into userresources (ResourceId, UserId) values (1, 2)
insert into userresources (ResourceId, UserId) values (2, 2)
insert into userresources (ResourceId, UserId) values (3, 2)

insert into userresources (ResourceId, UserId) values (4, 1)
insert into userresources (ResourceId, UserId) values (5, 1)

insert into userresources (ResourceId, UserId) values (6, 2)
insert into userresources (ResourceId, UserId) values (7, 2)
insert into userresources (ResourceId, UserId) values (8, 2)
insert into userresources (ResourceId, UserId) values (9, 2)