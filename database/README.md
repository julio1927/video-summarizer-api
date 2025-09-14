# Database Directory

This directory contains database-related files for the Video Summarizer API.

## Structure

```
database/
├── migrations/          # Entity Framework migrations
├── seeds/              # Database seed data
├── schemas/            # Database schema documentation
├── scripts/            # Utility scripts
└── README.md          # This file
```

## What Goes Here

### ✅ Include in Git
- **Migration files** (`*.cs`) - Entity Framework migrations
- **Seed scripts** (`*.sql`, `*.cs`) - Database seeding scripts
- **Schema documentation** (`*.md`) - Database schema docs
- **Utility scripts** (`*.ps1`, `*.sh`) - Database management scripts
- **Sample data** (`*.json`, `*.csv`) - Non-sensitive test data

### ❌ Ignore in Git
- **Local database files** (`*.db`, `*.db-shm`, `*.db-wal`)
- **Database dumps** (`*.sql` dumps, `*.bak` files)
- **Sensitive data** (production data, user data)
- **Generated files** (auto-generated migrations)

## Usage

### Running Migrations
```bash
# From the API project directory
dotnet ef database update --project apps/api
```

### Seeding Data
```bash
# Run seed scripts
dotnet run --project apps/api --seed
```

### Creating Migrations
```bash
# Add a new migration
dotnet ef migrations add MigrationName --project apps/api
```

## Best Practices

1. **Always review migrations** before committing
2. **Test migrations** on a copy of production data
3. **Document schema changes** in schema/ directory
4. **Use descriptive names** for migrations and scripts
5. **Never commit sensitive data** or production dumps
