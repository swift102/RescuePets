
# 🐾 RescuePets

RescuePets is a web application designed to support pet adoption, donations, and general animal welfare initiatives.  
The system allows users to browse pets available for adoption, register as adopters, make donations, and manage adoption processes online.  

---

## 📌 Features

- **Pet Listings**  
  Browse available pets by type, breed, age, gender, and location.  

- **Adoption Management**  
  Submit adoption requests, fill in adoption forms, and view adoption status.  

- **Donations**  
  Make contributions to support shelters and animal welfare initiatives.  

- **User Accounts**  
  Secure login and registration for adopters, admins, and shelter staff.  

- **Admin Dashboard**  
  Manage pets, adoptions, and donation records from one central dashboard.  

---

## 🛠️ Tech Stack

### Backend
- **ASP.NET Core MVC / Web API**  
- **Entity Framework Core** for ORM  
- **SQL Server** for database storage  

### Frontend
- **Razor Views** (MVC) or Angular/React (if extended)  
- **Bootstrap 5** for responsive styling  
- **JavaScript/TypeScript** for interactivity  

---

## 📂 Project Structure

```

RescuePets/
│── Controllers/      # MVC Controllers for Pets, Adoptions, Donations, Users
│── Models/           # Data models (Pet, User, Donation, Adoption, etc.)
│── ViewModels/       # View models for form binding and data transfer
│── Views/            # Razor views for UI rendering
│── wwwroot/          # Static files (CSS, JS, images)
│── Migrations/       # EF Core database migrations
│── RescuePets.csproj # Project file

````

---

## ⚙️ Getting Started

### Prerequisites
- [.NET 6 or later](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Visual Studio 2022 or VS Code

### Installation
1. Clone the repo:
   ```bash
   git clone https://github.com/swift102/RescuePets.git
   cd RescuePets
````

2. Update the database:

   ```bash
   dotnet ef database update
   ```

3. Run the project:

   ```bash
   dotnet run
   ```

4. Open in browser:

   ```
   http://localhost:5000
   ```

---

## 👩‍💻 Usage

* **Adopters**: Sign up, browse pets, and submit adoption requests.
* **Admins**: Approve/reject adoptions, manage pets, and track donations.
* **Donors**: Make donations securely via the donations page.

---

## 🤝 Contributing

1. Fork the repo
2. Create a new branch (`feature/my-feature`)
3. Commit your changes
4. Push the branch
5. Create a Pull Request

---

## 📜 License

This project is licensed under the MIT License – see the [LICENSE](LICENSE) file for details.

