CREATE DATABASE billing_system;

CREATE TABLE items (
    id SERIAL NOT NULL,
    name VARCHAR(100) NOT NULL,
    manufacturer VARCHAR(100) NULL,
    price DECIMAL(7, 2) NOT NULL,
    discount REAL NOT NULL,
    CONSTRAINT pk_item PRIMARY KEY (id)
);

CREATE UNIQUE INDEX "idx_item_id"
	ON items (id);
