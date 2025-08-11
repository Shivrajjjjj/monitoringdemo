MONITORING STACK (Prometheus + Grafana + Loki)


1. PREREQUISITES
----------------
- Install Docker & Docker Compose on your system.
- Keep docker-compose.yml and config files in the same folder.

**package**
 dotnet add package prometheus-net.AspNetCore
 dotnet add package Serilog.AspNetCore
 dotnet add package Serilog.Sinks.Grafana.Loki
 dotnet add package OpenTelemetry
  dotnet add package OpenTelemetry.Exporter.Prometheus
 dotnet add package OpenTelemetry.Exporter.Prometheus  --prerelease
dotnet add package Serilog.Sinks.Grafana.Loki

2. START SERVICES/ run
-----------------
docker-compose up -d
docker compose up --build -d
docker ps  

3. STOP SERVICES
----------------
docker-compose down

4. RESET GRAFANA ADMIN PASSWORD (if login fails)
------------------------------------------------
docker-compose run --rm grafana grafana-cli admin reset-admin-password admin

5. SERVICE URLs
---------------
- Prometheus:  http://localhost:9090
- Grafana:     http://localhost:3000
- Loki API:    http://localhost:3100

6. DEFAULT GRAFANA LOGIN
------------------------
Username: admin
Password: admin

(If the above doesn’t work, run the reset password command above.)

7. VIEW LOGS FOR A SERVICE
--------------------------
docker-compose logs -f grafana
docker-compose logs -f prometheus
docker-compose logs -f loki

8. RESTART A SPECIFIC SERVICE
-----------------------------
docker-compose restart grafana
docker-compose restart prometheus
docker-compose restart loki
