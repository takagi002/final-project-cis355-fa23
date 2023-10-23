#!/bin/bash
set -e

# Default user and database created by the postgres Docker image
POSTGRES_USER=postgres
POSTGRES_DB=postgres

# Create UserIdentity database if it doesn't exist
psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE DATABASE "UserIdentity";
    GRANT ALL PRIVILEGES ON DATABASE "UserIdentity" TO postgres;
EOSQL

# Check if Users table exists and seed data if not
psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "UserIdentity" <<-EOSQL
    CREATE TABLE IF NOT EXISTS Users (
        Id SERIAL PRIMARY KEY,
        Username VARCHAR(50) NOT NULL,
        Email VARCHAR(50) NOT NULL,
        PasswordHash VARCHAR(255) NOT NULL,
        CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
    );
EOSQL