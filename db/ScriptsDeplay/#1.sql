CREATE DATABASE [products_db];
GO
USE [products_db];
GO

create view [dbo].[rand_view] as
select rand() value;
go

create function [dbo].[get_random](@min int, @max int) returns int as
begin
	return round(((@max-@min-1)*(select r.value from rand_view r)+@min),0);
end
go

create function [dbo].[get_random_chars](@chars varchar(max), @len int) returns varchar(max) as
begin
	declare @aux varchar(max) set @aux='';
	declare @count int set @count=0;
	while (@count<@len)
	begin
		declare @r int set @r=dbo.get_random(1,len(@chars));
		set @aux+=substring(@chars,@r,1);
		set @count+=1;
	end
	return @aux;
end
go

create function [dbo].[new_id]() returns char(8) as
begin
	return [dbo].[get_random_chars]('0123456789ABCDEFGHIJKLMNOPQRSTUVXWYZabcdefghijklmnopqrstuvxwyz',(8));
end
go

CREATE TABLE [dbo].[products] (
    [id] CHAR(8) NOT NULL DEFAULT dbo.new_id(),  
    [name] NVARCHAR(200) NOT NULL,              
    [description] NVARCHAR(max) NOT NULL,      
    [price] DECIMAL(18, 2) NOT NULL,          
    [stock_quantity] INT NOT NULL,
    [removed] BIT NOT NULL DEFAULT 0,    
	[created_at] DATETIME DEFAULT GETDATE()        
);
GO