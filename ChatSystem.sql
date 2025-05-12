CREATE TABLE ChatMessages (
    MessageId INT PRIMARY KEY IDENTITY(1,1),
    Sender NVARCHAR(100) NOT NULL,
    Receiver NVARCHAR(100) NOT NULL,
    MessageText NVARCHAR(MAX) NOT NULL,
    SentTime DATETIME DEFAULT GETDATE()
);
GO

INSERT INTO ChatMessages (Sender, Receiver, MessageText)
VALUES 
('Alice', 'Bob', 'Hey Bob! Are you free for a quick call?'),
('Bob', 'Alice', 'Sure, give me 5 mins.'),
('Charlie', 'Dave', 'Let’s meet at 2 PM today.');

SELECT * FROM ChatMessages;