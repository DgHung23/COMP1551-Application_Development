Create DATABASE Comp1551Coursework

use Comp1551Coursework

CREATE TABLE PersonRole (
  Id int PRIMARY KEY IDENTITY(1,1),
  [Name] nvarchar(150) NOT NULL,
  [Description] nvarchar(1000) null,
);


CREATE TABLE People (
  Id int PRIMARY KEY IDENTITY(1,1),
  FullName nvarchar(75) NOT NULL,
  Telephone varchar(15),
  Email varchar(100),
  RoleId int NOT NULL, 
  CONSTRAINT FK_PersonRole
    FOREIGN KEY (RoleId)
    REFERENCES PersonRole(Id)
);