# TaskFlow

TaskFlow, görev ve proje yönetimini kolaylaştırmak amacıyla geliştirilmiş bir web uygulamasıdır.

Proje; **ASP.NET Core Web API**, **ASP.NET Core MVC**, **Entity Framework Core** ve **SQL Server** kullanılarak geliştirilmiştir.

Sistem iki farklı kullanıcı rolüne sahiptir: **Admin** ve **User**.

---

## ====== Özellikler =======

### 👤 Kullanıcı

- Sisteme kullanıcı adı ve şifre ile giriş yapabilir.
- Kendisine atanmış görevleri görüntüleyebilir.
- Görevlerin proje, öncelik, durum ve son teslim tarihi bilgilerini görebilir.
- Yalnızca kendisine atanmış görevlerin durumunu değiştirebilir.
- Görev durumunu:
  - Bekliyor
  - Devam Ediyor
  - Tamamlandı
  
  olarak güncelleyebilir.
- Başka kullanıcılara ait görevleri değiştiremez.
- Dashboard üzerinden görev istatistiklerini görüntüleyebilir.
- Yaklaşan ve geciken görevlerini takip edebilir.

### 🔐 Admin

Admin, kullanıcı özelliklerine ek olarak:

- Kullanıcı oluşturabilir, düzenleyebilir ve silebilir.
- Proje oluşturabilir, düzenleyebilir ve silebilir.
- Kullanıcılara görev atayabilir.
- Sistemdeki tüm görevleri görüntüleyebilir.
- Görevleri düzenleyebilir ve silebilir.
- Admin Dashboard üzerinden:
  - Toplam kullanıcı
  - Toplam proje
  - Toplam görev
  - Tamamlanan görev
  
  sayılarını görüntüleyebilir.

---

## 🔒 Güvenlik

- JWT Bearer Authentication
- Role-Based Authorization
- Admin ve User rol ayrımı
- BCrypt ile şifre hashleme
- Kullanıcıların yalnızca kendi görev durumlarını değiştirebilmesi
- Admin işlemlerinin API seviyesinde korunması
- JWT Secret bilgisinin User Secrets ile saklanması

Yetkisiz işlemlerde API uygun HTTP durum kodlarını döndürür:

- `401 Unauthorized`
- `403 Forbidden`
- `404 Not Found`

---

## 🛠️ Kullanılan Teknolojiler

### Backend

- C#
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- AutoMapper
- JWT Authentication
- BCrypt
- Repository Pattern
- Service Layer

### Frontend

- ASP.NET Core MVC
- Razor Views
- Bootstrap
- HTML
- CSS

### Database

- Microsoft SQL Server
- SQL Server Management Studio (SSMS)

### Diğer

- Swagger / OpenAPI
- Git
- GitHub
- Visual Studio 2022

---

## 🏗️ Proje Yapısı

Solution iki ana projeden oluşmaktadır:

### TaskFlow.Api

Uygulamanın backend tarafıdır.

API içerisinde:

- Controllers
- DTOs
- Entities
- Repositories
- Services
- AutoMapper
- Authentication & Authorization

katmanları bulunmaktadır.

### TaskFlow.Web

Uygulamanın kullanıcı arayüzüdür.

MVC yapısı kullanılarak geliştirilmiştir ve `TaskFlow.Api` ile HTTP istekleri üzerinden iletişim kurmaktadır.

---

## 📋 Görev Durumları

| ID | Durum |
|---|---|
| 1 | Bekliyor |
| 2 | Devam Ediyor |
| 3 | Tamamlandı |

Bir görev **Tamamlandı** durumuna getirildiğinde `CompletedDate` otomatik olarak oluşturulur.

Görev tekrar **Bekliyor** veya **Devam Ediyor** durumuna alınırsa `CompletedDate` temizlenir.

---

## 🔑 Yetkilendirme

API endpointleri JWT ile korunmaktadır.

Örneğin:

```text
POST /api/TaskItems