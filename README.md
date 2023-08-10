Asp.Net Core API Project


Clean Architecture using CQRS Pattern
-	Implementing Rich Domain Model that allows us to control the main behavior of our models in the Domain Layer (Factory Pattern and Encapsulation)

-	Implementing Registrars: Extensive method to scan all application services and then run them. It doesn’t matter how many they are, and we keep the Program.cs clean

-	Configuration (IEntityTypeConfiguration): Certain references that occur from the DDD. No annotations in the domain layer.

-	Swagger Configuration

-	Automapper

-	Mediator: connect to handlers(CQRS) via Send() method

-	DTOS (Contracts): using records instead of classes to compare instances. Import Value Based Comparison

-	Application layer: Generic Response Class

-	Custom Error Handling. OperationResult class, Error response system

-	Model Validations using Filters and Data Annotations – Custom Exceptions handling

-	FluentValidation NuGet for Model Validators

-	Asp.Net Core Identity

-	Authentication,Authorisation. Registration/Login Setup and JWTBearer
