# FluentValidation Implementation Details

This document explains the detailed implementation of FluentValidation in our Clean Architecture solution.

## 1. Command Validator Implementation

**File:** `/Application/Blogs/Commands/CreateBlog/CreateBlogCommandValidator.cs`

The `CreateBlogCommandValidator` class implements validation rules for the blog creation command:

```csharp
public class CreateBlogCommandValidator : AbstractValidator<CreateBlogCommand>
{
    public CreateBlogCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 Characters");

        // ...other rules
    }
}
```

### Validation Rules Breakdown:

1. **Name Validation**

   - Must not be empty (null or whitespace)
   - Maximum length of 200 characters
   - Custom error messages for better user feedback

2. **Description Validation**

   - Must not be empty
   - Ensures blog posts have content

3. **Author Validation**
   - Must not be empty
   - Maximum length of 20 characters
   - Prevents overly long author names

## 2. Validation Behavior Pipeline

**File:** `/Application/Common/Behaviours/ValidationBehaviour.cs`

The `ValidationBehaviour` class implements MediatR's pipeline behavior to perform validation:

### Key Components:

1. **Constructor Injection**

```csharp
private readonly IEnumerable<IValidator<TRequest>> _validators;
public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
{
    _validators = validators;
}
```

- Receives all validators for the current request type through DI

2. **Validation Process**

- Creates validation context
- Executes all validators asynchronously
- Collects validation failures
- Throws exception if validation fails

### Validation Flow:

1. Request enters the pipeline
2. ValidationBehaviour checks for validators
3. If validators exist:
   - Creates validation context
   - Runs all validators
   - Collects failures
   - Either throws or continues
4. If validation passes, proceeds to handler

## 3. Dependency Registration

**File:** `/Application/DependencyInjection.cs`

### Registration Components:

1. **Validator Registration**

```csharp
services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
```

- Scans assembly for validator classes
- Registers them with DI container
- Enables automatic validator discovery

2. **Pipeline Behavior Registration**

```csharp
ctg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
```

- Registers validation behavior in MediatR pipeline
- Ensures validation runs before handlers

## Runtime Execution Flow

1. **Request Initiation**

   - Controller receives HTTP request
   - Maps to command/query
   - Sends through MediatR

2. **Validation Pipeline**

   - ValidationBehaviour intercepts request
   - Resolves validators from DI
   - Runs validation rules

3. **Validation Results**

   - Success: continues to handler
   - Failure: throws ValidationException

4. **Error Handling**
   - ValidationException caught by middleware
   - Mapped to 400 Bad Request
   - Returns validation errors to client

## Example Validation Response

```json
{
  "errors": {
    "Name": ["Name is required", "Name must not exceed 200 Characters"],
    "Author": ["Author is required"]
  }
}
```

## Benefits of This Implementation

1. **Separation of Concerns**

   - Validation logic separate from handlers
   - Reusable validation rules
   - Clean handler code

2. **Consistent Validation**

   - Centralized validation pipeline
   - Uniform error handling
   - Automatic validation execution

3. **Maintainability**
   - Easy to add/modify rules
   - Single responsibility principle
   - Clear validation flow
