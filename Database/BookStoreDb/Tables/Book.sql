CREATE TABLE Book 
  ( 
     Id          BIGINT IDENTITY(1, 1) NOT NULL, 
     Title       NVARCHAR(100) NOT NULL, 
     CategoryId  BIGINT NOT NULL, 
     PublisherId BIGINT NOT NULL, 
     PRIMARY KEY (Id), 
     FOREIGN KEY (CategoryId) REFERENCES BookCategory(Id), 
     FOREIGN KEY (PublisherId) REFERENCES Publisher(Id) 
  ) 