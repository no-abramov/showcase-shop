# 🏗️ API Gateway & Microservices for Product Sales System

## 📌 Описание проекта
Этот **pet-проект** демонстрирует работу микросервисной архитектуры на **ASP.NET Core 8**.  
Он моделирует систему управления товарами и продажами с использованием **Ocelot API Gateway** и **RabbitMQ**.

🔹 **Product Service** — управляет товарами (CRUD, получение цен).  
🔹 **SalesPoint Service** — управляет точками продаж и продажами (CRUD, оформление покупки).  
🔹 **Ocelot API Gateway** — маршрутизирует запросы между сервисами.  
🔹 **RabbitMQ** — передает сообщения между сервисами.  

---

## 🛠 **Стек технологий**
✅ **ASP.NET Core 8.0**  
✅ **Entity Framework Core (InMemory DB)**  
✅ **RabbitMQ** (очереди сообщений между сервисами)  
✅ **Ocelot API Gateway** (маршрутизация запросов)  
✅ **Swagger/OpenAPI** (автодокументация)  
✅ **Postman** (тестирование API)  

---

## 🚀 **Текущая стадия разработки**
✅ **Полностью реализованы CRUD-операции** для `Product`, `SalesPoint`, `Sale`.  
✅ **Добавлена маршрутизация через API Gateway (`Ocelot`)**.  
✅ **Реализована работа с RabbitMQ для запроса цен на товары**.  
✅ **Swagger интегрирован, все методы документированы**.  

---

## 🔧 **Запуск проекта**

### 1️⃣ Клонирование репозитория
```sh
git clone https://github.com/yourusername/microservices-product-sales.git
cd microservices-product-sales
