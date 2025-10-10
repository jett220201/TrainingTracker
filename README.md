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

[![My Skills](https://skillicons.dev/icons?i=visualstudio,dotnet,cs,graphql)](https://skillicons.dev)

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
 ┃ ┃ ┣ 📜UserGoalController.cs
 ┃ ┃ ┣ 📜UserProgressController.cs
 ┃ ┃ ┗ 📜WorkoutsController.cs
 ┃ ┣ 📂Extensions
 ┃ ┃ ┗ 📜AppExtension.cs
 ┃ ┣ 📂GraphQL
 ┃ ┃ ┗ 📂Queries
 ┃ ┃ ┃ ┣ 📜ExercisesQuery.cs
 ┃ ┃ ┃ ┣ 📜GoalsQuery.cs
 ┃ ┃ ┃ ┣ 📜HomeQuery.cs
 ┃ ┃ ┃ ┣ 📜UserProgressQuery.cs
 ┃ ┃ ┃ ┣ 📜UserQuery.cs
 ┃ ┃ ┃ ┗ 📜WorkoutQuery.cs
 ┃ ┣ 📂Middlewares
 ┃ ┃ ┗ 📜UserLanguageMiddleware.cs
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
 ┃ ┃ ┃ ┣ 📂Entities
 ┃ ┃ ┃ ┃ ┣ 📂Exercise
 ┃ ┃ ┃ ┃ ┃ ┣ 📜ExerciseGraphQLDto.cs
 ┃ ┃ ┃ ┃ ┃ ┗ 📜ExerciseConnection.cs
 ┃ ┃ ┃ ┃ ┣ 📂User
 ┃ ┃ ┃ ┃ ┃ ┗ 📜UserGraphQLDto.cs
 ┃ ┃ ┃ ┃ ┣ 📂UserGoal
 ┃ ┃ ┃ ┃ ┃ ┗ 📜UserGoalGraphQLDto.cs
 ┃ ┃ ┃ ┃ ┣ 📂UserProgress
 ┃ ┃ ┃ ┃ ┃ ┗ 📜UserProgressGraphQLDto.cs
 ┃ ┃ ┃ ┃ ┗ 📂Workout
 ┃ ┃ ┃ ┃ ┃ ┣ 📜WorkoutExercisesAssociationGraphQLDto.cs
 ┃ ┃ ┃ ┃ ┃ ┗ 📜WorkoutGraphQLDto.cs
 ┃ ┃ ┃ ┣ 📂ViewModels
 ┃ ┃ ┃ ┃ ┣ 📜GoalsOverviewGraphQLDto.cs
 ┃ ┃ ┃ ┃ ┣ 📜HomeOverviewGraphQLDto.cs
 ┃ ┃ ┃ ┃ ┣ 📜UserProgressOverviewGraphQLDto.cs
 ┃ ┃ ┃ ┃ ┗ 📜WorkoutsOverviewGraphQLDto.cs
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
 ┃ ┃ ┃ ┃ ┣ 📜UserBasicResponseDto.cs
 ┃ ┃ ┃ ┃ ┣ 📜UserChangeLanguageRequestDto.cs
 ┃ ┃ ┃ ┃ ┣ 📜UserChangePasswordRequestDto.cs
 ┃ ┃ ┃ ┃ ┣ 📜UserDeleteAccountRequestDto.cs
 ┃ ┃ ┃ ┃ ┣ 📜UserEditRequestDto.cs
 ┃ ┃ ┃ ┃ ┣ 📜UserRecoverPasswordRequestDto.cs
 ┃ ┃ ┃ ┃ ┣ 📜UserRecoveryPasswordRequestDto.cs
 ┃ ┃ ┃ ┃ ┗ 📜UserRegistrationRequestDto.cs
 ┃ ┃ ┃ ┣ 📂UserGoal
 ┃ ┃ ┃ ┃ ┗ 📜UserGoalRequestDto.cs
 ┃ ┃ ┃ ┣ 📂UserProgress
 ┃ ┃ ┃ ┃ ┗ 📜UserProgressDto.cs
 ┃ ┃ ┃ ┗ 📂Workout
 ┃ ┃ ┃ ┃ ┣ 📜WorkoutDto.cs
 ┃ ┃ ┃ ┃ ┗ 📜WorkoutExercisesAssociationDto.cs
 ┃ ┣ 📂Interfaces
 ┃ ┃ ┣ 📂Helpers
 ┃ ┃ ┃ ┣ 📜IEmailHelper.cs
 ┃ ┃ ┃ ┣ 📜IFitnessCalculator.cs
 ┃ ┃ ┃ ┗ 📜ISecurityHelper.cs
 ┃ ┃ ┣ 📂Repository
 ┃ ┃ ┃ ┣ 📜IExercisesRepository.cs
 ┃ ┃ ┃ ┣ 📜IGenericRepository.cs
 ┃ ┃ ┃ ┣ 📜IRecoveryTokensRepository.cs
 ┃ ┃ ┃ ┣ 📜IRefreshTokensRepository.cs
 ┃ ┃ ┃ ┣ 📜IUserGoalsRepository.cs
 ┃ ┃ ┃ ┣ 📜IUserProgressesRepository.cs
 ┃ ┃ ┃ ┣ 📜IUsersRepository.cs
 ┃ ┃ ┃ ┣ 📜IWorkoutExercisesAssociationsRepository.cs
 ┃ ┃ ┃ ┗ 📜IWorkoutsRepository.cs
 ┃ ┃ ┗ 📂Services
 ┃ ┃ ┃ ┣ 📜IExercisesService.cs
 ┃ ┃ ┃ ┣ 📜IGenericService.cs
 ┃ ┃ ┃ ┣ 📜IRecoveryTokensService.cs
 ┃ ┃ ┃ ┣ 📜IRefreshTokensService.cs
 ┃ ┃ ┃ ┣ 📜IUserGoalsService.cs
 ┃ ┃ ┃ ┣ 📜IUserProgressesService.cs
 ┃ ┃ ┃ ┣ 📜IUserService.cs
 ┃ ┃ ┃ ┣ 📜IWorkoutExercisesAssociationsService.cs
 ┃ ┃ ┃ ┗ 📜IWorkoutsService.cs
 ┃ ┣ 📂Services
 ┃ ┃ ┣ 📜ExercisesService.cs
 ┃ ┃ ┣ 📜RecoveryTokensService.cs
 ┃ ┃ ┣ 📜RefreshTokensService.cs
 ┃ ┃ ┣ 📜UserGoalsService.cs
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
 ┃ ┃ ┃ ┣ 📜UserGoal.cs
 ┃ ┃ ┃ ┣ 📜UserProgress.cs
 ┃ ┃ ┃ ┣ 📜Workout.cs
 ┃ ┃ ┃ ┗ 📜WorkoutExercisesAssociation.cs
 ┃ ┃ ┗ 📂ENUM
 ┃ ┃ ┃ ┣ 📜Gender.cs
 ┃ ┃ ┃ ┣ 📜GoalDirection.cs
 ┃ ┃ ┃ ┣ 📜GoalStatus.cs
 ┃ ┃ ┃ ┣ 📜GoalType.cs
 ┃ ┃ ┃ ┗ 📜MuscleGroup.cs
 ┃ ┗ 📜TrainingTracker.Domain.csproj
 ┣ 📂TrainingTracker.Infrastructure
 ┃ ┣ 📂Helpers
 ┃ ┃ ┣ 📜FitnessCalculator.cs
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
 ┃ ┃ ┣ 📜20250809193749_AddNewColumnsForUser.cs
 ┃ ┃ ┣ 📜20250809193749_AddNewColumnsForUser.Designer.cs
 ┃ ┃ ┣ 📜20250809194551_AddNewTableUserGoals.cs
 ┃ ┃ ┣ 📜20250809194551_AddNewTableUserGoals.Designer.cs
 ┃ ┃ ┣ 📜20250809202453_FixDataTypeForDates.cs
 ┃ ┃ ┣ 📜20250809202453_FixDataTypeForDates.Designer.cs
 ┃ ┃ ┣ 📜20250813004821_AddGoalDirectionColumnToUserGoalTable.cs
 ┃ ┃ ┣ 📜20250813004821_AddGoalDirectionColumnToUserGoalTable.Designer.cs
 ┃ ┃ ┣ 📜20250814015154_AddPrefLangForUserTable.cs
 ┃ ┃ ┣ 📜20250814015154_AddPrefLangForUserTable.Designer.cs
 ┃ ┃ ┗ 📜CoreDBContextModelSnapshot.cs
 ┃ ┣ 📂Persistence
 ┃ ┃ ┗ 📜CoreDBContext.cs
 ┃ ┣ 📂Repository
 ┃ ┃ ┣ 📜ExercisesRepository.cs
 ┃ ┃ ┣ 📜GenericRepository.cs
 ┃ ┃ ┣ 📜RecoveryTokensRepository.cs
 ┃ ┃ ┣ 📜RefreshTokensRepository.cs
 ┃ ┃ ┣ 📜UserGoalsRepository.cs
 ┃ ┃ ┣ 📜UserProgressesRepository.cs
 ┃ ┃ ┣ 📜UsersRepository.cs
 ┃ ┃ ┣ 📜WorkoutExercisesAssociationsRepository.cs
 ┃ ┃ ┗ 📜WorkoutsRepository.cs
 ┃ ┗ 📜TrainingTracker.Infrastructure.csproj
 ┣ 📂TrainingTracker.Localization
 ┃ ┣ 📂Resources
 ┃ ┃ ┣ 📂Services
 ┃ ┃ ┃ ┣ 📜ExercisesServiceResource.es.resx
 ┃ ┃ ┃ ┣ 📜ExercisesServiceResource.resx
 ┃ ┃ ┃ ┣ 📜UserProgressesServiceResource.es.resx
 ┃ ┃ ┃ ┣ 📜UserProgressesServiceResource.resx
 ┃ ┃ ┃ ┣ 📜UsersServiceResource.es.resx
 ┃ ┃ ┃ ┣ 📜UsersServiceResource.resx
 ┃ ┃ ┃ ┣ 📜WorkoutsServiceResource.es.resx
 ┃ ┃ ┃ ┗ 📜WorkoutsServiceResource.resx
 ┃ ┃ ┗ 📂Shared
 ┃ ┃ ┃ ┣ 📜SharedResources.es.resx
 ┃ ┃ ┃ ┗ 📜SharedResources.resx
 ┃ ┗ 📜TrainingTracker.Localization.csproj
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
| `POST`  | `/api/auth/login`      | `AuthController`  | Authenticates the user with username and password, returns a JWT access token and refresh token. Locks the user after 3 failed attempts for 15 minutes. | Username and password | JWT token and Refresh token |
| `POST`  | `/api/auth/refresh`    | `AuthController`  | Refresh the JWT token using a valid refresh token. | Refresh token | JWT token and Refresh token |
| `POST`  | `/api/auth/me`    | `AuthController`  | Retrieve details of the currently authenticated user. | User ID (get by claims in JWT) | User's full name, gender and preferred language |
| `POST`  | `/api/auth/logout`     | `AuthController`  | Revoke the refresh token to log out the user  | Refresh token | Success message |
| `POST`  | `/api/auth/lang-change`     | `AuthController`  | Change the preferred language of the authenticated user  | Preferred language short name | JWT token and Refresh token |
| `POST`  | `/api/exercises/add`   | `ExercisesController` | Add a new exercise to the system  | Name, Description and muscle group | Success message |
| `POST`  | `/api/user/register`   | `UserController`  | Register a new user in the application | Username, name, last name, email, password, gender, height, date of birth and preferred language | Success message|
| `POST`  | `/api/user/edit`   | `UserController`  | Edit user info | Name, last name, gender, height | Success message|
| `POST`  | `/api/user/change-password` | `UserController`  | Change the password of the authenticated user  | Username, current password and new passsword | Success message |
| `POST`  | `/api/user/change-password-recovery` | `UserController` | Change the password using a recovery token  | Recovery token and new password | Success message |
| `POST`  | `/api/user/recover-password`  | `UserController` | Send a password recovery email to the user | Email | Success message |
| `POST`  | `/api/user/delete`     | `UserController` | Delete the authenticated user's account   | Email and password | Success message |
| `POST`  | `/api/usergoal/add`     | `UserGoalController` | Add a new registry with details of the user goal   | Description, target value, type, direction and goal date | Success message |
| `POST`  | `/api/usergoal/delete`     | `UserGoalController` | Delete a user goal by its ID  | Goal ID | Success message |
| `POST`  | `/api/usergoal/edit`     | `UserGoalController` | Edit an existing user goal | Description, target value, type, direction and goal date | Success message |
| `POST`  | `/api/userprogress/add` | `UserProgressController` | Add a new registry with details of the user progress | Body fat percentage, user's weight | Success message |
| `POST`  | `/api/workouts/add`    | `WorkoutsController` | Add a new workout to the system | Name, list of exercises with # repetitions, #sets, weight, exercise ID and time of rest | Success message |
| `POST`  | `/api/workouts/delete`    | `WorkoutsController` | Delete a workout by its ID | Workout ID | Success message |
| `POST`  | `/api/workouts/edit`    | `WorkoutsController` | Edit an existing workout | Workout ID, name, list of exercises with # repetitions, # sets, weight, exercise ID and time of rest | Success message |

**GraphQL**
| Type     | Name                 | Arguments       | Description                          |
|----------|----------------------|-----------------|--------------------------------------|
| `Query`  | `userInfoById`       | `none`*         | Returns all basic user information.  |
| `Query`  | `userProgressByUser` | `none`*         | Returns the user's progress history. |
| `Query`  | `workoutsByUser`     | `search: String [nullable]`* | Returns the list of user's workouts. |
| `Query`  | `userInfo`           | `none`*         | Returns the basic info for home view. |
| `Query`  | `exercises`          | `muscleGroup: Int [nullable], search: String [nullable], first: Int [nullable], last: Int [nullable], after: String [nullable], before: String [nullable]`| Returns the list of exercises that match the search arguments. |
| `Query`  | `goalsByUser`        | `search: String [nullable]`*         | Returns the list of goals by user that match the search argument. |

> [!NOTE]
> Arguments marked with * has the userId argument, but this is acquired from claims.

## 🗂️ Database model
The following diagram explain how the database is defined:
![Database Diagram](db_diagram.png)

## 🧠 Future features
- Deploy the platform!

## 💻📲 Frontend project
https://github.com/jett220201/TrainingTracker-Frontend

## :octocat: Authors
  - **Juan Esteban Torres Tamayo**

## 📜 License
This project is licensed under the [MIT](LICENSE.md)
License - see the [LICENSE.md](LICENSE.md) file for
details.
