# TrainingTracker 💪

API for managing user routines and progress, developed with .NET.

## 🚀 Technologies Used
- **.NET 8** 💎
- **JWT for Authentication** 🔒
- **Entity Framework Core with Migrations** 🛠️
- **SQL Server** 🗃️
- **HotChocolate** 🍫
- **Swagger for documentation (REST)** 📗
- **Banana Cake POP for documentation (GraphQL)** 📙
- **SendGrid for messaging** 📨

## 📥 Getting Started
These instructions will give you a give to install and run the
project in your local machine.

### 📂 Project structure
This project has the following structure:
```
📦TrainingTracker
 ┣ 📂.github
 ┃ ┗ 📂workflows
 ┣ 📂TrainingTracker
 ┃ ┣ 📂Controllers
 ┃ ┃ ┣ 📜AuthController.cs
 ┃ ┃ ┣ 📜BaseApiController.cs
 ┃ ┃ ┣ 📜ExercisesController.cs
 ┃ ┃ ┣ 📜UserController.cs
 ┃ ┃ ┣ 📜UserProgressController.cs
 ┃ ┃ ┗ 📜WorkoutsController.cs
 ┃ ┣ 📂Extensions
 ┃ ┃ ┗ 📜AppExtension.cs
 ┃ ┣ 📂GraphQL
 ┃ ┃ ┗ 📂Queries
 ┃ ┃ ┃ ┣ 📜UserProgressQuery.cs
 ┃ ┃ ┃ ┣ 📜UserQuery.cs
 ┃ ┃ ┃ ┗ 📜WorkoutQuery.cs
 ┃ ┣ 📂Properties
 ┃ ┃ ┗ 📜launchSettings.json
 ┃ ┣ 📜appsettings.Development.json
 ┃ ┣ 📜appsettings.json
 ┃ ┣ 📜log4net.config
 ┃ ┣ 📜Program.cs
 ┃ ┣ 📜TrainingTracker.API.csproj
 ┃ ┣ 📜TrainingTracker.API.csproj.user
 ┃ ┗ 📜TrainingTracker.http
 ┣ 📂TrainingTracker.Application
 ┃ ┣ 📂DTOs
 ┃ ┃ ┣ 📂GraphQL
 ┃ ┃ ┃ ┣ 📂Exercise
 ┃ ┃ ┃ ┃ ┗ 📜ExerciseGraphQLDto.cs
 ┃ ┃ ┃ ┣ 📂User
 ┃ ┃ ┃ ┃ ┗ 📜UserGraphQLDto.cs
 ┃ ┃ ┃ ┣ 📂UserProgress
 ┃ ┃ ┃ ┃ ┗ 📜UserProgressGraphQLDto.cs
 ┃ ┃ ┃ ┗ 📂Workout
 ┃ ┃ ┃ ┃ ┣ 📜WorkoutExercisesAssociationGraphQLDto.cs
 ┃ ┃ ┃ ┃ ┗ 📜WorkoutGraphQLDto.cs
 ┃ ┃ ┗ 📂REST
 ┃ ┃ ┃ ┣ 📂Exercise
 ┃ ┃ ┃ ┃ ┗ 📜ExerciseDto.cs
 ┃ ┃ ┃ ┣ 📂General
 ┃ ┃ ┃ ┃ ┣ 📜ApiResponseDto.cs
 ┃ ┃ ┃ ┃ ┗ 📜ErrorResponseDto.cs
 ┃ ┃ ┃ ┣ 📂Login
 ┃ ┃ ┃ ┃ ┣ 📜LoginRequestDto.cs
 ┃ ┃ ┃ ┃ ┣ 📜LoginResponseDto.cs
 ┃ ┃ ┃ ┃ ┣ 📜LogoutRequestDto.cs
 ┃ ┃ ┃ ┃ ┗ 📜RefreshTokenRequestDto.cs
 ┃ ┃ ┃ ┣ 📂User
 ┃ ┃ ┃ ┃ ┣ 📜UserChangePasswordRequestDto.cs
 ┃ ┃ ┃ ┃ ┣ 📜UserDeleteAccountRequestDto.cs
 ┃ ┃ ┃ ┃ ┣ 📜UserRecoverPasswordRequestDto.cs
 ┃ ┃ ┃ ┃ ┣ 📜UserRecoveryPasswordRequestDto.cs
 ┃ ┃ ┃ ┃ ┗ 📜UserRegistrationRequestDto.cs
 ┃ ┃ ┃ ┣ 📂UserProgress
 ┃ ┃ ┃ ┃ ┗ 📜UserProgressDto.cs
 ┃ ┃ ┃ ┗ 📂Workout
 ┃ ┃ ┃ ┃ ┣ 📜WorkoutDto.cs
 ┃ ┃ ┃ ┃ ┗ 📜WorkoutExercisesAssociationDto.cs
 ┃ ┣ 📂Interfaces
 ┃ ┃ ┣ 📂Helpers
 ┃ ┃ ┃ ┣ 📜IEmailHelper.cs
 ┃ ┃ ┃ ┗ 📜ISecurityHelper.cs
 ┃ ┃ ┣ 📂Repository
 ┃ ┃ ┃ ┣ 📜IExercisesRepository.cs
 ┃ ┃ ┃ ┣ 📜IGenericRepository.cs
 ┃ ┃ ┃ ┣ 📜IRecoveryTokensRepository.cs
 ┃ ┃ ┃ ┣ 📜IRefreshTokensRepository.cs
 ┃ ┃ ┃ ┣ 📜IUserProgressesRepository.cs
 ┃ ┃ ┃ ┣ 📜IUsersRepository.cs
 ┃ ┃ ┃ ┣ 📜IWorkoutExercisesAssociationsRepository.cs
 ┃ ┃ ┃ ┗ 📜IWorkoutsRepository.cs
 ┃ ┃ ┗ 📂Services
 ┃ ┃ ┃ ┣ 📜IExercisesService.cs
 ┃ ┃ ┃ ┣ 📜IGenericService.cs
 ┃ ┃ ┃ ┣ 📜IRecoveryTokensService.cs
 ┃ ┃ ┃ ┣ 📜IRefreshTokensService.cs
 ┃ ┃ ┃ ┣ 📜IUserProgressesService.cs
 ┃ ┃ ┃ ┣ 📜IUserService.cs
 ┃ ┃ ┃ ┣ 📜IWorkoutExercisesAssociationsService.cs
 ┃ ┃ ┃ ┗ 📜IWorkoutsService.cs
 ┃ ┣ 📂Services
 ┃ ┃ ┣ 📜ExercisesService.cs
 ┃ ┃ ┣ 📜RecoveryTokensService.cs
 ┃ ┃ ┣ 📜RefreshTokensService.cs
 ┃ ┃ ┣ 📜UserProgressesService.cs
 ┃ ┃ ┣ 📜UsersService.cs
 ┃ ┃ ┣ 📜WorkoutExercisesAssociationsService.cs
 ┃ ┃ ┗ 📜WorkoutsService.cs
 ┃ ┗ 📜TrainingTracker.Application.csproj
 ┣ 📂TrainingTracker.Domain
 ┃ ┣ 📂Entities
 ┃ ┃ ┣ 📂DB
 ┃ ┃ ┃ ┣ 📜Exercise.cs
 ┃ ┃ ┃ ┣ 📜RecoveryToken.cs
 ┃ ┃ ┃ ┣ 📜RefreshToken.cs
 ┃ ┃ ┃ ┣ 📜User.cs
 ┃ ┃ ┃ ┣ 📜UserProgress.cs
 ┃ ┃ ┃ ┣ 📜Workout.cs
 ┃ ┃ ┃ ┗ 📜WorkoutExercisesAssociation.cs
 ┃ ┃ ┗ 📂ENUM
 ┃ ┃ ┃ ┗ 📜MuscleGroup.cs
 ┃ ┗ 📜TrainingTracker.Domain.csproj
 ┣ 📂TrainingTracker.Infrastructure
 ┃ ┣ 📂Helpers
 ┃ ┃ ┣ 📜SecurityHelper.cs
 ┃ ┃ ┗ 📜SendGridEmailHelper.cs
 ┃ ┣ 📂Migrations
 ┃ ┃ ┣ 📜20250724014547_InitialCreate.cs
 ┃ ┃ ┣ 📜20250724014547_InitialCreate.Designer.cs
 ┃ ┃ ┣ 📜20250726010809_AddEmailToUserTable.cs
 ┃ ┃ ┣ 📜20250726010809_AddEmailToUserTable.Designer.cs
 ┃ ┃ ┣ 📜20250730001946_AddLockOutAndFailedAttemptsToUser.cs
 ┃ ┃ ┣ 📜20250730001946_AddLockOutAndFailedAttemptsToUser.Designer.cs
 ┃ ┃ ┣ 📜20250731005348_AddNameAndLastNameToUser.cs
 ┃ ┃ ┣ 📜20250731005348_AddNameAndLastNameToUser.Designer.cs
 ┃ ┃ ┣ 📜20250805001947_RemoveScheduleColumnInWorkout.cs
 ┃ ┃ ┣ 📜20250805001947_RemoveScheduleColumnInWorkout.Designer.cs
 ┃ ┃ ┣ 📜20250807215614_AddRecoveryTokensTable.cs
 ┃ ┃ ┣ 📜20250807215614_AddRecoveryTokensTable.Designer.cs
 ┃ ┃ ┗ 📜CoreDBContextModelSnapshot.cs
 ┃ ┣ 📂Persistence
 ┃ ┃ ┗ 📜CoreDBContext.cs
 ┃ ┣ 📂Repository
 ┃ ┃ ┣ 📜ExercisesRepository.cs
 ┃ ┃ ┣ 📜GenericRepository.cs
 ┃ ┃ ┣ 📜RecoveryTokensRepository.cs
 ┃ ┃ ┣ 📜RefreshTokensRepository.cs
 ┃ ┃ ┣ 📜UserProgressesRepository.cs
 ┃ ┃ ┣ 📜UsersRepository.cs
 ┃ ┃ ┣ 📜WorkoutExercisesAssociationsRepository.cs
 ┃ ┃ ┗ 📜WorkoutsRepository.cs
 ┃ ┗ 📜TrainingTracker.Infrastructure.csproj
 ┣ 📜.gitattributes
 ┣ 📜.gitignore
 ┣ 📜LICENSE.txt
 ┣ 📜README.md
 ┗ 📜TrainingTracker.sln
```

### 🔥 Installing

1. Clone the repository:
```sh
git clone https://github.com/jett220201/TrainingTracker.git
cd TrainingTracker
```
2. Set connection string:
```sh
{
  "ConnectionStrings": {
    "DefaultConnection": "Server={your_server};Initial Catalog={your_db_catalog};Persist Security Info=False;User ID={user};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}
```
3. Restore NuGet packages:
```sh
dotnet restore
```
4. Run project:
```sh
dotnet run
```
5. Open Swagger in http://localhost:{port}/swagger for REST methods
6. Open Banana Cake Pop (Nitro) in http://localhost:{port}/graphql for GraphQL methods

## ⚙️ API architecture
This project follows Clean Architecture principles, dividing the application into well-defined layers:

- **Application 🛠️ → Contains the business logic, rules and use cases.**
- **Domain 📦 → Defines the entities and contracts of the application.**
- **Infrastructure 🏗️ → Manages data persistence, database access and external services.**
- **Presentation (API) 🌍 → Exposes drivers and endpoints for interaction with external clients.**

This separation improves the modularity, maintainability and testability of the code, allowing changing technologies without affecting the core business. 🚀

## 📌 API Endpoints
**REST**

| Method  | Endpoint               | Controller         | Description               | Parameters       | Expected Response |
|---------|------------------------|--------------------|---------------------------|------------------|-------------------|
| `POST`  | `/api/auth/login`      | `AuthController`  | Authenticates the user with username and password, returns a JWT access token and refresh token. Locks the user after 3 failed attempts for 15 minutes. | Username and Password | JWT token and Refresh token |
| `POST`  | `/api/auth/refresh`    | `AuthController`  | Refresh the JWT token using a valid refresh token. | Refresh token | JWT token and Refresh token |
| `POST`  | `/api/auth/logout`     | `AuthController`  | Revoke the refresh token to log out the user  | Refresh token | Success message |
| `POST`  | `/api/exercises/add`   | `ExercisesController` | Add a new exercise to the system  | Name, Description and muscle group | Success message |
| `POST`  | `/api/user/register`   | `UserController`  | Register a new user in the application | Username, name, last name, email, password | Success message |
| `POST`  | `/api/user/change-password` | `UserController`  | Change the password of the authenticated user  | Username, current password and new passsword | Success message |
| `POST`  | `/api/user/change-password-recovery` | `UserController` | Change the password using a recovery token  | Recovery token and new password | Success message |
| `POST`  | `/api/user/recover-password`  | `UserController` | Send a password recovery email to the user | Email | Success message |
| `POST`  | `/api/user/delete`     | `UserController` | Delete the authenticated user's account   | Email and password | Success message |
| `POST`  | `/api/userprogress/add` | `UserProgressController` | Add a new registry with details of the user progress | User ID, Body fat percentage, user's weight | Success message |
| `POST`  | `/api/workouts/add`    | `WorkoutsController` | Add a new workout to the system | User ID, name, list of exercises with # repetitions, #sets, weight, exercise ID | Success message |

**GraphQL**
| Type     | Name                 | Arguments     | Description                          |
|----------|----------------------|---------------|--------------------------------------|
| `Query`  | `userInfoById`       | `id: Int`     | Returns all basic user information.  |
| `Query`  | `userProgressByUser` | `idUser: Int` | Returns the user's progress history. |
| `Query`  | `workoutsByUser`     | `idUser: Int` | Returns the list of user's workouts. |

## 🗂️ Database model
The following diagram explain how the database is defined:
![Database Diagram](db_diagram.png)

## 🧠 Future features
- Develop a modern frontend that displays platform information
- Develop functionality to handle multiple languages
- Develop periodic tasks that clean data from the database
- Deploy the platform!

## :octocat: Authors
  - **Juan Esteban Torres Tamayo**

## 📜 License
This project is licensed under the [MIT](LICENSE.md)
License - see the [LICENSE.md](LICENSE.md) file for
details.
