DROP DATABASE IF EXISTS ittoolsdb;
CREATE DATABASE ittoolsdb;
USE ittoolsdb;

-- Bảng người dùng
CREATE TABLE users (
    usid INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(50) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    fullname VARCHAR(100),
    premium BOOLEAN DEFAULT FALSE
);

-- Bảng công cụ (tools)
CREATE TABLE tools (
    tid INT PRIMARY KEY AUTO_INCREMENT,
    tool_name VARCHAR(100) NOT NULL,
    description VARCHAR(100) NOT NULL,
    enabled BOOLEAN DEFAULT TRUE,
    premium_required BOOLEAN DEFAULT FALSE,
    pathtool VARCHAR(100) NOT NULL,
    category_id INT
);

-- Bảng danh mục công cụ (categories)
CREATE TABLE categories (
    category_id INT PRIMARY KEY AUTO_INCREMENT,
    category_name VARCHAR(50) NOT NULL
);

-- Bảng quản trị viên
CREATE TABLE admins (
    aid INT PRIMARY KEY AUTO_INCREMENT,
    admin_name VARCHAR(100) NOT NULL
);

-- Bảng công cụ yêu thích (favorites)
CREATE TABLE favorites (
    usid INT,
    tid INT,
    PRIMARY KEY (usid, tid),
    FOREIGN KEY (usid) REFERENCES users(usid) ON DELETE CASCADE,
    FOREIGN KEY (tid) REFERENCES tools(tid) ON DELETE CASCADE
);

-- Thêm khóa ngoại cho bảng tools
ALTER TABLE tools
ADD CONSTRAINT fk_category FOREIGN KEY (category_id) REFERENCES categories(category_id);

insert into categories(category_name) values
('crypto'),
('converter');

-- Thêm dữ liệu cho bảng tools
INSERT INTO tools (tool_name, description, enabled, premium_required, pathTool, category_id) VALUES
('Hash Text', 'Công cụ mã hóa văn bản giúp bảo vệ dữ liệu bằng các thuật toán băm như MD5, SHA-256.', true, false,'/crypto/hash_text', 1),
('Token Generator', 'Tạo token bảo mật JWT để xác thực và phân quyền trong hệ thống.', true, false,'/crypto/token_generator', 1),
('Date-Time Converter', 'Chuyển đổi giữa các múi giờ và định dạng ngày giờ khác nhau.', true, false, '/converter/date_time_converter', 2),
('Integer Base Converter', 'Chuyển đổi giữa các hệ cơ số như nhị phân, thập phân và thập lục phân.', true, false,'/converter/integer_base_converter', 2);

INSERT INTO tools (tool_name, description, enabled, premium_required, pathTool, category_id) VALUES
('Hash Text 2', 'Công cụ mã hóa văn bản giúp bảo vệ dữ liệu bằng các thuật toán băm như MD5, SHA-256.', true, false,'/crypto/hash_text_2', 1);
select * from tools