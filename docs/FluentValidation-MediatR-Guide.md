
# FluentValidation + MediatR Pipeline — Step‑by‑Step Guide

This guide shows how your request validation is wired end‑to‑end using **FluentValidation** and **MediatR**. It covers the pipeline behavior, DI registration, and the concrete validator for `CreateBlogCommand`.

---

## Step 1 — Create a Validator for Each Request

**File:** `CreateBlogCommandValidator.cs`

```csharp
using FluentValidation;

namespace CleanArchWithCQRSandMediatr.Application.Blogs.Commands.CreateBlog;

// Validator for CreateBlogCommand: declares rules that must pass before the command is handled.
public class CreateBlogCommandValidator : AbstractValidator<CreateBlogCommand>
{
    public CreateBlogCommandValidator()
    {
        // Name: required and limited to 200 chars
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 Characters");

        // Description: required
        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Description is Required");

        // Author: required and limited to 20 chars
        RuleFor(v => v.Author)
            .NotEmpty().WithMessage("Author is required")
            .MaximumLength(20).WithMessage("Name must not exceed 20 characters.");
    }
}
```

**What it does**

- Inherits `AbstractValidator<CreateBlogCommand>` so FluentValidation knows which request type this validator applies to.
- Declares **property-level rules** using `RuleFor`:
  - `Name` must be non-empty and no longer than **200** characters.
  - `Description` must be non-empty.
  - `Author` must be non-empty and no longer than **20** characters.
- Messages provided by `WithMessage(...)` are what the client will see if a rule fails.

> **Note:** The message for the `Author` length rule currently says **"Name must not exceed 20 characters."** — that looks like a copy/paste typo. Consider changing it to **"Author must not exceed 20 characters."**

**Optional polish**

- If you want to treat **whitespace-only** values as empty, add:
  ```csharp
  RuleFor(v => v.Name).Must(s => !string.IsNullOrWhiteSpace(s)).WithMessage("Name is required");
  ```
  (You can combine with `NotEmpty()` or replace it — `NotEmpty()` alone treats `"   "` as non-empty.)
- To stop at the **first failing rule per property**, set:
  ```csharp
  RuleLevelCascadeMode = CascadeMode.Stop;
  ```
- To customize the field name in messages without editing every string, use:
  ```csharp
  RuleFor(v => v.Author).NotEmpty().WithName("Author");
  ```

---

## Step 2 — Register Validators in DI

```csharp
services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
```

- Scans the current assembly and registers every `IValidator<T>` (i.e., your `AbstractValidator<T>` classes).
- No need to register each validator manually.

---

## Step 3 — Create a Validation Pipeline Behavior

```csharp
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Any())
                throw new ValidationException(failures);
        }

        return await next();
    }
}
```

**What it does**

- Acts like **middleware** for all MediatR requests.
- Resolves all `IValidator<TRequest>` for the current request and runs them (in parallel via `Task.WhenAll`).
- If any rule fails, it throws `ValidationException` and **prevents the handler from running**.
- If nothing fails, execution continues to the next behavior/handler via `next()`.

---

## Step 4 — Wire Everything in MediatR

```csharp
services.AddMediatR(ctg =>
{
    ctg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    ctg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
});
```

- Registers your MediatR handlers and requests.
- Adds the `ValidationBehaviour<TRequest, TResponse>` to the pipeline so **every** request is validated before handling.

---

## Step 5 — Execution Flow

1. `IMediator.Send(new CreateBlogCommand(...))` is called.
2. MediatR runs pipeline behaviors. `ValidationBehaviour` executes first (before your handler).
3. DI provides all validators for `CreateBlogCommand` (thanks to `AddValidatorsFromAssembly`).
4. Validators check the request:
   - **If any failure** → `ValidationException` is thrown; request **does not** reach the handler.
   - **If valid** → control passes to your actual `CreateBlogCommandHandler`.

---

## Step 6 — Returning API Errors (Typical Pattern)

If you’re building a Web API, catch `ValidationException` and convert it to a structured response:

```csharp
app.UseExceptionHandler(builder =>
{
    builder.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerFeature>();
        if (feature?.Error is ValidationException vex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var errors = vex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            await context.Response.WriteAsJsonAsync(new { errors });
            return;
        }

        // fallback for other exceptions
    });
});
```

**Example response**
```json
{
  "errors": {
    "Name": ["Name is required"],
    "Author": ["Author must not exceed 20 characters."]
  }
}
```

---

## Step 7 — Quick Checklist

- ✅ Create `AbstractValidator<TRequest>` per request.
- ✅ Register validators: `AddValidatorsFromAssembly(...)`.
- ✅ Add `ValidationBehaviour<,>` implementing `IPipelineBehavior<,>`.
- ✅ Register the behavior with MediatR: `AddBehavior(...)`.
- ✅ (API) Convert `ValidationException` to a 400 response body.
- ✅ (Nice‑to‑have) Add `CascadeMode.Stop`, whitespace checks, and consistent messages.

---

## FAQ

**Q. Do validators run in parallel?**  
Yes — the behavior uses `Task.WhenAll`, so each `ValidateAsync` runs concurrently.

**Q. Do I need both `NotNull()` and `NotEmpty()`?**  
Usually **no**. `NotEmpty()` implies not null for reference types, but it does **not** treat whitespace-only as empty. Add a `.Must(s => !string.IsNullOrWhiteSpace(s))` if you want that behavior.

**Q. Can I have different rules for create vs update?**  
Yes — use **RuleSets** or separate validators per request type.
