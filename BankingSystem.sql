CREATE TABLE Accounts (
    AccountId INT PRIMARY KEY IDENTITY(1,1),
    AccountType NVARCHAR(50),  -- 'Savings' or 'Checking'
    CustomerName NVARCHAR(100),
    Balance DECIMAL(18,2)
);

INSERT INTO Accounts (AccountType, CustomerName, Balance) VALUES
('Savings', 'Iliaz', 1000.00),
('Checking', 'Iliaz', 500.00);

