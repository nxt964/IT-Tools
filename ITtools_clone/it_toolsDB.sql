DROP DATABASE IF EXISTS ittoolsdb;
CREATE DATABASE ittoolsdb;
USE ittoolsdb;

-- Bảng người dùng
CREATE TABLE users (
    usid INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(50) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    email VARCHAR(100),
    premium BOOLEAN DEFAULT FALSE,
    is_admin BOOLEAN DEFAULT FALSE
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