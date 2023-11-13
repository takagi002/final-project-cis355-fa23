#!/bin/bash

# Function to log messages
log() {
    echo "[$(date +'%Y-%m-%dT%H:%M:%S%z')] $1"
}

# Check if the PostgreSQL repository configuration already exists
PG_REPO_FILE="/etc/apt/sources.list.d/pgdg.list"
PG_REPO_CONTENT="deb https://apt.postgresql.org/pub/repos/apt $(lsb_release -cs)-pgdg main"

if [[ -f "$PG_REPO_FILE" && $(cat "$PG_REPO_FILE") == "$PG_REPO_CONTENT" ]]; then
    log "PostgreSQL repository configuration already exists."
else
    log "Creating PostgreSQL repository configuration..."
    sudo sh -c "echo '$PG_REPO_CONTENT' > $PG_REPO_FILE" && \
    log "Repository configuration created successfully." || \
    { log "Failed to create repository configuration."; exit 1; }
fi

# Import the repository signing key
PG_KEY_URL="https://www.postgresql.org/media/keys/ACCC4CF8.asc"

if sudo apt-key list | grep -q $(echo $PG_KEY_URL | awk -F '/' '{print $NF}' | cut -c1-8); then
    log "PostgreSQL repository signing key already imported."
else
    log "Importing PostgreSQL repository signing key..."
    wget --quiet -O - $PG_KEY_URL | sudo apt-key add - && \
    log "Repository signing key imported successfully." || \
    { log "Failed to import repository signing key."; exit 1; }
fi

# Update the package lists
log "Updating package lists..."
sudo apt-get update && \
log "Package lists updated successfully." || \
{ log "Failed to update package lists."; exit 1; }

# Install PostgreSQL
log "Installing PostgreSQL..."
if dpkg -s postgresql > /dev/null 2>&1; then
    log "PostgreSQL is already installed."
else
    sudo apt-get -y install postgresql && \
    log "PostgreSQL installed successfully." || \
    { log "Failed to install PostgreSQL."; exit 1; }
fi

# Database connection parameters
DB_HOST="localhost"     
DB_PORT="5432"         
DB_USER="postgres"     
DB_PASSWORD="postgres" 
DB_NAME="postgres"   

# Directory containing your SQL scripts
SCRIPTS_DIR="scripts/databaseInit"

# Check if the scripts directory exists
if [ ! -d "$SCRIPTS_DIR" ]; then
    log "Scripts directory not found: $SCRIPTS_DIR"
    exit 1
fi

# Iterate over each SQL file in the directory
for script in "$SCRIPTS_DIR"/*.sql; do
    # Check if the file exists
    if [ ! -f "$script" ]; then
        log "No SQL files found in $SCRIPTS_DIR"
        continue
    fi

    log "Running script: $script"

    # Execute the SQL script
    psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" -d "$DB_NAME" -f "$script" && \
    log "Successfully executed $script" || \
    { log "Error occurred while executing $script"; exit 1; }
done

# Database connection parameters
DB_HOST="localhost"     
DB_PORT="5432"         
DB_USER="postgres"     
DB_PASSWORD="postgres" 
DB_NAME="UserDb"   

# Directory containing your SQL scripts
SCRIPTS_DIR="scripts/schemaInit"

# Check if the scripts directory exists
if [ ! -d "$SCRIPTS_DIR" ]; then
    log "Scripts directory not found: $SCRIPTS_DIR"
    exit 1
fi

# Iterate over each SQL file in the directory
for script in "$SCRIPTS_DIR"/*.sql; do
    # Check if the file exists
    if [ ! -f "$script" ]; then
        log "No SQL files found in $SCRIPTS_DIR"
        continue
    fi

    log "Running script: $script"

    # Execute the SQL script
    psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" -d "$DB_NAME" -f "$script" && \
    log "Successfully executed $script" || \
    { log "Error occurred while executing $script"; exit 1; }
done

log "All scripts executed successfully."