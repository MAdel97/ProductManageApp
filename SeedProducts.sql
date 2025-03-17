-- Check if products already exist
IF NOT EXISTS (SELECT 1 FROM Products WHERE Name = 'MacBook Pro 16"')
BEGIN
    -- Electronics category
    INSERT INTO Products (Name, Description, Price, IsAvailable, CreatedAt)
    VALUES ('MacBook Pro 16"', 'Apple MacBook Pro with M2 Pro chip, 16GB RAM, 512GB SSD', 2499.99, 1, GETDATE());
    
    INSERT INTO Products (Name, Description, Price, IsAvailable, CreatedAt)
    VALUES ('Dell XPS 15', 'Dell XPS 15 with Intel i9, 32GB RAM, 1TB SSD, NVIDIA RTX 3050 Ti', 2199.99, 1, GETDATE());
    
    -- Continue with the rest of your product insertions
    -- ...
END 