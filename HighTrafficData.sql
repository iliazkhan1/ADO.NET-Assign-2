CREATE TABLE Employees (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100),
    JoinDate DATE
);


INSERT INTO Employees (Name, JoinDate) VALUES
('John Doe', DATEADD(MONTH, -2, GETDATE())),
('Jane Smith', DATEADD(MONTH, -5, GETDATE())),
('Alice Johnson', DATEADD(MONTH, -7, GETDATE())),
('Bob Williams', DATEADD(DAY, -20, GETDATE())),
('Charlie Brown', DATEADD(MONTH, -3, GETDATE()));


CREATE INDEX IX_Employees_JoinDate ON Employees(JoinDate);
