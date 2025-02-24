CREATE TABLE IF NOT EXISTS ImportHistory (
    ImportID INT PRIMARY KEY AUTO_INCREMENT,
    ImportDate DATETIME NOT NULL,
    ImportType VARCHAR(50) NOT NULL,
    FileName VARCHAR(255) NOT NULL,
    RowsProcessed INT DEFAULT 0,
    RowsSuccessful INT DEFAULT 0,
    RowsFailed INT DEFAULT 0,
    CompletedDate DATETIME,
    ErrorLog TEXT
);

CREATE TABLE IF NOT EXISTS ImportErrors (
    ErrorID INT PRIMARY KEY AUTO_INCREMENT,
    ImportID INT NOT NULL,
    RowNumber INT NOT NULL,
    ErrorMessage TEXT NOT NULL,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ImportID) REFERENCES ImportHistory(ImportID)
);

CREATE TABLE IF NOT EXISTS ImportStaging (
    StagingID INT PRIMARY KEY AUTO_INCREMENT,
    ImportID INT NOT NULL,
    RowNumber INT NOT NULL,
    RawData JSON NOT NULL,
    ValidationStatus VARCHAR(20) DEFAULT 'Pending',
    ErrorMessage TEXT,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ImportID) REFERENCES ImportHistory(ImportID)
); 