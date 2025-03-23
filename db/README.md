Run container: docker compose up -d
Export database: docker exec db pg_dump -U postgres -d devutils > db_dump.sql
Restore database: docker exec -i db psql -U myuser -d devutils < db_dump.sql
