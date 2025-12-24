CREATE DATABASE ShopThoiTrang
GO
USE ShopThoiTrang
GO

-- ===================================================
-- TẠO CÁC BẢNG CƠ BẢN
-- ===================================================

CREATE TABLE CategoryGroup (
    GroupId     INT IDENTITY PRIMARY KEY,
    GroupCode   VARCHAR(40) UNIQUE,
    GroupName   NVARCHAR(120),
    SortOrder   INT DEFAULT 0,
    IsActive    BIT DEFAULT 1
);

CREATE TABLE Category (
    CategoryId INT IDENTITY PRIMARY KEY,
    GroupId    INT NOT NULL,
    CatSlug    VARCHAR(60) UNIQUE,
    CatName    NVARCHAR(120),
    SortOrder  INT DEFAULT 0,
    IsActive   BIT DEFAULT 1,
    CONSTRAINT FK_Category_Group FOREIGN KEY(GroupId) REFERENCES dbo.CategoryGroup(GroupId)
);

CREATE TABLE Product (
    ProductId   INT IDENTITY PRIMARY KEY,
    SKU         VARCHAR(40) NOT NULL UNIQUE,
    ProductName NVARCHAR(180) NOT NULL,
    Slug        VARCHAR(120) NOT NULL UNIQUE,
    Description NVARCHAR(MAX) NULL,
    BasePrice   DECIMAL(12,0) NOT NULL,
    IsActive    BIT NOT NULL DEFAULT 1,
    CreatedAt   DATETIME2 DEFAULT SYSUTCDATETIME(),
    CategoryId  INT NOT NULL,
    FOREIGN KEY(CategoryId) REFERENCES dbo.Category(CategoryId)
);

CREATE TABLE Size (
    SizeId INT IDENTITY PRIMARY KEY,
    SizeCode VARCHAR(10) NOT NULL UNIQUE,
    SortOrder INT NOT NULL
);

CREATE TABLE dbo.Color (
    ColorId INT IDENTITY PRIMARY KEY,
    ColorHex VARCHAR(30) UNIQUE,
    ColorName NVARCHAR(50)
);

CREATE TABLE ProductVariant (
    VariantId INT IDENTITY PRIMARY KEY,
    ProductId INT NOT NULL,
    SizeId    INT NOT NULL,
    ColorId   INT NOT NULL,
    Price     DECIMAL(12,0) NOT NULL,
    Stock     INT NOT NULL DEFAULT 0,
    SKU       VARCHAR(50) UNIQUE,
    CONSTRAINT FK_Variant_Product FOREIGN KEY(ProductId) REFERENCES dbo.Product(ProductId),
    CONSTRAINT FK_Variant_Size FOREIGN KEY(SizeId) REFERENCES dbo.Size(SizeId),
    CONSTRAINT FK_Variant_Color FOREIGN KEY(ColorId) REFERENCES dbo.Color(ColorId),
    CONSTRAINT UQ_ProductVariant UNIQUE(ProductId, SizeId, ColorId)
);

CREATE TABLE AppUser (
    UserId          INT IDENTITY(1,1) PRIMARY KEY,
    Email           VARCHAR(120) NOT NULL UNIQUE,
    PasswordHash    VARCHAR(256) NOT NULL,
    FullName        NVARCHAR(120) NULL,
    Phone           VARCHAR(20) NULL,
    IsActive        BIT NOT NULL DEFAULT(1),
    CreatedAt       DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE dbo.Cart(
    CartId      INT IDENTITY(1,1) PRIMARY KEY,
    CartToken   VARCHAR(64) NOT NULL UNIQUE,
    UserId      INT NULL,
    CreatedAt   DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt   DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Cart_User FOREIGN KEY(UserId) REFERENCES dbo.AppUser(UserId)
);

CREATE TABLE CartItem (
    CartItemId INT IDENTITY PRIMARY KEY,
    CartId     INT NOT NULL,
    VariantId  INT NOT NULL,
    Quantity   INT CHECK (Quantity > 0),
    UnitPrice  DECIMAL(12,0) NOT NULL,
    FOREIGN KEY(CartId) REFERENCES dbo.Cart(CartId),
    FOREIGN KEY(VariantId) REFERENCES dbo.ProductVariant(VariantId)
);

CREATE TABLE CustomerAddress(
    AddressId   INT IDENTITY(1,1) PRIMARY KEY,
    UserId      INT NOT NULL,
    Line1       NVARCHAR(200) NOT NULL,
    Ward        NVARCHAR(100) NULL,
    District    NVARCHAR(100) NULL,
    Province    NVARCHAR(100) NULL,
    Note        NVARCHAR(200) NULL,
    CreatedAt   DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_CustomerAddress_User FOREIGN KEY(UserId) REFERENCES AppUser(UserId)
);

CREATE TABLE [Order](
    OrderId         INT IDENTITY(1,1) PRIMARY KEY,
    OrderCode       VARCHAR(30) NOT NULL UNIQUE,
    UserId          INT NULL,
    CustomerName    NVARCHAR(120) NOT NULL,
    Phone           VARCHAR(20) NOT NULL,
    AddressLine     NVARCHAR(220) NULL,
    MessageCard     NVARCHAR(240) NULL,
    Status          VARCHAR(20) NOT NULL DEFAULT('PENDING'),
    TotalAmount     DECIMAL(12,0) NOT NULL DEFAULT(0),
    CreatedAt       DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Order_User FOREIGN KEY(UserId) REFERENCES AppUser(UserId)
);

CREATE TABLE OrderItem (
    OrderItemId INT IDENTITY PRIMARY KEY,
    OrderId     INT NOT NULL,
    VariantId   INT NOT NULL,
    ProductName NVARCHAR(180),
    SizeName    NVARCHAR(50),
    ColorName   NVARCHAR(50),
    Quantity    INT,
    UnitPrice   DECIMAL(12,0),
    FOREIGN KEY(OrderId) REFERENCES [Order](OrderId)
);

CREATE TABLE [Role](
    Rid INT IDENTITY PRIMARY KEY,
    RName NVARCHAR(20)
);

CREATE TABLE UserRole (
    Rid INT,
    UserId INT,
    PRIMARY KEY (Rid, UserId),
    FOREIGN KEY (Rid) REFERENCES [Role](Rid),
    FOREIGN KEY (UserId) REFERENCES AppUser(UserId)
);

CREATE TABLE ProductImage(
    ImageId     INT IDENTITY(1,1) PRIMARY KEY,
    ProductId   INT NOT NULL,
    ImageUrl    NVARCHAR(260) NOT NULL,
    IsPrimary   BIT NOT NULL DEFAULT(0),
    SortOrder   INT NOT NULL DEFAULT(0),
    CONSTRAINT FK_ProductImage_Product FOREIGN KEY(ProductId) REFERENCES dbo.Product(ProductId)
);

-- ===================================================
-- BẢNG MỚI: ĐÁNH GIÁ SẢN PHẨM
-- ===================================================
CREATE TABLE ProductReview (
    ReviewId        INT IDENTITY(1,1) PRIMARY KEY,
    ReviewCode      VARCHAR(30) NOT NULL UNIQUE,
    OrderItemId     INT NOT NULL,
    Comment         NVARCHAR(500) NULL,
    CreatedAt       DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Review_OrderItem FOREIGN KEY(OrderItemId) REFERENCES OrderItem(OrderItemId)
);

-- ===================================================
-- NHẬP DỮ LIỆU
-- ===================================================

-- Insert Size
INSERT Size(SizeCode, SortOrder) VALUES
('S',1),('M',2),('L',3),('XL',4),('2XL',5),('3XL',6);

-- Insert Color
INSERT dbo.Color (ColorHex, ColorName) VALUES
('#000000', N'Đen'),
('#FFFFFF', N'Trắng'),
('#FF0000', N'Đỏ'),
('#0000FF', N'Xanh dương'),
('#00FF00', N'Xanh lá'),
('#F5DEB3', N'Be');

-- Insert CategoryGroup
INSERT INTO CategoryGroup (GroupCode, GroupName, SortOrder, IsActive) VALUES
('ao', N'Áo', 0, 1),
('quan', N'Quần', 1, 1),
('phu-kien', N'Phụ Kiện', 2, 1);

-- Insert Category
INSERT INTO Category (GroupId, CatSlug, CatName, SortOrder, IsActive) VALUES
(1, 'ao-thun', N'Áo Thun', 0, 1),
(1, 'ao-so-mi', N'Áo Sơ Mi', 1, 1),
(1, 'ao-khoac', N'Áo Khoác', 2, 1),
(3, 'non', N'Nón', 0, 1),
(3, 'vo', N'Vớ', 1, 1),
(2, 'quan-short', N'Quần Short', 0, 1),
(2, 'quan-dai', N'Quần Dài', 1, 1);

-- Insert Product (ĐÃ SỬA ĐÚNG CATEGORYID)
INSERT INTO Product (SKU, ProductName, Slug, Description, BasePrice, IsActive, CreatedAt, CategoryId)
VALUES
-- CategoryId = 1: Áo Thun
('P-001', N'Áo Thun Waffle Đen', 'ao-thun-waffle-den', N'Áo thun waffle thoáng mát, dễ phối đồ', 120650, 1, GETDATE(), 1),
('P-002', N'Áo Thun Pique Seventy Seven 013 Đen', 'ao-thun-pique-013', N'Áo thun pique cao cấp, form dáng basic', 149150, 1, GETDATE(), 1),
('P-003', N'Áo Thun Thể Thao Ultra Thin The Beginner 001 Đỏ Đậm', 'ao-thun-the-thao-001', N'Áo thun thể thao siêu mỏng, thấm hút tốt', 149150, 1, GETDATE(), 1),

-- CategoryId = 2: Áo Sơ Mi
('P-004', N'Áo Sơ Mi Trắng Tay Dài', 'ao-so-mi-trang-tay-dai', N'Áo sơ mi công sở lịch sự', 299000, 1, GETDATE(), 2),
('P-005', N'Áo Sơ Mi Xanh Nhạt', 'ao-so-mi-xanh-nhat', N'Áo sơ mi trẻ trung năng động', 299000, 1, GETDATE(), 2),
('P-006', N'Áo Sơ Mi Caro', 'ao-so-mi-caro', N'Áo sơ mi caro phong cách casual', 319000, 1, GETDATE(), 2),

-- CategoryId = 3: Áo Khoác
('P-007', N'Áo Khoác Kaki', 'ao-khoac-kaki', N'Áo khoác kaki phong cách Hàn Quốc', 499000, 1, GETDATE(), 3),
('P-008', N'Áo Khoác Jean', 'ao-khoac-jean', N'Áo khoác jean bền đẹp', 549000, 1, GETDATE(), 3),
('P-009', N'Áo Khoác Dù', 'ao-khoac-du', N'Áo khoác chống gió chống nước', 459000, 1, GETDATE(), 3),

-- CategoryId = 4: Nón
('P-010', N'Nón Lưỡi Trai Đen', 'non-luoi-trai-den', N'Nón lưỡi trai thời trang', 159000, 1, GETDATE(), 4),
('P-011', N'Nón Lưỡi Trai Trắng', 'non-luoi-trai-trang', N'Nón lưỡi trai basic', 159000, 1, GETDATE(), 4),
('P-012', N'Nón Bucket', 'non-bucket', N'Nón bucket phong cách Hàn Quốc', 179000, 1, GETDATE(), 4),

-- CategoryId = 5: Vớ
('P-013', N'Vớ Cổ Thấp', 'vo-co-thap', N'Vớ cotton thoáng khí', 49000, 1, GETDATE(), 5),
('P-014', N'Vớ Cổ Cao', 'vo-co-cao', N'Vớ thể thao co giãn tốt', 59000, 1, GETDATE(), 5),
('P-015', N'Vớ Trơn Basic', 'vo-tron-basic', N'Vớ trơn mặc hằng ngày', 45000, 1, GETDATE(), 5),

-- CategoryId = 6: Quần Short
('P-016', N'Quần Short Kaki', 'quan-short-kaki', N'Quần short kaki thoải mái', 259000, 1, GETDATE(), 6),
('P-017', N'Quần Short Jean', 'quan-short-jean', N'Quần short jean năng động', 279000, 1, GETDATE(), 6),
('P-018', N'Quần Short Thể Thao', 'quan-short-the-thao', N'Quần short vận động thoáng mát', 229000, 1, GETDATE(), 6),

-- CategoryId = 7: Quần Dài
('P-019', N'Quần Jean Slimfit', 'quan-jean-slimfit', N'Quần jean ôm vừa vặn', 399000, 1, GETDATE(), 7),
('P-020', N'Quần Tây Công Sở', 'quan-tay-cong-so', N'Quần tây lịch sự', 429000, 1, GETDATE(), 7),
('P-021', N'Quần Jogger', 'quan-jogger', N'Quần jogger phong cách thể thao', 349000, 1, GETDATE(), 7);
GO

-- Insert ProductImage
INSERT INTO ProductImage (ProductId, ImageUrl, IsPrimary, SortOrder)
VALUES
-- Áo Thun
(1, 'ao-thun1.jpg', 1, 0),
(2, 'ao-thun2.jpg', 1, 0),
(3, 'ao-thun3.jpg', 1, 0),
-- Áo Sơ Mi
(4, 'so-mi1.jpg', 1, 0),
(5, 'so-mi2.jpg', 1, 0),
(6, 'so-mi3.jpg', 1, 0),
-- Áo Khoác
(7, 'khoac1.jpg', 1, 0),
(8, 'khoac2.jpg', 1, 0),
(9, 'khoac3.jpg', 1, 0),
-- Nón
(10, 'non1.jpg', 1, 0),
(11, 'non2.jpg', 1, 0),
(12, 'non3.jpg', 1, 0),
-- Vớ
(13, 'vo1.jpg', 1, 0),
(14, 'vo2.jpg', 1, 0),
(15, 'vo3.jpg', 1, 0),
-- Quần Short
(16, 'quan-short1.jpg', 1, 0),
(17, 'quan-short2.jpg', 1, 0),
(18, 'quan-short3.jpg', 1, 0),
-- Quần Dài
(19, 'quan-dai1.jpg', 1, 0),
(20, 'quan-dai2.jpg', 1, 0),
(21, 'quan-dai3.jpg', 1, 0);
GO

-- Insert ProductVariant
INSERT INTO ProductVariant (ProductId, SizeId, ColorId, Price, Stock, SKU)
VALUES
-- ProductId 1-3: Áo Thun
(1, 1, 1, 120650, 50, 'P001-S-BLK'),
(1, 2, 1, 120650, 30, 'P001-M-BLK'),
(2, 1, 1, 149150, 40, 'P002-S-BLK'),
(2, 2, 1, 149150, 20, 'P002-M-BLK'),
(3, 1, 3, 149150, 50, 'P003-S-RED'),

-- ProductId 4-6: Áo Sơ Mi
(4, 1, 2, 299000, 40, 'P004-S-WHT'),
(4, 2, 2, 299000, 30, 'P004-M-WHT'),
(5, 1, 5, 299000, 35, 'P005-S-GRN'),
(6, 2, 1, 319000, 20, 'P006-M-BLK'),

-- ProductId 7-9: Áo Khoác
(7, 2, 1, 499000, 25, 'P007-M-BLK'),
(8, 2, 4, 549000, 20, 'P008-M-BLU'),
(9, 3, 6, 459000, 15, 'P009-L-BE'),

-- ProductId 10-12: Nón
(10, 1, 1, 159000, 100, 'P010-ONE-BLK'),
(11, 1, 2, 159000, 80, 'P011-ONE-WHT'),
(12, 1, 6, 179000, 60, 'P012-ONE-BE'),

-- ProductId 13-15: Vớ
(13, 1, 1, 49000, 200, 'P013-ONE-BLK'),
(14, 1, 4, 59000, 150, 'P014-ONE-BLU'),
(15, 1, 2, 45000, 180, 'P015-ONE-WHT'),

-- ProductId 16-18: Quần Short
(16, 2, 1, 259000, 50, 'P016-M-BLK'),
(16, 3, 1, 259000, 30, 'P016-L-BLK'),
(17, 2, 1, 279000, 40, 'P017-M-BLK'),
(17, 3, 1, 279000, 20, 'P017-L-BLK'),
(18, 1, 1, 229000, 50, 'P018-S-BLK'),

-- ProductId 19-21: Quần Dài
(19, 2, 1, 399000, 30, 'P019-M-BLK'),
(19, 3, 1, 399000, 20, 'P019-L-BLK'),
(20, 2, 1, 429000, 25, 'P020-M-BLK'),
(21, 3, 1, 349000, 15, 'P021-L-BLK');
GO

-- ===================================================
-- HOÀN THÀNH
-- ===================================================
PRINT N'Database ShopThoiTrang đã được tạo thành công!';
PRINT N'Bảng ProductReview đã được thêm để lưu đánh giá sản phẩm.';
GO