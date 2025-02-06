# Showcase Shop

## О проекте
**Showcase Shop** — это pet-проект, предназначенный для изучения микросервисной архитектуры и работы с современными технологиями. Он реализует процесс управления товарами и продажами с использованием микросервисного подхода. Проект включает два основных сервиса:
- **ProductService** — отвечает за управление товарами (CRUD-операции, хранение информации о цене).
- **SalesPointService** — управляет точками продаж, обработкой покупок и взаимодействует с сервисом товаров через RabbitMQ.
- **ApiGateway** – отвечает за маршрутизацию запросов между клиентами и микросервисами, использует Ocelot для проксирования HTTP-запросов.

## Стек технологий
- .NET 8 (ASP.NET Core Web API)
- Entity Framework Core (InMemory DB)
- RabbitMQ (очереди сообщений между сервисами)
- Ocelot API Gateway (маршрутизация запросов)
- Swagger/OpenAPI (автодокументирование API)
- Postman (тестирование API)

## Текущая стадия проекта
На данный момент реализованы основные микросервисные функции:
- Разработка и разворачивание ProductService и SalesPointService
- Взаимодействие сервисов через REST API и RabbitMQ
- Базовая бизнес-логика продаж и учета товаров
- Swagger-документирование API

## Что дальше?
- Полное соблюдение принципов **SOLID**
- Переход с **InMemory DB** на **PostgreSQL**
- Версионирование API
- Покрытие кода тестами (**xUnit, Moq**)
- Глубокое логирование и глобальный обработчик ошибок
- Общий Swagger **(реализация в ApiGateway)**
- Перенос проекта в **Docker** для контейнеризации и удобного развертывания

## Запуск проекта
1. **Клонировать репозиторий**:
   ```sh
   git clone https://github.com/no-abramov/showcase-shop.git
   cd showcase-shop
   ```

2. **Запустить RabbitMQ** (если он не запущен):
   ```sh
   docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:management
   ```

3. **Запустить API Gateway**:
   ```sh
   cd ApiGateway
   dotnet run
   ```

4. **Запустить сервис товаров**:
   ```sh
   cd ProductServices
   dotnet run
   ```

5. **Запустить сервис точек продаж**:
   ```sh
   cd SalesPointServices
   dotnet run
   ```

6. **Тестирование API**
   - Перейти в Swagger UI (ProductService, SalesPointService): `http://localhost:XXXX/swagger`
   - Выполнять тестовые запросы через **Postman** или **Swagger UI**

---
Проект остается в стадии активной разработки. Любые идеи и предложения приветствуются!

