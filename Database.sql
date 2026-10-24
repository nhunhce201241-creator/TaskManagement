USE master;
GO

-- 1. Tạo Database mới chuẩn chỉnh
CREATE DATABASE TaskManagementDb;
GO

USE TaskManagementDb;
GO

-- 2. Tạo bảng Projects (Quản lý dự án)
CREATE TABLE Projects (
    ProjectId INT IDENTITY(1,1) PRIMARY KEY,
    ProjectName NVARCHAR(255) NOT NULL
);
GO

-- 3. Tạo bảng TaskItems (Quản lý công việc hàng ngày)
CREATE TABLE TaskItems (
    TaskId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(500) NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    Priority NVARCHAR(50) NOT NULL DEFAULT 'Medium',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    ProjectId INT NOT NULL,
    CONSTRAINT FK_TaskItems_Projects FOREIGN KEY (ProjectId) REFERENCES Projects(ProjectId) ON DELETE CASCADE
);
GO

-- 4. Chèn dữ liệu mẫu ban đầu để giao diện có sẵn dữ liệu hiển thị
INSERT INTO Projects (ProjectName) VALUES 
(N'FocusMate Workspace'), 
(N'Dự án cá nhân');
GO

INSERT INTO TaskItems (Title, Status, Priority, CreatedAt, ProjectId) VALUES 
(N'Thiết kế giao diện Pastel Pink', 'Completed', 'High', GETDATE(), 1),
(N'Làm xong phần Todo List mỗi ngày', 'Pending', 'Medium', GETDATE(), 1);
GO